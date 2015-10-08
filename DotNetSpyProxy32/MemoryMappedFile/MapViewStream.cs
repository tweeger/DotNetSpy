//
// MapViewStream.cs
//    
//    Implementation of a library to use Win32 Memory Mapped
//    Files from within .NET applications
//
// COPYRIGHT (C) 2001, Tomas Restrepo (tomasr@mvps.org)
//   Original concept and implementation
// COPYRIGHT (C) 2006, Steve Simpson (s.simpson64@gmail.com)
//   Modifications to allow dynamic paging for seek, read, and write methods
// COPYRIGHT (C) 2008, Mikael Svenson (miksvenson@gmail.com)
//   Removed dynamic paging, since it slows things down and 64bit can address as much space

//   as it feels like. Kept some of the other modifications.
//   Removed unused constants.
//   Removed code to position the view.
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
//

using System;
using System.IO;
using System.Runtime.InteropServices;

namespace DotNetSpyProxy32.IO.FileMap
{
    /// <summary>
    ///   Allows you to read/write from/to
    ///   a view of a memory mapped file.
    /// </summary>
    public class MapViewStream : Stream, IDisposable
    {
        #region Map/View Related Fields

        protected MemoryMappedFile _backingFile;
        protected MapAccess _access = MapAccess.FileMapWrite;
        protected bool _isWriteable;
        IntPtr _viewBaseAddr = IntPtr.Zero; // Pointer to the base address of the currently mapped view
        protected long _mapSize;
        protected long _viewStartIdx = -1;
        protected long _viewSize = -1;
        long _position; //! our current position in the stream buffer


        #region Properties
        public IntPtr ViewBaseAddr
        {
            get { return _viewBaseAddr; }
        }
        public bool IsViewMapped
        {
            get { return (_viewStartIdx != -1) && (_viewStartIdx + _viewSize) <= (_mapSize); }
        }

        #endregion

        #endregion // Map/View Related Fields

        #region Map / Unmap View

        #region Unmap View

        protected void UnmapView()
        {
            if (IsViewMapped)
            {
                _backingFile.UnMapView(this);
                _viewStartIdx = -1;
                _viewSize = -1;
            }
        }

        #endregion

        #region Map View

        protected void MapView(ref long viewStartIdx, ref long viewSize)
        {
            // Now map the view
            _viewBaseAddr = _backingFile.MapView(_access, viewStartIdx, viewSize);
            _viewStartIdx = viewStartIdx;
            _viewSize = viewSize;

        }
        #endregion

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor used internally by MemoryMappedFile.
        /// </summary>
        /// <param name="backingFile">Preconstructed MemoryMappedFile</param>
        /// <param name="mapSize">Size of the view, in bytes.</param>
        /// <param name="isWriteable">True if Read/Write access is desired, False otherwise</param>
        internal MapViewStream(MemoryMappedFile backingFile, long mapSize, bool isWriteable)
        {
            if (backingFile == null)
            {
                throw new Exception("MapViewStream.MapViewStream - backingFile is null");
            }
            if (!backingFile.IsOpen)
            {
                throw new Exception("MapViewStream.MapViewStream - backingFile is not open");
            }
            if ((mapSize < 1) || (mapSize > backingFile.MaxSize))
            {
                throw new Exception(string.Format("MapViewStream.MapViewStream - mapSize is invalid.  mapSize == {0}, backingFile.MaxSize == {1}", mapSize, backingFile.MaxSize));
            }

            _backingFile = backingFile;
            _isWriteable = isWriteable;
            _access = isWriteable ? MapAccess.FileMapWrite : MapAccess.FileMapRead;
            // Need a backingFile.SupportsAccess function that takes a MapAccess compares it against its stored MapProtection protection and returns bool
            _mapSize = mapSize;

            _isOpen = true;

            // Map the first view

            Seek(0, SeekOrigin.Begin);
        }

        #endregion

        #region Stream Properties

        public override bool CanRead
        {
            get { return true; }
        }
        public override bool CanSeek
        {
            get { return true; }
        }
        public override bool CanWrite
        {
            get { return _isWriteable; }
        }
        public override long Length
        {
            get { return _mapSize; }
        }

        public override long Position
        {
            get { return _position; }
            set { Seek(value, SeekOrigin.Begin); }
        }

        #endregion // Stream Properties

        #region Stream Methods

        public override void Flush()
        {
            if (!IsOpen)
                throw new ObjectDisposedException("Winterdom.IO.FileMap.MapViewStream.Flush - Stream is closed");

            // flush the view but leave the buffer intact
            _backingFile.Flush(this);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (!IsOpen)
                throw new ObjectDisposedException("Stream is closed");

            if (buffer.Length - offset < count)
                throw new ArgumentException("Invalid Offset");

            int bytesToRead = (int)Math.Min(Length - _position, count);
            //Marshal.Copy((IntPtr)(_viewBaseAddr.ToInt64() + _position), buffer, offset, bytesToRead);

            UnsafeRead(buffer, offset, bytesToRead);

            _position += bytesToRead;
            return bytesToRead;
        }

        private void UnsafeRead(byte[] buffer, int offset, int count)
        {
            unsafe
            {
                fixed (byte* destPtr = buffer)
                {
                    byte* src = (byte*)(_viewBaseAddr.ToInt64() + _position);
                    byte* dest = destPtr + offset;
                    while (count >= 8)
                    {
                        *((Int64*)dest) = *((Int64*)src);
                        dest += 8;
                        src += 8;
                        count -= 8;
                    }
                    if (count == 0) return;
                    while (count >= 4)
                    {
                        *((Int32*)dest) = *((Int32*)src);
                        dest += 4;
                        src += 4;
                        count -= 4;
                    }
                    if (count == 0) return;
                    while (count >= 2)
                    {
                        *((Int16*)dest) = *((Int16*)src);
                        dest += 2;
                        src += 2;
                        count -= 2;
                    }
                    while (count >= 1)
                    {
                        *dest = *src;
                        dest += 1;
                        src += 1;
                        count -= 1;
                    }
                }
            }
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (!IsOpen)
                throw new ObjectDisposedException("Stream is closed");
            if (!CanWrite)
                throw new FileMapIOException("Stream cannot be written to");

            if (buffer.Length - offset < count)
                throw new ArgumentException("Invalid Offset");

            int bytesToWrite = (int)Math.Min(Length - _position, count);
            if (bytesToWrite == 0)
                return;

            //Marshal.Copy(buffer, offset, (IntPtr)(_viewBaseAddr.ToInt64() + _position), bytesToWrite);
            UnsafeWrite(buffer, offset, bytesToWrite);

            _position += bytesToWrite;
        }

        private void UnsafeWrite(byte[] buffer, int offset, int count)
        {
            unsafe
            {
                fixed (byte* srcPtr = buffer)
                {
                    byte* src = srcPtr + offset;
                    byte* dest = (byte*)(_viewBaseAddr.ToInt64() + _position);
                    while (count >= 8)
                    {
                        *((Int64*)dest) = *((Int64*)src);
                        src += 8;
                        dest += 8;
                        count -= 8;
                    }
                    if (count == 0) return;
                    while (count >= 4)
                    {
                        *((Int32*)dest) = *((Int32*)src);
                        src += 4;
                        dest += 4;
                        count -= 4;
                    }
                    if (count == 0) return;
                    while (count >= 2)
                    {
                        *((Int16*)dest) = *((Int16*)src);
                        src += 2;
                        dest += 2;
                        count -= 2;
                    }
                    while (count >= 1)
                    {
                        *dest = *src;
                        src += 1;
                        dest += 1;
                        count -= 1;
                    }
                }
            }
        }


        public override long Seek(long offset, SeekOrigin origin)
        {
            if (!IsOpen)
                throw new ObjectDisposedException("Stream is closed");

            long newpos = 0;
            switch (origin)
            {
                case SeekOrigin.Begin: newpos = offset; break;
                case SeekOrigin.Current: newpos = Position + offset; break;
                case SeekOrigin.End: newpos = Length + offset; break;
            }
            // sanity check
            if (newpos < 0 || newpos > Length)
                throw new FileMapIOException("Invalid Seek Offset");
            _position = newpos;

            if (!IsViewMapped)
            {
                MapView(ref newpos, ref _mapSize); // use _mapsize here??
            }

            return newpos;
        }

        public override void SetLength(long value)
        {
            // not supported!
            throw new NotSupportedException("Winterdom.IO.FileMap.MapViewStream.SetLength - Can't change map size");
        }

        public override void Close()
        {
            Dispose(true);
        }

        #endregion // Stream methods

        #region IDisposable Implementation

        private bool _isOpen;
        public bool IsOpen { get { return _isOpen; } }

        public new void Dispose()
        {
            Dispose(true);
        }

        protected new virtual void Dispose(bool disposing)
        {
            if (IsOpen)
            {
                Flush();
                UnmapView();
                _isOpen = false;
            }

            if (disposing)
                GC.SuppressFinalize(this);
        }

        ~MapViewStream()
        {
            Dispose(false);
        }

        #endregion // IDisposable Implementation

    } // class MapViewStream

} // namespace Winterdom.IO.FileMap
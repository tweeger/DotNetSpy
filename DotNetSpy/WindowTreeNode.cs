using System;
using System.Windows.Forms;

namespace DotNetSpy
{
    #region Native Window Information
    public class NativeWindow
    {
        public IntPtr Handle{get; set;}
        public string ClassName { get; set; }
        public string Text { get; set; }
    }
    #endregion

	enum ImageIndices : int
	{
		Window = 0,
		WindowHidden = 1,
		ManagedWindow = 2,
		ManagedWindowHidden = 3
	};
	
	/// <summary>
	/// Summary description for WindowTreeNode.
	/// </summary>
	public class WindowTreeNode : TreeNode
    {
        private bool _isManaged = false;
        private IntPtr hwnd = IntPtr.Zero;

		public WindowTreeNode(IntPtr hwnd) :
            this(hwnd, NativeMethods.IsDotNetWindow(hwnd))
		{
		}

        public WindowTreeNode(IntPtr hwnd, bool isManaged)
        {
            this._isManaged = isManaged;
            this.hwnd = hwnd;
            string className = this.WindowClassName;
            this.Text = String.Format("Window {0:X8} \"{1}\" {2}", hwnd.ToInt32(), WindowText, className);

            //this.SetImageIndex(isManaged);
        }

        private void SetImageIndex(bool isManaged)
        {
            if (isManaged)
            {
                if (NativeMethods.IsWindowVisible(hwnd))
                    this.ImageIndex = (int)ImageIndices.ManagedWindow;
                else
                    this.ImageIndex = (int)ImageIndices.ManagedWindowHidden;
            }
            else
            {
                if (NativeMethods.IsWindowVisible(hwnd))
                    this.ImageIndex = (int)ImageIndices.Window;
                else
                    this.ImageIndex = (int)ImageIndices.WindowHidden;
            }

            this.SelectedImageIndex = this.ImageIndex;
        }
		
		public bool IsManaged
		{
			get
			{
                return this._isManaged;
                //return (this.ImageIndex == (int)ImageIndices.ManagedWindow) || 
                //    (this.ImageIndex == (int)ImageIndices.ManagedWindowHidden);
			}
		}

		public IntPtr Hwnd
		{
			get
			{
				return hwnd;
			}
		}

		public string WindowClassName
		{
			get
			{
				return NativeMethods.GetClassName(hwnd);
			}
		}
	
		public string WindowText
		{
			get
			{
				return NativeMethods.GetWindowText(hwnd);
			}
		}
	}
}

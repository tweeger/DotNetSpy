using System;
using System.IO;
using System.Drawing;
using System.Threading;
using System.Collections.Generic;
using System.Windows.Forms;
using DotNetSpy.Properties;

namespace DotNetSpy
{
    public partial class DotNetSpyForm : Form
    {
        #region Const Fields
        private const int WM_HOTKEY = 0x0312;

        private const uint MOD_CONTROL = 0x0002;
        private const uint MOD_ALT = 0x0001;
        private const uint MOD_SHIFT = 0x0004;
        private const uint MOD_WIN = 0x0008;

        private const int DrawFramePenWidth = 3;
        private static Pen DrawFramePen = new Pen(SystemBrushes.HotTrack, DrawFramePenWidth);
        #endregion

        #region Constructor & Disposer
        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetSpy"/> class.
        /// </summary>
        public DotNetSpyForm()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Form.Load"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.tsbtFindWindow.Image = this.NormalBackground;
            this.AttachEventHandlers();
        }
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                if (this.isHotKeyRegistered)
                {
                    NativeMethods.UnregisterHotKey(this.GetHashCode().ToString(), this.Handle);
                    this.isHotKeyRegistered = false;
                }

                if (this._bmpNormal != null)
                {
                    this._bmpNormal.Dispose();
                    this._bmpNormal = null;
                }
                if (this._bmpMouseDownBackground != null)
                {
                    this._bmpMouseDownBackground.Dispose();
                    this._bmpMouseDownBackground = null;
                }

                if (this._aboutForm != null)
                {
                    this._aboutForm.Dispose();
                    this._aboutForm = null;
                }
                this.DetachEventHandlers();
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion

        #region Properties
        private Panel PropertiesViewContainer
        {
            get
            {
                return this.splitContainer.Panel2;
            }
        }
        private AboutForm _aboutForm = null;
        private AboutForm AboutForm
        {
            get
            {
                if (this._aboutForm == null)
                {
                    this._aboutForm = new AboutForm();
                    this._aboutForm.Owner = this.Owner;
                }
                return this._aboutForm;
            }
        }

        private ActiveWindowCatcher _catcher = null;
        private ActiveWindowCatcher Catcher
        {
            get
            {
                if (_catcher == null)
                {
                    _catcher = new ActiveWindowCatcher();
                }
                return _catcher;
            }
        }

        private IntPtr _tagWindow = IntPtr.Zero;
        private IntPtr TagWindowHandle
        {
            get
            {
                return this._tagWindow;
            }
            set
            {
                if (value != this._tagWindow)
                {
                    this._tagWindow = value;
                    this.OnTagWindowHandleChanged();
                }
            }
        }

        private Bitmap _bmpNormal = null;
        private Bitmap NormalBackground
        {
            get
            {
                if (this._bmpNormal == null)
                {
                    this._bmpNormal = Bitmap.FromHicon(Resources.FinderImage.Handle);
                }
                return this._bmpNormal;
            }
        }
        private Bitmap _bmpMouseDownBackground = null;
        private Bitmap MouseDownBackground
        {
            get
            {
                if (this._bmpMouseDownBackground == null)
                {
                    this._bmpMouseDownBackground = Bitmap.FromHicon(Resources.FinderMouseDownImage.Handle);
                }
                return this._bmpMouseDownBackground;
            }
        }

        private Cursor _curMouseDown = null;
        private Cursor MouseDownCursor
        {
            get
            {
                if (this._curMouseDown == null)
                {
                    this._curMouseDown = new Cursor(new MemoryStream(Resources.finder));
                }
                return this._curMouseDown;
            }
        }
        #endregion

        #region Event Handlers

        private void AttachEventHandlers()
        { 
            this.tsmiWindow.Click += new EventHandler(this.ToolStripItemEventHandlerDispatcher);
            //this.tsbtWindow.Click += new EventHandler(this.ToolStripItemEventHandlerDispatcher);
            this.tsmiActiveWindowCatcher.Click += new EventHandler(this.ToolStripItemEventHandlerDispatcher);
            this.tsbDotNetOnly.Click += new EventHandler(this.ToolStripItemEventHandlerDispatcher);
            this.tsmiAlwaysOnTop.Click += new EventHandler(this.ToolStripItemEventHandlerDispatcher);
            this.tsmiAboutSpyDotNet.Click += new EventHandler(this.ToolStripItemEventHandlerDispatcher);
            this.tvWindowTree.AfterSelect += new TreeViewEventHandler(this.OnWindowTreeAfterSelect);
            this.splitContainer.Panel2.SizeChanged += new EventHandler(OnPropertyPanelSizeChanged);

            this.tsbtFindWindow.MouseDown += new MouseEventHandler(this.OnFinderMouseDown);
            this.toolStrip.MouseUp += new MouseEventHandler(this.OnFinderMouseUp);
            this.toolStrip.MouseMove += new MouseEventHandler(this.OnFinderMouseMove);
            this.toolStrip.MouseCaptureChanged += new EventHandler(this.OnFinderMouseCaptureChanged);
        }
        private void DetachEventHandlers()
        {
            this.tsmiWindow.Click -= new EventHandler(this.ToolStripItemEventHandlerDispatcher);
            //this.tsbtWindow.Click -= new EventHandler(this.ToolStripItemEventHandlerDispatcher);
            this.tsmiActiveWindowCatcher.Click -= new EventHandler(this.ToolStripItemEventHandlerDispatcher);
            this.tsbDotNetOnly.Click -= new EventHandler(this.ToolStripItemEventHandlerDispatcher);
            this.tsmiAlwaysOnTop.Click -= new EventHandler(this.ToolStripItemEventHandlerDispatcher);
            this.tsmiAboutSpyDotNet.Click -= new EventHandler(this.ToolStripItemEventHandlerDispatcher);
            this.tvWindowTree.AfterSelect -= new TreeViewEventHandler(this.OnWindowTreeAfterSelect);
            this.splitContainer.Panel2.SizeChanged -= new EventHandler(OnPropertyPanelSizeChanged);

            this.tsbtFindWindow.MouseDown -= new MouseEventHandler(this.OnFinderMouseDown);
            this.toolStrip.MouseUp -= new MouseEventHandler(this.OnFinderMouseUp);
            this.toolStrip.MouseMove -= new MouseEventHandler(this.OnFinderMouseMove);
            this.toolStrip.MouseCaptureChanged -= new EventHandler(this.OnFinderMouseCaptureChanged);
        }
        private void ToolStripItemEventHandlerDispatcher(object sender, EventArgs e)
        {
            string name = string.Empty;
            if (sender is ToolStripItem) name = (sender as ToolStripItem).Name;

            switch (name)
            {
                case "tsmiWindow":
                case "tsbtWindow":
                    if (this.TagWindowHandle != NativeMethods.GetDesktopWindow())
                    {
                        this.TagWindowHandle = NativeMethods.GetDesktopWindow();
                    }
                    else
                    {
                        this.FillWindowTree(NativeMethods.GetDesktopWindow());
                    }
                    break;
                case "tsmiActiveWindowCatcher":
                    this.OptionActiveWindowCatcher();
                    break;
                case "tsbDotNetOnly":
                    //this.FillWindowTree(this.TagWindowHandle);
                    this.FillWindowTree(NativeMethods.GetDesktopWindow());
                    break;
                case "tsmiAlwaysOnTop":
                    this.TopMost = (sender as ToolStripMenuItem).Checked;
                    break;
                case "tsmiAboutSpyDotNet":
                    this.AboutForm.ShowDialog();
                    break;
                default:
                    break;
            }
        }

        private Cursor oldCursor = null;
        private void OnFinderMouseCaptureChanged(object sender, EventArgs e)
        {
            if (!this.toolStrip.Capture)
            {
                this.RedrawTargetWindow(this.TagWindowHandle);
                this.tsbtFindWindow.Image = this.NormalBackground;
                this.Cursor = this.oldCursor;
            }
        }
        private void OnFinderMouseDown(object sender, MouseEventArgs e)
        {
            this.toolStrip.Capture = true;
            this.oldCursor = this.Cursor;
            this.tsbtFindWindow.Image = this.MouseDownBackground;
            this.Cursor = this.MouseDownCursor;
        }
        private void OnFinderMouseUp(object sender, MouseEventArgs e)
        {
            this.TagWindowHandle = this.hWindow;
            this.RedrawTargetWindow(this.TagWindowHandle);
            this.tsbtFindWindow.Image = this.NormalBackground;
            this.Cursor = this.oldCursor;
        }
        private IntPtr hWindow = IntPtr.Zero;
        private void OnFinderMouseMove(object sender, MouseEventArgs e)
        {
            ToolStrip control = sender as ToolStrip;
            if (control == null || !control.Capture) return;

            // Grab the window from the screen location of the mouse.
            //Point location = e.Location;
            //location.Offset(-control.Location.X, -control.Location.Y);
            //POINT windowPoint = POINT.FromPoint(this.PointToScreen(new Point(e.X, e.Y)));
            POINT windowPoint = POINT.FromPoint(control.PointToScreen(e.Location));
            IntPtr found = NativeMethods.WindowFromPoint(windowPoint);

            // we have a valid window handle
            if (found != IntPtr.Zero && NativeMethods.IsTargetInDifferentProcess(found))
            {
                // give it another try, it might be a child window (disabled, hidden .. something else)
                // offset the point to be a client point of the active window
                if (NativeMethods.ScreenToClient(found, ref windowPoint))
                {
                    // check if there is some hidden/disabled child window at this point
                    IntPtr childWindow = NativeMethods.ChildWindowFromPoint(found, windowPoint);
                    if (childWindow != IntPtr.Zero)
                    { // great, we have the inner child
                        found = childWindow;
                    }
                }

                if (this.hWindow != found)
                {
                    this.RedrawTargetWindow(this.hWindow);
                    this.hWindow = found;
                    this.RedrawTargetWindow(this.hWindow);
                    this.HighlightTargetWindow(this.hWindow);
                }
            }
        }

        /// <summary>
        /// Processes Windows messages.
        /// </summary>
        /// <param name="m">The Windows <see cref="T:System.Windows.Forms.Message"/> to process.</param>
        protected override void WndProc(ref Message msg)
        {
            switch (msg.Msg)
            {
                case WM_HOTKEY:
                    if ((uint)msg.WParam == this.idHotKey)
                    {
                        this.ProcessActiveWindowCatcher();
                    }
                    break;
                default:
                    base.WndProc(ref msg);
                    break;
            }
        }

        #endregion

        #region Help Methods

        private void OptionActiveWindowCatcher()
        {
            if (this.Catcher.ShowDialog() == DialogResult.OK)
            {
                this.OnHotKeyChanged(this.Catcher.HotKey, this.Catcher.Modifiers);
            }
        }
        private void ProcessActiveWindowCatcher()
        {
            IntPtr hWndTarget = NativeMethods.GetWindowFocusControl();
            if (IntPtr.Zero != hWndTarget && NativeMethods.IsTargetInDifferentProcess(hWndTarget))
            {
                this.TagWindowHandle = hWndTarget;
            }
        }

        private ushort idHotKey = 0;
        private bool isHotKeyRegistered = false;
        /// <summary>
        /// Called when [hot key changed].
        /// </summary>
        private void OnHotKeyChanged(uint hotKey, uint modifiers)
        {
            if (this.isHotKeyRegistered)
            {
                NativeMethods.UnregisterHotKey(this.Handle);
                this.isHotKeyRegistered = false;
            }
            this.idHotKey = NativeMethods.RegisterHotKey(this.Handle, modifiers, hotKey);
            this.isHotKeyRegistered = (this.idHotKey > 0);
            if (this.isHotKeyRegistered)
            {
                MessageBox.Show("Hot key registered!");
            }
        }

        private void OnTagWindowHandleChanged()
        {
            this.FillWindowTree(this.TagWindowHandle);
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Normal;
            }
        }

        private void OnPropertyPanelSizeChanged(object sender, EventArgs e)
        {
            IntPtr hwnd = NativeMethods.GetWindow(this.PropertiesViewContainer.Handle, (int)GetWindowCmd.GW_CHILD);
            if (IntPtr.Zero != hwnd)
            {
                NativeMethods.MoveWindow(hwnd, 0, 0, this.PropertiesViewContainer.Width, this.PropertiesViewContainer.Height, true);
            }
            this.PropertiesViewContainer.Refresh();
        }
        private void OnWindowTreeAfterSelect(object sender, TreeViewEventArgs e)
        {
            IntPtr hwnd = NativeMethods.GetWindow(this.PropertiesViewContainer.Handle, (int)GetWindowCmd.GW_CHILD);
            if (hwnd != IntPtr.Zero)
            {
                NativeMethods.SendMessage(hwnd, NativeMethods.WM_CLOSEVIEW, IntPtr.Zero, IntPtr.Zero);
            }

            if (e.Node.Tag != null)
            {
                PropertyGridEx grid = new PropertyGridEx();
                grid.Dock = DockStyle.Fill;
                this.PropertiesViewContainer.Controls.Clear();
                this.PropertiesViewContainer.Controls.Add(grid);

                grid.SelectedObject = e.Node.Tag;
            }
            else
            {
                SypHookHelper.Hook((e.Node as WindowTreeNode).Hwnd, this.PropertiesViewContainer.Handle);
            }
        }

        private void RedrawTargetWindow(IntPtr hWndTarget)
        {
            if (hWndTarget == IntPtr.Zero) return;

            NativeMethods.InvalidateRect(hWndTarget, IntPtr.Zero, true);
            NativeMethods.UpdateWindow(hWndTarget);
            NativeMethods.RedrawWindow(hWndTarget, IntPtr.Zero, IntPtr.Zero,
                NativeMethods.RDW_FRAME | NativeMethods.RDW_INVALIDATE |
                NativeMethods.RDW_UPDATENOW | NativeMethods.RDW_ERASENOW |
                NativeMethods.RDW_ALLCHILDREN);
        }
        private void HighlightTargetWindow(IntPtr hWndTarget)
        {
            IntPtr hwndDC = NativeMethods.GetWindowDC(hWndTarget);
            if (hwndDC != IntPtr.Zero)
            {
                using (Graphics graphics = Graphics.FromHdc(hwndDC, hWndTarget))
                {
                    Rectangle rectClient = NativeMethods.GetClientRect(hWndTarget);

                    long style = NativeMethods.GetWindowLong(hWndTarget, WindowLongFlags.GWL_STYLE);
                    if ((style & (uint)WindowStyles.WS_CHILD) != 0)
                    {
                        rectClient.Offset(1, 1);
                        rectClient.Width -= DrawFramePenWidth;
                        rectClient.Height -= DrawFramePenWidth;
                    }
                    else
                    {
                        Rectangle rectWnd = NativeMethods.GetWindowRect(hWndTarget);
                        Point ptClient = NativeMethods.MapPointToScreen(hWndTarget, Point.Empty);
                        rectClient.Offset(ptClient.X - rectWnd.X, ptClient.Y - rectWnd.Y);
                        rectClient.Inflate(-1, -1);
                    }
                    graphics.DrawRectangle(DotNetSpyForm.DrawFramePen, rectClient);
                }
                NativeMethods.ReleaseDC(hWndTarget, hwndDC);
            }
        }

        private ImageList _imageList = null;
        private ImageList WindowTreeImageList
        {
            get
            {
                if (this._imageList == null)
                {
                    this._imageList = new ImageList();
                    this._imageList.ColorDepth = ColorDepth.Depth32Bit;
                    this._imageList.TransparentColor = SystemColors.WindowText;
                }
                return this._imageList;
            }
        }
        private void FillWindowTree(IntPtr hWndTarget)
        {
            this.tvWindowTree.Nodes.Clear();
            this.tvWindowTree.ImageList = null;
            this.WindowTreeImageList.Images.Clear();

            Thread thread = new Thread(new ParameterizedThreadStart(this.UpdateWindowTreeProc));
            thread.Start(hWndTarget);
        }

        private void UpdateWindowTreeProc(object param)
        {
            IntPtr tagWindow = (IntPtr)param;
            if (tagWindow != IntPtr.Zero)
            {
                this.UpdateControlState(false);

                WindowTreeBuilder builder = new WindowTreeBuilder(tagWindow);
                builder.BuildAllWindowsTree();

                this.SetWindowTreeNodeImage(this.WindowTreeImageList, builder.RootNode);
                this.UpdateWindowTree(builder);

                this.UpdateControlState(true);
            }
        }

        private delegate void UpdateWindowTreeInvoker(WindowTreeBuilder builder);
        private void UpdateWindowTree(WindowTreeBuilder builder)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new UpdateWindowTreeInvoker(this.UpdateWindowTree), new object[] { builder });
            }
            else
            {
                if (this.IsHandleCreated)
                    builder.RemoveWindow(this.Handle);

                //if (this.tsbDotNetOnly.Checked) builder.FilterUnmanagedWindows();
                builder.FilterUnmanagedWindows();

                this.tvWindowTree.BeginUpdate();
                this.tvWindowTree.ImageList = this.WindowTreeImageList;
                this.tvWindowTree.Nodes.Add(builder.RootNode);
                this.tvWindowTree.SelectedNode = builder.RootNode;
                this.tvWindowTree.EndUpdate();
            }
        }

        private delegate void UpdateControlStateInvoker(bool enabled);
        private void UpdateControlState(bool enabled)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new UpdateControlStateInvoker(this.UpdateControlState), new object[] { enabled });
            }
            else
            {
                this.menuStrip.Enabled = enabled;
                this.toolStrip.Enabled = enabled;
                this.splitContainer.Enabled = enabled;
            }
        }

        private void SetWindowTreeNodeImage(ImageList imageList, WindowTreeNode node)
        {
            this.SetNodeImage(imageList, node);
            if (node.Nodes.Count > 0)
            {
                foreach (WindowTreeNode subNode in node.Nodes)
                {
                    this.SetWindowTreeNodeImage(imageList, subNode);
                }
            }
        }
        private void SetNodeImage(ImageList imageList, WindowTreeNode node)
        {
            IntPtr hIcon = NativeMethods.GetClassLongPtr(node.Hwnd, NativeMethods.GCLP_HICON);
            if (IntPtr.Zero == hIcon)
            {
                if (node.IsManaged)
                {
                    imageList.Images.Add(node.Hwnd.ToString(), Resources.DotNetWindow);
                }
                else
                {
                    imageList.Images.Add(node.Hwnd.ToString(), Resources.Window);
                }
            }
            else
            {
                Icon icon = Icon.FromHandle(hIcon);
                if (null == icon)
                {
                    hIcon = NativeMethods.SendMessage(
                        node.Hwnd, 
                        NativeMethods.WM_GETICON, 
                        new IntPtr(NativeMethods.ICON_BIG), 
                        IntPtr.Zero);
                }
                if (null != icon)
                {
                    imageList.Images.Add(node.Hwnd.ToString(), icon);
                }
            }
            node.ImageKey = node.Hwnd.ToString();
            node.SelectedImageKey = node.Hwnd.ToString();
            node.StateImageKey = node.Hwnd.ToString();
        }

        #endregion
    }
}

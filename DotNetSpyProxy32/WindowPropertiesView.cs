using System;
using System.Windows.Forms;
using System.Collections;
using System.IO;

namespace DotNetSpyProxy32
{
	/// <summary>
	/// Summary description for WindowsPropertyView.
	/// </summary>
    public class WindowPropertiesView : System.Windows.Forms.UserControl
    {
        private System.ComponentModel.IContainer components;
        private TabControl tabControl;
        private TabPage tpgProperty;
        private TabPage tpgEvent;
        private ImageList il;
        private SplitContainer splitContainer3;
        private TreeView tvwEvent;
        private SplitContainer splitContainer4;
        private TreeView tvwFiredEvent;
        private PropertyGrid ppgEventArgs;
        private PropertyGridEx propertyGrid;

        private Control ctlCurrent = null;

        public WindowPropertiesView()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.RemoveEventsInfo();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WindowPropertiesView));
            this.propertyGrid = new DotNetSpyProxy32.PropertyGridEx();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tpgProperty = new System.Windows.Forms.TabPage();
            this.tpgEvent = new System.Windows.Forms.TabPage();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.tvwEvent = new System.Windows.Forms.TreeView();
            this.il = new System.Windows.Forms.ImageList(this.components);
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.tvwFiredEvent = new System.Windows.Forms.TreeView();
            this.ppgEventArgs = new System.Windows.Forms.PropertyGrid();
            this.tabControl.SuspendLayout();
            this.tpgProperty.SuspendLayout();
            this.tpgEvent.SuspendLayout();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            this.SuspendLayout();
            // 
            // propertyGrid
            // 
            this.propertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid.LineColor = System.Drawing.SystemColors.ScrollBar;
            this.propertyGrid.Location = new System.Drawing.Point(3, 3);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.PropertySort = System.Windows.Forms.PropertySort.Alphabetical;
            this.propertyGrid.Size = new System.Drawing.Size(266, 351);
            this.propertyGrid.TabIndex = 0;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tpgProperty);
            this.tabControl.Controls.Add(this.tpgEvent);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.HotTrack = true;
            this.tabControl.ImageList = this.il;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(280, 384);
            this.tabControl.TabIndex = 1;
            // 
            // tpgProperty
            // 
            this.tpgProperty.Controls.Add(this.propertyGrid);
            this.tpgProperty.ImageIndex = 0;
            this.tpgProperty.Location = new System.Drawing.Point(4, 23);
            this.tpgProperty.Name = "tpgProperty";
            this.tpgProperty.Padding = new System.Windows.Forms.Padding(3);
            this.tpgProperty.Size = new System.Drawing.Size(272, 357);
            this.tpgProperty.TabIndex = 0;
            this.tpgProperty.Text = "Properties";
            this.tpgProperty.UseVisualStyleBackColor = true;
            // 
            // tpgEvent
            // 
            this.tpgEvent.Controls.Add(this.splitContainer3);
            this.tpgEvent.ImageIndex = 2;
            this.tpgEvent.Location = new System.Drawing.Point(4, 23);
            this.tpgEvent.Name = "tpgEvent";
            this.tpgEvent.Padding = new System.Windows.Forms.Padding(3);
            this.tpgEvent.Size = new System.Drawing.Size(272, 357);
            this.tpgEvent.TabIndex = 1;
            this.tpgEvent.Text = "Events";
            this.tpgEvent.UseVisualStyleBackColor = true;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(3, 3);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.tvwEvent);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.splitContainer4);
            this.splitContainer3.Size = new System.Drawing.Size(266, 351);
            this.splitContainer3.SplitterDistance = 144;
            this.splitContainer3.TabIndex = 12;
            this.splitContainer3.Text = "splitContainer3";
            // 
            // tvwEvent
            // 
            this.tvwEvent.CheckBoxes = true;
            this.tvwEvent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvwEvent.ImageIndex = 2;
            this.tvwEvent.ImageList = this.il;
            this.tvwEvent.Location = new System.Drawing.Point(0, 0);
            this.tvwEvent.Name = "tvwEvent";
            this.tvwEvent.SelectedImageIndex = 2;
            this.tvwEvent.Size = new System.Drawing.Size(144, 351);
            this.tvwEvent.TabIndex = 1;
            this.tvwEvent.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.OnTvwEventAfterCheck);
            // 
            // il
            // 
            this.il.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("il.ImageStream")));
            this.il.TransparentColor = System.Drawing.Color.Transparent;
            this.il.Images.SetKeyName(0, "");
            this.il.Images.SetKeyName(1, "");
            this.il.Images.SetKeyName(2, "");
            this.il.Images.SetKeyName(3, "");
            this.il.Images.SetKeyName(4, "");
            this.il.Images.SetKeyName(5, "LegendHS.png");
            this.il.Images.SetKeyName(6, "DisplayInColorHS.png");
            this.il.Images.SetKeyName(7, "HtmlBalanceBracesHS.png");
            // 
            // splitContainer4
            // 
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.Location = new System.Drawing.Point(0, 0);
            this.splitContainer4.Name = "splitContainer4";
            this.splitContainer4.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.tvwFiredEvent);
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add(this.ppgEventArgs);
            this.splitContainer4.Size = new System.Drawing.Size(118, 351);
            this.splitContainer4.SplitterDistance = 173;
            this.splitContainer4.TabIndex = 0;
            this.splitContainer4.Text = "splitContainer4";
            // 
            // tvwFiredEvent
            // 
            this.tvwFiredEvent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvwFiredEvent.ImageIndex = 0;
            this.tvwFiredEvent.ImageList = this.il;
            this.tvwFiredEvent.Location = new System.Drawing.Point(0, 0);
            this.tvwFiredEvent.Name = "tvwFiredEvent";
            this.tvwFiredEvent.SelectedImageIndex = 0;
            this.tvwFiredEvent.Size = new System.Drawing.Size(118, 173);
            this.tvwFiredEvent.TabIndex = 2;
            this.tvwFiredEvent.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.OnTvwFiredEventAfterSelect);
            this.tvwFiredEvent.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnTvwFiredEventKeyDown);
            // 
            // ppgEventArgs
            // 
            this.ppgEventArgs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ppgEventArgs.HelpVisible = false;
            this.ppgEventArgs.LineColor = System.Drawing.SystemColors.ScrollBar;
            this.ppgEventArgs.Location = new System.Drawing.Point(0, 0);
            this.ppgEventArgs.Name = "ppgEventArgs";
            this.ppgEventArgs.PropertySort = System.Windows.Forms.PropertySort.Alphabetical;
            this.ppgEventArgs.Size = new System.Drawing.Size(118, 174);
            this.ppgEventArgs.TabIndex = 3;
            this.ppgEventArgs.ToolbarVisible = false;
            // 
            // WindowPropertiesView
            // 
            this.Controls.Add(this.tabControl);
            this.Name = "WindowPropertiesView";
            this.Size = new System.Drawing.Size(280, 384);
            this.tabControl.ResumeLayout(false);
            this.tpgProperty.ResumeLayout(false);
            this.tpgEvent.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            this.splitContainer3.ResumeLayout(false);
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel2.ResumeLayout(false);
            this.splitContainer4.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion
                
        protected override void WndProc(ref Message msg)
        {
            if (msg.Msg == NativeMethods.WM_CLOSEVIEW)
            {
                this.Visible = false;
                this.Dispose();
            }
            base.WndProc(ref msg);
        }

        #region IHookInstall Members

        public void OnInstallHook(byte[] data)
        {
            this.CreateInstance((IntPtr)BitConverter.ToInt32(data, 0));
            this.OnSelectedWindowHandleChanged((IntPtr)BitConverter.ToInt32(data, 4));
        }

        #endregion

        private void CreateInstance(IntPtr hWndParent)
        {
            try
            {
                if (this.Handle == IntPtr.Zero)
                {
                    this.CreateControl();
                }

                NativeMethods.SetParent(this.Handle, hWndParent);

                RECT rc = new RECT();
                NativeMethods.GetClientRect(hWndParent, ref rc);
                NativeMethods.MoveWindow(this.Handle, 0, 0, rc.Width, rc.Height, true);
            }
            catch { }
        }

        private void OnSelectedWindowHandleChanged(IntPtr hWndTarget)
        {
            try
            {
                Control control = Control.FromHandle(hWndTarget);

                if (control != null)
                {
                    this.propertyGrid.SelectedObject = new WindowProperties(control);
                    this.AddEventsInfo(control);
                    this.ctlCurrent = control;
                }
            }
            catch { }
        }

        private void OnTvwEventAfterCheck(object sender, TreeViewEventArgs e)
        {
            this.tvwEvent.AfterCheck -= new TreeViewEventHandler(OnTvwEventAfterCheck);

            if (e.Node.Parent != null)
            {
                if (e.Node.Checked)
                {
                    Delegate d = RegisterEvent(this.ctlCurrent, e.Node.Text);
                    e.Node.Tag = d;
                }
                else
                {
                    UnRegisterEvent(this.ctlCurrent, e.Node.Text, (Delegate)e.Node.Tag);
                }
            }

            foreach (TreeNode node in e.Node.Nodes)
            {
                this.SetTreeNodeChecked(node, e.Node.Checked);
            }
            this.SetParentTreeNodeChecked(e.Node.Parent);

            this.tvwEvent.AfterCheck += new TreeViewEventHandler(OnTvwEventAfterCheck);
        }

        private void OnTvwFiredEventAfterSelect(object sender, TreeViewEventArgs e)
        {
            ppgEventArgs.SelectedObject = tvwFiredEvent.SelectedNode.Tag;
        }

        private void OnTvwFiredEventKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && e.Control == true)
            {
                tvwFiredEvent.Nodes.Clear();
            }
            else if (e.KeyCode == Keys.Delete && tvwFiredEvent.SelectedNode != null)
            {
                tvwFiredEvent.Nodes.Remove(tvwFiredEvent.SelectedNode);
            }
        }

        #region Help Methods

        private void AddEventsInfo(Control ctl)
        {
            TreeNode tn = tvwEvent.Nodes.Add(ctl.Name);
            Type type = ctl.GetType();
            System.Reflection.EventInfo[] eis = type.GetEvents();
            Array.Sort(eis, new EventComparer());
            foreach (System.Reflection.EventInfo ei in eis)
            {
                tn.Nodes.Add(ei.Name);
            }
        }
        private void RemoveEventsInfo()
        {
            if (tvwEvent.Nodes.Count > 0)
            {
                tvwEvent.Nodes[0].Checked = false;
            }
        }

        private void SetTreeNodeChecked(TreeNode node, bool isCheck)
        {
            if (node.Checked != isCheck)
            {
                node.Checked = isCheck;
                if (isCheck)
                {
                    Delegate d = RegisterEvent(this.ctlCurrent, node.Text);
                    node.Tag = d;
                }
                else
                {
                    UnRegisterEvent(this.ctlCurrent, node.Text, (Delegate)node.Tag);
                }
            }

            foreach (TreeNode child in node.Nodes)
            {
                SetTreeNodeChecked(child, isCheck);
            }
        }
        private void SetParentTreeNodeChecked(TreeNode parent)
        {
            if (parent != null)
            {
                bool isCheck = true;
                foreach (TreeNode child in parent.Nodes)
                {
                    isCheck &= child.Checked;
                }

                parent.Checked = isCheck;
            }
        }

        private System.Reflection.Emit.ModuleBuilder GetModuleBuilder()
        {
            System.Reflection.AssemblyName an = new System.Reflection.AssemblyName();
            an.Name = "TempAssembly";
            System.AppDomain ad = System.AppDomain.CurrentDomain;
            System.Reflection.Assembly[] assemblys = ad.GetAssemblies();
            System.Reflection.Emit.ModuleBuilder mb;
            foreach (System.Reflection.Assembly a in assemblys)
            {
                if (a.GetType() == Type.GetType("System.Reflection.Emit.AssemblyBuilder") && a.GetName().Name == "TempAssembly")
                {
                    a.GetModules();
                    mb = (System.Reflection.Emit.ModuleBuilder)a.GetModule("TempModule");
                    return mb;
                }
            }
            return ad.DefineDynamicAssembly(an, System.Reflection.Emit.AssemblyBuilderAccess.Run).DefineDynamicModule("TempModule");
        }

        private void UnRegisterEvent(Control TargetControl, string EventName, Delegate d)
        {
            Type type = TargetControl.GetType();
            System.Reflection.EventInfo ei = type.GetEvent(EventName);
            ei.RemoveEventHandler(TargetControl, d);
        }

        private Delegate RegisterEvent(Control TargetControl, string EventName)
        {
            Type type = TargetControl.GetType();
            System.Reflection.EventInfo ei = type.GetEvent(EventName);
            System.Reflection.Emit.ModuleBuilder mb = GetModuleBuilder();

            System.Reflection.ParameterInfo[] pis = ei.EventHandlerType.GetMethod("Invoke").GetParameters();
            Type[] ts = new Type[pis.Length];
            int eventArgsPos = -1;
            for (int i = 0; i < pis.Length; i++)
            {
                ts[i] = pis[i].ParameterType;
                if (Type.GetType("System.EventArgs").IsAssignableFrom(ts[i]))
                {
                    eventArgsPos = i;
                }
            }
            string typeName = "handle" + TargetControl.Name + "_" + ei.Name;
            type = mb.GetType(typeName);
            if (type == null)
            {
                System.Reflection.Emit.TypeBuilder tb = mb.DefineType(typeName, System.Reflection.TypeAttributes.Public);
                System.Reflection.Emit.FieldBuilder fb = tb.DefineField("TempField", this.GetType(), System.Reflection.FieldAttributes.Static | System.Reflection.FieldAttributes.Public);
                System.Reflection.Emit.MethodBuilder mmb = tb.DefineMethod("TempMethod", System.Reflection.MethodAttributes.Static | System.Reflection.MethodAttributes.Public, Type.GetType("System.Void"), ts);
                System.Reflection.Emit.ILGenerator ilg = mmb.GetILGenerator();
                ilg.Emit(System.Reflection.Emit.OpCodes.Ldsfld, fb);
                ilg.Emit(System.Reflection.Emit.OpCodes.Ldstr, TargetControl.Name);
                ilg.Emit(System.Reflection.Emit.OpCodes.Ldstr, ei.Name);
                if (eventArgsPos == -1)
                    ilg.Emit(System.Reflection.Emit.OpCodes.Ldnull);
                else
                    ilg.Emit(System.Reflection.Emit.OpCodes.Ldarg_S, eventArgsPos);
                ilg.Emit(System.Reflection.Emit.OpCodes.Call, this.GetType().GetMethod("TempEvent"));
                ilg.Emit(System.Reflection.Emit.OpCodes.Ret);
                type = tb.CreateType();
            }
            System.Reflection.FieldInfo fi = type.GetField("TempField");
            fi.SetValue(null, this);
            System.Reflection.MethodInfo mi = type.GetMethod("TempMethod");
            Delegate d = Delegate.CreateDelegate(ei.EventHandlerType, mi);
            ei.AddEventHandler(TargetControl, d);
            return d;
        }

        public void TempEvent(string strTargetName, string strEventName, System.EventArgs eventArgs)
        {
            try
            {
                TreeNode tnControl;
                TreeNode[] tns = tvwFiredEvent.Nodes.Find(strTargetName, false);
                if (tns.Length == 0)
                {
                    tnControl = tvwFiredEvent.Nodes.Add(strTargetName, strTargetName, 2, 2);
                }
                else
                {
                    tnControl = tns[0];
                }

                TreeNode tnEvent = new TreeNode(strEventName, 2, 2);
                tnEvent.Tag = eventArgs;

                tnControl.Nodes.Add(tnEvent);
                tvwFiredEvent.CollapseAll();

                tvwFiredEvent.SelectedNode = tnEvent;
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        #endregion

        #region Compare classes

        internal class EventComparer : IComparer
        {
            public int Compare(object o1, object o2)
            {
                if (o1 is System.Reflection.EventInfo && o2 is System.Reflection.EventInfo)
                {
                    System.Reflection.EventInfo e1 = (System.Reflection.EventInfo)o1;
                    System.Reflection.EventInfo e2 = (System.Reflection.EventInfo)o2;

                    return e1.Name.CompareTo(e2.Name);
                }
                return 0;
            }
        }

        internal class MethodComparer : IComparer
        {
            public int Compare(object o1, object o2)
            {
                if (o1 is System.Reflection.MethodInfo && o2 is System.Reflection.MethodInfo)
                {
                    System.Reflection.MethodInfo m1 = (System.Reflection.MethodInfo)o1;
                    System.Reflection.MethodInfo m2 = (System.Reflection.MethodInfo)o2;

                    return m1.Name.CompareTo(m2.Name);
                }
                return 0;
            }
        }

        #endregion
    }
}

using System;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;

using DotNetSpy.Properties;

namespace DotNetSpy
{
    internal class PropertyGridEx : PropertyGrid
    {
        private ToolStripDropDownButton _tsDDButton;
        private ToolStrip _toolStrip;
        private ContextMenuStrip _contextMenuStrip = new ContextMenuStrip();

        public PropertyGridEx()
            : base()
        {

            foreach (Control ctl in this.Controls)
            {
                if (ctl.GetType() == typeof(ToolStrip))
                {
                    _toolStrip = ctl as ToolStrip;
                    InitializeImage(_toolStrip);
                    InitializeButtons(_toolStrip);
                    break;
                }
            }

            InitializeContextMenuStrip();
        }

        protected override void OnSelectedObjectsChanged(EventArgs e)
        {
            base.OnSelectedObjectsChanged(e);
            if (_toolStrip.ImageList.Images["AllProperties"] == null)
            {
                InitializeImage(_toolStrip);
            }
            if (_tsDDButton.Owner == null)
            {
                _toolStrip.Items.Add(new ToolStripSeparator());
                _toolStrip.Items.Add(_tsDDButton);
            }

            this.BrowsableAttributes = new AttributeCollection(new Attribute[] { });
        }

        private void InitializeContextMenuStrip()
        {
            this.ContextMenuStrip = _contextMenuStrip;

            ToolStripMenuItem resetMenu = new ToolStripMenuItem("Reset");
            resetMenu.Click += new EventHandler(delegate(object sender, EventArgs e) { this.ResetSelectedProperty(); });
            _contextMenuStrip.Items.Add(resetMenu);
            _contextMenuStrip.Opening += new CancelEventHandler(_contextMenuStrip_Opening);

        }

        void _contextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            _contextMenuStrip.Items[0].Enabled = CanPropertyReset();
        }

        private void InitializeImage(ToolStrip toolStrip)
        {
            toolStrip.ImageList.TransparentColor = Color.Magenta;
            toolStrip.ImageList.Images.Add("AllProperties", Resources.AllProperties);
            toolStrip.ImageList.Images.Add("HiddenProperties", Resources.HiddenProperties);
            toolStrip.ImageList.Images.Add("VisibleProperties", Resources.VisibleProperties);
        }

        private void InitializeButtons(ToolStrip toolStrip)
        {
            toolStrip.Items.Add(new ToolStripSeparator());
            _tsDDButton = new ToolStripDropDownButton("All Properties");
            _tsDDButton.ImageKey = "AllProperties";

            ToolStripMenuItem tsMenu;
            tsMenu = new ToolStripMenuItem("All Properties", toolStrip.ImageList.Images["AllProperties"]);
            tsMenu.Click += new EventHandler(DropDownItem_Click);
            tsMenu.Tag = new AttributeCollection(new Attribute[] { });
            _tsDDButton.DropDownItems.Add(tsMenu);

            tsMenu = new ToolStripMenuItem("Visible Properties", toolStrip.ImageList.Images["VisibleProperties"]);
            tsMenu.Click += new EventHandler(DropDownItem_Click);
            tsMenu.Tag = null;
            _tsDDButton.DropDownItems.Add(tsMenu);

            tsMenu = new ToolStripMenuItem("Hidden Properties", toolStrip.ImageList.Images["HiddenProperties"]);
            tsMenu.Click += new EventHandler(DropDownItem_Click);
            tsMenu.Tag = new AttributeCollection(new Attribute[] { new BrowsableAttribute(false) });
            _tsDDButton.DropDownItems.Add(tsMenu);

            toolStrip.Items.Add(_tsDDButton);
        }

        void DropDownItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tsMenu = (ToolStripMenuItem)sender;

            _tsDDButton.Text = tsMenu.Text;
            _tsDDButton.Image = tsMenu.Image;

            this.BrowsableAttributes = tsMenu.Tag as AttributeCollection;
        }

        private bool CanPropertyReset()
        {
            if (((SelectedObjects == null) || (SelectedObjects.Length == 0)) || (SelectedGridItem == null))
            {
                return false;
            }
            foreach (object selectedObject in SelectedObjects)
            {
                try
                {
                    GridItem selectedGridItem = SelectedGridItem;
                    if (selectedGridItem.Parent != null)
                    {
                        if (!selectedGridItem.PropertyDescriptor.CanResetValue(selectedGridItem.Parent.Value))
                        {
                            return false;
                        }
                    }
                    else if (!selectedGridItem.PropertyDescriptor.CanResetValue(selectedObject))
                    {
                        return false;
                    }
                }
                catch (InvalidCastException)
                {
                    return false;
                }
                catch (Exception)
                {
                }
            }
            return true;
        }
    }
}

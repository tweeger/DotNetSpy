namespace DotNetSpy
{
    partial class DotNetSpyForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DotNetSpyForm));
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.spyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiWindow = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiActiveWindowCatcher = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiAlwaysOnTop = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiAboutSpyDotNet = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.tsbtFindWindow = new System.Windows.Forms.ToolStripButton();
            this.tsbDotNetOnly = new System.Windows.Forms.ToolStripButton();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.tvWindowTree = new System.Windows.Forms.TreeView();
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.menuStrip.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.spyToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(501, 24);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip1";
            // 
            // spyToolStripMenuItem
            // 
            this.spyToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiWindow,
            this.toolStripSeparator1,
            this.tsmiActiveWindowCatcher});
            this.spyToolStripMenuItem.Name = "spyToolStripMenuItem";
            this.spyToolStripMenuItem.Size = new System.Drawing.Size(38, 20);
            this.spyToolStripMenuItem.Text = "Spy";
            // 
            // tsmiWindow
            // 
            this.tsmiWindow.Image = global::DotNetSpy.Properties.Resources.DotNetWindow;
            this.tsmiWindow.Name = "tsmiWindow";
            this.tsmiWindow.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W)));
            this.tsmiWindow.Size = new System.Drawing.Size(193, 22);
            this.tsmiWindow.Text = ".Net Windows";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(190, 6);
            // 
            // tsmiActiveWindowCatcher
            // 
            this.tsmiActiveWindowCatcher.Image = global::DotNetSpy.Properties.Resources.keybd;
            this.tsmiActiveWindowCatcher.Name = "tsmiActiveWindowCatcher";
            this.tsmiActiveWindowCatcher.Size = new System.Drawing.Size(193, 22);
            this.tsmiActiveWindowCatcher.Text = "Hot key...";
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiAlwaysOnTop});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // tsmiAlwaysOnTop
            // 
            this.tsmiAlwaysOnTop.CheckOnClick = true;
            this.tsmiAlwaysOnTop.Image = global::DotNetSpy.Properties.Resources.PushPin;
            this.tsmiAlwaysOnTop.Name = "tsmiAlwaysOnTop";
            this.tsmiAlwaysOnTop.Size = new System.Drawing.Size(154, 22);
            this.tsmiAlwaysOnTop.Text = "Always On Top";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiAboutSpyDotNet});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // tsmiAboutSpyDotNet
            // 
            this.tsmiAboutSpyDotNet.Image = ((System.Drawing.Image)(resources.GetObject("tsmiAboutSpyDotNet.Image")));
            this.tsmiAboutSpyDotNet.Name = "tsmiAboutSpyDotNet";
            this.tsmiAboutSpyDotNet.Size = new System.Drawing.Size(176, 22);
            this.tsmiAboutSpyDotNet.Text = "About DotNetSpy...";
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbtFindWindow,
            this.tsbDotNetOnly});
            this.toolStrip.Location = new System.Drawing.Point(0, 24);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(501, 25);
            this.toolStrip.TabIndex = 1;
            this.toolStrip.Text = "toolStrip1";
            // 
            // tsbtFindWindow
            // 
            this.tsbtFindWindow.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtFindWindow.Name = "tsbtFindWindow";
            this.tsbtFindWindow.Size = new System.Drawing.Size(23, 22);
            this.tsbtFindWindow.Text = "Find window";
            // 
            // tsbDotNetOnly
            // 
            this.tsbDotNetOnly.Image = global::DotNetSpy.Properties.Resources.DotNetWindow;
            this.tsbDotNetOnly.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbDotNetOnly.Name = "tsbDotNetOnly";
            this.tsbDotNetOnly.Size = new System.Drawing.Size(23, 22);
            this.tsbDotNetOnly.ToolTipText = ".Net Windows";
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer.Location = new System.Drawing.Point(0, 49);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.tvWindowTree);
            this.splitContainer.Size = new System.Drawing.Size(501, 372);
            this.splitContainer.SplitterDistance = 173;
            this.splitContainer.TabIndex = 2;
            // 
            // tvWindowTree
            // 
            this.tvWindowTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvWindowTree.Location = new System.Drawing.Point(0, 0);
            this.tvWindowTree.Name = "tvWindowTree";
            this.tvWindowTree.Size = new System.Drawing.Size(173, 372);
            this.tvWindowTree.TabIndex = 0;
            // 
            // DotNetSpyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(501, 421);
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.menuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip;
            this.Name = "DotNetSpyForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DotNetSpy";
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem spyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsmiAlwaysOnTop;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsmiAboutSpyDotNet;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripMenuItem tsmiWindow;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tsbtFindWindow;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.TreeView tvWindowTree;
        private System.ComponentModel.BackgroundWorker backgroundWorker;
        private System.Windows.Forms.ToolStripButton tsbDotNetOnly;
        private System.Windows.Forms.ToolStripMenuItem tsmiActiveWindowCatcher;
    }
}


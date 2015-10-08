using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DotNetSpy
{
    public partial class ActiveWindowCatcher : Form
    {
        #region Const Fields
        private const uint MOD_CONTROL = 0x0002;
        private const uint MOD_ALT = 0x0001;
        private const uint MOD_SHIFT = 0x0004;
        private const uint MOD_WIN = 0x0008;

        private static readonly string JoinSymbol = @"+";
        private static readonly string KeyCtrl = @"Ctrl";
        private static readonly string KeyAlt = @"Alt";
        private static readonly string KeyShift = @"Shift";
        private static readonly string KeyWin = @"Win";
        #endregion

        public ActiveWindowCatcher()
        {
            InitializeComponent();
        }

        private uint _hotKey = 0;
        /// <summary>
        /// Gets or sets the hot key.
        /// </summary>
        /// <value>The hot key.</value>
        public uint HotKey
        {
            get
            {
                return this._hotKey;
            }
            private set
            {
                if (this._hotKey != value)
                {
                    this._hotKey = value;
                }
            }
        }
        private uint _modifiers = 0;
        /// <summary>
        /// Gets or sets the modifiers.
        /// </summary>
        /// <value>The modifiers.</value>
        public uint Modifiers
        {
            get
            {
                return this._modifiers;
            }
            private set
            {
                if (this._modifiers != value)
                {
                    this._modifiers = value;
                }
            }
        }

        private void OnHotkeyPreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null && e.Modifiers == Keys.None)
            {
                if (e.KeyCode != Keys.F12 && e.KeyCode != Keys.Enter && e.KeyCode != Keys.Tab)
                {
                    textBox.Text = this.GetModifierKeyString() + e.KeyCode.ToString();
                    this.HotKey = (uint)e.KeyCode;
                }
            }
            e.IsInputKey = false;
        }
        private void OnModifierKeyCheckedChanged(object sender, EventArgs e)
        {
            string name = string.Empty;
            CheckBox cb = sender as CheckBox;
            if (cb != null) name = cb.Name;

            uint modifiers = 0;
            if(string.Equals(name, "cbCtrl", StringComparison.OrdinalIgnoreCase))
            {
                modifiers = modifiers | MOD_CONTROL;
            }
            if (string.Equals(name, "cbAlt", StringComparison.OrdinalIgnoreCase))
            {
                modifiers = modifiers | MOD_ALT;
            }
            if (string.Equals(name, "cbShift", StringComparison.OrdinalIgnoreCase))
            {
                modifiers = modifiers | MOD_SHIFT;
            }
            if (string.Equals(name, "cbWin", StringComparison.OrdinalIgnoreCase))
            {
                modifiers = modifiers | MOD_WIN;
            }
            this.Modifiers = modifiers;

            this.tbHotkey.Text = this.GetModifierKeyString() + ((Keys)this.HotKey).ToString();
        }

        private string GetModifierKeyString()
        {
            StringBuilder strKeys = new StringBuilder();
            if (this.cbCtrl.Checked)
            {
                strKeys.Append(KeyCtrl);
                strKeys.Append(JoinSymbol);
            }
            if (this.cbAlt.Checked)
            {
                strKeys.Append(KeyAlt);
                strKeys.Append(JoinSymbol);
            }
            if (this.cbShift.Checked)
            {
                strKeys.Append(KeyShift);
                strKeys.Append(JoinSymbol);
            }
            if (this.cbWin.Checked)
            {
                strKeys.Append(KeyWin);
                strKeys.Append(JoinSymbol);
            }
            return strKeys.ToString();
        }
    }
}

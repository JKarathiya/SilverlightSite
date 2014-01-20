using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TMTemplate
{
    public partial class Button3 : UserControl
    {
        [Category("Common Properties")]
        public string Text
        {
            get
            {
                return this.text.Text;
            }
            set
            {
                this.text.Text = value;
            }
        }

        public Button3()
        {
            this.InitializeComponent();
            this.MouseEnter += (MouseEventHandler) ((s, e) => VisualStateManager.GoToState((Control) this, "Enter", false));
            this.MouseLeave += (MouseEventHandler) ((s, e) => VisualStateManager.GoToState((Control) this, "Normal", false));
        }
    }
}

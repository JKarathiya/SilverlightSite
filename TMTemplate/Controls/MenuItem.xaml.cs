// Type: TMTemplate.MenuItem
// Assembly: TMTemplate, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// Assembly location: D:\Projects\Devtuson\TMTemplate.dll

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace TMTemplate
{
    public partial class MenuItem : UserControl
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

        public MenuItem()
        {
            this.InitializeComponent();
            this.VisualStateGroup.CurrentStateChanging += new EventHandler<VisualStateChangedEventArgs>(this._StateChanging);
            VisualStateManager.GoToState((Control)this, "Normal", false);
            this.MouseEnter += (MouseEventHandler)((s, e) =>
            {
                VisualStateManager.GoToState((Control)this, "Enter", false);
                this.Enter_.Begin();
            });
            this.MouseLeave += (MouseEventHandler)((s, e) => VisualStateManager.GoToState((Control)this, "Normal", false));
        }

        private void _StateChanging(object sender, VisualStateChangedEventArgs e)
        {
            if (e.NewState != this.Normal)
                return;
            this.Enter_.Stop();
        }
    }
}

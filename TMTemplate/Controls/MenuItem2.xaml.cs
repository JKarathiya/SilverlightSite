// Type: TMTemplate.MenuItem2
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
    public partial class MenuItem2 : UserControl
    {
        [Category("Common Properties")]
        public string Text
        {
            get { return this.text.Text; }
            set
            {
                this.text.Text = value;
                char[] chArray = value.ToCharArray();
                string str = "";
                for (int index = chArray.Length - 1; index >= 0; --index)
                    str = str + (object) chArray[index];
                this.text1.Text = str;
            }
        }

        public MenuItem2()
        {
            this.InitializeComponent();
            this.VisualStateGroup.CurrentStateChanging +=
                new EventHandler<VisualStateChangedEventArgs>(this._StateChanging);
            VisualStateManager.GoToState((Control) this, "Normal", false);
            this.MouseEnter += (MouseEventHandler) ((s, e) =>
                {
                    VisualStateManager.GoToState((Control) this, "Enter", false);
                    this.Enter_.Begin();
                });
            this.MouseLeave +=
                (MouseEventHandler) ((s, e) => VisualStateManager.GoToState((Control) this, "Normal", false));
        }

        private void _StateChanging(object sender, VisualStateChangedEventArgs e)
        {
            if (e.NewState != this.Normal)
                return;
            this.Enter_.Stop();
        }
    }
}

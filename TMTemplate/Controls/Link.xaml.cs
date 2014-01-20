// Type: TMTemplate.Link
// Assembly: TMTemplate, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// Assembly location: D:\Projects\Devtuson\TMTemplate.dll

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace TMTemplate
{
    public partial class Link : UserControl
    {
      
        [Category("Common Properties")]
        public string Text
        {
            get
            {
                return this.textBlock.Text;
            }
            set
            {
                this.textBlock.Text = value;
            }
        }

        public Link()
        {
            this.InitializeComponent();
            this.MouseEnter += (MouseEventHandler)((s, e) =>
            {
                VisualStateManager.GoToState((Control)this, "Enter", false);
                this.Enter1.Begin();
            });
            this.MouseLeave += (MouseEventHandler)((s, e) =>
            {
                VisualStateManager.GoToState((Control)this, "Normal", false);
                this.Enter1.Stop();
            });
        }
    }
}

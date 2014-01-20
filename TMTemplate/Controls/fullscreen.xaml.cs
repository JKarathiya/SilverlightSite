using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;

namespace TMTemplate
{
    public partial class FullScreen : UserControl
    {
        public FullScreen()
        {
            this.InitializeComponent();
            VisualStateManager.GoToState((Control)this, "Normal", false);
            this.MouseEnter += (MouseEventHandler)((s, e) => VisualStateManager.GoToState((Control)this, "Enter", false));
            this.MouseLeave += (MouseEventHandler)((s, e) => VisualStateManager.GoToState((Control)this, "Normal", false));
            this.MouseLeftButtonUp += new MouseButtonEventHandler(this.Click);
        }

        private void Click(object sender, MouseButtonEventArgs e)
        {
            if (Application.Current.Host.Content.IsFullScreen)
                Application.Current.Host.Content.IsFullScreen = false;
            else
                Application.Current.Host.Content.IsFullScreen = true;
        }
    }
}

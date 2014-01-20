using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TMTemplate
{
	public partial class PrivacyPolicyLink : UserControl
	{
        public PrivacyPolicyLink()
		{
			InitializeComponent();
			this.MouseEnter += new MouseEventHandler((s, e) => { VisualStateManager.GoToState(this, "Enter", false); });
            this.MouseLeave += new MouseEventHandler((s, e) => { VisualStateManager.GoToState(this, "Normal", false); });
		}
	}
}

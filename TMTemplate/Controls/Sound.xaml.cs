using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Resources;
using System.Windows.Threading;
using System.Windows.Input;

namespace TMTemplate
{
	public partial class Sound : UserControl
	{
        DispatcherTimer timer = new DispatcherTimer();
		public Sound()
		{
			InitializeComponent();
			this.MouseEnter += new MouseEventHandler((s, e) => { VisualStateManager.GoToState(this, "Enter", false); });
            this.MouseLeave += new MouseEventHandler((s, e) => { VisualStateManager.GoToState(this, "Normal", false); });
            if (!DesignerProperties.IsInDesignTool)
            {
                Loop.MediaOpened += new RoutedEventHandler(Loop_MediaOpened);
                this.MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(Sound_MouseLeftButtonUp);
            }
		}
        void Loop_MediaOpened(object sender, RoutedEventArgs e)
        {
            if (!DesignerProperties.IsInDesignTool)
            {
                timer.Interval = Loop.NaturalDuration.TimeSpan.Duration();
                timer.Tick += new EventHandler(timer_Tick);
                Loop.Play(); timer.Start();
            }
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (!DesignerProperties.IsInDesignTool)
            {
                Loop.Stop();
                Loop.Position = new TimeSpan();
                Loop.Play();
            }
        }
        //-----------------------------------------------
        bool isSoundOn = true;
        void Sound_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isSoundOn = !isSoundOn;
            if (isSoundOn)
                SoundOnAnimation.Begin();
            else
                SoundOffAnumation.Begin();
        }
	}
}

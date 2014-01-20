using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace TMTemplate
{
    public partial class MainPage : UserControl
    {
        private bool NormalCompleted = false;


        public MainPage()
        {
            InitializeComponent();
            VisualStateManager.GoToState(this, "Loading", false);
            this.Loaded += new System.Windows.RoutedEventHandler(MainPage_Loaded);
            PageManager.This.PageChanged += new PageManager.PageManagerEventHandler(pageManager_PageChanged);
        }

        private void pageManager_PageChanged(PageManager.PageManagerEventArgs e)
        {
            if (!this.NormalCompleted)
                return;
            DispatcherTimer t = new DispatcherTimer();
            t.Interval = new TimeSpan(0, 0, 0, 0, 400);
            t.Tick += (EventHandler)((_s, _e) =>
            {
                this.PageSound.Stop();
                this.PageSound.Position = TimeSpan.Zero;
                this.PageSound.Play();
                t.Stop();
            });
            t.Start();
            if (this.pageManager.isBlankPage(this.pageManager.Page))
                VisualStateManager.GoToState((Control)this, "ToBlank", false);
            else
                VisualStateManager.GoToState((Control)this, "ToPage", false);
        }

        private void MainPage_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            timer.Tick += (EventHandler)((_s, _e) =>
            {
                timer.Stop();
                this.Normal.Storyboard.Completed += (EventHandler)((_s_, _e_) =>
                {
                    this.NormalCompleted = true;
                    if (this.pageManager.isBlankPage(this.pageManager.Page))
                        return;
                    this.pageManager.Visibility = Visibility.Visible;
                    this.pageManager.Blank2Page.Begin();
                    VisualStateManager.GoToState((Control)this, "ToPage", false);
                });
                VisualStateManager.GoToState((Control)this, "Normal", false);
            });
            timer.Start();
        }

    }
}

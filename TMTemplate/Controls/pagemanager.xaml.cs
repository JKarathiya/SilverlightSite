using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.ComponentModel;

namespace TMTemplate
{
    public partial class PageManager : UserControl
    {
        public static PageManager This;
        public PageManager()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(PageManager_Loaded);
            This = this;
        }

        void PageManager_Loaded(object sender, RoutedEventArgs e)
        {
            Page2Blank.Completed += new EventHandler(_AnimationCompleted);
            Page2Page.Completed += new EventHandler(_AnimationCompleted);
            Blank2Page.Completed += new EventHandler(_AnimationCompleted);

            ContentPage.Navigating += new System.Windows.Navigation.NavigatingCancelEventHandler(_Navigating);
            ContentPage.Navigated += new System.Windows.Navigation.NavigatedEventHandler(_Navigated);
            ContentPage.NavigationFailed += new System.Windows.Navigation.NavigationFailedEventHandler(_Failed);
        }

        void _AnimationCompleted(object sender, EventArgs e)
        {
            if (AnimationCompleted != null)
                AnimationCompleted.Invoke(new PageManagerEventArgs(oldPage, newPage));
        }

        private void _Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            newPage = ContentPage.CurrentSource.OriginalString;
            BeginAnimation();
            if (PageChanged != null)
                PageChanged.Invoke(new PageManagerEventArgs(oldPage, newPage));
        }

        private void _Navigating(object sender, System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            WriteableBitmap wb = new WriteableBitmap(ContentPage, null);
            ContentScreen.Source = wb;
            oldPage = ContentPage.CurrentSource.OriginalString;
        }

        private void _Failed(object sender, System.Windows.Navigation.NavigationFailedEventArgs e)
        {
			e.Handled = true;
            ContentPage.Source = new Uri("NotFound", UriKind.Relative);
        }

        string oldPage, newPage;
        [Category("Common Properties")]
        public string Page
        {
            get { return ContentPage.CurrentSource.OriginalString; }
            set
            {
                if (!(isBlankPage(value) && isBlankPage(Page)))
                    ContentPage.Source = new System.Uri(value, UriKind.Relative);
            }
        }

        void BeginAnimation()
        {
            if (isBlankPage(Page) && !isBlankPage(oldPage))
                Page2Blank.Begin();
            else if (!isBlankPage(Page) && !isBlankPage(oldPage))
                Page2Page.Begin();
            else if (!isBlankPage(Page) && isBlankPage(oldPage))
                Blank2Page.Begin();
        }

        public bool isBlankPage(string Uri)
        {
            if (Uri == "" || Uri == "_")
            {
                return true;
            }
            return false;
        }

        public event PageManagerEventHandler PageChanged;
        public event PageManagerEventHandler AnimationCompleted;

        public delegate void PageManagerEventHandler(PageManagerEventArgs e);
        public class PageManagerEventArgs : EventArgs
        {
            public PageManagerEventArgs(string from, string to)
            {
                FromPage = from;
                ToPage = to;
            }
            public string FromPage { get; set; }
            public string ToPage { get; set; }
        }
    }
}

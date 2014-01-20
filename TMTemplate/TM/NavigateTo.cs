using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.ComponentModel;

namespace TMTemplate
{
	public class NavigateTo : Behavior<Control>
	{
		public NavigateTo() { }
        Control obj;
		protected override void OnAttached()
		{
            obj = AssociatedObject as Control;
            obj.Loaded += new RoutedEventHandler(obj_Loaded);
            base.OnAttached();
		}

        void obj_Loaded(object sender, RoutedEventArgs e)
        {
            var pm = PageManager.This;
            obj.MouseLeftButtonUp += new MouseButtonEventHandler(Click);
            pm.AnimationCompleted += new PageManager.PageManagerEventHandler(pm_AnimationCompleted);
            pm.PageChanged += new PageManager.PageManagerEventHandler(pm_PageChanged);
        }

        void pm_PageChanged(PageManager.PageManagerEventArgs e)
        {
            if (e.ToPage == this.Page)
            {
                VisualStateManager.GoToState(obj, "Enter", false);
                obj.IsEnabled = false;
            }
            else
            {
                VisualStateManager.GoToState(obj, "Normal", false);
                obj.IsEnabled = true;
            }
            isAnimation = true;
        }

        void pm_AnimationCompleted(PageManager.PageManagerEventArgs e)
        {
            isAnimation = false;
        }
		protected override void OnDetaching()
		{
			base.OnDetaching();
		}
        void Click(object sender, MouseButtonEventArgs e)
        {
            if (!isAnimation)
                if (!String.IsNullOrEmpty(Page))
                    PageManager.This.Page = Page;
        }
        bool isAnimation = false;
        #region PageProperty
        public static readonly DependencyProperty PageProperty
            = DependencyProperty.Register("Page", typeof(string), typeof(NavigateTo),
            new PropertyMetadata("", new PropertyChangedCallback(OnPageChanged)));
        private static void OnPageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        { }
        [Category("Common Properties")]
        public string Page
        {
            get { return (string)GetValue(PageProperty); }
            set { SetValue(PageProperty, value); }
        }
        #endregion
    }
}

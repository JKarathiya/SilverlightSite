using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.ComponentModel;
using System.Windows.Interactivity;

namespace TMTemplate
{
    public class Prev : Behavior<Control>
    {
        [Category("Common Properties"), CustomPropertyValueEditor(CustomPropertyValueEditor.Element)]
        public string gallery
        { get; set; }
        Control Gallery;

        Control obj;
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Loaded += new RoutedEventHandler(AssociatedObject_Loaded);
        }

        void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(gallery))
            {
                Gallery = findGallery();
                if (Gallery is Gallery)
                {
                    obj = AssociatedObject as Control;
                    obj.MouseLeftButtonUp += new MouseButtonEventHandler(Click);
                }
            }
        }

        void Click(object sender, MouseButtonEventArgs e)
        {
            if (((Gallery)Gallery).Image > 1)
            {
                ((Gallery)Gallery).Image--;
            }
        }

        Gallery findGallery()
        {
            FrameworkElement a = AssociatedObject;
            do
            {
                a = a.Parent as FrameworkElement;
            }
            while (!(a is UserControl || a is Page));
            return a.FindName(gallery) as Gallery;
        }
    }
}

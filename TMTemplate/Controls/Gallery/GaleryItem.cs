using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.ComponentModel;
using System.Windows.Interactivity;

namespace TMTemplate
{
    public class GaleryItem : Behavior<Control>
	{
		[Category("Common Properties"), CustomPropertyValueEditor(CustomPropertyValueEditor.Element)]
        public string gallery
        {  get; set; }
        Control Gallery;
        [Category("Common Properties")]
        public int ImageNumber
        {  get; set; }

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
                    ((Gallery)Gallery).ImageChanging += new Gallery.GalleryEventHandler(ImageChanging);
                }
            }
        }

        void ImageChanging(Gallery.GalleryEventArgs e)
        {
            if (e.Image == ImageNumber)
            {
                VisualStateManager.GoToState(obj, "Enter", false);
                obj.IsEnabled = false;
            }
            else
            {
                VisualStateManager.GoToState(obj, "Normal", false);
                obj.IsEnabled = true;
            }
        }

        void Click(object sender, MouseButtonEventArgs e)
        {
            ((Gallery)Gallery).Image = ImageNumber;
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

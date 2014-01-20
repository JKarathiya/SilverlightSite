using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;
using System.Windows.Media;
namespace TMTemplate
{
    [TemplatePart(Name = "LayoutRoot", Type = typeof(Grid))]
    [TemplatePart(Name = "Frame", Type = typeof(Canvas))]
    [TemplatePart(Name = "Film", Type = typeof(Canvas))]
    public class ImageSequence : Control
    {
        private double imageHeight;
        private int offsetTop = 0;
        private System.Windows.Controls.Image imgNext;
        public static readonly DependencyProperty CurrentImageNumProperty =
            DependencyProperty.Register("CurrentImageNum", typeof(double), typeof(ImageSequence), new PropertyMetadata(1.0, CurrentImageNumPropertyChanged));
        [Category("Common Properties")]
        public double CurrentImageNum
        {
            get { return (double)this.GetValue(ImageSequence.CurrentImageNumProperty); }
            set { this.SetValue(ImageSequence.CurrentImageNumProperty, value); }
        }

        private static void CurrentImageNumPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ImageSequence source = d as ImageSequence;
                source.UpdateSequence();
        }

        private void UpdateSequence()
        {
            if (!DesignerProperties.IsInDesignTool)
            {
                Dispatcher.BeginInvoke(() =>
                {
                    if (imgNext != null)
                        imgNext.Source = new BitmapImage(new Uri("/TMTemplate;component/Images/Effects/" + ImagesFolder + "/" + Math.Floor(CurrentImageNum) + "." + ImageFilesExtension, UriKind.RelativeOrAbsolute));
                });
            }
            else
            {
                if (Film != null)
                    Film.SetValue(Canvas.TopProperty, Math.Floor(CurrentImageNum - 1) * -imageHeight);
            }
        }
        private Grid LayoutRoot { get; set; }
        private Canvas Frame { get; set; }
        private Canvas Film { get; set; }
        public static readonly DependencyProperty ImagesFolderProperty =
            DependencyProperty.Register("ImagesFolder", typeof(string), typeof(ImageSequence), new PropertyMetadata("", ImagesFolderPropertyChanged));
        private static void ImagesFolderPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ImageSequence source = d as ImageSequence;
            source.PrepareImages();
            if (DesignerProperties.IsInDesignTool)
            {
                if (source.Film != null)
                    source.Film.SetValue(Canvas.TopProperty, Math.Floor(source.CurrentImageNum - 1) * -source.imageHeight);
            }
        }

        [Category("Common Properties"), Description("All images must be named by numbers starting from 1")]
        public string ImagesFolder
        {
            get { return (string)this.GetValue(ImageSequence.ImagesFolderProperty); }
            set { this.SetValue(ImageSequence.ImagesFolderProperty, value); }
        }


        string _imageFilesExtension = "png";
        [Category("Common Properties")]
        public string ImageFilesExtension
        {
            get { return _imageFilesExtension; }
            set { _imageFilesExtension = value; }
        }

        public ImageSequence()
        {
            DefaultStyleKey = typeof(ImageSequence);
            imageHeight = 1600;
            Loaded += new System.Windows.RoutedEventHandler(ImageSequence_Loaded);
            SizeChanged += new SizeChangedEventHandler(ImageSequence_SizeChanged);
        }

        void ImageSequence_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (DesignerProperties.IsInDesignTool)
            {
                Rect r = new Rect();
                r.X = 0;
                r.Y = 0;
                r.Width = this.ActualWidth;
                r.Height = this.ActualHeight;
                //--
                RectangleGeometry rg = new RectangleGeometry();
                rg.Rect = r;
                //--
                LayoutRoot.Clip = rg;
            }
        }

        private void ImageSequence_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (DesignerProperties.IsInDesignTool)
            {
                PrepareImages();
            }
        }

        private void PrepareImages()
        {
            if (DesignerProperties.IsInDesignTool)
            {
                if (Film != null)
                    Film.Children.Clear();
                offsetTop = 0;
                for (int i = 1; i <= 200; i++)
                {
                    imgNext = new System.Windows.Controls.Image();
                    imgNext.Source = new BitmapImage(new Uri("/Images/Effects/" + ImagesFolder + "/" + i + "." + ImageFilesExtension, UriKind.RelativeOrAbsolute));
                    imgNext.SetValue(Canvas.TopProperty, imageHeight * offsetTop++);
                    if (Film != null)
                        Film.Children.Add(imgNext);
                }
                if (Film != null)
                    Film.SetValue(Canvas.TopProperty, Math.Floor(CurrentImageNum - 1) * -imageHeight);
            }
        }

        public override void OnApplyTemplate()
        {
            LayoutRoot = GetTemplateChild("LayoutRoot") as Grid; 
            Frame = GetTemplateChild("Frame") as Canvas;
            Film = GetTemplateChild("Film") as Canvas;
            if (!DesignerProperties.IsInDesignTool)
            {
                imgNext = new System.Windows.Controls.Image();
                imgNext.Source = new BitmapImage(new Uri("/TMTemplate;component/Images/Effects/" + ImagesFolder + "/" + Math.Floor(CurrentImageNum) + "." + ImageFilesExtension, UriKind.RelativeOrAbsolute));
                if (Film != null)
                    Film.Children.Add(imgNext);
            }
        }
    }
}

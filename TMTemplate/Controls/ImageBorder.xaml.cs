using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace TMTemplate
{
    public partial class ImageBorder : UserControl
    {
        public static readonly DependencyProperty MyMarginProperty = DependencyProperty.Register("MyMargin", typeof(double), typeof(ImageBorder), new PropertyMetadata((object)0.0, new PropertyChangedCallback(ImageBorder.MyMarginPropertyChanged)));
        public static readonly DependencyProperty MyBorderProperty = DependencyProperty.Register("MyBorder", typeof(double), typeof(ImageBorder), new PropertyMetadata((object)3.0, new PropertyChangedCallback(ImageBorder.MyBorderPropertyChanged)));
      
        [Category("Common Properties")]
        public double MyMargin
        {
            get
            {
                return (double)this.GetValue(ImageBorder.MyMarginProperty);
            }
            set
            {
                this.SetValue(ImageBorder.MyMarginProperty, (object)value);
            }
        }

        [Category("Common Properties")]
        public double MyBorder
        {
            get
            {
                return (double)this.GetValue(ImageBorder.MyBorderProperty);
            }
            set
            {
                this.SetValue(ImageBorder.MyBorderProperty, (object)value);
            }
        }

        static ImageBorder()
        {
        }

        public ImageBorder()
        {
            this.InitializeComponent();
            this.Loaded += (RoutedEventHandler)((s, e) => this.CreatePathData());
        }

        private static void MyMarginPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ImageBorder).CreatePathData();
        }

        private static void MyBorderPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ImageBorder).CreatePathData();
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            this.CreatePathData();
            return availableSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            this.CreatePathData();
            return finalSize;
        }

        private void CreatePathData()
        {
            double num = this.MyMargin + this.MyBorder;
            double actualWidth = this.ActualWidth;
            double actualHeight = this.ActualHeight;
            if (this.MyMargin >= 0.0 && this.MyBorder >= 0.0)
            {
                if (num * 2.0 < actualHeight && num * 2.0 < actualWidth)
                {
                    PathFigure pathFigure1 = new PathFigure();
                    pathFigure1.StartPoint = new Point(this.MyMargin, this.MyMargin);
                    pathFigure1.Segments.Add((PathSegment)new LineSegment()
                    {
                        Point = new Point(actualWidth - this.MyMargin, this.MyMargin)
                    });
                    pathFigure1.Segments.Add((PathSegment)new LineSegment()
                    {
                        Point = new Point(actualWidth - this.MyMargin, actualHeight - this.MyMargin)
                    });
                    pathFigure1.Segments.Add((PathSegment)new LineSegment()
                    {
                        Point = new Point(this.MyMargin, actualHeight - this.MyMargin)
                    });
                    pathFigure1.Segments.Add((PathSegment)new LineSegment()
                    {
                        Point = new Point(this.MyMargin, this.MyMargin)
                    });
                    pathFigure1.IsClosed = true;
                    PathFigure pathFigure2 = new PathFigure();
                    pathFigure2.StartPoint = new Point(num, num);
                    pathFigure2.Segments.Add((PathSegment)new LineSegment()
                    {
                        Point = new Point(actualWidth - num, num)
                    });
                    pathFigure2.Segments.Add((PathSegment)new LineSegment()
                    {
                        Point = new Point(actualWidth - num, actualHeight - num)
                    });
                    pathFigure2.Segments.Add((PathSegment)new LineSegment()
                    {
                        Point = new Point(num, actualHeight - num)
                    });
                    pathFigure2.Segments.Add((PathSegment)new LineSegment()
                    {
                        Point = new Point(num, num)
                    });
                    pathFigure2.IsClosed = true;
                    PathGeometry pathGeometry = new PathGeometry();
                    pathGeometry.Figures.Add(pathFigure1);
                    pathGeometry.Figures.Add(pathFigure2);
                    this.path.Data = (Geometry)pathGeometry;
                }
                else
                    this.path.Data = (Geometry)null;
            }
            else
                this.path.Data = (Geometry)null;
        }
    }
}

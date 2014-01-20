// Type: TMTemplate.Image
// Assembly: TMTemplate, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// Assembly location: D:\Projects\Devtuson\TMTemplate.dll

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace TMTemplate
{
    public partial class Image : UserControl
    {
        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register("Source", typeof(ImageSource), typeof(TMTemplate.Image), new PropertyMetadata((object)null, new PropertyChangedCallback(TMTemplate.Image.SourcePropertyChanged)));

        [Category("Common Properties")]
        public ImageSource Source
        {
            get
            {
                return (ImageSource)this.GetValue(TMTemplate.Image.ImageProperty);
            }
            set
            {
                this.SetValue(TMTemplate.Image.ImageProperty, (object)value);
            }
        }

        static Image()
        {
        }

        public Image()
        {
            this.InitializeComponent();
            this.MouseEnter += (MouseEventHandler)((s, e) => VisualStateManager.GoToState((Control)this, "Enter", false));
            this.MouseLeave += (MouseEventHandler)((s, e) => VisualStateManager.GoToState((Control)this, "Normal", false));
        }

        private static void SourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((TMTemplate.Image)d).MainImage.Source = ((TMTemplate.Image)d).Source;
        }
    }
}

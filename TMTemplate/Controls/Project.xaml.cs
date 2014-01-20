using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace TMTemplate
{
    public partial class Project : UserControl
    {
        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register("Source", typeof(ImageSource), typeof(Project), new PropertyMetadata((object)null, new PropertyChangedCallback(Project.SourcePropertyChanged)));

        [Category("Common Properties")]
        public ImageSource Source
        {
            get
            {
                return (ImageSource)this.GetValue(Project.ImageProperty);
            }
            set
            {
                this.SetValue(Project.ImageProperty, (object)value);
            }
        }

        [Category("Common Properties")]
        public string Text1
        {
            get
            {
                return this.text1.Text;
            }
            set
            {
                this.text1.Text = value;
            }
        }

        [Category("Common Properties")]
        public string Text2
        {
            get
            {
                return this.text2.Text;
            }
            set
            {
                this.text2.Text = value;
            }
        }

        static Project()
        {
        }

        public Project()
        {
            this.InitializeComponent();
            this.MouseEnter += (MouseEventHandler)((s, e) => VisualStateManager.GoToState((Control)this, "Enter", false));
            this.MouseLeave += (MouseEventHandler)((s, e) => VisualStateManager.GoToState((Control)this, "Normal", false));
        }

        private static void SourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Project)d).image.Source = ((Project)d).Source;
        }
    }
}

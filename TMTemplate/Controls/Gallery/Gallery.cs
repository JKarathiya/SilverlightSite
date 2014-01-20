using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using System.Windows.Media.Imaging;
using System.ComponentModel;
using System.Collections.Generic;

namespace TMTemplate
{
    [TemplatePart(Name = "Image", Type = typeof(System.Windows.Controls.Image))]
    [TemplatePart(Name = "OldImage", Type = typeof(System.Windows.Controls.Image))]
    [TemplatePart(Name = "NextImage", Type = typeof(Storyboard))]
    [TemplatePart(Name = "Sound", Type = typeof(MediaElement))]
    public class Gallery : Control
    {
        public Gallery() { this.DefaultStyleKey = typeof(Gallery); }
        System.Windows.Controls.Image image, oldImage;
        Storyboard NextImage;
        MediaElement Sound;
        DispatcherTimer timer = new DispatcherTimer();


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            image = (System.Windows.Controls.Image)GetTemplateChild("Image");
            oldImage = (System.Windows.Controls.Image)GetTemplateChild("OldImage");
            NextImage = (Storyboard)GetTemplateChild("NextImage");
            Sound = (MediaElement)GetTemplateChild("Sound");
            if (image != null && oldImage != null && NextImage != null)
            {
                Initialize = true;
                Image = 1;
                if (!DesignerProperties.IsInDesignTool)
                {
                    InitTimer();
                    NextImage.Completed += new EventHandler(AnimationCompleted);
                    if (Sound != null)
                    {
                        Sound.Stop();
                    }
                }
            }
        }


        void AnimationCompleted(object sender, EventArgs e)
        {
            animation = false;
            if (ImageChanged != null)
                ImageChanged.Invoke(new GalleryEventArgs(Image, Images));
        }

        bool Initialize = false;



        #region Timer
        void Tick(object sender, EventArgs e)
        {
            
            if (Image == Images)
            {
                if (CyclingImages)
                    Image = 1;
                else
                {
                    timer.Stop();
                    if (EndOfGallery != null)
                        EndOfGallery.Invoke(new GalleryEventArgs(Image, Images));
                }
            }
            else
                Image++;
        }
        void InitTimer()
        {
            if (Timer > 0)
            {
                timer.Interval = new TimeSpan(0, 0, 0, Timer);
                timer.Tick += new EventHandler(Tick);
                timer.Start();
            }
        }
        public void TimerStart()
        {
            if (Timer > 0)
            {
                if (Image == Images)
                    Image = 1;
                timer.Start();
            }
        }
        public void TimerStop()
        {
            timer.Stop();
        }
        #endregion

        bool animation = false;
        int _curentImage = 0;
        public int Image
        {
            get { return _curentImage; }
            set 
            {
                if (!animation && Initialize)
                {
                    Dispatcher.BeginInvoke(delegate()
                    {
                        oldImage.Source = new BitmapImage(new Uri
                            ("/TMTemplate;component/Images/Gallery/" + Folder + "/" + _curentImage + ".jpg",
                            UriKind.RelativeOrAbsolute));
                        _curentImage = value;
                        animation = true;
                        image.Source = new BitmapImage(new Uri
                            ("/TMTemplate;component/Images/Gallery/" + Folder + "/" + _curentImage + ".jpg",
                            UriKind.RelativeOrAbsolute));

                        if (!DesignerProperties.IsInDesignTool)
                        {
                            NextImage.Begin();
                            if (Sound != null && Sound.Source != null)
                            {
                                Sound.Stop();
                                Sound.Position = TimeSpan.Zero;
                                Sound.Play();
                            }
                        }
                        if (ImageChanging != null)
                            ImageChanging.Invoke(new GalleryEventArgs(Image, Images));
                    });
                }
            }
        }

        #region Properties
        int im = 2;
        [Category("Gallery Properties")]
        public int Images
        {
            get { return im; }
            set { im = value; }
        }
        int t = 5;
        [Category("Gallery Properties")]
        public int Timer
        {
            get { return t; }
            set { t = value; }
        }
        bool cyclingImages = false;
        [Category("Gallery Properties")]
        public bool CyclingImages
        {
            get { return cyclingImages; }
            set { cyclingImages = value; }
        }
        [Category("Gallery Properties")]
        public string Folder { get; set; }
        #endregion

        #region Events
        public event GalleryEventHandler ImageChanging;
        public event GalleryEventHandler ImageChanged;
        public event GalleryEventHandler EndOfGallery;

        public delegate void GalleryEventHandler(GalleryEventArgs e);
        public class GalleryEventArgs : EventArgs
        {
            public GalleryEventArgs(int image, int images)
            {
                Image = image;
                Images = images;
            }
            public int Image { get; set; }
            public int Images { get; set; }
        }
        #endregion
    }
}

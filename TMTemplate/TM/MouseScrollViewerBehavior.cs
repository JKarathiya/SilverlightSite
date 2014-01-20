using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace TMTemplate
{
    /// <summary>
    /// Smoother scrolling experience
    /// </summary>
    /// <remarks>
    /// <code>
    /// <ScrollViewer>
    ///		<i:Interaction.Behaviors>
    ///			<behaviors:MouseScrollViewer />
    ///		</i:Interaction.Behaviors>
    /// </ScrollViewer>
    /// </code>
    /// </remarks>
    public class MouseScrollViewerBehavior : Behavior<ScrollViewer>
    {
        double target = 0;
        int direction = 0;
        private Storyboard storyboard;
        private DoubleAnimation animation;

        protected override void OnAttached()
        {
            CreateStoryBoard();
            base.OnAttached();
            ScrollingStarted += delegate(object sender, EventArgs e) { };
            ScrollingCompleted += delegate(object sender, EventArgs e) { };
            AssociatedObject.Loaded += new RoutedEventHandler(AssociatedObject_Loaded);
        }

        void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(delegate()
            {
                (VisualTreeHelper.GetChild((sender as FrameworkElement), 0) as FrameworkElement).MouseWheel += AssociatedObject_MouseWheel;
            });
        }

        protected override void OnDetaching()
        {
            storyboard = null;
            base.OnDetaching();
        }

        public void AssociatedObject_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Animate(-Math.Sign(e.Delta) * ScrollAmount))
            {
                e.Handled = true;
            }
        }

        public bool Animate(double offset)
        {
            storyboard.Pause();
            if (Math.Sign(offset) != direction)
            {   //scroll direction reversed while animating. Flip around immediately

                if (!IsHorizontal)
                    target = AssociatedObject.VerticalOffset;
                else
                    target = AssociatedObject.HorizontalOffset;

                direction = Math.Sign(offset);
            }
            target += offset;

            if (!IsHorizontal)
                target = Math.Max(Math.Min(target, AssociatedObject.ScrollableHeight), 0);
            else
                target = Math.Max(Math.Min(target, AssociatedObject.ScrollableWidth), 0);

            animation.To = target;

            if (!IsHorizontal)
                animation.From = AssociatedObject.VerticalOffset;
            else
                animation.From = AssociatedObject.HorizontalOffset;

            if (animation.From != animation.To)
            {
                storyboard.Begin();
                ScrollingStarted.Invoke(storyboard, new EventArgs());
                return true;
            }
            return false;
        }

        private void CreateStoryBoard()
        {
            storyboard = new Storyboard();
            animation = new DoubleAnimation()
            {
                Duration = Duration,
                EasingFunction = EasingFunction
            };
            animation.SetValue(Storyboard.TargetPropertyProperty, new PropertyPath("OffsetMediator"));
            Storyboard.SetTarget(animation, this);
            storyboard.Children.Add(animation);
            storyboard.Completed += (s, e) => { direction = 0; ScrollingCompleted.Invoke(s, e); };
        }

        internal double OffsetMediator
        {
            get { return (double)GetValue(OffsetMediatorProperty); }
            set { SetValue(OffsetMediatorProperty, value); }
        }

        internal static readonly DependencyProperty OffsetMediatorProperty =
            DependencyProperty.Register("OffsetMediator", typeof(double), typeof(MouseScrollViewerBehavior), new PropertyMetadata(0.0, OnOffsetMediatorPropertyChanged));

        private static void OnOffsetMediatorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MouseScrollViewerBehavior msv = d as MouseScrollViewerBehavior;
            if (msv != null && msv.AssociatedObject != null)
            {
                if (!msv.IsHorizontal)
                    msv.AssociatedObject.ScrollToVerticalOffset((double)e.NewValue);
                else
                    msv.AssociatedObject.ScrollToHorizontalOffset((double)e.NewValue);
            }
        }

        /// <summary>
        /// Gets or sets the scroll amount.
        /// </summary>
        /// <value>The scroll amount.</value>
        /// <remarks>Defaults to 50.</remarks>
        [System.ComponentModel.Category("Common Properties"), System.ComponentModel.Description("Gets or sets the scroll amount")]
        public double ScrollAmount
        {
            get { return (double)GetValue(ScrollAmountProperty); }
            set { SetValue(ScrollAmountProperty, value); }
        }

        public static readonly DependencyProperty ScrollAmountProperty =
            DependencyProperty.Register("ScrollAmount", typeof(double), typeof(MouseScrollViewerBehavior), new PropertyMetadata(50.0));

        /// <summary>
        /// Gets or sets the number of seconds one step of scrolling will take to complete
        /// </summary>
        [System.ComponentModel.Category("Animation Properties"), System.ComponentModel.Description("Gets or sets the number of seconds one step of scrolling will take to complete")]
        public Duration Duration
        {
            get { return (Duration)GetValue(DurationProperty); }
            set { SetValue(DurationProperty, value); }
        }

        public static readonly DependencyProperty DurationProperty =
            DependencyProperty.Register("Duration", typeof(Duration), typeof(MouseScrollViewerBehavior), new PropertyMetadata(new Duration(new TimeSpan(0, 0, 1))));

        /// <summary>
        /// Gets or sets the easing function.
        /// </summary>
        [System.ComponentModel.Category("Animation Properties"), System.ComponentModel.Description("Gets or sets the easing function")]
        public EasingFunctionBase EasingFunction
        {
            get { return (EasingFunctionBase)GetValue(EasingFunctionProperty); }
            set { SetValue(EasingFunctionProperty, value); }
        }

        public static readonly DependencyProperty EasingFunctionProperty =
            DependencyProperty.Register("EasingFunction", typeof(EasingFunctionBase), typeof(MouseScrollViewerBehavior),
            new PropertyMetadata(new ExponentialEase() { EasingMode = EasingMode.EaseOut }));

        [System.ComponentModel.Category("Common Properties")]
        public bool IsHorizontal
        {
            get { return (bool)GetValue(IsHorizontalProperty); }
            set { SetValue(IsHorizontalProperty, value); }
        }

        public static readonly DependencyProperty IsHorizontalProperty =
            DependencyProperty.Register("IsHorizontal", typeof(bool), typeof(MouseScrollViewerBehavior), new PropertyMetadata(false));

        public event EventHandler ScrollingCompleted;
        public event EventHandler ScrollingStarted;

    }
}
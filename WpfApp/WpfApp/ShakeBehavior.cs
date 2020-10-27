using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Microsoft.Xaml.Behaviors;

namespace WpfApp
{
    public class ShakeBehavior : Behavior<Image>
    {
        private const double DefaultRepeatInterval = 10.0;
        private const double DefaultSpeedRatio = 1.0;
        private const double DefaultFrameCount = 8.0;
        private const double DefaultTimeOffsetInSeconds = 0.25;

        private const string RepeatIntervalName = "RepeatInterval";
        private const string SpeedRatioName = "SpeedRatio";
        private const string KeyFrameCountName = "KeyFrameCount";
        private const string TimeOffsetInSecondsName = "TimeOffsetInSeconds";

        public static readonly DependencyProperty RepeatIntervalProperty =
            DependencyProperty.Register(RepeatIntervalName,
                typeof(double),
                typeof(ShakeBehavior),
                new PropertyMetadata(DefaultRepeatInterval));

        public static readonly DependencyProperty SpeedRatioProperty =
            DependencyProperty.Register(SpeedRatioName,
                typeof(double),
                typeof(ShakeBehavior),
                new PropertyMetadata(DefaultSpeedRatio));

        public static readonly DependencyProperty KeyFrameCountProperty =
            DependencyProperty.Register(KeyFrameCountName,
                typeof(double),
                typeof(ShakeBehavior),
                new PropertyMetadata(DefaultFrameCount));

        public static readonly DependencyProperty TimeOffsetInSecondsProperty =
            DependencyProperty.Register(TimeOffsetInSecondsName,
                typeof(double),
                typeof(ShakeBehavior),
                new PropertyMetadata(DefaultTimeOffsetInSeconds));

        private Style _originalStyle;

        /// <summary>
        ///     Gets or sets the time interval in seconds between each shake.
        /// </summary>
        /// <value>
        ///     The time interval in seconds between each shake.
        /// </value>
        /// <remarks>
        ///     If interval is less than total shake time, then it will shake
        ///     constantly without pause. If you want it to only shake
        ///     once, simply set it to 0.
        /// </remarks>
        public double RepeatInterval
        {
            get => (double) GetValue(RepeatIntervalProperty);
            set => SetValue(RepeatIntervalProperty, value);
        }

        /// <summary>
        ///     Gets or sets the ratio at which time progresses on the Shakes
        ///     Timeline, relative to its parent.
        /// </summary>
        /// <value>
        ///     The ratio at which time progresses on the Shakes Timeline, relative
        ///     to its parent.
        /// </value>
        /// <remarks>
        ///     If Acceleration or Deceleration are specified, this ratio is the
        ///     average ratio over the natural length of the Shake's Timeline. This
        ///     property has a default value of 1.0. If set to zero or less it
        ///     will be reset back to the default value.
        /// </remarks>
        public double SpeedRatio
        {
            get => (double) GetValue(SpeedRatioProperty);
            set => SetValue(SpeedRatioProperty, value);
        }

        /// <summary>
        ///     Gets or sets the number of time one keyframe
        ///     will be repeated.
        /// </summary>
        /// <value>
        ///     The number of keyframes repetition.
        /// </value>
        /// <remarks>
        ///     By default, its value is 8.0. The totalAnimationLength is
        ///     obtained by multiplying the KeyFrameCount and the TimeOffsetInSeconds.
        /// </remarks>
        public double KeyFrameCount
        {
            get => (double) GetValue(KeyFrameCountProperty);
            set => SetValue(KeyFrameCountProperty, value);
        }

        /// <summary>
        ///     Gets or sets the duration one keyframe will take
        ///     to make its rotation.
        /// </summary>
        /// <value>
        ///     The duration of one keyframe.
        /// </value>
        /// <remarks>
        ///     By default, its value is 0.25. The totalAnimationLength is
        ///     obtained by multiplying the KeyFrameCount and the TimeOffsetInSeconds.
        /// </remarks>
        public double TimeOffsetInSeconds
        {
            get => (double) GetValue(TimeOffsetInSecondsProperty);
            set => SetValue(TimeOffsetInSecondsProperty, value);
        }

        protected override void OnAttached()
        {
            _originalStyle = AssociatedObject.Style;
            AssociatedObject.Style = CreateShakeStyle();
        }

        protected override void OnDetaching()
        {
            AssociatedObject.Style = _originalStyle;
        }

        private Style CreateShakeStyle()
        {
            var newStyle = new Style(AssociatedObject.GetType(), AssociatedObject.Style);
            /*
             * The following will replace/override any existing RenderTransform
             * and RenderTransformOrigin properties on the FrameworkElement
             * once the the new Style is applied to it.
             */
            newStyle.Setters.Add(new Setter(UIElement.RenderTransformProperty, new RotateTransform(0)));
            newStyle.Setters.Add(new Setter(UIElement.RenderTransformOriginProperty, new Point(0.5, 0.5)));

            newStyle.Triggers.Add(CreateTrigger());

            return newStyle;
        }

        private DataTrigger CreateTrigger()
        {
            var trigger = new DataTrigger
            {
                Binding = new Binding
                {
                    RelativeSource = new RelativeSource
                    {
                        Mode = RelativeSourceMode.FindAncestor,
                        AncestorType = typeof(UIElement)
                    },
                    Path = new PropertyPath(UIElement.IsVisibleProperty)
                },
                Value = true
            };

            trigger.EnterActions.Add(new BeginStoryboard {Storyboard = CreateStoryboard()});

            return trigger;
        }

        private Storyboard CreateStoryboard()
        {
            var speedRatio = SpeedRatio;

            // Must be greater than zero
            if (speedRatio <= 0.0)
                SpeedRatio = DefaultSpeedRatio;

            var storyboard = new Storyboard
            {
                SpeedRatio = speedRatio,
                // If RepeatBehavior = 0 do not repeat
                RepeatBehavior = RepeatInterval == 0 ? new RepeatBehavior(1) : RepeatBehavior.Forever
            };

            storyboard.Children.Add(CreateAnimationTimeline());

            return storyboard;
        }

        private Timeline CreateAnimationTimeline()
        {
            var animation = new DoubleAnimationUsingKeyFrames();

            animation.SetValue(Storyboard.TargetPropertyProperty,
                new PropertyPath("(0).(1)", UIElement.RenderTransformProperty, RotateTransform.AngleProperty));

            var totalAnimationLength = KeyFrameCount * TimeOffsetInSeconds;
            var repeatInterval = RepeatInterval;

            // Can't be less than zero and pointless to be less than total length
            if (repeatInterval < totalAnimationLength)
                repeatInterval = totalAnimationLength;

            animation.Duration = new Duration(TimeSpan.FromSeconds(repeatInterval));

            const int targetValue = 12;
            for (var i = 0; i < KeyFrameCount; i++)
                animation.KeyFrames.Add(new LinearDoubleKeyFrame(i % 2 == 0 ? targetValue : -targetValue,
                    KeyTime.FromTimeSpan(TimeSpan.FromSeconds(i * TimeOffsetInSeconds))));

            animation.KeyFrames.Add(new LinearDoubleKeyFrame(0,
                KeyTime.FromTimeSpan(TimeSpan.FromSeconds(totalAnimationLength))));
            return animation;
        }
    }
}
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using Microsoft.Xaml.Behaviors;

namespace WpfApp
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SubmitTheQuestion_OnClick(object sender, RoutedEventArgs e)
        {
            if (QuestionTextBox.Text != "")
            {
                QuestionTextBox.Text = "";
                GiveAnAnswer();
            }

            async void GiveAnAnswer()
            {
                SubmitTheQuestion.IsEnabled = false;

                // Processing Effect
                AnswerDisplay.Source = new BitmapImage(new Uri("pack://application:,,,/img/M8B_Processing.png"));
                var shake = new ShakeBehavior {RepeatInterval = 0, SpeedRatio = 2};
                Interaction.GetBehaviors(AnswerDisplay).Add(shake);

                // Wait for the duration of the shaking effect
                var animationLength = shake.KeyFrameCount * shake.TimeOffsetInSeconds / shake.SpeedRatio;
                await Task.Delay(TimeSpan.FromSeconds(animationLength));

                // Cast an answer
                CastAnAnswer();

                SubmitTheQuestion.IsEnabled = true;


                void CastAnAnswer()
                {
                    // Answers images paths
                    string[] answersPath =
                    {
                        // 10 positive answers
                        "pack://application:,,,/img/M8B_Positive01.png",
                        "pack://application:,,,/img/M8B_Positive02.png",
                        "pack://application:,,,/img/M8B_Positive03.png",
                        "pack://application:,,,/img/M8B_Positive04.png",
                        "pack://application:,,,/img/M8B_Positive05.png",
                        "pack://application:,,,/img/M8B_Positive06.png",
                        "pack://application:,,,/img/M8B_Positive07.png",
                        "pack://application:,,,/img/M8B_Positive08.png",
                        "pack://application:,,,/img/M8B_Positive09.png",
                        "pack://application:,,,/img/M8B_Positive10.png",

                        // 5 neutral answers
                        "pack://application:,,,/img/M8B_Neutral01.png",
                        "pack://application:,,,/img/M8B_Neutral02.png",
                        "pack://application:,,,/img/M8B_Neutral03.png",
                        "pack://application:,,,/img/M8B_Neutral04.png",
                        "pack://application:,,,/img/M8B_Neutral05.png",

                        // 5 negative answers
                        "pack://application:,,,/img/M8B_Negative01.png",
                        "pack://application:,,,/img/M8B_Negative02.png",
                        "pack://application:,,,/img/M8B_Negative03.png",
                        "pack://application:,,,/img/M8B_Negative04.png",
                        "pack://application:,,,/img/M8B_Negative05.png"
                    };

                    // Cast a random number
                    var rnd = new Random();
                    var answerIndex = rnd.Next(answersPath.Length);
                    var randomAnswer = answersPath[answerIndex];

                    AnswerDisplay.Source = new BitmapImage(new Uri(randomAnswer));
                }
            }
        }
    }
}
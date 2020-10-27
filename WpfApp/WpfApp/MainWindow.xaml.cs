using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Xaml.Behaviors;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SubmitTheQuestion_OnClick(object sender, RoutedEventArgs e)
        {
            QuestionTextBox.Text = "";
            GiveAnAnswer();

            void GiveAnAnswer()
            {
                ProcessingEffect();
                CastAnAnswer();

                void ProcessingEffect()
                {
                    AnswerDisplay.Source = new BitmapImage(new Uri("pack://application:,,,/img/M8B_Processing.png"));   
                    var shake = new ShakeBehavior {RepeatInterval = 0, SpeedRatio = 1};
                    Interaction.GetBehaviors(AnswerDisplay).Add(shake);
                    Task.Delay(TimeSpan.FromSeconds(5));
                }

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
                        "pack://application:,,,/img/M8B_Negative05.png",
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
using System;

namespace ConsoleApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            StartTheMagic8Ball();

            void StartTheMagic8Ball()
            {
                var exit = false;

                while (!exit)
                {
                    // Get the user input
                    Console.Write("Ask a question to the magic 8-ball: ");
                    var question = Console.ReadLine();

                    // Print an answer
                    Console.WriteLine("{0} Hum... Let me think...", question);
                    Console.WriteLine(GetAnAnswer());

                    // Ask another question
                    var hasEnteredACorrectChar = false;

                    while (!hasEnteredACorrectChar)
                        try
                        {
                            Console.Write("Do you want to ask another question? Y/N: ");
                            var exitChar = Console.ReadLine();

                            if (string.Equals(exitChar, "Y", StringComparison.OrdinalIgnoreCase) ||
                                string.Equals(exitChar, "N", StringComparison.OrdinalIgnoreCase))
                            {
                                hasEnteredACorrectChar = true;

                                // Add 2 blank lines
                                Console.Write(Environment.NewLine + Environment.NewLine);

                                if (string.Equals(exitChar, "N", StringComparison.OrdinalIgnoreCase)) exit = true;
                            }

                            else
                            {
                                throw new ArgumentException();
                            }
                        }
                        catch (ArgumentException)
                        {
                            Console.WriteLine(Environment.NewLine + "You haven't pressed Y or N! Try again.");
                        }
                }

                Console.WriteLine("Bye!");

                // Get a random answer
                string GetAnAnswer()
                {
                    // List of possible answers
                    string[] answersList =
                    {
                        // 10 positive answers
                        "It is certain.",
                        "It is decidedly so.",
                        "Without a doubt.",
                        "Yes - definitely.",
                        "You may rely on it.",
                        "As I see, yes.",
                        "Most likely.",
                        "Outlook good.",
                        "Yes.",
                        "Signs point to yes.",

                        // 5 neutral answers
                        "Reply hazy, please try again.",
                        "Ask again later.",
                        "Better not tell you right now.",
                        "Cannot predict now.",
                        "Concentrate and ask again.",

                        // 5 negative answers
                        "Don't count on it.",
                        "My reply is no,",
                        "My sources say no.",
                        "Outlook not so good.",
                        "Very doubtful."
                    };

                    // Cast a random number
                    var rnd = new Random();
                    var answerIndex = rnd.Next(answersList.Length);
                    var randomAnswer = answersList[answerIndex];

                    return randomAnswer;
                }
            }
        }
    }
}
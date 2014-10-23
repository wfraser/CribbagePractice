using System;
using System.Collections.Generic;
using System.Linq;

namespace CribbagePractice
{
    public class Program : IUserInterface
    {
        private Hand m_hand;
        private int m_playerScore = 0;
        private int m_computerScore = 0;

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Cribbage Practice");

            var program = new Program();
            var game = new Game(program);
            var rand = new Random();
            var deck = new Deck();

            do
            {
                Console.WriteLine();

                deck.Shuffle(rand);
                game.Play(deck);

                Console.WriteLine("Score total: You: {0}", program.m_playerScore);
                Console.WriteLine("        Computer: {0}", program.m_computerScore);
                Console.Write("Play again? [y/n] ");
            } while (Console.ReadLine().ToLower() == "y");
        }

        public void DisplayHand(Hand h)
        {
            m_hand = h;
            Console.WriteLine(string.Join<Card>(" ", h.Cards));
        }

        public void DisplayMissedCombos(IEnumerable<Combo> combos)
        {
            Console.WriteLine("You missed some:");
            foreach (Combo c in combos)
            {
                Console.WriteLine("{0}: {1} points for a {2}", string.Join<Card>(" ", c.Cards), c.Score, c.Text);
            }
        }

        public void DisplayWinMessage(int score)
        {
            Console.WriteLine("Aww yiss! {0} points for you!", score);
        }

        public void DisplayLoseMessage(int score)
        {
            Console.WriteLine("Computer gets muggins of {0} points.", score);
        }

        public void DisplayBadGuessWrongScore(Combo actualCombo)
        {
            Console.WriteLine("Nope, score is {0} for a {1}", actualCombo.Score, actualCombo.Text);
        }

        public void DisplayBadGuessInvalidCombo()
        {
            Console.WriteLine("Nope! That's nothing.");
        }

        public void DisplayCorrectGuess(Combo combo)
        {
            Console.WriteLine("Correct! {0} points for a {1}", combo.Score, combo.Text);
        }

        public void AddScoreComputer(int score)
        {
            m_computerScore += score;
        }

        public void AddScorePlayer(int score)
        {
            m_playerScore += score;
        }

        public Guess GetGuess()
        {
            Guess guess = new Guess();

            var cards = new HashSet<Card>();
            guess.Score = 0;

            string line;
            while (!string.IsNullOrEmpty(line = Console.ReadLine()))
            {
                try
                {
                    var parts = new List<string>(line.Split(' '));

                    if (!int.TryParse(parts[parts.Count - 1], out guess.Score))
                    {
                        Console.Write("Score? ");
                        string scoreStr = Console.ReadLine();

                        if (!int.TryParse(scoreStr, out guess.Score))
                        {
                            Console.WriteLine("bad input.");
                            throw new Exception();
                        }
                        parts.Add("");
                    }

                    cards.Clear();
                    for (int i = 0; i < parts.Count - 1; i++)
                    {
                        var c = new Card(parts[i]);
                        if (cards.Contains(c))
                        {
                            Console.WriteLine("You typed {0} twice!", c);
                            throw new Exception();
                        }
                        if (!m_hand.Cards.Contains(c))
                        {
                            Console.WriteLine("That card, {0}, isn't in your hand!", c);
                            throw new Exception();
                        }
                        cards.Add(c);
                    }
                }
                catch (Exception)
                {
                    continue;
                }

                guess.Cards = cards.ToList();
                return guess;
            }

            return null;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace CribbagePractice
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Cribbage Practice");

            var rand = new Random();
            var deck = new Deck();
            do
            {
                deck.Shuffle(rand);
                Console.WriteLine();
                bool win = Play(deck);
                if (win)
                    Console.Write("Aww yiss! ");
                Console.Write("Play again? [y/n] ");
            } while (Console.ReadLine().ToLower() == "y");
        }

        class Guess
        {
            public List<Card> Cards;
            public int Score;
        }

        static bool GetGuess(Hand h, out Guess guess)
        {
            guess = new Guess();

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
                        if (!h.Cards.Contains(c))
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
                return true;
            }

            return false;
        }

        static bool Play(Deck deck)
        {
            Hand h = deck.DealHand(5);
            Console.WriteLine(string.Join<Card>(" ", h.Cards));

            List<Combo> combos = Combo.FindAllCombos(h.Cards);

            Guess guess;
            while (true)
            {
                bool guessed = GetGuess(h, out guess);
                if (!guessed)
                {
                    if (combos.Count > 0)
                    {
                        Console.WriteLine("You missed some. Try some more or press <enter> again.");
                        guessed = GetGuess(h, out guess);

                        if (!guessed)
                        {
                            foreach (Combo c in combos)
                            {
                                Console.WriteLine("{0}: {1} points - {2}", string.Join<Card>(" ", c.Cards), c.Score, c.Text);
                            }
                            return false;
                        }
                    }
                    else
                    {
                        return true;
                    }
                }

                Combo combo = combos.FirstOrDefault(o => o.Equals(guess.Cards));
                if (combo == null)
                {
                    Console.WriteLine("Nope! That's nothing.");
                }
                else
                {
                    if (guess.Score == combo.Score)
                    {
                        Console.WriteLine("Correct!");
                        combos.Remove(combo);
                    }
                    else
                    {
                        Console.WriteLine("Nope, score is {0}: {1}", combo.Score, combo.Text);
                    }
                }
            }
        }
    }
}

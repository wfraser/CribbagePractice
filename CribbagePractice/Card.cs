using System;

namespace CribbagePractice
{
    public enum Suit
    {
        Spades,
        Clubs,
        Hearts,
        Diamonds,
    }

    public class Card
    {
        public Card(Suit suit, int number)
        {
            Suit = suit;
            Number = number;
        }

        public Card(string s)
        {
            s = s.ToLower();
            switch (s[s.Length - 1])
            {
                case 's': Suit = Suit.Spades; break;
                case 'c': Suit = Suit.Clubs; break;
                case 'h': Suit = Suit.Hearts; break;
                case 'd': Suit = Suit.Diamonds; break;
                default: throw new Exception("invalid suit");
            }

            string numStr = s.Substring(0, s.Length - 1);
            int number = 0;
            switch (numStr)
            {
                case "a": number = 1; break;
                case "j": number = 11; break;
                case "q": number = 12; break;
                case "k": number = 13; break;
                default: int.TryParse(numStr, out number); break;
            }

            if (number < 1 || number > 13)
                throw new Exception("invalid card");
            Number = number;
        }

        public Suit Suit { get; private set; }
        public int Number { get; private set; }

        public int Value
        {
            get
            {
                if (Number > 10)
                    return 10;
                else
                    return Number;
            }
        }

        public override string ToString()
        {
            string s = string.Empty;
            switch (Number)
            {
                case 1:  s += "A"; break;
                case 11: s += "J"; break;
                case 12: s += "Q"; break;
                case 13: s += "K"; break;
                default: s += Number.ToString(); break;
            }
            switch (Suit)
            {
                case Suit.Spades: s += "S"; break;
                case Suit.Clubs: s += "C"; break;
                case Suit.Hearts: s += "H"; break;
                case Suit.Diamonds: s += "D"; break;

                    /*
                case Suit.Spades: s += "♠"; break;
                case Suit.Clubs: s += "♣"; break;
                case Suit.Hearts: s += "♥"; break;
                case Suit.Diamonds: s += "♦"; break;
                     */
            }
            return s;
        }

        public override int GetHashCode()
        {
            return Number << 2 + (int)Suit;
        }

        public override bool Equals(object obj)
        {
            if (obj is Card)
            {
                return Equals((Card)obj);
            }
            return base.Equals(obj);
        }

        public bool Equals(Card other)
        {
            return (Number == other.Number) && (Suit == other.Suit);
        }
    }
}

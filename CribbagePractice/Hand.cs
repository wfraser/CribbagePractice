using System.Collections.Generic;

namespace CribbagePractice
{
    public class Hand
    {
        public Hand(IEnumerable<Card> cards)
        {
            Cards = new List<Card>();
            Cards.AddRange(cards);
        }

        public Hand(string str)
        {
            Cards = new List<Card>();
            foreach (string s in str.Split(' '))
            {
                Cards.Add(new Card(s));
            }
        }

        public List<Card> Cards { get; private set; }

        public override string ToString()
        {
            return string.Join<Card>(" ", Cards);
        }
    }
}

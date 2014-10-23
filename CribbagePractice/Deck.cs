using System;
using System.Collections.Generic;
using System.Linq;

namespace CribbagePractice
{
    public class Deck
    {
        public Deck()
        {
            m_cards = new List<Card>();
            for (int i = 0; i < 13; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    m_cards.Add(new Card((Suit)j, i + 1));
                }
            }
        }

        public void Shuffle(Random rand)
        {
            m_cards = m_cards.OrderBy(a => rand.Next()).ToList();
        }

        public Hand DealHand(int size)
        {
            return new Hand(m_cards.Take(size));
        }

        private List<Card> m_cards;
    }
}

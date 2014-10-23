using System;
using System.Collections.Generic;
using System.Linq;

namespace CribbagePractice
{
    // http://stackoverflow.com/a/349919
    public static class PowerSet4<T>
    {
        static public IEnumerable<IList<T>> powerset(IList<T> currentGroupList)
        {
            int count = currentGroupList.Count;
            Dictionary<long, T> powerToIndex = new Dictionary<long, T>();
            long mask = 1L;
            for (int i = 0; i < count; i++)
            {
                powerToIndex[mask] = currentGroupList[i];
                mask <<= 1;
            }

            Dictionary<long, T> result = new Dictionary<long, T>();
            yield return result.Values.ToArray();

            long max = 1L << count;
            for (long i = 1L; i < max; i++)
            {
                long key = i & -i;
                if (result.ContainsKey(key))
                    result.Remove(key);
                else
                    result[key] = powerToIndex[key];
                yield return result.Values.ToArray();
            }
        }

    }

    public class Combo
    {
        public static List<Combo> FindAllCombos(IList<Card> cards)
        {
            var combos = new List<Combo>();
            foreach (IList<Card> set in PowerSet4<Card>.powerset(cards))
            {
                Combo c = Equals15(set);
                if (c != null)
                    combos.Add(c);
            }
            combos.AddRange(NOfKind(cards));
            combos.AddRange(NRun(cards));
            combos.AddRange(Flush(cards));

            // Check if any combo is a subset of another.
            var hiddenCombos = new List<Combo>();
            foreach (Combo c in combos)
            {
                foreach (Combo other in combos)
                {
                    if (c == other)
                        continue;

                    if ((c.Text.EndsWith(" of a kind") && other.Text.EndsWith(" of a kind"))
                        || (c.Text.EndsWith(" flush") && other.Text.EndsWith(" flush"))
                        || (c.Text.StartsWith("run of ") && other.Text.StartsWith("run of ")))
                    {
                        if (c.IsSubsetOf(other))
                        {
                            hiddenCombos.Add(c);
                        }
                    }
                }
            }

            foreach (Combo c in hiddenCombos)
            {
                combos.Remove(c);
            }

            return combos;
        }

        private Combo(IList<Card> cards, int score, string text)
        {
            Cards = cards;
            Score = score;
            Text = text;
        }

        private static Combo Equals15(IList<Card> cards)
        {
            int sum = 0;
            for (int i = 0, n = cards.Count; i < n; i++)
            {
                sum += cards[i].Value;
            }

            if (sum == 15)
            {
                return new Combo(cards, 2, "fifteen");
            }
            return null;
        }

        private static IList<Combo> NOfKind(IList<Card> cards)
        {
            var combos = new List<Combo>();
            var groupByNumber = cards.GroupBy(o => o.Number);
            foreach (IGrouping<int, Card> group in groupByNumber)
            {
                int count = group.Count();
                if (count > 1)
                {
                    int score = 0;
                    switch (count)
                    {
                        case 2: score = 2; break;
                        case 3: score = 6; break;
                        case 4: score = 12; break;
                        default:
                            Console.WriteLine("ERROR: >4 of a kind is impossible");
                            System.Diagnostics.Debug.Assert(false);
                            throw new Exception();
                    }
                    combos.Add(new Combo(group.ToList(), score, string.Format("{0} of a kind", count)));
                }
            }
            return combos;
        }

        private static IList<Combo> Flush(IList<Card> cards)
        {
            var combos = new List<Combo>();
            if (cards.GroupBy(o => o.Suit).Count() == 1)
            {
                if (cards.Count >= 4)
                {
                    combos.Add(new Combo(cards, cards.Count, string.Format("{0} card flush", cards.Count)));
                }
            }
            return combos;
        }

        private static IList<Combo> NRun(IList<Card> cards)
        {
            var combos = new List<Combo>();
            var runs = new List<List<Card>>();

            int n = 1;
            var groups = cards.GroupBy(o => o.Number).OrderBy(o => o.Key).ToList();

            for (int i = 0; i < groups.Count - 1; i++)
            {
                if (groups[i + 1].Key - groups[i].Key == 1)
                {
                    var g1 = groups[i].ToList();
                    if (n == 1)
                    {
                        foreach (var card in g1)
                        {
                            runs.Add(new List<Card>(new Card[] { card }));
                        }
                    }

                    var g2 = groups[i + 1].ToList();
                    var newRuns = new List<List<Card>>();
                    for (int j = 0; j < g2.Count; j++)
                    {
                        foreach (var run in runs)
                        {
                            if (j == 0)
                            {
                                run.Add(g2[j]);
                            }
                            else
                            {
                                var newRun = new List<Card>(run);
                                newRun[run.Count - 1] = g2[j];
                                newRuns.Add(newRun);
                            }
                        }
                    }
                    runs.AddRange(newRuns);
                    n++;
                }
                else if (n > 1)
                {
                    if (n > 2)
                    {
                        foreach (var run in runs)
                        {
                            combos.Add(new Combo(run, n, string.Format("run of {0}", n)));
                        }
                    }
                    n = 1;
                    runs.Clear();
                }
            }

            if (n > 2)
            {
                foreach (var run in runs)
                {
                    combos.Add(new Combo(run, n, string.Format("run of {0}", n)));
                }
            }

            return combos;
        }

        public override int GetHashCode()
        {
            return Cards.GetHashCode() + Score + Text.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is Combo)
            {
                return Equals((Combo)obj);
            }
            return base.Equals(obj);
        }

        public bool Equals(Combo obj)
        {
            return Equals(obj.Cards);
        }

        public bool Equals(IEnumerable<Card> otherCards)
        {
            var mySeq = Cards.OrderBy(o => o.Number).ThenBy(o => o.Suit);
            var otherSeq = otherCards.OrderBy(o => o.Number).ThenBy(o => o.Suit);
            return (mySeq.SequenceEqual(otherSeq));
        }

        public bool IsSubsetOf(Combo other)
        {
            var mySeq = Cards.OrderBy(o => o.Number).ThenBy(o => o.Suit);
            var otherSeq = other.Cards.OrderBy(o => o.Number).ThenBy(o => o.Suit);
            return (otherSeq.Intersect(mySeq).SequenceEqual(mySeq));
        }

        public IList<Card> Cards { get; private set; }

        public int Score { get; private set; }

        public string Text { get; private set; }

        public override string ToString()
        {
            return string.Format("{0}: {1} - {2}", string.Join<Card>(" ", Cards), Score, Text);
        }
    }
}

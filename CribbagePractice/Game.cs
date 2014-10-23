using System.Collections.Generic;
using System.Linq;

namespace CribbagePractice
{
    public class Game
    {
        private const int ScoreBadGuessWrongScore = 1;
        private const int ScoreBadGuessInvalidCombo = 2;

        public Game(IUserInterface ui)
        {
            m_ui = ui;
        }

        public void Play(Deck deck)
        {
            Hand h = deck.DealHand(5);
            m_ui.DisplayHand(h);

            List<Combo> combos = Combo.FindAllCombos(h.Cards);

            int playerScore = 0;
            while (true)
            {
                Guess guess = m_ui.GetGuess();
                if (guess == null)
                {
                    if (combos.Count > 0)
                    {
                        m_ui.DisplayMissedCombos(combos);
                        int score = combos.Aggregate(0, (sum, c) => (sum + c.Score));
                        m_ui.DisplayLoseMessage(score);
                        m_ui.AddScoreComputer(score);
                    }
                    else
                    {
                        m_ui.DisplayWinMessage(playerScore);
                        m_ui.AddScorePlayer(playerScore);
                    }

                    break;
                }
                else
                {
                    Combo combo = combos.FirstOrDefault(o => o.Equals(guess.Cards));
                    if (combo == null)
                    {
                        m_ui.DisplayBadGuessInvalidCombo();
                        m_ui.AddScoreComputer(ScoreBadGuessInvalidCombo);
                    }
                    else
                    {
                        if (guess.Score == combo.Score)
                        {
                            m_ui.DisplayCorrectGuess(combo);
                            playerScore += combo.Score;
                        }
                        else
                        {
                            m_ui.DisplayBadGuessWrongScore(combo);
                            m_ui.AddScoreComputer(ScoreBadGuessWrongScore);
                        }
                        combos.Remove(combo);
                    }
                }
            }
        }

        private IUserInterface m_ui;
    }
}

using System.Collections.Generic;

namespace CribbagePractice
{
    public class Guess
    {
        public List<Card> Cards;
        public int Score;
    }

    public interface IUserInterface
    {
        void DisplayHand(Hand h);

        void DisplayMissedCombos(IEnumerable<Combo> combos);

        void DisplayWinMessage(int score);

        void DisplayLoseMessage(int score);

        void DisplayBadGuessWrongScore(Combo actualCombo);

        void DisplayBadGuessInvalidCombo();

        void DisplayCorrectGuess(Combo combo);

        void AddScoreComputer(int score);

        void AddScorePlayer(int score);

        // Get a guess from the user. Returns null if the user has finished scoring.
        Guess GetGuess();
    }
}

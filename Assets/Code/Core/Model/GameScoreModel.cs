using UnityEngine;

namespace Code.Core.Model
{
    [CreateAssetMenu(menuName = "StopBlock/GameScoreModel", fileName = "NewGameScoreModel")]
    public class GameScoreModel : AbstractGameModel
    {
        public int Score { get; private set; }
        public int Level { get; private set; }

        public void SetLevel(int level)
        {
            Level = level;
            OnPropertyChanged(nameof(Level));
        }
        
        public void SetScore(int score)
        {
            Score = score;
            OnPropertyChanged(nameof(Score));
        }

        public void Reset()
        {
            SetScore(0);
            SetLevel(0);
        }
    }
}
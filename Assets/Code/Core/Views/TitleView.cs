using System;
using TMPro;
using UnityEngine;

namespace Code.Core.Views
{
    public class TitleView : AbstractGameView
    {
        public Action StartGameCalled;

        [SerializeField] private TextMeshProUGUI HighScoreText;
        
        public void OnStartGame()
        {
            StartGameCalled?.Invoke();
        }

        public void ShowHighScore(int score)
        {
            HighScoreText.text = score.ToString();
        }
    }
}

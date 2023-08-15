using System;
using System.ComponentModel;
using Code.Core.Model;
using TMPro;
using UnityEngine;

namespace Code.Core.Views
{
    public class GameHudView : AbstractGameView
    {
        [SerializeField] private GameScoreModel GameScoreModel;
        [SerializeField] private MessageBoxView MessageBox;
        
        [SerializeField] private TextMeshProUGUI ScoreText;
        [SerializeField] private TextMeshProUGUI LevelText;
        
        private Action _onCountdownComplete;

        public override void Awake()
        {
            base.Awake();
            GameScoreModel.PropertyChanged += GameScoreModelOnPropertyChanged;
        }

        public void DoCountdown(Action onCountdownComplete)
        {
            _onCountdownComplete = onCountdownComplete;
            MessageBox.MessageBoxCountdownFinishedEvent += MessageBoxOnMessageBoxCountdownFinishedEvent;
            MessageBox.DoCountdown();
        }

        private void MessageBoxOnMessageBoxCountdownFinishedEvent(object sender, EventArgs e)
        {
            MessageBox.MessageBoxCountdownFinishedEvent -= MessageBoxOnMessageBoxCountdownFinishedEvent;
            _onCountdownComplete?.Invoke();
        }

        private void GameScoreModelOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            ScoreText.text = GameScoreModel.Score.ToString();
            LevelText.text = GameScoreModel.Level.ToString();
        }

        public void DoMessage(string message, Action onComplete, float delay)
        {
            MessageBox.ShowMessage(message, onComplete, delay);
        }
    }
}

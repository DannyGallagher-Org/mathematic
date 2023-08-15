using System;
using Code.Core.Model;
using Code.Core.Views;
using Code.Definitions;
using Code.Core.GameBlock;
using Code.ScoreZones;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using hStates;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Core.States
{
    public class InGameState : IEnterState, IInjectable
    {
        private IState _nextState;
        private GameHudView _gameHudView;
        private ScoreZoneManager _scoreZoneManager;
        private GameScoreModel _gameScoreModel;
        private GameBlockScript _littleBlock;
        private Button _bigGameButton;
        private Canvas _mainCanvas;
        private bool _gameplayActive;
        private float _mainCanvasWidth;
        private float _mainCanvasHeight;
        private TweenerCore<Vector2,Vector2,VectorOptions> _littleBlockMove;
        private LoadingView _loadingView;
        private int _currentScore;
        private int _currentLevel;

        public void Inject(ServiceLocator services)
        {
            _mainCanvas = services.GetService<Canvas>();
            _gameHudView = services.GetServiceByString(GameDefs.HudViewKey) as GameHudView;
            _loadingView = services.GetServiceByString(GameDefs.LoadingViewKey) as LoadingView;
            _scoreZoneManager = services.GetService<ScoreZoneManager>();
            _gameScoreModel = services.GetService<GameScoreModel>();
            _littleBlock = services.GetService<GameBlockScript>();
            _bigGameButton = services.GetServiceByString(GameDefs.BigGameButtonKey) as Button;
        }
        
        public void OnEnter()
        {
            _gameScoreModel.Reset();
            _gameHudView.AnimateOn(OnGameHudViewShow);
            _bigGameButton.gameObject.SetActive(true);
            _bigGameButton.onClick.AddListener(OnBigButtonClick);
            var mainCanvasRect = _mainCanvas.GetComponent<RectTransform>().rect;
            _mainCanvasWidth = mainCanvasRect.width;
            _mainCanvasHeight = mainCanvasRect.height * 0.8f;
        }

        private void OnBigButtonClick()
        {
            if (!_gameplayActive)
            {
                return;
            }

            _gameplayActive = false;
            _littleBlockMove.Kill();
            var zoneType = _scoreZoneManager.GetZone(_littleBlock.RectTransform.anchoredPosition.x);
            switch (zoneType)
            {
                case ZoneType.Perfect:
                    _gameHudView.DoMessage("Perfect! +3", ScoreMessageComplete, 0.5f);
                    _currentScore += 3;
                    break;
                case ZoneType.Good:
                    _gameHudView.DoMessage("Good! +2", ScoreMessageComplete, 0.5f);
                    _currentScore += 2;
                    break;
                case ZoneType.Limit:
                    _gameHudView.DoMessage("Ok! +1", ScoreMessageComplete, 0.5f);
                    _currentScore += 1;
                    break;
                case ZoneType.Fail:
                    BlockMoveFinished();
                    break;
                case ZoneType.All:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            _gameScoreModel.SetScore(_currentScore);
        }

        private void ScoreMessageComplete()
        {
            _currentLevel++;
            _gameScoreModel.SetLevel(_currentLevel);
            StartNextLevel();
        }

        private void OnGameHudViewShow()
        {
            _gameHudView.DoCountdown(StartNextLevel);
        }

        private void StartNextLevel()
        {
            _scoreZoneManager.ResizeAllWithRanges();
            _littleBlock.GameBlockReadyComplete += OnGameBlockReadyComplete;
            _littleBlock.ReadyBlock(_mainCanvasWidth, _mainCanvasHeight);
        }

        private void OnGameBlockReadyComplete()
        {
            _littleBlock.GameBlockReadyComplete -= OnGameBlockReadyComplete;
            DoMoveBlock();
        }

        private void DoMoveBlock()
        {
            _gameplayActive = true;
            var isLeft = _littleBlock.RectTransform.anchoredPosition.x < 0f;
            var endValue = new Vector2(isLeft ? _mainCanvasWidth/2f : -_mainCanvasWidth/2f, _littleBlock.RectTransform.anchoredPosition.y);
            var delta = 1f-((1f/100f)*_currentLevel);
            Debug.Log($"InGameState.DoMoveBlock  {nameof(delta)}: {delta}");
            var duration = Mathf.Lerp(0f,1f, delta);
            Debug.Log($"InGameState.DoMoveBlock  {nameof(duration)}: {duration}");
            _littleBlockMove = _littleBlock.RectTransform.DOAnchorPos(endValue, duration)
                .OnComplete(BlockMoveFinished);
        }

        private void BlockMoveFinished()
        {
            _gameplayActive = false;
            _scoreZoneManager.Hide(0.5f);
            _gameHudView.DoMessage("GAME OVER", OnGameOverMessageComplete, 2f);
        }

        private void OnGameOverMessageComplete()
        {
            _gameHudView.AnimateOff(GameHudViewHidden);
            var currentHighScore = PlayerPrefs.GetInt(GameDefs.SaveHighScoreKey);
            PlayerPrefs.SetInt(GameDefs.SaveHighScoreKey, Math.Max(currentHighScore, _gameScoreModel.Score));
        }

        private void GameHudViewHidden()
        {
            _loadingView.AnimateOn(LoadingViewShown);
            _littleBlock.ResetBlock();
        }

        private void LoadingViewShown()
        {
            _nextState = new TitleState();
        }

        public void OnExit()
        {
            _bigGameButton.onClick.RemoveAllListeners();
            _bigGameButton.gameObject.SetActive(false);
        }

        public bool CanTransition() => _nextState != default;
        public IState NextState() => _nextState;
    }
}
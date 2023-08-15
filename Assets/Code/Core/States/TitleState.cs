using Code.Core.GameBlock;
using Code.Core.Views;
using Code.Definitions;
using DG.Tweening;
using hStates;
using UnityEngine;

namespace Code.Core.States
{
    public class TitleState : IEnterState, IInjectable
    {
        private IState _nextState;
        private AbstractGameView _loadingView;
        private TitleView _titleView;
        private GameBlockScript _littleGameBlock;

        public void Inject(ServiceLocator services)
        {
            _loadingView = services.GetServiceByString(GameDefs.LoadingViewKey) as AbstractGameView;
            _titleView = services.GetServiceByString(GameDefs.TitleViewKey) as TitleView;
            _littleGameBlock = services.GetService<GameBlockScript>();
        }

        public void OnEnter()
        {
            if (!PlayerPrefs.HasKey(GameDefs.SaveHighScoreKey))
            {
                PlayerPrefs.SetInt(GameDefs.SaveHighScoreKey, 0);
                _titleView.ShowHighScore(0);
            }
            else
            {
                _titleView.ShowHighScore(PlayerPrefs.GetInt(GameDefs.SaveHighScoreKey));
            }
            
            _loadingView.AnimateOff(OnLoadingViewOff, 1f);
        }

        private void OnLoadingViewOff()
        {
            _titleView.AnimateOn(TitleViewOn, 1f);
        }

        private void TitleViewOn()
        {
            _titleView.StartGameCalled += StartGameCalled;
        }

        private void StartGameCalled()
        {
            _titleView.StartGameCalled -= StartGameCalled;
            _littleGameBlock.Hide();
            _titleView.AnimateOff(TitleViewOffComplete, 1f);
        }

        private void TitleViewOffComplete()
        {
            _nextState = new InGameState();
        }

        public void OnExit()
        {
            
        }

        public bool CanTransition() => _nextState != default;
        public IState NextState() => _nextState;
    }
}
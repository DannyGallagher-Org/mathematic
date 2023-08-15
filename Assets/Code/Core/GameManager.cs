using System.Collections.Generic;
using Code.Core.GameBlock;
using Code.Core.Model;
using Code.Core.States;
using Code.Core.Views;
using Code.Definitions;
using Code.ScoreZones;
using hStates;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Core
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameScoreModel GameScoreModel;
        [SerializeField] private ScoreZoneManager ScoreZoneManager;
        [SerializeField] private GameBlockScript LittleGameBlock;
        [SerializeField] private Button BigGameButton;
        [SerializeField] private Canvas MainCanvas;
        
        [SerializeField] private AbstractGameView TitleView;
        [SerializeField] private AbstractGameView GameHudView;
        [SerializeField] private AbstractGameView LoadingView;

        private StateMachine _stateMachine;

        private void Awake()
        {
            GameHudView.AnimateOff(null, 0f);
            TitleView.AnimateOff(null, 0f);
            BigGameButton.gameObject.SetActive(false);
            
            var services = new Dictionary<object, object>
            {
                {GameDefs.TitleViewKey, TitleView},
                {GameDefs.HudViewKey, GameHudView},
                {GameDefs.LoadingViewKey, LoadingView},
                {typeof(ScoreZoneManager), ScoreZoneManager},
                {typeof(GameScoreModel), GameScoreModel},
                {typeof(GameBlockScript), LittleGameBlock},
                {GameDefs.BigGameButtonKey, BigGameButton},
                {typeof(Canvas), MainCanvas}
            };
            _stateMachine = new StateMachine();
            _stateMachine.SetServices(new ServiceLocator(services));
            _stateMachine.ChangeState(new TitleState());
        }

        private void Update()
        {
            _stateMachine.Update();
        }

        private void LateUpdate()
        {
            _stateMachine.LateUpdate();
        }
    }
}

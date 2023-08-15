using System;
using Code.Definitions;
using DG.Tweening;
using UnityEngine;

namespace Code.Core.Views
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class AbstractGameView : MonoBehaviour
    {
        private CanvasGroup _myCanvasGroup;

        public virtual void Awake()
        {
            _myCanvasGroup = GetComponent<CanvasGroup>();
        }

        public void AnimateOn(Action onComplete, float speed = GameDefs.AnimationBaseSpeed)
        {
            _myCanvasGroup.DOFade(1f, speed)
                .OnComplete(() =>
                {
                    _myCanvasGroup.interactable = true;
                    onComplete?.Invoke();
                });
        }
        
        public void AnimateOff(Action onComplete, float speed = GameDefs.AnimationBaseSpeed)
        {
            _myCanvasGroup.interactable = false;
            _myCanvasGroup.DOFade(0f, speed)
                .OnComplete(() => onComplete?.Invoke());
        }
    }
}

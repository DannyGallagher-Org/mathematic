using System;
using System.Collections;
using Code.Definitions;
using TMPro;
using UnityEngine;

namespace Code.Core.Views
{
    public class MessageBoxView : AbstractGameView
    {
        public event EventHandler MessageBoxCountdownFinishedEvent;

        [SerializeField] private TextMeshProUGUI MessageBox;

        public void Clear()
        {
            StopAllCoroutines();
            AnimateOff(null, 0f);
        }
        
        public void ShowMessage(string msg, Action onComplete, float delay = 1f, float decay = GameDefs.AnimationBaseSpeed)
        {
            StartCoroutine(ShowMessageCoroutine(msg, onComplete, delay, decay));
        }

        public void DoCountdown()
        {
            StartCoroutine(DoCountdownCoroutine());
        }

        private IEnumerator DoCountdownCoroutine()
        {
            ShowMessage("Get Ready!", null);
            yield return new WaitForSeconds(1f+GameDefs.AnimationBaseSpeed);
            ShowMessage("Get Set!", null);
            yield return new WaitForSeconds(1f+GameDefs.AnimationBaseSpeed);
            ShowMessage("GO!", null);
            yield return new WaitForSeconds(1f+GameDefs.AnimationBaseSpeed);
            MessageBoxCountdownFinishedEvent?.Invoke(this, EventArgs.Empty);
        }

        private IEnumerator ShowMessageCoroutine(string msg, Action onComplete, float delay, float decay)
        {
            MessageBox.text = msg;
            AnimateOn(null, 0f);
            yield return new WaitForSeconds(delay);
            AnimateOff(onComplete, decay);
        }
    }
}

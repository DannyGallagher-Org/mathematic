using System;
using Code.Definitions;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Code.ScoreZones
{
    public class ScoreZone : MonoBehaviour
    {
        [SerializeField] private Canvas MainCanvas;
        
        [SerializeField] private Image _visualIndicator;
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private ZoneType ThisZoneType;
        public RectTransform RectTransform => _rectTransform;

        private void Awake()
        {
            _visualIndicator = GetComponent<Image>();
            _rectTransform = GetComponent<RectTransform>();
        }

        public void Resize(float min, float max, float duration, Action<ZoneEventArgs> onComplete)
        {
            var rectTransform = MainCanvas.GetComponent<RectTransform>();

            var rectWidth = rectTransform.rect.width;
            var rectHeight = rectTransform.rect.height;

            _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, rectHeight);

            _rectTransform.DOSizeDelta(new Vector2(Random.Range(min, max) * rectWidth, rectHeight), duration)
                .SetEase(Ease.InSine)
                .OnComplete(() => onComplete?.Invoke(new ZoneEventArgs(ThisZoneType)));
        }
    }
}

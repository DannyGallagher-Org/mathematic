using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.Core.GameBlock
{
    public class GameBlockScript : MonoBehaviour
    {
        public Action GameBlockReadyComplete;
        
        [SerializeField] private float SpinSpeed = 200f;
        private RectTransform _myRectTransform;
        private Vector2 _startPosition;
        public RectTransform RectTransform => _myRectTransform;

        private void Awake()
        {
            _myRectTransform = GetComponent<RectTransform>();
            _startPosition = _myRectTransform.anchoredPosition;
        }

        void Update()
        {
            transform.RotateAround(transform.position, Vector3.forward, SpinSpeed * Time.deltaTime);
        }

        public void ReadyBlock(float widthRange, float heightRange)
        {
            var left = Random.Range(0f, 1f) > 0.5f;
            var sidePosX = widthRange/2f;
            var sidePosYMax = heightRange/2f;
            _myRectTransform.anchoredPosition = new Vector2(left ? -sidePosX : sidePosX,
                Random.Range(-sidePosYMax, sidePosYMax));
            _myRectTransform.DOSizeDelta(new Vector2(50f, 50f), 1f)
                .SetEase(Ease.InBounce)
                .OnComplete(() => GameBlockReadyComplete?.Invoke());
        }

        public void Hide()
        {
            _myRectTransform.DOSizeDelta(new Vector2(0f, 0f), 0.25f)
                .SetEase(Ease.InBounce);
        }

        public void ResetBlock()
        {
            _myRectTransform.DOAnchorPos(_startPosition, 1f);
        }
    }
}

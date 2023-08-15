using System;
using Code.Definitions;
using UnityEngine;

namespace Code.ScoreZones
{
    public class ScoreZoneManager : MonoBehaviour
    {
        public event EventHandler<ZoneEventArgs> ScoreZonesAllAnimatedAction;

        [SerializeField] private ScoreZone Fail;
        [SerializeField] private ScoreZone Limit;
        [SerializeField] private ScoreZone Good;
        [SerializeField] private ScoreZone Perfect;
        private int _zonesAnimating;

        private void Start()
        {
            Hide(0f);
        }

        public void Hide(float duration)
        {
            Fail.Resize(0f, 0f, duration, null);
            Limit.Resize(0f, 0f, duration, null);
            Good.Resize(0f, 0f, duration, null);
            Perfect.Resize(0f, 0f, duration, null);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ResizeAllWithRanges();
            }
        }

        public void ResizeAllWithRanges()
        {
            _zonesAnimating = 4;
            
            Fail.Resize(GameDefs.FailRange.x, GameDefs.FailRange.y, GameDefs.AnimationBaseSpeed, OnAnimateZoneComplete);
            Limit.Resize(GameDefs.LimitRange.x, GameDefs.LimitRange.y, GameDefs.AnimationBaseSpeed, OnAnimateZoneComplete);
            Good.Resize(GameDefs.GoodRange.x, GameDefs.GoodRange.y, GameDefs.AnimationBaseSpeed, OnAnimateZoneComplete);
            Perfect.Resize(GameDefs.PerfectRange.x, GameDefs.PerfectRange.y, GameDefs.AnimationBaseSpeed, OnAnimateZoneComplete);
        }

        private void OnAnimateZoneComplete(ZoneEventArgs args)
        {
            _zonesAnimating--;
            if (_zonesAnimating == 0)
            {
                ScoreZonesAllAnimatedAction?.Invoke(this, new ZoneEventArgs(ZoneType.All));
            }
        }

        public ZoneType GetZone(float anchoredPositionX)
        {
            var perfectRange = Perfect.RectTransform.sizeDelta.x/2f;
            if (anchoredPositionX < perfectRange && anchoredPositionX > -perfectRange)
            {
                return ZoneType.Perfect;
            }

            var goodRange = Good.RectTransform.sizeDelta.x/2f;
            if (anchoredPositionX < goodRange && anchoredPositionX > -goodRange)
            {
                return ZoneType.Good;
            }

            var limitRange = Limit.RectTransform.sizeDelta.x/2f;
            if (anchoredPositionX < limitRange && anchoredPositionX > -limitRange)
            {
                return ZoneType.Limit;
            }

            return ZoneType.Fail;
        }
    }
}

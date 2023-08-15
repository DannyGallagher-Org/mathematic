using UnityEngine;

namespace Code.Definitions
{
    public static class GameDefs
    {
        public const float AnimationBaseSpeed = 0.33f;
        
        public const string TitleViewKey = "TITLE_VIEW";
        public const string HudViewKey = "GAME_HUD";
        public const string LoadingViewKey = "LOADING_VIEW";
        public const string BigGameButtonKey = "BigGameButton";

        public const string SaveHighScoreKey = "SAVE_HIGH_SCORE";

        public static Vector2 PerfectRange = new Vector2(0.05f, 0.15f);
        public static Vector2 GoodRange = new Vector2(0.35f, 0.5f);
        public static Vector2 LimitRange = new Vector2(0.6f, 0.8f);
        public static Vector2 FailRange = new Vector2(1f, 1f);
    }
}

using System;
using System.Collections.Generic;

namespace Glidrive.Player
{
    [Serializable]
    public struct PlayerMoveSpeed
    {
        public float InitialSpeed;  // 初速
        public float BaseSpeed;     // 基準速度
        public float MaxSpeed;      // 最大速度
        public float MinSpeed;      // 最低速度
        public acceleration Acceleration; // 加速度

        [Serializable]
        public struct acceleration
        {
            public float Straight;  // 直線
            public float Curve;     // カーブ
            public float Ring;      // リング通過
        }
    }
}

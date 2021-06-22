using System;
using System.Collections.Generic;
using UnityEngine;

// 参考サイト : http://nnana-gamedev.hatenablog.com/entry/2017/09/07/012721

namespace Glidrive.Player
{
    /// <summary> 移動速度計算機 </summary>
    [Serializable] public class MoveSpeedCalculator
    {
        private readonly PlayerMoveSpeed speedData;          // 速度に関する初期データ
        private readonly PlayerFlightData flightData = null; // 現在の飛行データ

        private float previousSpeed = 0; // 前フレームの速度

        public MoveSpeedCalculator(PlayerMoveSpeed speed, PlayerFlightData flightData) {
            speedData = speed;
            this.flightData = flightData;
            previousSpeed = speed.InitialSpeed; // 初速度を前フレームの速度とする
        }

        // TODO : BaseSpeedに戻る処理を書くこと。
        public float Speed()
        {
            var speed = previousSpeed;

            if (flightData.IsCurving) // カーブ中なら
            {
                speed += speedData.Acceleration.Curve * Time.deltaTime; // 加速度*時間 を速度に加える
            }
            else
            {
                speed += speedData.Acceleration.Straight * Time.deltaTime;
            }

            speed = Mathf.Clamp(speed, speedData.MinSpeed, speedData.MaxSpeed); // 速度を丸める
            previousSpeed = speed;
            return speed;
        }
    }
}

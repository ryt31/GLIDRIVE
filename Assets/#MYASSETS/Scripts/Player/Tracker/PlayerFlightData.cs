using System;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Glidrive.Player
{
    // 飛行中のプレイヤー情報を計測する
    public class PlayerFlightData : MonoBehaviour
    {
        /// <summary> 累計飛行時間 </summary>
        public float RidingTime => ridingTime;
        [SerializeField] private float ridingTime = 0.0f;

        /// <summary> 水平方向のデータ, 使用軸はY </summary>
        public directionData HorizontalData => horizontalData;
        [SerializeField] private directionData horizontalData;

        /// <summary> 垂直方向のデータ, 使用軸はX </summary>
        public directionData VerticalData => verticalData;
        [SerializeField] private directionData verticalData;

        [SerializeField] private PlayerMove playerMove = null;
        private DirectionDataCalculator calculator = new DirectionDataCalculator(); // 向き計算機

        /// <summary> 現在角でカーブ中かどうか </summary>
        public bool IsCurving => HorizontalData.Type != directionData.type.middle;

        private void OnValidate()
        {
            if (playerMove == null)
                playerMove = GetComponent<PlayerMove>();
        }

        private void Awake()
        {
            if (playerMove == null)
                playerMove = FindObjectOfType<PlayerMove>();

            // ディスクに乗っているときにデータの更新を行う
            this.UpdateAsObservable()
                .Where(_ => playerMove.IsRiding.Value)
                .Subscribe(_ =>UpdateFlightDatas())
                .AddTo(gameObject);
        }

        /// <summary> 飛行データの更新を行う関数 </summary>
        private void UpdateFlightDatas()
        {
            var dir = playerMove.MoveRotation.eulerAngles; // 現在角を取得

            ridingTime += Time.deltaTime;

            UpdateReferenceAngle(); // 基準角の更新を行う

            horizontalData = calculator.UpdateValues(horizontalData, dir); // 現在角を元にデータの更新
            verticalData = calculator.UpdateValues(verticalData, dir);
        }

        private void UpdateReferenceAngle()
        {
            var dir = playerMove.RotaterRotation.eulerAngles; // 基準角を取得

            horizontalData.ReferenceAngle = dir.y; // 基準角の更新
            verticalData.ReferenceAngle = dir.x;
        }

        /// <summary> 向きデータ </summary>
        [Serializable] public struct directionData
        {
            public axis RotationalAxis; // 回転軸
            public float AngleLine; // 向き変化の境界角度
            public float ReferenceAngle; // 基準角
            public type Type;
            public float Time; // 経過時間

            public directionData(axis axis, float line, float reference, type type, float time = 0)
            {
                RotationalAxis = axis;
                AngleLine = line;
                ReferenceAngle = reference;
                Type = type;
                Time = time;
            }

            public enum axis { x, y, z }
            public enum type { middle = 0, top = 1, bottom = -1 }
        }

        /// <summary> 向き計算機 </summary>
        private class DirectionDataCalculator
        {
            /// <summary> 直進中かどうかを返す </summary>
            public bool IsStraight(Vector3 dir, directionData data)
            {
                var angle = 0.0f;
                switch (data.RotationalAxis)
                {
                    case directionData.axis.x:
                        angle = Angle(data.ReferenceAngle, dir.x);
                        break;
                    case directionData.axis.y:
                        angle = Angle(data.ReferenceAngle, dir.y);
                        break;
                    case directionData.axis.z:
                        angle = Angle(data.ReferenceAngle, dir.z);
                        break;
                    default:
                        break;
                }
                return angle <= data.AngleLine;
            }

            /// <summary> 現在データを返す </summary>
            /// <param name="data">基準データ</param>
            /// <param name="dir">現在角</param>
            /// <param name="deltaTime">前フレームとの時差</param>
            /// <returns></returns>
            public directionData UpdateValues(directionData data, Vector3 dir)
            {
                var result = data;
                var type = DirectionType(dir, data);

                if (data.Type == type) // 向きの変更なし
                {
                    result.Time += Time.deltaTime;
                }
                else // 向きの変更あり
                {
                    result.Type = type;
                    result.Time = Time.deltaTime;
                }

                return result;
            }

            /// <summary> toからfromまでの角度を返す(度数法) </summary>
            private float Angle(float to, float from)
            {
                var distance = to - from;
                var distanceAbs = Mathf.Abs(distance);
                var result = distance;

                if ((360 / 2) < distanceAbs)
                {
                    if (distance < 0)
                    {
                        result += 360;
                    }

                    if (distance > 0)
                    {
                        result -= 360;
                    }
                }

                return result;
            }

            /// <summary> 引数向きの際の種類を返す </summary>
            /// <param name="dir"> 想定する向き </param>
            private directionData.type DirectionType(Vector3 dir, directionData data)
            {
                var angle = 0.0f;
                switch (data.RotationalAxis)
                {
                    case directionData.axis.x:
                        angle = Angle(dir.x, data.ReferenceAngle);
                        break;
                    case directionData.axis.y:
                        angle = Angle(dir.y, data.ReferenceAngle);
                        break;
                    case directionData.axis.z:
                        angle = Angle(dir.z, data.ReferenceAngle);
                        break;
                    default:
                        break;
                }

                if (Mathf.Abs(angle) < data.AngleLine) // middle
                {
                    return directionData.type.middle;
                }
                else if (angle > 0) // top
                {
                    return directionData.type.top;
                }
                else // bottom
                {
                    return directionData.type.bottom;
                }
            }
        }
    }
}

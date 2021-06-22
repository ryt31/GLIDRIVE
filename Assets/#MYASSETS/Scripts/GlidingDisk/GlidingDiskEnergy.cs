using UniRx;
using UnityEngine;

namespace Glidrive.GlidingDisk
{
    public class GlidingDiskEnergy : BaseGlidingDisk
    {
        public AnimationCurve curve;
        private ReactiveProperty<float> remainEnergy;

        public ReactiveProperty<float> RemainEnergy
        {
            get => remainEnergy;
            set => remainEnergy.Value = value.Value;
        }

        protected override void OnInitialize()
        {
            remainEnergy = new ReactiveProperty<float>(glidingDiskParameters.maxEnergy);
        }

        /// <summary>
        ///     エネルギーを減少させる
        /// </summary>
        /// <param name="rotation">上下方向のカメラのrotationの値</param>
        public void AutoDecreaseEnergy(float rotation)
        {
            remainEnergy.Value -= CalculateDecreaseEnergy(rotation);
        }

        /// <summary>
        ///     エネルギーの減少量を算出する
        /// </summary>
        /// <param name="rotation">上下方向のカメラのrotationの値</param>
        /// <returns></returns>
        private float CalculateDecreaseEnergy(float rotation)
        {
            var decreaseValue = curve.Evaluate(rotation);
            return decreaseValue;
        }
    }
}
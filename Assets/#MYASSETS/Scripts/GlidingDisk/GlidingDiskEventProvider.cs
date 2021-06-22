using UniRx;
using UnityEngine;

namespace Glidrive.GlidingDisk
{
    public class GlidingDiskEventProvider : MonoBehaviour
    {
        private GlidingDiskThrow diskThrow;
        public IReadOnlyReactiveProperty<bool> IsThrow => diskThrow.IsThrow;

        private GlidingDiskEnergy remainEnergy;
        public ReactiveProperty<float> RemainEnergy
        {
            get => remainEnergy.RemainEnergy;
            set => remainEnergy.RemainEnergy.Value = value.Value;
        }

        private void Awake()
        {
            diskThrow = GetComponent<GlidingDiskThrow>();
            remainEnergy = GetComponent<GlidingDiskEnergy>();
        }

        public void DecreaseEnergy(float hmdRotation)
        {
            remainEnergy.AutoDecreaseEnergy(hmdRotation);
        }
    }
}
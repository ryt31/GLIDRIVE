using System;
using UnityEngine;

namespace Glidrive.GlidingDisk
{
    internal class GlidingDiskParameters : MonoBehaviour
    {
        [SerializeField] private GlidingDiskParam param;

        public GlidingDiskParam glidingDiskParam => param;
    }

    [Serializable]
    public struct GlidingDiskParam
    {
        public float maxEnergy; // エネルギー最大量
    }
}
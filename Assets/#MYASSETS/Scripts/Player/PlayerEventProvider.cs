using UniRx;
using UnityEngine;

namespace Glidrive.Player
{
    public class PlayerEventProvider : MonoBehaviour
    {
        private PlayerCore core;
        private PlayerMove move;
        public Transform HmdTransform => core.hmdTransform;
        public Collider BodyCollider => core.BodyCollider;
        public IReadOnlyReactiveProperty<bool> IsRiding => move.IsRiding;

        private void Awake()
        {
            core = GetComponent<PlayerCore>();
            move = GetComponent<PlayerMove>();
        }
    }
}
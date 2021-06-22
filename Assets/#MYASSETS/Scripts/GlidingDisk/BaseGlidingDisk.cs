using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Glidrive.GlidingDisk
{
    public abstract class BaseGlidingDisk : MonoBehaviour
    {
        // ディスクのRigidBody
        protected Rigidbody diskRigidBody;

        // ディスクの各パラメータ
        protected GlidingDiskParam glidingDiskParameters;

        // フリスビーが地面に触れているかどうかのイベント
        private readonly ReactiveProperty<bool> isGround = new ReactiveProperty<bool>();
        public IReadOnlyReactiveProperty<bool> IsGround => isGround;
        public GlidingDiskParam GlidingDiskParameters => glidingDiskParameters;
        public Vector3 DiskVelocity => diskRigidBody.velocity;

        private void Awake()
        {
            diskRigidBody = GetComponent<Rigidbody>();
            glidingDiskParameters = GetComponent<GlidingDiskParameters>().glidingDiskParam;

            // 地面に着地したとき
            this.OnCollisionEnterAsObservable()
                .Where(x => x.gameObject.tag == "Ground")
                .Subscribe(_ =>
                {
                    isGround.Value = true;
                    diskRigidBody.constraints = RigidbodyConstraints.None;
                }).AddTo(gameObject);

            // 地面から離れたとき
            this.OnCollisionExitAsObservable()
                .Where(x => x.gameObject.tag == "Ground")
                .Subscribe(_ =>
                {
                    isGround.Value = false;
                    diskRigidBody.constraints =
                        RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
                }).AddTo(gameObject);

            OnInitialize();
        }

        protected abstract void OnInitialize(); // 派生クラスでまとめて初期化
    }
}
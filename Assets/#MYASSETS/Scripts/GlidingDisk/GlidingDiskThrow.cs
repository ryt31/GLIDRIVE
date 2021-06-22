using System.Collections;
using UniRx;
using UnityEngine;
using Valve.VR.InteractionSystem;

namespace Glidrive.GlidingDisk
{
    [RequireComponent(typeof(Interactable))]
    public class GlidingDiskThrow : BaseGlidingDisk, IThrowable, IEventSwitchable
    {
        private const Hand.AttachmentFlags attachmentFlags = 
            Hand.AttachmentFlags.ParentToHand | Hand.AttachmentFlags.TurnOnKinematic;

        // 投げたか投げていないか
        [SerializeField] private BoolReactiveProperty isThrow = new BoolReactiveProperty(false);
        public IReadOnlyReactiveProperty<bool> IsThrow => isThrow;
        private readonly AnimationCurve scaleReleaseVelocityCurve = AnimationCurve.EaseInOut(0.0f, 0.1f, 1.0f, 1.0f);

        private bool attached; // ディスクを持っているかどうか
        private Vector3 attachPosition; // ディスクを持ったときの位置を保持
        private Quaternion attachRotation; // ディスクを持ったときの回転情報を保持
        
        private Vector3 startLocalScale; // 初期のディスクの大きさ
        private Vector3 targetLocalScale; // 変化後のディスクの大きさ
        private const float changeSpeed = 2.0f; // ディスクのサイズ変更速度

        private Interactable interactable;
        private VelocityEstimator velocityEstimator; // コントローラの速度、加速度、角速度を計測可能

        private GlidingDiskAudio audio;

        protected override void OnInitialize()
        {
            velocityEstimator = GetComponent<VelocityEstimator>();
            interactable = GetComponent<Interactable>();
            audio = GetComponent<GlidingDiskAudio>();
            startLocalScale = transform.localScale;
            targetLocalScale = new Vector3(0.4f, 0.025f, 0.4f);
        }
        
        public void SwitchEvent(string flagName, bool flag)
        {
            if (flagName == "IsThrow" && !flag) isThrow.Value = flag;
        }

        // ディスクを持っている間
        public void HandAttachedUpdate(Hand hand)
        {
            if (hand.IsGrabEnding(gameObject)) hand.DetachObject(gameObject);
        }

        // ディスクにHoverしている間
        public void HandHoverUpdate(Hand hand)
        {
            if (attached || isThrow.Value) return;    // ディスクを持っているときと投げたあとは処理しない
            var startingGrabType = hand.GetGrabStarting();

            if (startingGrabType == GrabTypes.None) return;
            hand.AttachObject(gameObject, startingGrabType, attachmentFlags);

            // ディスクが小さくなるときの処理
            ShrinkDiskScale();
        }

        // ディスクを持った時
        public void OnAttachedToHand(Hand hand)
        {
            attached = true;
            hand.HoverLock(null);
            diskRigidBody.interpolation = RigidbodyInterpolation.None;

            // 速度計測開始
            if (velocityEstimator != null)
                velocityEstimator.BeginEstimatingVelocity();

            attachPosition = transform.position;
            attachRotation = transform.rotation;
        }

        // ディスクを離した時
        public void OnDetachedFromHand(Hand hand)
        {
            attached = false;
            hand.HoverUnlock(null);

            GetReleaseVelocities(hand, out var velocity);
            diskRigidBody.velocity = velocity;

            // 速度が一定の速さ以上ではないなら初期位置にディスクを戻す
            if (diskRigidBody.velocity.magnitude <= 2.0f)
            {
                diskRigidBody.velocity = Vector3.zero;
                diskRigidBody.angularVelocity = Vector3.zero;
                transform.position = attachPosition;
                transform.rotation = attachRotation;

                // ディスクが大きくなる処理
                EnlargeDiskScale();
            }
            else
            {
                isThrow.Value = true;
                audio.ShotAudio();
            }
        }

        // ディスクにHoverした時
        public void OnHandHoverBegin(Hand hand)
        {
            if (attached || isThrow.Value) return;    // ディスクを持っているときと投げたあとは処理しない
            //interactable.CreateHighlightRenderers();
            //interactable.UpdateHighlightRenderers();

            var bestGrabType = hand.GetBestGrabbingType();
            if (bestGrabType != GrabTypes.None) hand.AttachObject(gameObject, bestGrabType, attachmentFlags);
        }

        // ディスクのvelocityを計算
        private void GetReleaseVelocities(Hand hand, out Vector3 velocity)
        {
            if (velocityEstimator != null)
            {
                velocityEstimator.FinishEstimatingVelocity();
                velocity = velocityEstimator.GetVelocityEstimate();
            }
            else
            {
                Debug.LogWarning(
                    "[SteamVR Interaction System] Throwable: No Velocity Estimator component on object but release style set to short estimation. Please add one or change the release style.");
                velocity = diskRigidBody.velocity;
            }

            const float scaleReleaseVelocity = 3.0f;
            const float scaleReleaseVelocityThreshold = 7.0f;
            // 初速度によってディスクの速度が変わる
            var scaleFactor =
                Mathf.Clamp01(scaleReleaseVelocityCurve.Evaluate(velocity.magnitude / scaleReleaseVelocityThreshold));
            velocity *= scaleFactor * scaleReleaseVelocity;
        }

        // ディスクを拡大する
        private void EnlargeDiskScale()
        {
            //StartCoroutine(EnlargeDisk()); //ディスク拡大のコルーチン
        }

        // ディスクを縮小する
        private void ShrinkDiskScale()
        {
            //StartCoroutine(ShrinkDisk()); //ディスク縮小のコルーチン
        }

        // ディスク拡大のコルーチン
        private IEnumerator EnlargeDisk()
        {
            var time = 0.5f;
            var rate = 1.0f / time * changeSpeed;
            while (time < 1.0f)
            {
                time += Time.deltaTime * rate;
                transform.localScale = Vector3.Lerp(targetLocalScale, startLocalScale, time);
                yield return null;
            }
        }

        // ディスクを縮小するコルーチン
        private IEnumerator ShrinkDisk()
        {
            var time = 0.5f;
            var rate = 1.0f / time * changeSpeed;
            while (time < 1.0f)
            {
                time += Time.deltaTime * rate;
                transform.localScale = Vector3.Lerp(startLocalScale, targetLocalScale, time);
                yield return null;
            }
        }
    }
}
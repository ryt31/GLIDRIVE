using System;
using System.Collections;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Valve.VR;

namespace Glidrive.Player
{
    [RequireComponent(typeof(PlayerFlightData))]
    public class PlayerMove : BasePlayerComponent, IEventSwitchable
    {
        private const float RIDE_FROM_THROW_TIME = 1.5f; // 投げてから乗るまでの時間

        // ディスクに乗った時、降りた時のイベント
        [SerializeField] private BoolReactiveProperty isRiding = new BoolReactiveProperty(false);

        private Coroutine coroutine; // コルーチン     

        public IReadOnlyReactiveProperty<bool> IsRiding => isRiding;

        // 速度に関するデータ
        public PlayerMoveSpeed MoveSpeed => moveSpeed;
        [SerializeField] private PlayerMoveSpeed moveSpeed = new PlayerMoveSpeed();
        private MoveSpeedCalculator speedCalculator;            // 速度計算機
        private PlayerTransformRotater transformRotater = null; // 基準軸用のTransform
        
        // 移動方向
        private Vector3 MoveDirection => MoveTransfrom.forward;
        // 現在の回転角
        public Quaternion MoveRotation => MoveTransfrom.rotation;
        // 回転の基準角
        public Quaternion RotaterRotation => transformRotater.transform.rotation;

        private void OnValidate()
        {
            if (transformRotater == null) // 基本的にはPlayer直下のオブジェクトから取得
                transformRotater = GetComponentInChildren<PlayerTransformRotater>();
        }

        /// <summary>
        ///     ディスクに乗るフラグをTrueに
        /// </summary>
        public void SwitchEvent(string flagName, bool flag)
        {
            if (flagName == "IsRiding" && flag)
                if (coroutine == null)
                    coroutine = StartCoroutine(SwitchOnIsRideCoroutine());
        }

        protected override void OnInitialize()
        {
            // ディスクに乗っている間のイベント
            this.FixedUpdateAsObservable()
                .Where(_ => isRiding.Value)
                .Subscribe(_ =>
                {
                    var force = speedCalculator.Speed();//600.0f;
                    var velocity = new Vector3(MoveDirection.x * force, MoveDirection.y * (force / 2),
                        MoveDirection.z * force);
                    playerRigidBody.AddForce(velocity, ForceMode.Force);
                    Stop();
                }).AddTo(gameObject);
            
            // 体のコライダーとの衝突のイベント(OnTrigger)
            pCore.BodyCollider.OnTriggerEnterAsObservable()
                .Subscribe(collider =>
                {
                    var i = collider.GetComponent<ICollisionRunnable>();
                    i?.CollisionRun();
                }).AddTo(gameObject);

            // ステージオブジェクトなどに衝突したとき
            // TODO 暗転処理を加える
            this.OnCollisionEnterAsObservable()
                .Subscribe(collision =>
                {
                    try
                    {
                        // 親要素のIDarkableを取得（親要素がなければ例外が飛ぶ）
                        if (collision.transform.parent.TryGetComponent<IDarkable>(out var i))
                        {
                            //i.FadeOut();
                            coroutine = StartCoroutine(CollidingReStartCoroutine(1.5f));
                        }
                        else
                        {
                            // 衝突したオブジェクトからIDarkableを取得
                            if (collision.gameObject.TryGetComponent<IDarkable>(out var j))
                                //j.FadeOut();
                                coroutine = StartCoroutine(CollidingReStartCoroutine(1.5f));
                        }
                    }
                    catch (NullReferenceException e)
                    {
                        DebugLogger.Log("親要素が存在しない=" + e.Message);
                    }
                });

            // 計算機をインスタンス
            speedCalculator = new MoveSpeedCalculator(moveSpeed, GetComponent<PlayerFlightData>());

            // まだ取得されていないとき子オブジェクトから検索して取得する
            if (transformRotater == null)
                transformRotater = GetComponentInChildren<PlayerTransformRotater>();
        }

        /// <summary>
        ///     rigidbodyのvelocityを0にする
        /// </summary>
        private void Stop()
        {
            playerRigidBody.velocity = Vector3.zero;
        }

        /// <summary>
        ///     衝突時，ゲーム復帰するルーチン
        /// </summary>
        /// <param name="duration">時間を指定: 秒</param>
        /// <returns></returns>
        private IEnumerator CollidingReStartCoroutine(float duration)
        {
            var elapsedTime = 0.0f; // 経過時間
            var currentPos = transform.position; // 現在のposition
            var targetPos = playerPositionLog.GetFirstElement(); // 目標のposition
            var timeRatio = elapsedTime / duration; // 指定時間に対する経過時間の割合

            while (timeRatio < 1.0f)
            {
                elapsedTime += Time.deltaTime;
                timeRatio = elapsedTime / duration; // 指定時間に対する経過時間の割合

                transform.position = Vector3.Lerp(currentPos, targetPos, timeRatio);
                yield return null;
            }

            coroutine = null;
        }

        /// <summary>
        ///     ディスクに乗るフラグをTrueに切り替えるコルーチン
        /// </summary>
        private IEnumerator SwitchOnIsRideCoroutine()
        {
            yield return new WaitForSeconds(RIDE_FROM_THROW_TIME);

            isRiding.Value = true;

            if (coroutine == null) yield break;
            StopCoroutine(coroutine);
            coroutine = null;
        }

        // リスポン時に各種フラグをリセットしたいとき使用
        protected override void ResetFlagToRespawn()
        {
            isRiding.Value = false;
            Stop(); // 動き続けないように止める
        }

        private Transform MoveTransfrom
        {
            get
            {
                Transform result;
                // トラッカーを使用しない場合はHMDを使用
                if (pCore.TrackerInfo == null)
                    result = pCore.hmdTransform;
                else
                    result = pCore.TrackerInfo.TrackerDictionary[SteamVR_Input_Sources.Waist] ?? pCore.hmdTransform;
                return result;
            }
        }
    }
}
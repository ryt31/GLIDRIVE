using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Glidrive.Player
{
    public abstract class BasePlayerComponent : MonoBehaviour
    {
        public delegate void Respawn(Vector3 v, Vector3 q); // リスポンメソッドへの参照（デリゲート）

        protected readonly PlayerPositionLog playerPositionLog = new PlayerPositionLog(10);

        protected PlayerCore pCore;
        private PlayerEventProvider playerEventProvider;
        protected Rigidbody playerRigidBody;
        private Collider bodyCollider;

        private void Awake()
        {
            pCore = GetComponent<PlayerCore>();
            playerEventProvider = GetComponent<PlayerEventProvider>();
            playerRigidBody = GetComponent<Rigidbody>();
            bodyCollider = GetComponent<Collider>();

            // プレイヤーのpositionの履歴をリストに残す
            this.UpdateAsObservable()
                .ThrottleFirst(TimeSpan.FromSeconds(0.2f))
                .Subscribe(_ => { playerPositionLog.AddLog(transform.position); }).AddTo(gameObject);

            OnInitialize();

            Respawn respawn = RespawnPlayer; // リスポンメソッドを登録
            var playerData = new RegisterPlayerData(transform, respawn); // Playerのデータを格納
            CheckPointManager.Instance.GetComponent<IPlayerDataRegisterable>().RegisterPlayer(playerData);
        }

        protected abstract void OnInitialize(); // 派生クラスでまとめて初期化
        protected abstract void ResetFlagToRespawn();

        /// <summary>
        ///     プレイヤーのリスポン処理
        /// </summary>
        /// <param name="respawnPos">リスポンする位置</param>
        public void RespawnPlayer(Vector3 respawnPos, Vector3 direction)
        {
            transform.position = respawnPos; // プレイヤーのポジションをセット
            transform.rotation = Quaternion.LookRotation(direction); // プレイヤーの向きをセット
            pCore.hmdTransform.rotation = Quaternion.LookRotation(direction); // カメラの向きをセット
            ResetFlagToRespawn(); // 各種フラグをリセット(下位クラスとpresenterで調整)
        }
    }
}
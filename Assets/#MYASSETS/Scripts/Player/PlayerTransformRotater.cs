using System;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Glidrive.Player
{
    // アタッチしたゲームオブジェクトの角度をPlayerMoveのMoveRotationまで回転させる
    class PlayerTransformRotater : MonoBehaviour
    {
        [SerializeField] private float RotateSpeed = 0.3f; // 回転速度

        private PlayerMove playerMove = null;

        private void OnValidate()
        {
            if (playerMove == null)
                playerMove = transform.parent.GetComponent<PlayerMove>(); // Playerの子オブジェクトにアタッチされている想定
        }

        private void Awake()
        {
            if (playerMove == null)
                playerMove = FindObjectOfType<PlayerMove>(); // アタッチされていない場合はPlayerを検索

            this.UpdateAsObservable() // ディスクに乗っているときの処理
                .Where(_ => playerMove.IsRiding.Value)
                .Subscribe(_ => Rotate())
                .AddTo(gameObject);
        }

        // RotateSpeedで徐々に回転を行う
        private void Rotate()
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, playerMove.MoveRotation, RotateSpeed * Time.deltaTime);
        }
    }
}

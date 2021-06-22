using System;
using System.Collections;
using System.Collections.Generic;
using Glidrive.GlidingDisk;
using Glidrive.Manager;
using Glidrive.Player;
using Glidrive.Util;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class PlayerDiskPresenter : MonoBehaviour
{
    [SerializeField] private PlayerEventProvider player;

    [SerializeField] private GlidingDiskEventProvider disk;
    private IEventSwitchable isRideSwitchEvent;
    private PlayerDiskEffectEmiter effect;
    private RideOnAudio audio;
    private GlidingDiskAnimationController diskAnimation;

    private void Start()
    {
        var diskRigidBody = disk.GetComponent<Rigidbody>();
        var checkPointManager = CheckPointManager.Instance.GetComponent<CheckPointManager>();
        var currentScene = SceneStateManager.Instance.CurrentScene;
        isRideSwitchEvent = player.gameObject.GetComponent<IEventSwitchable>();
        effect = GetComponent<PlayerDiskEffectEmiter>();
        audio = GetComponent<RideOnAudio>();
        diskAnimation = diskRigidBody.gameObject.GetComponent<GlidingDiskAnimationController>();

        disk.IsThrow
            .Where(isThrow => isThrow)
            .Subscribe(_ =>
            {
                StartCoroutine(RideOnDiskRoutine(diskRigidBody, 2.0f));
            });
        
        // ディスクのエネルギーが0以下になったときのイベント
        disk.RemainEnergy
            .Where(energy => energy <= 0)
            .Subscribe(_ =>
            {
                // 一番近い場所にリスポン
                StartCoroutine(OutOfEnergyRoutine(diskRigidBody,checkPointManager));
            }).AddTo(gameObject);
        
        // ディスクのエネルギーが0以下になったときのイベント
        disk.RemainEnergy
            .Where(energy => energy <= 1.5f)
            .ThrottleFirst(TimeSpan.FromMinutes(10.0f))
            .Subscribe(_ =>
            {
                VRFade.Instance.BlackOut(1.0f);
            }).AddTo(gameObject);

        currentScene
            .Where(state => state == SceneStateType.Result)
            .Subscribe(_ =>
            {
                checkPointManager.Respawn();
                StepOffDisk(diskRigidBody);
                disk.gameObject.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
            });

        player.IsRiding
            .SkipLatestValueOnSubscribe()
            .Subscribe(isRide =>
            {
                if (isRide)
                {
                    RideDisk(diskRigidBody);
                }
                else
                {
                    if (!disk.gameObject.TryGetComponent(out IEventSwitchable e))
                        return;
                    e.SwitchEvent("IsThrow", false);
                    StepOffDisk(diskRigidBody);
                }
            });

        // ディスクのエネルギーを減少させることが可能なオブジェクトと衝突したとき
        player.BodyCollider
            .OnCollisionEnterAsObservable()
            .Subscribe(collider =>
            {
                var i = collider.gameObject.GetComponent<IDiskEnergyChangeable>();
                if (i == null) return;
                disk.RemainEnergy.Value -= i.DecreaseEnergy();
            });

        player.HmdTransform
            .UpdateAsObservable()
            .Where(_ => player.IsRiding.Value)
            .Where(_ => disk.RemainEnergy.Value >= 0.0f)
            .ThrottleFirst(TimeSpan.FromSeconds(0.1f))
            .Subscribe(_ => disk.DecreaseEnergy(player.HmdTransform.transform.rotation.x));
    }

    // ディスクに乗る際の処理
    private void RideDisk(Rigidbody diskRigidBody)
    {
        player.transform.position = disk.transform.position;
        diskRigidBody.isKinematic = true;
        diskRigidBody.useGravity = false;
        disk.transform.rotation = Quaternion.Euler(0.0f,0.0f,0.0f); // ディスクの傾きをもとに戻す
        diskAnimation.SpinOnAnimation();
        disk.transform.SetParent(player.transform); // ディスクをプレイヤーの子要素に
    }

    private IEnumerator RideOnDiskRoutine(Rigidbody diskRigidBody, float time)
    {
        yield return new WaitForSeconds(time);
        diskRigidBody.velocity = Vector3.zero;
        StartCoroutine(HighSpeedMove(diskRigidBody.gameObject.transform.position,time));
        isRideSwitchEvent.SwitchEvent("IsRiding", true);
    }

    private IEnumerator HighSpeedMove(Vector3 endPos, float time)
    {
        var epsilon = 0.15f; // 位置補正用定数
        var startPos = player.gameObject.transform.position;
        var elapsedTime = 0.0f;
        effect.MonochromeSaturation(); // 背景モノクロ化
        effect.AccelEffect(true);
        audio.ShotAudio();
        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            var location = (elapsedTime / time) + epsilon;
            effect.Intensity(location);
            player.gameObject.transform.position = Vector3.Lerp(startPos, endPos, location);
            yield return null;
        }
        effect.OriginallySaturation(); // 背景元に戻す
        effect.AccelEffect(false);
    }

    private IEnumerator OutOfEnergyRoutine(Rigidbody diskRigidBody, CheckPointManager checkPointManager)
    {
        yield return new WaitForSeconds(3.0f);
        checkPointManager.Respawn();
        StepOffDisk(diskRigidBody);
    }

    // ディスクから降りる際の処理
    private void StepOffDisk(Rigidbody diskRigidBody)
    {
        disk.gameObject.transform.parent = null; // ディスクを子オブジェクトから削除
        disk.transform.rotation = Quaternion.Euler(0.0f,0.0f,0.0f); // ディスクを地面と平行に
        disk.gameObject.transform.position = player.gameObject.transform.position + new Vector3(0.3f,0.5f,0.0f); // 位置調整
        disk.RemainEnergy.Value = disk.gameObject.GetComponent<GlidingDiskParameters>().glidingDiskParam.maxEnergy;    // エネルギーリセット
        diskRigidBody.isKinematic = false;
        diskAnimation.SpinOffAnimation();
        VRFade.Instance.BlackIn(2.0f);
        //diskRigidBody.useGravity = true;
    }
}
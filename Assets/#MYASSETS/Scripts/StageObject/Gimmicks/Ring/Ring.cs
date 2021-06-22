using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UniRx;

public class Ring : MonoBehaviour, ICollisionRunnable
{
    private List<GameObject> childObject = new List<GameObject>();       // リングの子要素
    private Collider ringCollider;                  // リングのコライダー
    private RingGimmickManager ringGimmickManager;  // リングを監視するマネージャ
    private RingAudio audio;
    private RingEffect effect;

    private void Awake()
    {
        ringGimmickManager = RingGimmickManager.Instance.GetComponent<RingGimmickManager>();
        
        foreach (Transform c in gameObject.transform)
        {
            childObject.Add(c.gameObject);
        }
        ringCollider = GetComponent<Collider>();
        audio = GetComponent<RingAudio>();
        effect = GetComponent<RingEffect>();
    }

    /// <summary>
    /// 衝突時の処理
    /// </summary>
    public void CollisionRun()
    {
        if (ringGimmickManager.ThroughRingCount.Value == 0)
        {
            ringGimmickManager.AllRingAppear();
            ThroughRing();
            Disappear();
            audio.ShotAudio();
            effect.EmitEffect();
        }
        // 最後のリングを通過するとき
        else if (ringGimmickManager.ThroughRingCount.Value >= ringGimmickManager.RING_NUM)
        {
            ThroughRing();
            Disappear();
            audio.ShotAudio();
            effect.EmitEffect();
        }
        // それ以外
        else
        {
            ThroughRing();
            Disappear();
            audio.ShotAudio();
            effect.EmitEffect();
        }
    }
    
    /// <summary>
    /// 子要素とコライダーを使用可能な状態に切り替え
    /// </summary>
    public void Appear()
    {
        ringCollider.enabled = true;
        foreach (var c in childObject)
        {
            c.SetActive(true);
        }
    }
    
    /// <summary>
    /// 子要素とコライダーを使用不能な状態に切り替え
    /// </summary>
    public void Disappear()
    {
        ringCollider.enabled = false;
        foreach (var c in childObject)
        {
            c.SetActive(false);
        }
    }

    /// <summary>
    /// リングを通過したことを通知
    /// </summary>
    private void ThroughRing()
    {
        ringGimmickManager.ThroughRingCount.Value++;
    }

    // TODO 音を鳴らすメソッドを実装してください
    /// <summary>
    /// リングをくぐったとき音を鳴らす
    /// </summary>
    private void ShotSE()
    {
        
    }
}

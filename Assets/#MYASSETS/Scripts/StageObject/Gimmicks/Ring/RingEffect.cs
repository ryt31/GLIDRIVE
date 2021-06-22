using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingEffect : MonoBehaviour,IEffectEmitable
{
    [SerializeField]
    private GameObject accelEffect = default;   // エフェクト
    private const float EFFECT_TIME = 2.0f;     // エフェクト起動時間
    private Coroutine routine = null;

    /// <summary>
    /// エフェクト起動、リングを通過したことを通知
    /// </summary>
    public void EmitEffect()
    {
        routine = StartCoroutine(EmitCoroutine());
    }

    /// <summary>
    /// リング通過時のエフェクトの処理
    /// </summary>
    public IEnumerator EmitCoroutine()
    {
        accelEffect.SetActive(true);
        yield return new WaitForSeconds(EFFECT_TIME);
        accelEffect.SetActive(false);
        StopCoroutine(routine);
    }
}

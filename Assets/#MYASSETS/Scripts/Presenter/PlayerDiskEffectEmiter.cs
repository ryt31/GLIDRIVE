using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerDiskEffectEmiter : MonoBehaviour
{
    [SerializeField] private GameObject accelEffect;
    [SerializeField] private Volume volume;
    [SerializeField] private AnimationCurve vignetteCurve;
    private Vignette vignette;    // 視界の周りを黒くぼかす
    private ColorAdjustments colorAdjustments; // 背景を灰色にする

    private void Start()
    {
        vignette = null;
        colorAdjustments = null;
        volume.profile.TryGet(out vignette);
        volume.profile.TryGet(out colorAdjustments);
    }
    
    // 加速エフェクトのコントロール
    public void AccelEffect(bool isEmit)
    {
        if (isEmit)
        {
            accelEffect.SetActive(true);
        }
        else
        {
            accelEffect.SetActive(false);
        }
    }
    
    // PostProcessのVignetteコンポーネントのIntensityの値をセット
    public void Intensity(float value)
    {
        vignette.intensity.value = vignetteCurve.Evaluate(value);
    }
    
    // 背景をモノクロ化
    public void MonochromeSaturation()
    {
        colorAdjustments.saturation.value = -100;
    }
    
    // 元の色に戻す
    public void OriginallySaturation()
    {
        colorAdjustments.saturation.value = 0;
    }
}

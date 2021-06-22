using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Glidrive.Player
{
    public class PlayerEffectEmitter : BasePlayerComponent
    {
        private const float MAX_VELOCITY = 17.5f; // プレイヤーの最高速度

        [SerializeField] private GameObject postProcessVolume;

        private Vignette vignette; // PostEffectのコンポーネント
        private AnimationCurve vignetteIntensityCurve; // Vignetteの強度のグラフ

        protected override void OnInitialize()
        {
            vignetteIntensityCurve = AnimationCurve.EaseInOut(0.0f, 0.0f, MAX_VELOCITY, 0.8f);

            var volume = postProcessVolume.GetComponent<Volume>();
            vignette = null;
            volume.profile.TryGet(out vignette);

            // 移動速度によって視界の広さを変化させる
            this.UpdateAsObservable()
                .Subscribe(_ =>
                {
                    var vignetteValue = vignetteIntensityCurve.Evaluate(playerRigidBody.velocity.magnitude);
                    Intensity(vignetteValue); // Vignetteの強度を設定
                }).AddTo(gameObject);
            
            // 体のコライダーとの衝突のイベント(OnTrigger)
            this.OnTriggerEnterAsObservable()
                .Subscribe(collider =>
                {
                    var j = collider.GetComponent<IEffectEmitable>();
                    j?.EmitEffect();
                }).AddTo(gameObject);
        }

        // PostProcessのVignetteコンポーネントのIntensityの値をセット
        private void Intensity(float value)
        {
            vignette.intensity.value = value;
        }

        // リスポン時に各種フラグをリセットしたいとき使用
        protected override void ResetFlagToRespawn()
        {
        }
    }
}
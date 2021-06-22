using System.Collections;

public interface IEffectEmitable
{
    void EmitEffect();              // 外部から使用するときはこのメソッドを走らせる
    IEnumerator EmitCoroutine();    // エフェクトの処理をここに書く
}

using UniRx;
using UnityEngine;

public class GimmickManager : ManagerBase<GimmickManager>, IGimmickNumEventProvider
{
    private static IntReactiveProperty gimmickNum = new IntReactiveProperty(0);
    public IReadOnlyReactiveProperty<int> GimmickNum => gimmickNum;

    /// <summary>
    ///     下位クラスでギミックが完了すると実行しギミックの数を減らす
    /// </summary>
    protected void CompleteGimmick()
    {
        if (gimmickNum.Value >= 3) return;
        gimmickNum.Value++;
        DebugLogger.Log(gimmickNum.Value + " : ギミック完了");
    }
}
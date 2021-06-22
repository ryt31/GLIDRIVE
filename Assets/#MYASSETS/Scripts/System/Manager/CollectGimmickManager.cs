using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class CollectGimmickManager : GimmickManager
{
    private const int BLACK_DEBRIS_NUM = 5;    // 黒い破片の数
    private ReactiveProperty<int> blackDebrisNum = new IntReactiveProperty(0);
    public ReactiveProperty<int> BlackDebrisNum => blackDebrisNum;

    private void Start()
    {
        blackDebrisNum
            .Where(num => BLACK_DEBRIS_NUM <= num)
            .First()
            .Subscribe(_ => CompleteGimmick());
    }

    public void CollectDebris()
    {
        blackDebrisNum.Value += 1;
    }
}

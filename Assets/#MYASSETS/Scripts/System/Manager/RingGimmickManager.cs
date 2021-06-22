using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class RingGimmickManager : GimmickManager
{
    private const double LIMIT_SECONDS = 6.0; // 連続でくぐらなければいけない制限時間
    public readonly int RING_NUM = 8;

    [SerializeField] private Ring firstRing;
    // 最初以外のリング
    [SerializeField]
    private List<Ring> listOtherThanFirstRings;

    // リング通過時のイベント
    private IntReactiveProperty throughRingCount = new IntReactiveProperty(0);

    public IntReactiveProperty ThroughRingCount
    {
        get => throughRingCount;
        set => throughRingCount.Value = value.Value;
    }

    private Coroutine coroutine;
    private bool isCompleteGimmick;

    private void Start()
    {
        throughRingCount
            .TimeInterval()
            .Subscribe(time =>
            {
                DebugLogger.Log(throughRingCount.Value);
                if (throughRingCount.Value >= RING_NUM)
                {
                    CompleteRingGimmick();
                }
                else if(throughRingCount.Value == 0)
                {
                    AllRingDisappear();
                    firstRing.Appear();
                }
                else
                {
                    CountThroughRingCount(time.Interval.TotalSeconds);
                }
            }).AddTo(gameObject);
    }

    // 最初以外のリングをすべて出現
    public void AllRingAppear()
    {
        foreach (var r in listOtherThanFirstRings)
        {
            r.Appear();
        }
    }
    
    // 最初以外のリングをすべて消す
    public void AllRingDisappear()
    {
        foreach (var r in listOtherThanFirstRings)
        {
            r.Disappear();
        }
    }

    /// <summary>
    ///     連続で通過したリングをカウントするメソッド
    /// </summary>
    /// <param name="timeSecond">前のOnNext()からの経過時間（秒）</param>
    private void CountThroughRingCount(double timeSecond)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine); // コルーチンを止め
            coroutine = StartCoroutine(MeasureIntervalRingTime()); // コルーチンを新しくスタート
        }
        else
        {
            coroutine = StartCoroutine(MeasureIntervalRingTime());
        }
    }

    /// <summary>
    ///     リング間の経過時間を計測し、制限時間を超過したときの処理を行う
    /// </summary>
    private IEnumerator MeasureIntervalRingTime()
    {
        var elapsedTime = 0.0f; // 経過時間
        // リング間の制限時間まで経過時間を計測
        while (elapsedTime < LIMIT_SECONDS)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // これより下はリング間の制限時間を超過したときの処理
        if (!isCompleteGimmick)
        {
            throughRingCount.Value = 0;
            coroutine = null;
        }
    }

    /// <summary>
    ///     リングを通過するギミックが完了されると実行されるメソッド
    /// </summary>
    private void CompleteRingGimmick()
    {
        isCompleteGimmick = true;
        CompleteGimmick();
    }
}
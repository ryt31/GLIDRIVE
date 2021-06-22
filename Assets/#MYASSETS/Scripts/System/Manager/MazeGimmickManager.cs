using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class MazeGimmickManager : GimmickManager
{
    [SerializeField]
    private Collider goalCollider;
    
    private void Start()
    {
        goalCollider
            .OnTriggerEnterAsObservable()
            .Subscribe(col =>
            {
                if (col.gameObject.CompareTag("Player"))
                {
                    CompleteGimmick();
                }
            }).AddTo(gameObject);
    }
}

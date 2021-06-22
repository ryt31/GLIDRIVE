using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class BlackDebrisPresenter : MonoBehaviour
{
    [SerializeField] private CollectGimmickManager manager;
    private BlackDebrisView view;
    
    private void Start()
    {
        view = GetComponent<BlackDebrisView>();

        manager.BlackDebrisNum
            .Subscribe(num => view.Text(num.ToString()));
    }
}

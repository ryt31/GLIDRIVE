using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEditor;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    private ReactiveProperty<MenuType> menuType = new ReactiveProperty<MenuType>(global::MenuType.TouchToStart);

    public ReactiveProperty<MenuType> MenuType
    {
        get => menuType;
        set => menuType.Value = value.Value;
    }

    private void Start()
    {
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Glidrive.Player;

public class KeyInputEventProvider : BasePlayerComponent, IInputEventProvider
{
    private ReactiveProperty<bool> isGrabPinch = new ReactiveProperty<bool>();
    public IReadOnlyReactiveProperty<bool> IsGrabPinch { get { return isGrabPinch; } }

    protected override void OnInitialize()
    {
    }

    protected override void ResetFlagToRespawn()
    {
    }
}

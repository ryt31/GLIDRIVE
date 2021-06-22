using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Valve.VR;
using Glidrive.Player;

public class VRInputEventProvider : BasePlayerComponent, IInputEventProvider
{
    private ReactiveProperty<bool> isGrabPinch = new ReactiveProperty<bool>();
    public IReadOnlyReactiveProperty<bool> IsGrabPinch { get { return isGrabPinch; } }

    // どの手の状態
    private SteamVR_Input_Sources rightHand = SteamVR_Input_Sources.RightHand;
    private SteamVR_Input_Sources leftHand = SteamVR_Input_Sources.LeftHand;

    // トリガーを引いてるとき(True)
    private SteamVR_Action_Boolean GrabAction = SteamVR_Actions.default_GrabPinch;

    protected override void OnInitialize()
    {
        this.UpdateAsObservable()
            .Select(_ => GrabAction.GetState(rightHand))
            .DistinctUntilChanged()
            .Subscribe(x => isGrabPinch.Value = x);

        this.UpdateAsObservable()
            .Select(_ => GrabAction.GetState(leftHand))
            .DistinctUntilChanged()
            .Subscribe(x => isGrabPinch.Value = x);
    }

    protected override void ResetFlagToRespawn()
    {
    }
}

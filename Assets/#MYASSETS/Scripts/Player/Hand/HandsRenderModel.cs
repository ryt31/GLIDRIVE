using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
using Glidrive.Player;

public class HandsRenderModel : RenderModel
{
    protected override void InitializeHand()
    {
        if (handPrefab != null)
        {
            handInstance = GameObject.Instantiate(handPrefab);
            handInstance.transform.parent = this.transform;
            handInstance.transform.localPosition = Vector3.zero;
            handInstance.transform.localRotation = Quaternion.identity;
            handInstance.transform.localScale = handPrefab.transform.localScale;
            handSkeleton = handInstance.GetComponent<SteamVR_Behaviour_Skeleton>();
            handSkeleton.origin = PlayerCore.Instance.trackingOriginTransform;
            handSkeleton.updatePose = false;
            handSkeleton.skeletonAction.onActiveChange += OnSkeletonActiveChange;

            handRenderers = handInstance.GetComponentsInChildren<Renderer>();
            if (displayHandByDefault == false)
                SetHandVisibility(false);

            handAnimator = handInstance.GetComponentInChildren<Animator>();

            if (handSkeleton.skeletonAction.activeBinding == false && handSkeleton.fallbackPoser == null)
            {
                Debug.LogWarning("Skeleton action: " + handSkeleton.skeletonAction.GetPath() + " is not bound. Your controller may not support SteamVR Skeleton Input. " +
                    "Please add a fallback skeleton poser to your skeleton if you want hands to be visible");
                DestroyHand();
            }
        }
    }

    public override void OnHandInitialized(int deviceIndex)
    {
        controllerRenderModel.SetInputSource(inputSource);
        controllerRenderModel.SetDeviceIndex(deviceIndex);
    }

}

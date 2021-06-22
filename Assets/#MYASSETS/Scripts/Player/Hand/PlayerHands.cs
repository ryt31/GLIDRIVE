//======= Copyright (c) Valve Corporation, All rights reserved. ===============
//
// Purpose: The hands used by the player in the vr interaction system
//
//=============================================================================

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine.Events;
using System.Threading;
using Valve.VR.InteractionSystem;
using Valve.VR;

namespace Glidrive.Player
{
    //-------------------------------------------------------------------------
    // Links with an appropriate SteamVR controller and facilitates
    // interactions with objects in the virtual world.
    //-------------------------------------------------------------------------
    public class PlayerHands : Hand
    {
        private PlayerCore playerCore;
        protected virtual IEnumerator Start()
        {
            // save off player instance
            playerCore = PlayerCore.Instance;
            if (!playerCore)
            {
                Debug.LogError("<b>[SteamVR Interaction]</b> No player instance found in Hand Start()", this);
            }

            if (this.gameObject.layer == 0)
                Debug.LogWarning("<b>[SteamVR Interaction]</b> Hand is on default layer. This puts unnecessary strain on hover checks as it is always true for hand colliders (which are then ignored).", this);
            else
                hoverLayerMask &= ~(1 << this.gameObject.layer); //ignore self for hovering

            // allocate array for colliders
            overlappingColliders = new Collider[ColliderArraySize];

            // We are a "no SteamVR fallback hand" if we have this camera set
            // we'll use the right mouse to look around and left mouse to interact
            // - don't need to find the device
            if (noSteamVRFallbackCamera)
            {
                yield break;
            }

            //Debug.Log( "<b>[SteamVR Interaction]</b> Hand - initializing connection routine" );

            while (true)
            {
                if (isPoseValid)
                {
                    InitController();
                    break;
                }

                yield return null;
            }
        }
    }
}
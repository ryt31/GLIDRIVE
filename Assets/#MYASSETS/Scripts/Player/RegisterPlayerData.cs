using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Glidrive.Player
{
    public class RegisterPlayerData
    {
        private readonly Transform playerTransform;
        public Transform PlayerTransform { get { return playerTransform; } }
        public readonly BasePlayerComponent.Respawn respawn;

        public RegisterPlayerData(Transform playerTransform, BasePlayerComponent.Respawn respawn)
        {
            this.playerTransform = playerTransform;
            this.respawn = respawn;
        }
    }
}

using System.Collections;
using System.Linq;
using Glidrive.Manager;
using UniRx;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

namespace Glidrive.Player
{
    public class PlayerCore : MonoBehaviour
    {
        // シングルトン
        private static PlayerCore instance;
        public Transform trackingOriginTransform;

        [SerializeField] private Transform[] hmdTransforms; // HMDのリスト VR:HMD 3DDebug:Camera(FallbackObject)
        [SerializeField] private AudioListener audioListener;

        public TrackerInfo TrackerInfo;
        public GameObject rigSteamVR; // VR用のRig
        public GameObject rig2DFallback; // 3DDebug用のRig
        public bool is3DDebug; // 3DDebug中かどうか

        public Transform hmdTransform
        {
            get
            {
                return hmdTransforms
                    .FirstOrDefault(hmdTransform => hmdTransform.gameObject.activeInHierarchy);
            }
        }
        
        [SerializeField]
        private Collider bodyCollider;
        public Collider BodyCollider
        {
            get { return bodyCollider; }
        }

        public static PlayerCore Instance
        {
            get
            {
                if (instance == null) instance = FindObjectOfType<PlayerCore>();
                return instance;
            }
        }

        private async void Awake()
        {
            if (trackingOriginTransform == null) trackingOriginTransform = transform;
            await StartCoroutine(InitializePlayerInstance());
        }

        // プレイヤーインスタンスの初期化
        private IEnumerator InitializePlayerInstance()
        {
            instance = this;
            while (SteamVR.initializedState == SteamVR.InitializedStates.None ||
                   SteamVR.initializedState == SteamVR.InitializedStates.Initializing)
                yield return null;

            if (SteamVR.instance != null && !is3DDebug)
                ActivateRig(rigSteamVR);
            else
                ActivateRig(rig2DFallback);
        }

        private void ActivateRig(GameObject rig)
        {
            rigSteamVR.SetActive(rig == rigSteamVR);
            rig2DFallback.SetActive(rig == rig2DFallback);
            if (audioListener)
            {
                audioListener.transform.parent = hmdTransform;
                audioListener.transform.localPosition = Vector3.zero;
                audioListener.transform.localRotation = Quaternion.identity;
            }
        }
    }
}
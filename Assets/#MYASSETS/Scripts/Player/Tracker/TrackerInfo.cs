using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

// トラッカーの情報を保持するコンポーネント
public class TrackerInfo : MonoBehaviour
{
    [SerializeField]
    private Transform[] trackerTransforms = default;

    private Dictionary<SteamVR_Input_Sources, Transform> trackerDictionary = new Dictionary<SteamVR_Input_Sources, Transform>();
    public IReadOnlyDictionary<SteamVR_Input_Sources, Transform> TrackerDictionary => trackerDictionary;

    private void Awake()
    {
        SetDictionary();
    }

    private void SetDictionary()
    {
        foreach (var tracker in trackerTransforms)
        {
            var pose = tracker.GetComponent<SteamVR_Behaviour_Pose>();
            Debug.Log(pose +" / "+ tracker);
            trackerDictionary.Add(pose.inputSource, tracker);
        }
    }
}

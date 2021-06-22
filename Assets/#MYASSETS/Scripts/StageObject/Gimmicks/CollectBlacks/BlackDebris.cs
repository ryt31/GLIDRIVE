using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class BlackDebris : MonoBehaviour, ICollisionRunnable
{
    private Coroutine routine = null;
    private CollectGimmickManager manager;
    private CollectAudio audio;
    [SerializeField] private Transform VRCamera;
    
    private void Start()
    {
        manager = GimmickManager.Instance.GetComponent<CollectGimmickManager>();
        audio = GetComponent<CollectAudio>();

        this.UpdateAsObservable()
            .Subscribe(_ =>
            {
                var direction = VRCamera.position - this.transform.position;
                direction.y = 0.0f;
                transform.rotation = Quaternion.LookRotation(direction);
            }).AddTo(gameObject);
    }

    public void CollisionRun()
    {
        if (routine == null)
        {
            manager.CollectDebris();
            audio.ShotAudio();
            routine = StartCoroutine(DestroyRoutine());
        }
    }

    private IEnumerator DestroyRoutine()
    {
        yield return new WaitForSeconds(1.0f);
        Destroy(gameObject);
    }
}

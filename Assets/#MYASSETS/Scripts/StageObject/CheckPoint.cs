using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class CheckPoint : MonoBehaviour, ICollisionRunnable
{
    private CheckPointData checkPointData;
    private CheckPointManager checkPointManager; // チェックポイントを監視するManager
    private CheckPointAudio audio;
    private VisualEffect effect;
    private Coroutine routine = null;
    private bool isCheck = false;

    private void Start()
    {
        checkPointManager = CheckPointManager.Instance.GetComponent<CheckPointManager>();
        checkPointData = new CheckPointData(gameObject.name, transform.position, transform.forward);
        audio = GetComponent<CheckPointAudio>();
        effect = GetComponentInChildren<VisualEffect>();
    }

    public void CollisionRun()
    {
        if (routine == null && !isCheck)
        {
            routine = StartCoroutine(RegisterRoutine());
        }
    }

    private IEnumerator RegisterRoutine()
    {
        RegisterCheckPoint(); // CheckPointData登録
        audio.ShotAudio();
        yield return new WaitForSeconds(3.0f);
        effect.enabled = false;
        routine = null;
    }

    /// <summary>
    ///     コレクションにData登録
    /// </summary>
    private void RegisterCheckPoint()
    {
        checkPointManager.AddCheckPoint(checkPointData);
        isCheck = true;
    }
}
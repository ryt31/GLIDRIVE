using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Glidrive.Manager;
using Glidrive.Util;
using UniRx;
using UnityEngine;

public class Elevator : MonoBehaviour,ICollisionRunnable
{
    private Coroutine routine = null;
    private List<Material> materials;
    private Material baseMaterial;
    private Material emissionMaterial;
    private Collider collider;
    private LineRenderer laser;
    private SceneStateManager manager;

    private void Start()
    {
        materials = GetComponentInChildren<SkinnedMeshRenderer>().materials.ToList();
        baseMaterial = materials[0];
        emissionMaterial = materials[1];
        collider = GetComponent<Collider>();
        laser = GetComponentInChildren<LineRenderer>();
        manager = SceneStateManager.Instance;
        
        // メインゲームに入るとエレベータ消える
        manager.CurrentScene
            .Where(state => state == SceneStateType.Main)
            .Subscribe(_ =>
            {
                Disappear();
            });

        var gimmickNum = GimmickManager.Instance.GimmickNum;
        // ギミックをすべてクリアすると出現
        gimmickNum
            .Where(num => num>=3)
            .Subscribe(_ =>
            {
                Appear();
            });
    }
    
    private IEnumerator AppearRoutine(float time)
    {
        collider.enabled = true;

        var elapsedTime = 0.0f;
        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            baseMaterial.SetFloat("_Cutoff",1.11f*((time-elapsedTime)/time));
            emissionMaterial.SetFloat("_Cutoff",1.11f*((time-elapsedTime)/time));
            yield return null;
        }

        laser.enabled = true;
        routine = null;
    }

    private void Appear()
    {
        if(routine != null) return;
        routine = StartCoroutine(AppearRoutine(3.0f));
    }

    private IEnumerator DisappearRoutine(float time)
    {
        laser.enabled = false;
        collider.enabled = false;
        
        var elapsedTime = 0.0f;
        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            baseMaterial.SetFloat("_Cutoff",1.11f*(elapsedTime/time));
            emissionMaterial.SetFloat("_Cutoff",1.11f*(elapsedTime/time));
            yield return null;
        }
        routine = null;
    }

    private void Disappear()
    {
        if(routine != null) return;
        routine = StartCoroutine(DisappearRoutine(3.0f));
    }

    public void CollisionRun()
    {
        collider.enabled = false;
        manager.TransitionToResult();
    }
}

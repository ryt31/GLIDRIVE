using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class WatchMap : MonoBehaviour
{
    [SerializeField] private Camera mapCamera = null;
    [SerializeField] private Transform playerIcon = null;
    [SerializeField] private Transform goalIcon = null;
    [SerializeField] private int minShootingRange = 10;//zoom時の撮影範囲
    [SerializeField] private int maxShootingRange = 50;//map全体撮影用の範囲
    [SerializeField] private Vector3 defaultIconScale = new Vector3(1.0f, 1.0f, 1.0f);
    private bool isZoomed = true;
    private float zoomMag = 1.0f;
    private Vector3 defaultGoalPos;//ゴールアイコンの初期位置
    private int defaultCameraY = 63;
    private int defaultIconY = 62;

    public void ChangePosition()
    {
        Vector3 toCameraPostion;
        if (isZoomed)
        {
            mapCamera.transform.parent = null;//カメラの追従を切る
            toCameraPostion = new Vector3(0, defaultCameraY, 0);//ワールド座標(0,64,0)から撮影する
            mapCamera.transform.position = toCameraPostion;
            ChangeShootingRange(minShootingRange, maxShootingRange);
            isZoomed = false;

        }
        else
        {
            toCameraPostion = playerIcon.transform.position;//プレイヤの上空前方から撮影する
            toCameraPostion.y = 63; toCameraPostion.z += 5;
            mapCamera.transform.parent = playerIcon.parent.transform;//カメラをプレイヤーに追従させる
            mapCamera.transform.position = toCameraPostion;
            ChangeShootingRange(maxShootingRange, minShootingRange);
            isZoomed = true;
        }
    }
    public void ChangeShootingRange(double nowSize, double toSize)
    {
        zoomMag *=(float)(toSize / nowSize);
        mapCamera.orthographicSize = minShootingRange * zoomMag;
        playerIcon.localScale =defaultIconScale*zoomMag;
        goalIcon.localScale = defaultIconScale*zoomMag;
    }
    private bool CheckInsideMap()
    {
        Vector3 cameraPos = mapCamera.transform.position;
        Vector3 targetPos = goalIcon.position;
        cameraPos.y = targetPos.y = 0;
        return Vector3.Distance(cameraPos, targetPos) <= (minShootingRange-1) * zoomMag;//goalがマップの範囲内にあるか否かを返す

    }
    private IEnumerator DispIcon()
    {
        while (true)
        {
            if (CheckInsideMap())
            {
                goalIcon.position = defaultGoalPos;
            }
            else
            {
                Vector3 cameraPos = mapCamera.transform.position;
                Vector3 offset = goalIcon.position - cameraPos;//カメラからゴールに向けたベクトルを計算
                Vector3 toGoalIconPos = cameraPos + Vector3.ClampMagnitude(offset, (float)(minShootingRange * zoomMag*0.85));//ゴールアイコンの位置を上のベクトルをもとに設定
                toGoalIconPos.y = defaultIconY;
                goalIcon.position = toGoalIconPos;
            }
            yield return new WaitForSeconds(0.2f);//点滅の頻度
        }
    }
    private void Start()
    {
        defaultGoalPos = goalIcon.position;//
        StartCoroutine("DispIcon");
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ChangePosition();
        }
    }
}

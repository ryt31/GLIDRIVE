using UnityEngine;

public class CheckPointData
{
    public CheckPointData(string checkPointName, Vector3 checkPointPos, Vector3 direction)
    {
        CheckPointName = checkPointName;
        CheckPointPos = checkPointPos;
        Direction = direction;
    }

    public string CheckPointName { get; }
    public Vector3 CheckPointPos { get; }
    public Vector3 Direction { get; }
}
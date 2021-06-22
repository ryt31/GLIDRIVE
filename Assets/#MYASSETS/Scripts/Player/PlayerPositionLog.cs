using System.Collections.Generic;
using UnityEngine;

public class PlayerPositionLog
{
    private readonly int logNum; // プレイヤーの移動履歴の数
    private readonly List<Vector3> playerPositionLogs; // プレイヤーの移動履歴を保持

    public PlayerPositionLog(int logNum)
    {
        this.logNum = logNum;
        playerPositionLogs = new List<Vector3>(logNum);
    }

    public IReadOnlyList<Vector3> PositionLogs => playerPositionLogs;

    public void AddLog(Vector3 log)
    {
        if (playerPositionLogs.Count < logNum)
        {
            playerPositionLogs.Add(log);
        }
        else
        {
            playerPositionLogs.RemoveAt(0);
            playerPositionLogs.Add(log);
        }
    }

    public Vector3 GetFirstElement()
    {
        return playerPositionLogs[0];
    }
}
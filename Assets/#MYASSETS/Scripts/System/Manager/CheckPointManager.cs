using System.Collections.Generic;
using System.Linq;
using Glidrive.Player;
using UniRx;
using UnityEngine;

public class CheckPointManager : ManagerBase<CheckPointManager>, IPlayerDataRegisterable
{
    [SerializeField] private List<Transform> checkPoints;

    // CheckPointDataのコレクション
    private readonly ReactiveCollection<CheckPointData> checkPointCollection = new ReactiveCollection<CheckPointData>();
    
    public IReadOnlyReactiveCollection<CheckPointData> CheckPointCollection => checkPointCollection;
    public RegisterPlayerData PlayerData { get; private set; }

    private void Start()
    {
        // コレクションに追加されたときのイベント
        // TODO 多分消す
        checkPointCollection
            .ObserveAdd()
            .Subscribe(point => { DebugLogger.Log(point.Value.CheckPointName); }).AddTo(gameObject);
        
        AddCheckPoint(new CheckPointData("InitialPoint", PlayerData.PlayerTransform.position,
            PlayerData.PlayerTransform.forward));
    }

    // TODO MainGameManagerとか作ってそこで登録してあげても良いかも
    /// <summary>
    ///     BasePlayerComponentから呼び出し情報を登録する
    /// </summary>
    /// <param name="playerData">Playerのデータ</param>
    public void RegisterPlayer(RegisterPlayerData playerData)
    {
        PlayerData = playerData;
    }

    /// <summary>
    ///     コレクションにチェックポイントのCheckPointDataを追加
    /// </summary>
    /// <param name="checkPointData">追加するオブジェクト</param>
    public void AddCheckPoint(CheckPointData checkPointData)
    {
        // チェックポイントの名前がnullなら終了
        if (checkPointData.CheckPointName == null) return;
        // リストの中のオブジェクトの名前を調べる
        var collect = checkPointCollection
            .Select(point => point.CheckPointName)
            .Where(name => name.Equals(checkPointData.CheckPointName))
            .ToList();

        // 重複したものがないなら追加
        if (collect.Count == 0) checkPointCollection.Add(checkPointData);
    }

    /// <summary>
    ///     リスポンできる候補の中から一番近いところにリスポンする
    /// </summary>
    public void Respawn()
    {
        var data = checkPointCollection
            .OrderBy(checkPoint => Vector3.Distance(PlayerData.PlayerTransform.position, checkPoint.CheckPointPos))
            .First();
        PlayerData.respawn(data.CheckPointPos, data.Direction);
    }
}
using ISDevTemplate;
using UnityEngine;
using System;

/// <summary>
/// ポイントの管理
/// </summary>
public class PointManager : SingletonMonoBehaviour<PointManager>
{
    public int Point => _point;

    public bool CanWin => _point >= _pointToWin;

    [SerializeField]
    [Header("勝利するのに必要なポイント数")]
    private int _pointToWin;

    private int _point;

    public event Action<int> OnPointChanged;

    public void AddPoint(int point)
    {
        _point += point;
        OnPointChanged?.Invoke(_point);
    }
}

using System;
using ISDevTemplate;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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

    [SerializeField]
    private Text _pointText;

    [SerializeField]
    private float _textFadeDuration;

    private int _point;

    public event Action<int> OnPointChanged;

    public void AddPoint(int point)
    {
        UpdateText(point);
        _point += point;
        OnPointChanged?.Invoke(_point);
    }

    private void UpdateText(int point)
    {
        int lastPoint = _point;
        _pointText.DOCounter(lastPoint,
            lastPoint + point, _textFadeDuration, true)
            .OnComplete(() => _pointText.text = $"{lastPoint + point}");
    }
}

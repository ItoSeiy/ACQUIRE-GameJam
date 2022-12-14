using System;
using ISDevTemplate;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using ISDevTemplate.Data;

/// <summary>
/// ポイントの管理
/// </summary>
public class PointManager : SingletonMonoBehaviour<PointManager>
{
    public int Point => _point;

    public int PointToWin => _pointToWin;

    public bool CanWin => _point >= _pointToWin;

    [SerializeField]
    [Header("勝利するのに必要なポイント数")]
    private int _pointToWin;

    [SerializeField]
    [Header("勝利するのに必要なポイント数のテキスト")]
    private Text _pointToWinText;

    [SerializeField]
    private Text _pointText;

    [SerializeField]
    private float _textFadeDuration = 0.5f;

    [SerializeField]
    private Text _highScoreText;

    private int _point;

    public event Action<int> OnPointChanged;

    private void Start()
    {
        _pointToWinText.text = _pointToWin.ToString();

        _highScoreText.text = SaveDataManager.Instance.SaveData.HighScore.ToString();
    }

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
            lastPoint + point, _textFadeDuration, false)
            .OnComplete(() => _pointText.text = $"{lastPoint + point}");
    }
}

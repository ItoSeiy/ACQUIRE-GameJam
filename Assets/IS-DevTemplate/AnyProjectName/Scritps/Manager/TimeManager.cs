using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ISDevTemplate.Manager;
using System;

/// <summary>
/// タイムの管理
/// 時間切れで勝利判定
/// </summary>
public class TimeManager : MonoBehaviour
{
    public float Timer => _timer;

    [SerializeField]
    [Header("制限時間")]
    private float _limitTime;

    private float _timer;

    private void Update()
    {
        if (GameManager.Instance.IsGameFinish) return;

        _timer += Time.deltaTime;
        CheckTimeOver();
    }

    private void CheckTimeOver()
    {
        if (_timer < _limitTime) return;

        if(PointManager.Instance.CanWin)
        {
            GameManager.Instance.GameClear();
        }
        else
        {
            GameManager.Instance.GameOver();
        }
    }
}

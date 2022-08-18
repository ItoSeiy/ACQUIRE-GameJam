using ISDevTemplate.Manager;
using UnityEngine;
using UnityEngine.UI;

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

    [SerializeField]
    private Text _timerText;

    private float _timer;

    private void Start()
    {
        _timer = _limitTime;
    }

    private void Update()
    {
        if (GameManager.Instance.IsGameFinish) return;

        UpdateTimer();
        UpdateText();
        CheckTimeOver();
    }

    private void UpdateTimer()
    {
        _timer -= Time.deltaTime;
    }

    private void CheckTimeOver()
    {
        if (_timer <= 0f) return;

        UpdateText(0);

        if(PointManager.Instance.CanWin)
        {
            GameManager.Instance.GameClear();
        }
        else
        {
            GameManager.Instance.GameOver();
        }
    }

    private void UpdateText()
    {
        _timerText.text = _timer.ToString("F0");
    }

    private void UpdateText(float time)
    {
        _timerText.text = time.ToString("F0");
    }
}

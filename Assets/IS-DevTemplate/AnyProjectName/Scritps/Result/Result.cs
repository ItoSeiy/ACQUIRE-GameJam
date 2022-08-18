using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ISDevTemplate.Manager;
using UnityEngine.UI;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using ISDevTemplate.Scene;

public class Result : MonoBehaviour
{
    [SerializeField]
    [Header("ダークヒーローの勝利画像")]
    private Image _darkHeroWinImage;

    [SerializeField]
    [Header("ダークヒーロー敗北時の画像")]
    private Image _darkHereLoseImage;

    [SerializeField]
    [Header("地球が救われた勝利画像")]
    private Image _earthWinImage;

    [SerializeField]
    [Header("地球が救われなかった画像")]
    private Image _earthLoseImage;

    [SerializeField]
    [Header("ダークヒーロー, 地球の画面切り替え時間 (ミリ秒)")]
    private int _imageChangeTime = 3500;

    [SerializeField]
    [Header("ダークヒーロー, 地球の画面切り替えのフェード時間 (ミリ秒)")]
    private int _imageFadeDuration = 500;

    [SerializeField]
    [Header("ポイントのテキスト")]
    private Text _pointText;

    [SerializeField]
    [Header("勝利するのに必要なポイント数のテキスト")]
    private Text _pointToWinText;

    [SerializeField]
    [Header("ポイントのテキストのフェード時間")]
    private float _pointTextFadeDuration = 0.5f;

    [SerializeField]
    [Header("タイトルに戻るボタン")]
    private Button _backTitleButton;

    [SerializeField]
    private string _titleSceneName = "Title";

    private ResultData _resultData;

    private void Start()
    {
        _darkHereLoseImage.gameObject.SetActive(false);

        _backTitleButton.onClick.AddListener(OnBackTitleButton);

        _resultData = GameManager.Instance.ResultData;
        SetResultTexts();
    }

    private async void SetResultTexts()
    {
        if(_resultData.ResultType == ResultType.GameClear)
        {
            _darkHeroWinImage.gameObject.SetActive(true);
            await UniTask.Delay(_imageChangeTime);
            _earthWinImage.DOFade(1f, _imageFadeDuration);
        }
        else
        {
            _darkHereLoseImage.gameObject.SetActive(true);
            await UniTask.Delay(_imageChangeTime);
            _earthLoseImage.DOFade(1f, _imageFadeDuration);
        }

        _pointToWinText.text = $"{_resultData.PointToWin}";

        await _pointText.DOCounter(0, _resultData.Point, _pointTextFadeDuration)
            .OnComplete(() => _pointText.text = $"{_resultData.Point}").AsyncWaitForCompletion();
    }

    private void OnBackTitleButton()
    {
        SceneLoder.Instance.LoadScene(_titleSceneName);
    }
}

public enum ResultType
{
    GameClear,
    GameOver
}

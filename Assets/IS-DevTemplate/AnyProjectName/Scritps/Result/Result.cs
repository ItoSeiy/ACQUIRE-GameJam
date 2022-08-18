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
    [Header("勝利時の地球のセリフ")]
    private string _gameClearEarthDialogue = "ありがとう";

    [SerializeField]
    [Header("敗北時の地球のセリフ")]
    private string _gameOverEarthDialogue = "(´・ω・｀)";

    [SerializeField]
    [Header("地球のセリフのテキスト")]
    private Text _earthDialogueText;

    [SerializeField]
    [Header("地球のセリフのフェード時間")]
    private float _earthDialogueTextFadeDuraiton = 0.8f;

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
        _backTitleButton.onClick.AddListener(OnBackTitleButton);

        _resultData = GameManager.Instance.ResultData;
        SetResultTexts();
    }

    private async void SetResultTexts()
    {
        _pointToWinText.text = $"{_resultData.PointToWin}";

        await _pointText.DOCounter(0, _resultData.Point, _pointTextFadeDuration)
            .OnComplete(() => _pointText.text = $"{_resultData.Point}").AsyncWaitForCompletion();

        if(_resultData.ResultType == ResultType.GameClear)
        {
            _earthDialogueText.DOText(_gameClearEarthDialogue, _earthDialogueTextFadeDuraiton);
        }
        else
        {
            _earthDialogueText.DOText(_gameOverEarthDialogue, _earthDialogueTextFadeDuraiton);
        }
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

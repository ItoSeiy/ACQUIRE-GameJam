using UnityEngine;
using UnityEngine.UI;
using ISDevTemplate.Scene;
using ISDevTemplate.Data;
using DG.Tweening;
using ISDevTemplate;
using ISDevTemplate.Sound;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Title : MonoBehaviour
{
    [SerializeField]
    [Header("初めから ボタン")]
    private Button _startButton;

    //[SerializeField]
    //[Header("続きから ボタン")]
    //private Button _continueButton;

    [SerializeField]
    [Header("終了ボタン")]
    private Button _endButton;

    [SerializeField]
    [Header("クレジットボタン")]
    private Button _creditButton;

    [SerializeField]
    [Header("クレジットを閉じるボタン")]
    private Button _creditCloseButton;

    [SerializeField]
    [Header("クレジットキャンバスグループ")]
    private CanvasGroup _creditCanvasGroup;

    [SerializeField]
    [Header("操作説明ボタン")]
    private Button _controlleExplanationButton;

    [SerializeField]
    [Header("操作説明画像")]
    private Image _controlleExplanationImage;

    private void Start()
    {
        SetButtonEvents();   
    }

    private void SetButtonEvents()
    {
        _startButton.onClick.AddListener(OnStartButton);

        //_continueButton.onClick.AddListener(OnContinueButton);

        _endButton.onClick.AddListener(OnEndButton);

        _creditButton.onClick.AddListener(OnCreditButton);

        _creditCloseButton.onClick.AddListener(OnCreditCloseButton);

        //_controlleExplanationButton.onClick.AddListener(OnControlleExplanationButton);
    }

    private void OnStartButton()
    {
        SoundManager.Instance.UseSFX("TitleButton");
        SceneLoder.Instance.LoadScene(SaveDataManager.Instance.SaveData.SceneName);
    }

    private void OnContinueButton()
    {
        SceneLoder.Instance.LoadScene(SaveDataManager.Instance.SaveData.SceneName);
    }

    private void OnEndButton()
    {
        SoundManager.Instance.UseSFX("TitleButton");
        #if UNITY_EDITOR
        EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

    private void OnCreditButton()
    {
        SoundManager.Instance.UseSFX("TitleButton");
        _creditCanvasGroup.Enable(0.5f);
    }

    private void OnCreditCloseButton()
    {
        SoundManager.Instance.UseSFX("ResultButton");
        _creditCanvasGroup.Disable(0.5f);
    }

    private void OnControlleExplanationButton()
    {
        _controlleExplanationImage.gameObject.SetActive(true);
    }
}

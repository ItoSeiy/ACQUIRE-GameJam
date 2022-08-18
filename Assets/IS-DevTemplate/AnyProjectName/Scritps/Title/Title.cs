using UnityEngine;
using UnityEngine.UI;
using ISDevTemplate.Scene;
using ISDevTemplate.Data;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Title : MonoBehaviour
{
    [SerializeField]
    [Header("初めから ボタン")]
    private Button _startButton;

    [SerializeField]
    [Header("続きから ボタン")]
    private Button _continueButton;

    [SerializeField]
    [Header("終了ボタン")]
    private Button _endButton;

    [SerializeField]
    [Header("クレジットボタン")]
    private Button _creditButton;

    [SerializeField]
    [Header("クレジット画像")]
    private Image _creditImage;

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

        _continueButton.onClick.AddListener(OnContinueButton);

        _endButton.onClick.AddListener(OnEndButton);

        _creditButton.onClick.AddListener(OnCreditButton);

        //_controlleExplanationButton.onClick.AddListener(OnControlleExplanationButton);
    }

    private async void OnStartButton()
    {
        await SaveDataManager.Instance.ResetSaveDataAsync();   
        SceneLoder.Instance.LoadScene(SaveDataManager.Instance.SaveData.SceneName);
    }

    private void OnContinueButton()
    {
        SceneLoder.Instance.LoadScene(SaveDataManager.Instance.SaveData.SceneName);
    }

    private void OnEndButton()
    {
        #if UNITY_EDITOR
        EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

    private void OnCreditButton()
    {
        _creditImage.gameObject.SetActive(true);
    }

    private void OnControlleExplanationButton()
    {
        _controlleExplanationImage.gameObject.SetActive(true);
    }
}

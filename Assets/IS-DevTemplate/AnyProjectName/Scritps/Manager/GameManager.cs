using System;
using ISDevTemplate.Data;
using ISDevTemplate.Scene;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace ISDevTemplate.Manager
{
    public class GameManager : SingletonMonoBehaviour<GameManager>
    {
        public bool IsGameFinish { get; private set; } = false;

        public ResultData ResultData { get; private set; }

        [SerializeField]
        private string _resultSceneName = "Result";

        [SerializeField]
        private int _sceneChangeTime = 1500;

        //[SerializeField]
        //private CanvasGroup _gameClearCanvas;

        //[SerializeField]
        //private Button _onGameClearButton;

        //[SerializeField]
        //private CanvasGroup _gameOverCanvas;

        //[SerializeField]
        //private Button _onGameOverButton;

        //[SerializeField]
        //private float _fadeDuration = 0.5f;

        public event Action OnGameClear;

        public event Action OnGameOver;

        protected override void Awake()
        {
            base.Awake();
            SetEvents();
        }

        /// <summary>
        /// ゲームクリア時に呼ばれる
        /// 
        /// TimeManagerからの呼び出し
        /// </summary>
        [ContextMenu("GameClear")]
        public async void GameClear()
        {
            IsGameFinish = true;
            OnGameClear?.Invoke();

            ResultData = new ResultData(PointManager.Instance.Point,
                PointManager.Instance.PointToWin, ResultType.GameClear);

            print("GameClear");

            await UniTask.Delay(_sceneChangeTime);

            SceneLoder.Instance.LoadScene(_resultSceneName);
        }

        /// <summary>
        /// ゲームオーバー時に呼ばれる
        /// 
        /// TimeManagerからの呼び出し
        /// </summary>
        [ContextMenu("GameOver")]
        public async void GameOver()
        {
            IsGameFinish = true;
            OnGameOver?.Invoke();

            ResultData = new ResultData(PointManager.Instance.Point,
                PointManager.Instance.PointToWin, ResultType.GameOver);

            print("GameOver");

            await UniTask.Delay(_sceneChangeTime);

            SceneLoder.Instance.LoadScene(_resultSceneName);
        }

        private void Init()
        {
            IsGameFinish = false;
        }

        /// <summary>
        /// デリゲートやUniRxのイベントを登録する関数
        /// </summary>
        private void SetEvents()
        {
            // シーンのロード後には毎回初期化を行う
            SceneLoder.Instance.OnLoadEnd += Init;

            // セーブデータの読み込み後の処理の登録
            //SaveDataManager.Instance.OnSaveDataLoded += OnSaveDataLoded;

            // ゲームクリア後に出るNextボタンの処理の登録
            //_onGameClearButton
            //    .OnClickAsObservable()
            //    .TakeUntilDestroy(this)
            //    .ThrottleFirst(TimeSpan.FromSeconds(_fadeDuration))
            //    .Subscribe(_ => OnGameClearButton());

            //// ゲームオーバー後に出るTry Againボタンの処理の登録
            //_onGameOverButton
            //    .OnClickAsObservable()
            //    .TakeUntilDestroy(this)
            //    .ThrottleFirst(TimeSpan.FromSeconds(_fadeDuration))
            //    .Subscribe(_ => OnGameOverButton());
        }

        ///// <summary>
        ///// セーブデータの初回読み込み後にシーンを遷移する
        ///// </summary>
        //public void OnSaveDataLoded(SaveData saveData)
        //{
        //    SceneLoder.Instance.LoadScene(saveData.SceneName);
        //}

        /// <summary>
        /// ゲームクリア後に表示されるボタンの処理
        /// </summary>
        //private void OnGameClearButton()
        //{
        //    _gameClearCanvas.Disable(_fadeDuration);
        //    SceneLoder.Instance.LoadScene(SaveDataManager.Instance.SaveData.SceneName);
        //}

        /// <summary>
        /// ゲームオーバー後に表示されるボタンの処理
        /// </summary>
        //private void OnGameOverButton()
        //{
        //    _gameOverCanvas.Disable(_fadeDuration);
        //    SceneLoder.Instance.LoadScene();
        //}
    }
}

using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.ParticleSystem;
using DG.Tweening;
using System.Net;

public class People : MonoBehaviour
{

    [SerializeField, Tooltip("移動速度")]
    private float _speed = 0.025f;

    [SerializeField, Tooltip("ゲーム画面に出てランダムに動き回るまでの最低時間")]
    float _nextMoveTime = 1.5f;

    [SerializeField, Tooltip("ゲーム画面の横幅")]
    private float _width = 0;

    [SerializeField, Tooltip("ゲーム画面の縦幅")]
    private float _verticalWidth = 0;

    [SerializeField, Tooltip("持っているスコア")]
    private int _score = 0;

    [SerializeField, Tooltip("初めに保持するスケール")]
    private Vector3 _originScale;

    /// <summary>スポーンの中心点</summary>
    public Transform _centerPoint = null;

    /// <summary>攻撃する際にプレイヤーを取得</summary>
    private GameObject _player = null;

    /// <summary>動きのState</summary>
    private MoveState _moveState = MoveState.AfterResporn;

    /// <summary>ランダムな進行方向</summary>
    private Vector3 _randomVelo;

    /// <summary>DoTween用の変数</summary>
    private Tween tween;

    /// <summary> 時間計測用 </summary>
    private float _timer = 0;

    void Update()
    {
        if (!IsActive) { return; }

        _timer += Time.deltaTime;

        switch (_moveState)
        {
            case MoveState.AfterResporn://生成後中心に向かって歩く

                Vector3 vero;

                Vector3 playerPosition;

                if (transform.position.x < _centerPoint.position.x) vero = new Vector3(_speed, 0, 0);
                else vero = new Vector3(-_speed, 0, 0);

                transform.position += vero;

                if (_timer > _nextMoveTime)//一定時間後それぞれの行動に移行
                {
                    _moveState = MoveState.Suction;
                    _player = FindObjectOfType<Player>().gameObject;
                    return;

                    if (Random.Range(0, 2) < 1)
                    {
                        if (this.name == "PeopleSanple(Clone)")//一般人はランダムに移動
                        {
                            _moveState = MoveState.RandamWalk;
                            _randomVelo = new Vector3(Random.value, Random.value, 0).normalized;
                            _timer = 0;
                        }
                        else if (this.name == "Yakuza(Clone)")//ヤクザ類は特攻
                        {
                            _moveState = MoveState.PlayerAtack;
                            _player = FindObjectOfType<Player>().gameObject;
                            _timer = 0;
                        }
                    }
                }
                break;

            case MoveState.RandamWalk://一定間隔で方向をランダムに決めて移動
                transform.position += _randomVelo * _speed;
                if (_timer > _nextMoveTime)
                {
                    if (Random.Range(0, 2) < 1)
                    {
                        _randomVelo = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
                        _timer = 0;
                    }
                }
                break;
            case MoveState.PlayerAtack:
                playerPosition = new Vector3(_player.transform.position.x, _player.transform.position.y, transform.position.z);
                transform.position = Vector3.MoveTowards(transform.position, playerPosition, _speed);
                break;
            case MoveState.Suction:
                playerPosition = new Vector3(_player.transform.position.x, _player.transform.position.y, transform.position.z);
                transform.position = Vector3.MoveTowards(transform.position, playerPosition, _speed);
                tween = transform.DOScale(Vector3.zero, 2f).SetAutoKill(false).OnComplete(SuctionDestroy);//目標のスケール値と演出時間
                break;
        }

        //画面外に出たら削除
        if (transform.position.x > _width + 2 || transform.position.x < -_width - 2
            || transform.position.y > _verticalWidth + 2 || transform.position.y < -_verticalWidth - 2)
        { Destroy(); }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!IsActive) { return; }

        if (name != "PeopleSanple(Clone)") { }
    }

    enum MoveState
    {
        AfterResporn,
        RandamWalk,
        PlayerAtack,
        Suction
    }

    bool _isActrive = false;
    public bool IsActive => _isActrive;

    public void DontMove()//吸い込まれた際などの処理停止
    {
        _isActrive = false;
    }

    public void MoveResom()//行動再開
    {
        _isActrive = true;
    }

    public void Create()//生成時
    {
        _timer = 0.0f;
        transform.localScale = _originScale;
        _isActrive = true;
    }

    public void Destroy()//削除
    {
        _timer = 0;
        _moveState = MoveState.AfterResporn;
        _isActrive = false;
        this.gameObject.SetActive(false);
    }

    private void SuctionDestroy()
    {
        PointManager.Instance.AddPoint(_score); Destroy();
    }
}

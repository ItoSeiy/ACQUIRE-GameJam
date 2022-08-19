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

    [SerializeField, Tooltip("プレイヤーを気絶させる時間")]
    private int _stanTime = 1;

    [SerializeField, Tooltip("初めに保持するスケール")]
    private Vector3 _originScale;

    [SerializeField, Tooltip("戦車の発射間隔")]
    private float _shotInterbal = 5;

    [SerializeField, Tooltip("戦車が発射する砲筒の位置")]
    private Transform[] _launchrPoint;

    /// <summary>スポーンの中心点</summary>
    public Transform _centerPoint = null;

    /// <summary>攻撃する際にプレイヤーを取得</summary>
    private GameObject _player = null;

    /// <summary>動きのState</summary>
    private MoveState _moveState = MoveState.AfterResporn;

    /// <summary>ランダムな進行方向</summary>
    private Vector3 _randomVelo;

    /// <summary>戦車が発射する弾 </summary>
    private GameObject _bullet;

    /// <summary>戦車が発射する際の爆発 </summary>
    private GameObject _shotBomb;

    /// <summary>DoTween用の変数</summary>
    private Tween tween;

    /// <summary> 時間計測用 </summary>
    private float _timer = 0;

    private SpriteRenderer _sp = null;

    private Animator _ani = null;

    private void Start()
    {
        if (this.name == "Tank(Clone)") 
        { 
            _bullet = Resources.Load("Bullet") as GameObject;
            _shotBomb = Resources.Load("ShotBomb") as GameObject;
        }

        _sp = GetComponent<SpriteRenderer>();

        _ani = GetComponent<Animator>();
    }

    void Update()
    {
        if (!IsActive) { return; }

        _timer += Time.deltaTime;

        switch (_moveState)
        {
            case MoveState.AfterResporn://生成後中心に向かって歩く

                Vector3 vero;

                Vector3 playerPosition;

                vero = transform.position.x < _centerPoint.position.x ? new Vector3(_speed, 0, 0) : new Vector3(-_speed, 0, 0);
                _sp.flipX = vero.x > 0 ? true : false;
                transform.position += vero;

                if (_timer > _nextMoveTime)//一定時間後それぞれの行動に移行
                {
                    if (this.name == "CommonPeople(Clone)")//一般人はランダムに移動
                    {
                        _moveState = MoveState.RandamWalk;
                        _randomVelo = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0).normalized;
                        _sp.flipX = _randomVelo.x > 0 ? true : false;
                        _timer = 0;
                    }
                    else if (this.name == "Yakuza(Clone)")//ヤクザ類は特攻
                    {
                        _moveState = MoveState.PlayerAtack;
                        _player = FindObjectOfType<Player>().gameObject;
                        _timer = 0;
                    }
                    else if (this.name == "Tank(Clone)")//戦車は動かずに一定間隔でプレイヤーを攻撃
                    {
                        _moveState = MoveState.Shot;
                        _player = FindObjectOfType<Player>().gameObject;
                        _timer = 10;
                    }
                }
                break;

            case MoveState.RandamWalk://一定間隔で方向をランダムに決めて移動
                transform.position += _randomVelo * _speed;
                if (_timer > _nextMoveTime)
                {
                    if (Random.Range(0, 2) < 1)
                    {
                        _randomVelo = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0).normalized;
                        _sp.flipX = _randomVelo.x > 0 ? true : false;
                        _timer = 0;
                    }
                }
                break;

            case MoveState.PlayerAtack://プレイヤーを追いかけて殴りに行く
                playerPosition = new Vector3(_player.transform.position.x, _player.transform.position.y, transform.position.z);
                transform.position = Vector3.MoveTowards(transform.position, playerPosition, _speed);
                _sp.flipX = transform.position.x - playerPosition.x > 0 ? true : false;
                break;

            //戦車は動かずに一定間隔で弾を発射する
            case MoveState.Shot:
                if (_timer > _shotInterbal)
                {
                    if (!_sp.flipX)
                    {
                        Instantiate(_bullet, _launchrPoint[0].position, Quaternion.identity);
                        Instantiate(_shotBomb, _launchrPoint[0].position, Quaternion.identity);
                    }
                    else
                    {
                        Instantiate(_bullet, _launchrPoint[1].position, Quaternion.identity);
                        Instantiate(_shotBomb, _launchrPoint[1].position, Quaternion.identity);
                    }
                    ISDevTemplate.Sound.SoundManager.Instance.UseSFX("GunShot");
                    _timer = 0;
                }
                break;

            //救引されたらプレイヤーに吸い寄せられるようにする
            //このときスケールを小さくすることで吸われていくのを表現
            case MoveState.Suction:
                playerPosition = new Vector3(_player.transform.position.x, _player.transform.position.y, transform.position.z);
                transform.position = Vector3.MoveTowards(transform.position, playerPosition, _speed);
                tween = transform.DOScale(Vector3.zero, 0.1f).SetAutoKill(false).OnComplete(SuctionDestroy);//目標のスケール値と演出時間
                break;
        }

        //画面外に出たら削除
        if (transform.position.x > _width + 2 || transform.position.x < -_width - 2
            || transform.position.y > _verticalWidth + 2 || transform.position.y < -_verticalWidth - 2)
        { Destroy(); }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!_isActrive) { return; }

        if (name == "Yakuza(Clone)" && collision.gameObject.name == "Player")
        {
            _ani.Play("Attack");
            ISDevTemplate.Sound.SoundManager.Instance.UseSFX("Hit");
            collision.gameObject.GetComponent<Player>().Stan(_stanTime);
        }
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (name == "CommonPeople(Clone)" && collision.gameObject.name == "Player" 
    //        || name == "CommonPeople(Clone)" && collision.gameObject.name == "Wall")
    //    { 
    //        _moveState = MoveState.RandamWalk;
    //        _randomVelo = new Vector3(-_randomVelo.x, -_randomVelo.y, _randomVelo.z);
    //        _timer = 0;
    //    } 
    //}



    enum MoveState
    {
        AfterResporn,
        RandamWalk,
        PlayerAtack,
        Shot,
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
        if (_ani != null && name == "Yakuza(Clone)") { _ani.Play("Idle"); }
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

    /// <summary>救引時に呼ぶ関数</summary>
    public void Suction()
    {
        ISDevTemplate.Sound.SoundManager.Instance.UseSFX("Suction");
        _moveState = MoveState.Suction;
        _player = FindObjectOfType<Player>().gameObject;
    }

    private void SuctionDestroy()//救引後消す処理
    {
        PointManager.Instance.AddPoint(_score); Destroy();
    }
}

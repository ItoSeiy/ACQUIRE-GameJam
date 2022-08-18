using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.ParticleSystem;

public class People : MonoBehaviour
{

    [SerializeField, Tooltip("移動速度")]
    private float _speed = 2.0f;

    [SerializeField, Tooltip("ゲーム画面に出てランダムに動き回るまでの最低時間")]
    float _nextMoveTime = 1.5f;

    [SerializeField, Tooltip("ゲーム画面の横幅")]
    private float _width = 0;

    [SerializeField, Tooltip("ゲーム画面の縦幅")]
    private float _verticalWidth = 0;

    /// <summary>スポーンの中心点</summary>
    public Transform _centerPoint = null;


    [SerializeField, Tooltip("持っているスコア")]
    public int _score = 0;

    /// <summary>動きのState</summary>
    private MoveState _moveState = MoveState.AfterResporn;

    /// <summary>ランダムな進行方向</summary>
    private Vector3 _randomVelo;

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

                if (transform.position.x < _centerPoint.position.x) vero = new Vector3(_speed, 0, 0);
                else vero = new Vector3(-_speed, 0, 0);

                transform.position += vero;

                if (_timer > _nextMoveTime)//一定時間後ランダムに移動
                {
                    if (Random.Range(0, 2) < 1)
                    {
                        _moveState = MoveState.RandamWalk;
                        _randomVelo = new Vector3(Random.value, Random.value, 0);
                        _timer = 0;
                    }
                }
                break;

            case MoveState.RandamWalk://一定間隔で方向をランダムに決めて移動
                transform.position += _randomVelo * _speed;
                if (_timer > _nextMoveTime)
                {
                    if (Random.Range(0, 2) < 1)
                    {
                        _randomVelo = new Vector3(Random.Range(-1, 2), Random.Range(-1, 2), 0);
                        _timer = 0;
                    }
                }
                break;
        }

        if(transform.position.x > _width + 2 || transform.position.x < -_width - 2 
            || transform.position.y > _verticalWidth + 2 || transform.position.y < -_verticalWidth - 2) Destroy();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!IsActive) { return; }
    }

    enum MoveState
    {
        AfterResporn,
        RandamWalk
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
        _isActrive = true;
    }

    public void Destroy()//削除
    {
        _timer = 0;
        _moveState = MoveState.AfterResporn;
        _isActrive = false;
        this.gameObject.SetActive(false);
    }
}

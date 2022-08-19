using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Bullet : MonoBehaviour
{
    [SerializeField, Tooltip("弾が進む速度")]
    private float _speed = 0.1f;

    [SerializeField, Tooltip("プレイヤーを気絶させる時間")]
    private int _stanTime = 1;

    /// <summary>弾が狙う方向</summary>
    private Vector3 _rot = Vector3.zero;

    /// <summary>プレイヤーの位置を知るために取得</summary>
    private GameObject _player;

    private Rigidbody2D _rb;

    private SpriteRenderer _sp;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();

        _sp = GetComponent<SpriteRenderer>();

        _player = FindObjectOfType<Player>().gameObject;

        _rot = Vector3.Scale(_player.transform.position - transform.position
                                                                    , new Vector3(1, 1, 0)).normalized;
        this.transform.rotation = Quaternion.FromToRotation(-Vector3.right , _player.transform.position - transform.position);

        _sp.flipY = _rot.x > 0 ? true : false; 

        _rb.velocity = _rot * _speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player") 
        {
            collision.gameObject.GetComponent<Player>().Stan(_stanTime);
            Destroy(gameObject);
        } 
    }
}

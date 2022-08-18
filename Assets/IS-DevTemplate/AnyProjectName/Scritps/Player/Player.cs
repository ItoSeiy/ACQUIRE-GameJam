using System.Collections;
using System.Collections.Generic;
using ISDevTemplate;
using UnityEngine;

public class Player : SingletonMonoBehaviour<Player>
{
    [SerializeField]
    public float speedx = 0.1f;
    public float speedy = 0.05f;

    private void Start()
    {
        this.GetComponent<SpriteRenderer>().flipX = true;
    }

    private void Update()
    {
        // TODO: キー入力で移動　進行方向で画像反転
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(0f, speedy, 0f);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(0f, -speedy, 0f);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(speedx, 0f, 0f);
            this.GetComponent<SpriteRenderer>().flipX = true;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(-speedx, 0f, 0f);
            this.GetComponent<SpriteRenderer>().flipX = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)    
    {
        // TODO: プレイヤーを引き寄せる実装をする　引き寄せ終わったらデストロイ
        Destroy(other.gameObject);
    }
    
}

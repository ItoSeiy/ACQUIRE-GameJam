using System.Collections;
using System.Collections.Generic;
using ISDevTemplate;
using UnityEngine;
using System.Threading.Tasks;

public class Player : SingletonMonoBehaviour<Player>
{
    [SerializeField]
    public float _speedx = 0.1f;
    public float _speedy = 0.05f;

   

    private void Start()
    {
        this.GetComponent<SpriteRenderer>().flipX = true;
    }

    private void Update()
    {
        // TODO: キー入力で移動　進行方向で画像反転
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(0f, _speedy, 0f);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(0f, -_speedy, 0f);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(_speedx, 0f, 0f);
            this.GetComponent<SpriteRenderer>().flipX = true;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(-_speedx, 0f, 0f);
            this.GetComponent<SpriteRenderer>().flipX = false;
        }

    }

    public async void Stan(int stanTime)
    {
        await Task.Delay(stanTime * 1000);
    }
    private void OnTriggerEnter2D(Collider2D other)    
    {
        // TODO: プレイヤーを引き寄せる実装をする　引き寄せ終わったらデストロイ
        Destroy(other.gameObject);
    }
    
}

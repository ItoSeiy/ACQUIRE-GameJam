using System.Collections;
using System.Collections.Generic;
using ISDevTemplate;
using UnityEngine;
using System.Threading.Tasks;

public class Player : SingletonMonoBehaviour<Player>
{
    private bool CanMove = true;

    [SerializeField]
    public float _speedx = 0.1f;  // 横移動速度
    public float _speedy = 0.05f; // 縦移動速度

    [SerializeField]
    Animator _animator;

    private void Start()
    {
        //this.GetComponent<SpriteRenderer>().flipX = true;
    }

    private void Update()
    {
        if (!CanMove) return;

        Vector3 direction = transform.localScale;

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
            //this.GetComponent<SpriteRenderer>().flipX = true;
            if(direction.x < 0)
            direction.x = -transform.localScale.x; 
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(-_speedx, 0f, 0f);
            //this.GetComponent<SpriteRenderer>().flipX = false;
            if (direction.x > 0)
                direction.x = -transform.localScale.x;
        }

        transform.localScale = direction; 

    }

    public async void Stan(int stanTime)
    {
        CanMove = false;
        _animator.Play("Stan");
        await Task.Delay(stanTime * 1000);
        _animator.Play("Idle");
        CanMove = true;
    }
    private void OnTriggerEnter2D(Collider2D other)    
    {
        // TODO: プレイヤーを引き寄せる実装をする　引き寄せ終わったらデストロイ
        People people = other.GetComponent<People>(); 
        if (people)
        {
            people.Suction();
        }
       
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class PeopleManager : MonoBehaviour
{
    [SerializeField, Tooltip("画面上に出す人間の種類数")]
    int _typeCount = 0;

    [SerializeField, Tooltip("人間を出現させる間隔")]
    float _appearanceInterval = 0.5f;

    [SerializeField, Tooltip("スポーンの中心点")]
    private Transform _centerPoint = null;

    [SerializeField, Tooltip("ゲーム画面の横幅")]
    private float _width = 0;

    [SerializeField, Tooltip("ゲーム画面の縦幅")]
    private float _verticalWidth = 0;

    /// <summary> 時間計測用</summary>
    float _timer = 0;

    void Start()
    {

    }

    void Update()
    {
        _timer += Time.deltaTime;

        if (_timer > _appearanceInterval)
        {
            float respawnWidth = 0;

            if (Random.Range(0, 2) < 1) respawnWidth = -_width;
            else respawnWidth = _width;

            int randomInt = Random.Range(0, _typeCount + 3);

            switch (randomInt)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                    GameObject people = ISDevTemplate.Pool.ObjectPool.Instance.UseObject("CommonPeople", new Vector3(respawnWidth, Random.Range(-_verticalWidth, _verticalWidth), 4.7f));
                    people.GetComponent<People>()._centerPoint = _centerPoint;
                    people.GetComponent<People>().Create();
                    break;

                //case 4:
                    ////GameObject yakuza = ISDevTemplate.Pool.ObjectPool.Instance.UseObject("Yakuza", new Vector3(respawnWidth, Random.Range(-_verticalWidth, _verticalWidth), 4.7f));
                    ////yakuza.GetComponent<People>()._centerPoint = _centerPoint;
                    ////yakuza.GetComponent<People>().Create();

                    //GameObject Tank = ISDevTemplate.Pool.ObjectPool.Instance.UseObject("Tank", new Vector3(respawnWidth, Random.Range(_centerPoint.position.y - _verticalWidth, _centerPoint.position.y + _verticalWidth), 4.7f));
                    //Tank.GetComponent<People>()._centerPoint = _centerPoint;
                    //Tank.GetComponent<People>().Create();
                    //break;
            }



            _timer = 0;
        }
    }
}

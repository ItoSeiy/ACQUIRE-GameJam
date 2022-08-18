using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class PeopleManager : MonoBehaviour
{
    [SerializeField]
    ISDevTemplate.Pool.ObjectPool _obp = null;

    [SerializeField, Tooltip("画面上に出す人間の上限")]
    int _maxCount = 0;

    [SerializeField, Tooltip("人間を出現させる間隔")]
    float _appearanceInterval = 0.5f;

    [SerializeField, Tooltip("スポーンの中心点")]
    private Transform _centerPoint = null;

    [SerializeField , Tooltip("ゲーム画面の横幅")]
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

            if (Random.Range(0, 2) < 1) respawnWidth = -_verticalWidth;
            else respawnWidth = _verticalWidth;

            GameObject people = _obp.UseObject("CommonPeople", new Vector3(respawnWidth, Random.Range(-_verticalWidth, _verticalWidth), 4.7f));
            people.GetComponent<People>()._centerPoint = _centerPoint;
            people.GetComponent<People>().Create();

            _timer = 0;
        }
    }
}

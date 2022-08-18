using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Result : MonoBehaviour
{
    private ResultData _resultData;

    private void Start()
    {
    }
}

public enum ResultType
{
    GameClear,
    GameOver
}

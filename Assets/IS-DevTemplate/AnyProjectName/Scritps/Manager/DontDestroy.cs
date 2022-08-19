using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ISDevTemplate.Manager
{
    public class DontDestroy : SingletonMonoBehaviour<DontDestroy>
    {
        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}

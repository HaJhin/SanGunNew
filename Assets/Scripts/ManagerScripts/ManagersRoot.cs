using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagersRoot : MonoBehaviour
{
    private static ManagersRoot instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

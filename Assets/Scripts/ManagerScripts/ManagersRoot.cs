using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagersRoot : MonoBehaviour
{
    public static ManagersRoot instance;

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

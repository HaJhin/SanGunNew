using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagManager : MonoBehaviour
{
    public static FlagManager Instance;

    private Dictionary<string,bool> flags = new Dictionary<string,bool>();

    public event Action<string,bool> FlagChanged;

    private void Awake()
    {
        if (Instance == null) { Instance = this; } else { Destroy(gameObject); }
    } // Awake ed

    public bool GetFlag(string key)
    {
        if(flags.ContainsKey(key)) return flags[key];
        return false;
    } // GetFlag Ed

    public void SetFlag(string key,bool value)
    {
        flags[key] = value;
        FlagChanged?.Invoke(key, value);
    }
}

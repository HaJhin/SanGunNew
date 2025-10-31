using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    public string flagName;
    public Collider Collider;

    private void Awake()
    {
        Collider = GetComponent<Collider>();
    }

    private void Start()
    {
        Collider.enabled = false;
    }

    private void OnEnable()
    {
        FlagManager.Instance.FlagChanged += OnFlagChanged;
    }

    private void OnDisable()
    {
        FlagManager.Instance.FlagChanged -= OnFlagChanged;
    }

    private void OnFlagChanged(string key, bool value)
    {
        if (key == flagName && value)
        {
            OpenTP();
        }
    } // OnFlagChanged ed

    private void OpenTP()
    {
        Collider.enabled = true;
    }
}

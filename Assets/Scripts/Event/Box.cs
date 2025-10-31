using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    public Transform lidBone;

    public float closedAngle = 90f;
    public float openAngle = -35f;
    private float currentAngle;
    public float speed = 1f;

    private bool isOpen = false;

    private string flagName = "OpenBox";

    void Start()
    {
        currentAngle = closedAngle;
        lidBone.localRotation = Quaternion.Euler(0f, 0f, currentAngle);
    }


    private void Update()
    {
        float targetAngle = isOpen ? openAngle : closedAngle;
        currentAngle = Mathf.Lerp(currentAngle, targetAngle, Time.deltaTime * speed);
        lidBone.localRotation = Quaternion.Euler(0f, 0f, currentAngle);
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
            BoxOpen();
        }
    } // OnFlagChanged ed

    private void BoxOpen()
    {
        isOpen = true;
    }
}

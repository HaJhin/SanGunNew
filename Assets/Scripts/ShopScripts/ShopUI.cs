using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class ShopUI : MonoBehaviour
{
    private GameObject shopPanel;

    private void Awake()
    {
        shopPanel = transform.Find("ShopUI").gameObject;
    }
    private void Start()
    {
        shopPanel.SetActive(false);
    }
}

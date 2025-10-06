using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDUI : MonoBehaviour
{
    public TMP_Text goldText;
    public TMP_Text item1Text;
    public TMP_Text item2Text;

    public void UpdateGold(int amount)
    {
        goldText.text = " : " + amount;
    }
    
    public void UpdateItem1(int amount)
    {
        item1Text.text = amount.ToString();
    } 

    public void UpdateItem2(int amount)
    {
        item2Text.text = amount.ToString();
    } 
}

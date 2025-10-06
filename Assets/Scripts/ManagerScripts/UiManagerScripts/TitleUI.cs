using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleUI : MonoBehaviour
{
   
    public void OnClickStart()
    {
        SceneManager.LoadScene("S1_1");
    } 

    public void OnClickExit()
    {
        Application.Quit();
    }
}

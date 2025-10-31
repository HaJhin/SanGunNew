using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleUI : MonoBehaviour
{

    private void Start()
    {
        if (ManagersRoot.instance != null)
        {
            Destroy(ManagersRoot.instance.gameObject);
        }
    }

    public void OnClickStart()
    {
        SceneManager.LoadScene("S1_1");
    } 

    public void OnClickExit()
    {
        Application.Quit();
    }
}

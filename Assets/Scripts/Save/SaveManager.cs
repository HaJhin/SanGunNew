using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveData currentData;

    public static void SaveGame(GameManager gm)
    {
        currentData = new SaveData
        {
            playerHP = gm.HP,
            playerShield = gm.shield,
            playerAtk = gm.atk,
            playerComboLev = gm.maxComboCount,
            playerDashAtk = gm.dashAtk,
            playerGold = gm.Gold,
            lastSceneName = SceneManager.GetActiveScene().name
        };
    } // SaveGame ed

    public static void LoadGame(GameManager gm)
    {
        if (currentData == null)
        {
            Debug.LogWarning("데이터 없음");
            return;
        } // if ed

        gm.HP = currentData.playerHP;
        gm.shield = currentData.playerShield;
        gm.atk = currentData.playerAtk;
        gm.maxComboCount = currentData.playerComboLev;
        gm.dashAtk = currentData.playerDashAtk;
        gm.Gold = currentData.playerGold;
    } // LoadGame ed

    public static void ClearSave()
    {
        currentData = null;
    } // ClearSave ed
}

using UnityEngine;
using System;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
[System.Serializable]
public class MajorData
{
    public Weapon weapon;
    public string sceneName;
}
public class SelectMajorScript : MonoBehaviour
{
    public PlayerData playerData;
    public MajorData[] majorData = new MajorData[4];
    
    public void ChangeStatus(int index)
    {
        playerData.ChangeValue(((int)majorData[index].weapon).ToString(), "weapon");
        SceneManager.LoadScene(majorData[index].sceneName);
    }
}

using UnityEngine;
using System;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
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
    public AudioClip selectSound;
    public void ChangeStatus(int index)
    {
        playerData.ChangeValue(((int)majorData[index].weapon).ToString(), "weapon");
        GetComponent<AudioSource>().PlayOneShot(selectSound);
        SceneManager.LoadScene(majorData[index].sceneName);
    }
}

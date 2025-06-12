using UnityEngine;
using System;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SceneClass
{
    public string SceneName;
    public string difficultyLevel = "Normal";
    public float possibility = 1;
}
[System.Serializable]
public class SyllabusClass
{
    public string sceneMode;
    public Color imageColor = Color.green;
    public SceneClass[] sceneClasses;
}
public class SyllabusScript : MonoBehaviour
{
    public PlayerSetting player;
    public SyllabusClass[] syllabusClasses = new SyllabusClass[3];
    private SceneClass[] sceneClasses = new SceneClass[3];
    public TextMeshProUGUI[] difficultyText = new TextMeshProUGUI[3];
    public TextMeshProUGUI[] displayText = new TextMeshProUGUI[3];
    public Image[] sceneSelectImage = new Image[3];
    List<int> getList = new List<int>();
    public void ChangeScene(int index)
    {
        SceneManager.LoadScene(syllabusClasses[index].sceneClasses[getList[index]].SceneName);
    }
    public void ReselectTheCources()
    {
        SelectRandomThree();
        for (int i = 0; i < 3; i++)
        {
            difficultyText[i].text = syllabusClasses[i].sceneClasses[getList[i]].difficultyLevel;
            displayText[i].text = syllabusClasses[i].sceneClasses[getList[i]].SceneName;
        }
    }
    void Start()
    {
        SelectRandomThree();
        for (int i = 0; i < 3; i++)
        {
            difficultyText[i].text = syllabusClasses[i].sceneClasses[getList[i]].difficultyLevel;
            displayText[i].text = syllabusClasses[i].sceneClasses[getList[i]].SceneName;
            //sceneSelectImage.color = syllabusClasses[i].imageColor;
        }
    }
    public void SelectRandomThree()
    {
        getList.Clear();
        for (int w = 0; w < 3; w++)
        {
            if (syllabusClasses[w].sceneClasses.Length == 0) return;
        }
        float[] possibilitySum = new float[3];
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < syllabusClasses[i].sceneClasses.Length; j++)
            {
                possibilitySum[i] += syllabusClasses[i].sceneClasses[j].possibility;
            }
        }
        for (int k = 0; k < 3; k++)
        {
            float rand = UnityEngine.Random.Range(0, possibilitySum[k]);
            float check = 0;
            for (int l = 0; l < syllabusClasses[k].sceneClasses.Length; l++)
            {
                check += syllabusClasses[k].sceneClasses[l].possibility;
                if(check > rand)
                {
                    getList.Add(l);
                    break;
                }
            }
        }
    }
}

using UnityEngine;
using System;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public enum ValueChangeMode
{
    Change, Add, Multiple, Call, Reset
}
[System.Serializable]
public class SceneClass
{
    public string SceneName;
    public string difficultyLevel = "Normal";
    public Sprite skillSprite;
    public string getSkillName;
    public string variableName;
    public string value;
    public ValueChangeMode valueMode;
    public float possibility = 1;
}
[System.Serializable]
public class SyllabusClass
{
    public string sceneMode;
    public Color imageColor = Color.green;
    public Image backGround;
    public TextMeshProUGUI difficultyDisplay;
    public Image skillIcon;
    public TextMeshProUGUI skillNameDisplay;
    public SceneClass[] sceneClasses;
}
public class SyllabusScript : MonoBehaviour
{
    public PlayerData playerData;
    public SyllabusClass[] syllabusClasses = new SyllabusClass[3];
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
            syllabusClasses[i].difficultyDisplay.text = syllabusClasses[i].sceneClasses[getList[i]].difficultyLevel;
            syllabusClasses[i].skillIcon.sprite = syllabusClasses[i].sceneClasses[getList[i]].skillSprite;
            syllabusClasses[i].skillNameDisplay.text = syllabusClasses[i].sceneClasses[getList[i]].getSkillName;
        }
    }
    void Start()
    {
        SelectRandomThree();
        for (int i = 0; i < 3; i++)
        {
            syllabusClasses[i].difficultyDisplay.text = syllabusClasses[i].sceneClasses[getList[i]].difficultyLevel;
            syllabusClasses[i].skillIcon.sprite = syllabusClasses[i].sceneClasses[getList[i]].skillSprite;
            syllabusClasses[i].backGround.color = syllabusClasses[i].imageColor;
            syllabusClasses[i].skillNameDisplay.text = syllabusClasses[i].sceneClasses[getList[i]].getSkillName;
        }
    }
    public void ChangeStatus(int index)
    {
        switch (syllabusClasses[index].sceneClasses[getList[index]].valueMode)
        {
            case ValueChangeMode.Change:
                playerData.ChangeValue(syllabusClasses[index].sceneClasses[getList[index]].value, syllabusClasses[index].sceneClasses[getList[index]].variableName);
                break;
            case ValueChangeMode.Add:
                playerData.AddValue(syllabusClasses[index].sceneClasses[getList[index]].value, syllabusClasses[index].sceneClasses[getList[index]].variableName);
                break;
            case ValueChangeMode.Multiple:
                playerData.MultipleValue(syllabusClasses[index].sceneClasses[getList[index]].value, syllabusClasses[index].sceneClasses[getList[index]].variableName);
                break;
            case ValueChangeMode.Call:
                playerData.CallMethod(syllabusClasses[index].sceneClasses[getList[index]].value, syllabusClasses[index].sceneClasses[getList[index]].variableName);
                break;
            case ValueChangeMode.Reset:
                playerData.ResetValue();
                break;
            default:
                break;
        }
        SceneManager.LoadScene(syllabusClasses[index].sceneClasses[getList[index]].SceneName);
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

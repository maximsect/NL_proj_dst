using UnityEngine;
using System;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

[System.Serializable]
public class ExamClass
{
    public Sprite skillSprite;
    public string getSkillName;
    public string variableName;
    public string value;
    public ValueChangeMode valueMode;
    public float possibility = 1;
}
public class NekomataPreparation : MonoBehaviour
{
    public enum PrepareFor
    {
        Professor, Nekomata
    }
    public PlayerData playerData;
    public SceneData sceneData;
    public PrepareFor prepareFor;
    public TextMeshProUGUI[] skillNameDisplay;
    public Image[] skillImage;
    public ExamClass[] examClasses;
    public string difficultyLevel;
    public SceneMode sceneMode;
    private List<int> getList = new List<int>();
    void Start() {
        SelectRandomElement();
    }
    public void ChangeScene()
    {
        switch (prepareFor)
        {
            case PrepareFor.Professor:
                SceneManager.LoadScene("ExaminationScene");
                break;
            case PrepareFor.Nekomata:
                SceneManager.LoadScene("NekomataScene");
                break;
            default:
                break;

        }

    }
    public void ChangeStatus(int index)
    {
        switch (examClasses[getList[index]].valueMode)
        {
            case ValueChangeMode.Change:
                playerData.ChangeValue(examClasses[getList[index]].value, examClasses[getList[index]].variableName);
                break;
            case ValueChangeMode.Add:
                playerData.AddValue(examClasses[getList[index]].value, examClasses[getList[index]].variableName);
                break;
            case ValueChangeMode.Multiple:
                playerData.MultipleValue(examClasses[getList[index]].value, examClasses[getList[index]].variableName);
                break;
            case ValueChangeMode.Call:
                playerData.CallMethod(examClasses[getList[index]].value, examClasses[getList[index]].variableName);
                break;
            case ValueChangeMode.Reset:
                playerData.ResetValue();
                break;
            default:
                break;
        }
        sceneData.stageLevel = "鬼";
        sceneData.sceneMode = sceneMode;
        ChangeScene();
    }
    public void SelectRandomElement()
    {   
        List<int> allNum = new List<int>();
        allNum.Clear();
        for (int i = 0; i < examClasses.Length; i++)
        {
            allNum.Add(i);
        }
        for (int i = 0; i < 2; i++)
        {
            int rand = allNum[UnityEngine.Random.Range(0, allNum.Count)];
            getList.Add(rand);
            allNum.Remove(rand);
        }
        
        for (int i = 0; i < 2; i++)
        {
            skillImage[i].sprite = examClasses[getList[i]].skillSprite;
            skillNameDisplay[i].text = examClasses[getList[i]].getSkillName;
        }

    }
}

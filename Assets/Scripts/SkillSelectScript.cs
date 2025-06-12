using UnityEngine;
using System;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class SkillSelectClass
{
    public string DisplayText;
    public string variableName;

    public VariableMode variableMode;
    public int intVal = 0;
    public float floatVal = 0;
    public bool booleanVal = false;
    public float possibility = 0;
}
public enum VariableMode
{
    ChangeInt, ChangeFloat, ChangeBool, AddInt, AddFloat, MultipleFloat, MultipleInt
}
public class SkillSelectScript : MonoBehaviour
{
    public PlayerSetting player;
    public SkillSelectClass[] skillSelectClasses;
    public GameObject[] textObjs = new GameObject[3];
    List<int> getList = new List<int>();
    public void ButtonSelect(int k)
    {
        ChangeValueManager(getList[k]);
    }
    public void Restart()
    {
        SelectRandomThree();
        for (int i = 0; i < 3; i++)
        {
            textObjs[i].GetComponent<TextMeshProUGUI>().text = skillSelectClasses[getList[i]].DisplayText;
        }
    }
    void Start()
    {
        SelectRandomThree();
        for (int i = 0; i < 3; i++)
        {
            textObjs[i].GetComponent<TextMeshProUGUI>().text = skillSelectClasses[getList[i]].DisplayText;
        }
    }
    public void SelectRandomThree()
    {
        getList.Clear();
        if (skillSelectClasses.Length < 3) return;
        float possibilitySum = 0;
        List<int> numlist = new List<int>();
        for (int i = 0; i < skillSelectClasses.Length; i++)
        {
            numlist.Add(i);
            possibilitySum += skillSelectClasses[i].possibility;
        }
        for (int k = 0; k < 3; k++)
        {
            float rand = UnityEngine.Random.Range(0, possibilitySum);
            float check = 0;
            for (int j = 0; j < numlist.Count; j++)
            {
                check += skillSelectClasses[numlist[j]].possibility;
                if (check > rand)
                {
                    getList.Add(numlist[j]);
                    possibilitySum -= skillSelectClasses[numlist[j]].possibility;
                    numlist.RemoveAt(j);
                    break;
                }
            }
        }
    }

    public void ChangeValueManager(int index)
    {

        switch (skillSelectClasses[index].variableMode)
        {
            case VariableMode.ChangeInt:
                ChangeValue(skillSelectClasses[index].intVal, skillSelectClasses[index].variableName);
                break;
            case VariableMode.ChangeFloat:
                ChangeValue(skillSelectClasses[index].floatVal, skillSelectClasses[index].variableName);
                break;
            case VariableMode.ChangeBool:
                ChangeValue(skillSelectClasses[index].booleanVal, skillSelectClasses[index].variableName);
                break;
            case VariableMode.AddInt:
                AddValue(skillSelectClasses[index].intVal, skillSelectClasses[index].variableName);
                break;
            case VariableMode.AddFloat:
                AddValue(skillSelectClasses[index].floatVal, skillSelectClasses[index].variableName);
                break;
            case VariableMode.MultipleFloat:
                MultipleValueFloat(skillSelectClasses[index].floatVal, skillSelectClasses[index].variableName);
                break;
            case VariableMode.MultipleInt:
                if (skillSelectClasses[index].floatVal != 0) { MultipleValueInt(skillSelectClasses[index].floatVal, skillSelectClasses[index].variableName); }
                else if (skillSelectClasses[index].intVal != 0) { MultipleValueInt((float)skillSelectClasses[index].intVal, skillSelectClasses[index].variableName); }
                else { Debug.LogError("Multiple By 0 is prohibited"); }
                break;
            default:
                break;
        }
        player.HealthUnderMax();
    }
    public void ChangeValue(int newVal, string varName)
    {
        player.ChangeValue<int>(newVal, varName);
    }
    public void ChangeValue(float newVal, string varName)
    {
        player.ChangeValue<float>(newVal, varName);
    }
    public void ChangeValue(bool newVal, string varName)
    {
        player.ChangeValue<bool>(newVal, varName);
    }
    public void AddValue(int newVal, string varName)
    {
        player.AddValue(newVal, varName);
    }
    public void AddValue(float newVal, string varName)
    {
        player.AddValue(newVal, varName);
    }
    public void MultipleValueFloat(float newVal, string varName)
    {
        player.MultipleValueFloat(newVal, varName);
    }
    public void MultipleValueInt(float newVal, string varName)
    {
        player.MultipleValueInt(newVal, varName);
    }
}
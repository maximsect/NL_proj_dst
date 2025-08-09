//NL_DST

using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using UnityEngine.SceneManagement;
public enum Weapon
{
    Bat,Spear,Bow,Hammer
}
[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/PlayerData")]
public class PlayerData : ScriptableObject
{
    public static PlayerData main { get; private set; }
    [Header("Status")]
    [field: SerializeField] public int hp { get; private set; } = 5;
    public int maxHp = 10;
    public int healAmount = 3;
    public float autoHealInterval = 10f;
    public float autoHealAmount = 1;
    public int score { get; private set; } = 0;
    public float luck = 0.02f;
    [Header("Movement")]
    public float moveSpeed = 10;
    public int numberOfDash = 1;
    public int numberOfJump = 1;
    public float jumpSpeed = 12;
    public float airJumpSpeed = 10;
    public float Speed = 20;
    public float dashSpeed = 30;
    public int direction = 1;
    [Header("Attack")]
    public int attackPower = 10;
    public float attackInterval = 0.5f;
    public Weapon weapon;
    [Header("Weapons")]
    public int batAttack = 10;
    public int spearAttack = 10;
    public int bowAttack = 10;
    public int hammerAttack = 10;
    public int arrowAttack = 10;
    public int skillAttack = 10;
    [Header("Variable")]
    public float invinsibleDuration = 0.3f;
    public PlayerData copySource;
    public SceneData sceneData;
    [field: SerializeField] public string LastSceneName { get; private set; } = "Stage1";
    public string resultText { get; private set; } = "";
    public float elapsedTime = 0;
    public void OnStartSetting(PlayerData playerData)
    {
        main = playerData;
        hp = maxHp;
        score = 0;
    }
    public void Damage(int delta)
    {
        hp = Mathf.Clamp(hp - delta, 0, maxHp);
    }
    public void Heal(int delta)
    {
        hp = Mathf.Clamp(hp + delta, 0, maxHp);
    }
    public void AddScore(int delta)
    {
        score += delta;
    }
    public void ChangeValue(string val, string varName)
    {
        FieldInfo field = GetType().GetField(varName);
        if (field != null)
        {
            switch (field.GetValue(this).GetType().ToString())
            {
                case "System.Single":
                    field.SetValue(this, float.Parse(val));
                    break;
                case "System.Int32":
                    field.SetValue(this, int.Parse(val));
                    break;
                case "System.String":
                    field.SetValue(this, val);
                    break;
                case "System.Boolean":
                    field.SetValue(this, val == "true");
                    break;
                case "Weapon":
                    field.SetValue(this, (Weapon)int.Parse(val));
                    break;
                default:
                    Debug.Log("NoMatches");
                    break;
            }
        }
    }
    public void AddValue(string val, string varName)
    {
        FieldInfo field = GetType().GetField(varName);
        if (field == null)
        {
            Debug.Log("No matches for AddValue");
        }
        else
        {
            if (field.GetValue(this).GetType() == typeof(float))
            {
                field.SetValue(this, (float)(object)field.GetValue(this) + float.Parse(val));
            }
            else if (field.GetValue(this).GetType() == typeof(int))
            {
                field.SetValue(this, (int)(object)field.GetValue(this) + int.Parse(val));
            }
        }
    }
    public void MultipleValue(string val, string varName)
    {
        FieldInfo field = GetType().GetField(varName);

        if (field == null)
        {
            Debug.Log("No matches for MultipleValue");
        }
        else
        {
            if (field.GetValue(this).GetType() == typeof(float))
            {
                field.SetValue(this, (float)(object)field.GetValue(this) * float.Parse(val));
            }
            else if (field.GetValue(this).GetType() == typeof(int))
            {
                field.SetValue(this, (int)((int)(object)field.GetValue(this) * float.Parse(val)));
            }
        }
    }
    public void CallMethod(string val, string functionName)
    {
        MethodInfo method = GetType().GetMethod(functionName);
        if (method == null)
        {
            Debug.Log("No matches for Method");
        }
        else
        {
            object[] arguments = new object[] { int.Parse(val) };
            method.Invoke(this, arguments);
        }
    }
    public void ResetValue()
    {
        OnStartSetting(this);
        FieldInfo[] fields = GetType().GetFields();
        foreach (var field in fields)
        {
            field.SetValue(this, field.GetValue(PlayerData.main.copySource));
        }
    }
    public string GetResultText()
    {
        resultText = "Result\n" + "Score: " + score + "\nTime: " + elapsedTime;
        return resultText;
    }
}

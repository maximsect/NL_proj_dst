using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System;

[CreateAssetMenu(fileName = "PlayerSetting2", menuName = "Scriptable Objects/PlayerSetting2")]
public class PlayerSetting2 : ScriptableObject
{
    [Header("NormalAbility")]
    public int health = 100; //体力
    public int maxHealth = 100;//体力最大値
    public int autoHealAmount = 1;//自然回復量
    public float autoHealInterval = 5;//自然回復間隔
    public int itemHealAmount = 30;//アイテム回復量
    public int defense = 0;//防御量
    public float movementSpeed = 5;//移動速度
    public float dashSpeed = 14;//ダッシュ速度
    public float jumpForce = 5;//ジャンプ力
    public float luck = 0;//運
    public PolarCoordinates gravity;//重力方向（－90が標準）

    [Space(10)]
    [Header("SpecialAbility")]
    public int numberOfJump = 1;//ジャンプの回数
    public bool chargedJump;//チャージジャンプ可能かどうか
    public int numberOfDash = 1;//空中でのダッシュの回数
    public bool chargedDash;//チャージダッシュ可能かどうか
    public float airHover = 0;//空中滞空時間
    public bool grapping;//引き寄せ可能かどうか
    public float grappingSpeed = 25;//引き寄せスピード
    public bool drillJump;//ドリルジャンプ可能か
    public bool stompAttack;//スタンプアタック可能か
    public int orbitingOrbNumber = 0;//サークルオーブの数
    public float critChance;//クリティカルヒットの可能性(0～1)
    public float criticalDamageRate;//クリティカル倍率
    public bool piercingArrow;//弓貫通するかどうか
    public bool splittingArrow;//弓が分裂するかどうか
    public float chanceOfInstantKill;//即死攻撃可能性(0～1)
    public bool wallStick;//壁張り付き

    [Space(10)]
    [Header("WeaponSetting")]
    public int weaponIndex = 0; //attackSetting[weaponIndex]の形で用いる
    public AttackSetting[] attackSetting;
    public int xp = 0;
    public PlayerSetting2 copySource, pasteTarget;//値をリセットすることができる

    public void ResetHealthAndScoreValues()
    {
        health = maxHealth;
        xp = 0;
    }
    public void ResetValues()//copySourceの値を全てpasteTargetに移す
    {
        FieldInfo[] fields = pasteTarget.GetType().GetFields();
        foreach (var field in fields)
        {
            field.SetValue(pasteTarget, field.GetValue(copySource));
        }
    }
    public void HealthUnderMax()//体力を最大値以下にする
    {
        health = Mathf.Clamp(health, 0, maxHealth);
    }
    public void ChangeValue<T>(T newVal, string varName)//後で授業選択のとき使うやつ
    {
        FieldInfo fieldInfo = GetType().GetField(varName);
        if (fieldInfo != null)
        {
            fieldInfo.SetValue(this, newVal);
        }
        else
        {
            fieldInfo = attackSetting[weaponIndex].GetType().GetField(varName);
            if (fieldInfo != null)
            {
                fieldInfo.SetValue(attackSetting[weaponIndex], newVal);
            }
        }
    }
    public void AddValue(int addVal, string varName)
    {
        FieldInfo fieldInfo = GetType().GetField(varName);
        int preVal = 0;
        if (fieldInfo != null)
        {
            preVal = (int)fieldInfo.GetValue(this);
            fieldInfo.SetValue(this, preVal + addVal);
        }
        else
        {
            fieldInfo = attackSetting[weaponIndex].GetType().GetField(varName);
            if (fieldInfo != null)
            {
                fieldInfo.SetValue(attackSetting[weaponIndex], preVal + addVal);
            }
        }
    }
    public void AddValue(float addVal, string varName)
    {
        FieldInfo fieldInfo = GetType().GetField(varName);
        float preVal = 0;
        if (fieldInfo != null)
        {
            preVal = (float)fieldInfo.GetValue(this);
            fieldInfo.SetValue(this, preVal + addVal);
        }
        else
        {
            fieldInfo = attackSetting[weaponIndex].GetType().GetField(varName);
            if (fieldInfo != null)
            {
                fieldInfo.SetValue(attackSetting[weaponIndex], preVal + addVal);
            }
        }
    }
    public void MultipleValueFloat(float multipleVal, string varName)
    {
        FieldInfo fieldInfo = GetType().GetField(varName);
        float preVal = 0;
        if (fieldInfo != null)
        {
            preVal = (float)fieldInfo.GetValue(this);
            fieldInfo.SetValue(this, preVal * multipleVal);
        }
        else
        {
            fieldInfo = attackSetting[weaponIndex].GetType().GetField(varName);
            if (fieldInfo != null)
            {
                preVal = (float)fieldInfo.GetValue(attackSetting[weaponIndex]);
                fieldInfo.SetValue(attackSetting[weaponIndex], preVal * multipleVal);
            }
        }
    }
    public void MultipleValueInt(float multipleVal, string varName)
    {
        FieldInfo fieldInfo = GetType().GetField(varName);
        int preVal = 0;
        if (fieldInfo != null)
        {
            preVal = (int)fieldInfo.GetValue(this);
            fieldInfo.SetValue(this, (int)(preVal * multipleVal));
        }
        else
        {
            fieldInfo = attackSetting[weaponIndex].GetType().GetField(varName);
            if (fieldInfo != null)
            {
                preVal = (int)fieldInfo.GetValue(attackSetting[weaponIndex]);
                fieldInfo.SetValue(attackSetting[weaponIndex], (int)(preVal * multipleVal));
            }
        }
    }
    public bool CheckTheTypes(string varName)
    {
        FieldInfo fieldInfo = GetType().GetField(varName);
        if (fieldInfo != null) { return true; }
        else
        {
            fieldInfo = attackSetting[weaponIndex].GetType().GetField(varName);
            if (fieldInfo != null) { return true; }
            else return false;
        }
    }
}
[CustomEditor(typeof(PlayerSetting2))]
public class PlayerSetting2Editor : Editor
{
    ///<summary>
    ///InspectorのGUI更新
    ///</summary>
    public override void OnInspectorGUI()
    {
        PlayerSetting2 playerSetting2 = target as PlayerSetting2;
        base.OnInspectorGUI();
        if (GUILayout.Button("Reset Health & Score"))
        {
            playerSetting2.ResetHealthAndScoreValues();
        }
        if (GUILayout.Button("ReplaceEveryValue"))
        {
            playerSetting2.ResetValues();
        }
    }
}
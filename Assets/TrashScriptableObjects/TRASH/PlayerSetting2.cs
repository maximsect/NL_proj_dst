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
    public int health = 100; //�̗�
    public int maxHealth = 100;//�̗͍ő�l
    public int autoHealAmount = 1;//���R�񕜗�
    public float autoHealInterval = 5;//���R�񕜊Ԋu
    public int itemHealAmount = 30;//�A�C�e���񕜗�
    public int defense = 0;//�h���
    public float movementSpeed = 5;//�ړ����x
    public float dashSpeed = 14;//�_�b�V�����x
    public float jumpForce = 5;//�W�����v��
    public float luck = 0;//�^
    public PolarCoordinates gravity;//�d�͕����i�|90���W���j

    [Space(10)]
    [Header("SpecialAbility")]
    public int numberOfJump = 1;//�W�����v�̉�
    public bool chargedJump;//�`���[�W�W�����v�\���ǂ���
    public int numberOfDash = 1;//�󒆂ł̃_�b�V���̉�
    public bool chargedDash;//�`���[�W�_�b�V���\���ǂ���
    public float airHover = 0;//�󒆑؋󎞊�
    public bool grapping;//�����񂹉\���ǂ���
    public float grappingSpeed = 25;//�����񂹃X�s�[�h
    public bool drillJump;//�h�����W�����v�\��
    public bool stompAttack;//�X�^���v�A�^�b�N�\��
    public int orbitingOrbNumber = 0;//�T�[�N���I�[�u�̐�
    public float critChance;//�N���e�B�J���q�b�g�̉\��(0�`1)
    public float criticalDamageRate;//�N���e�B�J���{��
    public bool piercingArrow;//�|�ђʂ��邩�ǂ���
    public bool splittingArrow;//�|�����􂷂邩�ǂ���
    public float chanceOfInstantKill;//�����U���\��(0�`1)
    public bool wallStick;//�ǒ���t��

    [Space(10)]
    [Header("WeaponSetting")]
    public int weaponIndex = 0; //attackSetting[weaponIndex]�̌`�ŗp����
    public AttackSetting[] attackSetting;
    public int xp = 0;
    public PlayerSetting2 copySource, pasteTarget;//�l�����Z�b�g���邱�Ƃ��ł���

    public void ResetHealthAndScoreValues()
    {
        health = maxHealth;
        xp = 0;
    }
    public void ResetValues()//copySource�̒l��S��pasteTarget�Ɉڂ�
    {
        FieldInfo[] fields = pasteTarget.GetType().GetFields();
        foreach (var field in fields)
        {
            field.SetValue(pasteTarget, field.GetValue(copySource));
        }
    }
    public void HealthUnderMax()//�̗͂��ő�l�ȉ��ɂ���
    {
        health = Mathf.Clamp(health, 0, maxHealth);
    }
    public void ChangeValue<T>(T newVal, string varName)//��Ŏ��ƑI���̂Ƃ��g�����
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
    ///Inspector��GUI�X�V
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
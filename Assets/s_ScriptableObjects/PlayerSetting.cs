using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System;

[System.Serializable]
public class PolarCoordinates//�ɍ��W�̋���radius�ƕΊpangle �d�͂Ŏg��
{
    public float radius = 10;
    public float angle = -90;
}

[System.Serializable]
public class AttackSetting
{
    public string weaponName;
    public float attackInterval;
    public float attackPower;
}

[CreateAssetMenu(fileName = "PlayerSetting", menuName = "Scriptable Objects/PlayerSetting")]
public class PlayerSetting : ScriptableObject
{
    [Header("NormalAbility")]
    public float health = 100; //�̗�
    public float maxHealth = 100;//�̗͍ő�l
    public float autoHealAmount = 1;//���R�񕜗�
    public float autoHealInterval = 5;//���R�񕜊Ԋu
    public float itemHealAmount = 30;//�A�C�e���񕜗�
    public float defense = 0;//�h���
    public float movementSpeed = 5;//�ړ����x
    public float dashSpeed = 14;//�_�b�V�����x
    public float jumpForce = 5;//�W�����v��
    public float luck = 0;//�^
    public PolarCoordinates gravity;//�d�͕����i�|90���W���j

    [Space(10)][Header("SpecialAbility")]
    public int numberOfJump = 1;//�W�����v�̉�
    public bool chargedJump;//�`���[�W�W�����v�\���ǂ���
    public int numberOfDash = 1;//�󒆂ł̃_�b�V���̉�
    public bool chargedDash;//�`���[�W�_�b�V���\���ǂ���
    public float airHover = 0;//�󒆑؋󎞊�
    public bool grapping;//�����񂹉\���ǂ���
    public float grappingSpeed = 5;//�����񂹃X�s�[�h
    public bool drillJump;//�h�����W�����v�\��
    public bool stompAttack;//�X�^���v�A�^�b�N�\��
    public int orbitingOrbNumber = 0;//�T�[�N���I�[�u�̐�
    public float critChance;//�N���e�B�J���q�b�g�̉\��(0�`1)
    public float criticalDamageRate;//�N���e�B�J���{��
    public bool piercingArrow;//�|�ђʂ��邩�ǂ���
    public bool splittingArrow;//�|�����􂷂邩�ǂ���
    public float chanceOfInstantKill;//�����U���\��(0�`1)
    public bool wallStick;//�ǒ���t��

    [Space(10)][Header("WeaponSetting")]
    public int weaponIndex = 0; //attackSetting[weaponIndex]�̌`�ŗp����
    public AttackSetting[] attackSetting;
    public float playerScore = 0;
    public PlayerSetting copySource, pasteTarget;//�l�����Z�b�g���邱�Ƃ��ł���

    public void ResetHealthAndScoreValues()
    {
        health = maxHealth;
        playerScore = 0;
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
        if(fieldInfo != null)
        {
            fieldInfo.SetValue(this, newVal);
        }
    }
}
[CustomEditor(typeof(PlayerSetting))]
public class PlayerSettingEditor : Editor
{
    ///<summary>
    ///Inspector��GUI�X�V
    ///</summary>
    public override void OnInspectorGUI()
    {
        PlayerSetting playerSetting = target as PlayerSetting;
        base.OnInspectorGUI();
        if (GUILayout.Button("Reset Health & Score"))
        {
            playerSetting.ResetHealthAndScoreValues();
        }
        if (GUILayout.Button("ReplaceEveryValue"))
        {
            playerSetting.ResetValues();
        }
    }
}
public static class GizmosUtility //SceneView�����ŕ`�悳����@�����ł���
{
    private static int _circleVertexCount = 64;

    /// <summary>
    /// �~��`��(2D)
    /// </summary>
    /// <param name="center">���S�ʒu</param>
    /// <param name="radius">���a</param>
    public static void DrawWireCircle(Vector3 center, float radius)
    {
        DrawWireRegularPolygon(_circleVertexCount, center, Quaternion.identity, radius);
    }

    /// <summary>
    /// �����p�`��`��(2D)
    /// </summary>
    /// <param name="vertexCount">�p�̐�</param>
    /// <param name="center">���S�ʒu</param>
    /// <param name="radius">���a</param>
    public static void DrawWireRegularPolygon(int vertexCount, Vector3 center, float radius)
    {
        DrawWireRegularPolygon(vertexCount, center, Quaternion.identity, radius);
    }

    /// <summary>
    /// �~��`��(3D)
    /// </summary>
    /// <param name="center">���S�ʒu</param>
    /// <param name="rotation">��]</param>
    /// <param name="radius">���a</param>
    public static void DrawWireCircle(Vector3 center, Quaternion rotation, float radius)
    {
        DrawWireRegularPolygon(_circleVertexCount, center, rotation, radius);
    }

    /// <summary>
    /// �����p�`��`��(3D)
    /// </summary>
    /// <param name="vertexCount">�p�̐�</param>
    /// <param name="center">���S�ʒu</param>
    /// <param name="rotation">��]</param>
    /// <param name="radius">���a</param>
    public static void DrawWireRegularPolygon(int vertexCount, Vector3 center, Quaternion rotation, float radius)
    {
        if (vertexCount < 3)
        {
            return;
        }

        Vector3 previousPosition = Vector3.zero;

        // ��������1�X�e�b�v�̊p�x
        float step = 2f * Mathf.PI / vertexCount;
        // ���������J�n�p�x(�����Ȃ甼�X�e�b�v���炷)
        float offset = Mathf.PI * 0.5f + ((vertexCount % 2 == 0) ? step * 0.5f : 0f);

        for (int i = 0; i <= vertexCount; i++)
        {
            float theta = step * i + offset;

            float x = radius * Mathf.Cos(theta);
            float y = radius * Mathf.Sin(theta);

            Vector3 nextPosition = center + rotation * new Vector3(x, y, 0f);

            if (i == 0)
            {
                previousPosition = nextPosition;
            }
            else
            {
                Gizmos.DrawLine(previousPosition, nextPosition);
            }

            previousPosition = nextPosition;
        }
    }
}

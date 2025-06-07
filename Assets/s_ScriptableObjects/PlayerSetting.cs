using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System;

[System.Serializable]
public class PolarCoordinates//極座標の距離radiusと偏角angle 重力で使う
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
    public float health = 100; //体力
    public float maxHealth = 100;//体力最大値
    public float autoHealAmount = 1;//自然回復量
    public float autoHealInterval = 5;//自然回復間隔
    public float itemHealAmount = 30;//アイテム回復量
    public float defense = 0;//防御量
    public float movementSpeed = 5;//移動速度
    public float dashSpeed = 14;//ダッシュ速度
    public float jumpForce = 5;//ジャンプ力
    public float luck = 0;//運
    public PolarCoordinates gravity;//重力方向（－90が標準）

    [Space(10)][Header("SpecialAbility")]
    public int numberOfJump = 1;//ジャンプの回数
    public bool chargedJump;//チャージジャンプ可能かどうか
    public int numberOfDash = 1;//空中でのダッシュの回数
    public bool chargedDash;//チャージダッシュ可能かどうか
    public float airHover = 0;//空中滞空時間
    public bool grapping;//引き寄せ可能かどうか
    public float grappingSpeed = 5;//引き寄せスピード
    public bool drillJump;//ドリルジャンプ可能か
    public bool stompAttack;//スタンプアタック可能か
    public int orbitingOrbNumber = 0;//サークルオーブの数
    public float critChance;//クリティカルヒットの可能性(0～1)
    public float criticalDamageRate;//クリティカル倍率
    public bool piercingArrow;//弓貫通するかどうか
    public bool splittingArrow;//弓が分裂するかどうか
    public float chanceOfInstantKill;//即死攻撃可能性(0～1)
    public bool wallStick;//壁張り付き

    [Space(10)][Header("WeaponSetting")]
    public int weaponIndex = 0; //attackSetting[weaponIndex]の形で用いる
    public AttackSetting[] attackSetting;
    public float playerScore = 0;
    public PlayerSetting copySource, pasteTarget;//値をリセットすることができる

    public void ResetHealthAndScoreValues()
    {
        health = maxHealth;
        playerScore = 0;
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
    ///InspectorのGUI更新
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
public static class GizmosUtility //SceneViewだけで描画されるやつ　無視でいい
{
    private static int _circleVertexCount = 64;

    /// <summary>
    /// 円を描く(2D)
    /// </summary>
    /// <param name="center">中心位置</param>
    /// <param name="radius">半径</param>
    public static void DrawWireCircle(Vector3 center, float radius)
    {
        DrawWireRegularPolygon(_circleVertexCount, center, Quaternion.identity, radius);
    }

    /// <summary>
    /// 正多角形を描く(2D)
    /// </summary>
    /// <param name="vertexCount">角の数</param>
    /// <param name="center">中心位置</param>
    /// <param name="radius">半径</param>
    public static void DrawWireRegularPolygon(int vertexCount, Vector3 center, float radius)
    {
        DrawWireRegularPolygon(vertexCount, center, Quaternion.identity, radius);
    }

    /// <summary>
    /// 円を描く(3D)
    /// </summary>
    /// <param name="center">中心位置</param>
    /// <param name="rotation">回転</param>
    /// <param name="radius">半径</param>
    public static void DrawWireCircle(Vector3 center, Quaternion rotation, float radius)
    {
        DrawWireRegularPolygon(_circleVertexCount, center, rotation, radius);
    }

    /// <summary>
    /// 正多角形を描く(3D)
    /// </summary>
    /// <param name="vertexCount">角の数</param>
    /// <param name="center">中心位置</param>
    /// <param name="rotation">回転</param>
    /// <param name="radius">半径</param>
    public static void DrawWireRegularPolygon(int vertexCount, Vector3 center, Quaternion rotation, float radius)
    {
        if (vertexCount < 3)
        {
            return;
        }

        Vector3 previousPosition = Vector3.zero;

        // 線を引く1ステップの角度
        float step = 2f * Mathf.PI / vertexCount;
        // 線を引く開始角度(偶数なら半ステップずらす)
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

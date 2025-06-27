using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public enum SceneMode
{
    Survivor, KillNumber, Damage, GetFlags
}
public class SceneScript : MonoBehaviour
{
    public PlayerSetting playerSetting;
    public SceneMode sceneMode;
    public static int killNumber = 0;
    public static int damageCount = 0;
    public static int flagsCount = 0;
    public float displayDelay = 0.1f;
    public float displayDuration = 0.7f;
    public Image panelImage;
    public TMP_Text tmpText;
    public TextMeshProUGUI timeDisplay;
    public float limitTime = 90;
    private float limitTimer = 0;
    bool trigg = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        killNumber = 0;
        damageCount = 0;
        flagsCount = 0;
        tmpText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        limitTimer += Time.deltaTime;
        timeDisplay.text = Mathf.Ceil(limitTime - limitTimer).ToString();
        if (limitTimer >= limitTime || playerSetting.health == 0)
        {
            limitTimer = 0;
            if (!trigg)
            {
                trigg = true;
                Result();
            }
        }
    }
    public static void KillEnemy()
    {
        killNumber++;
    }
    public static void GetDamaged()
    {
        damageCount++;
    }
    public static void GetFlag()
    {
        flagsCount++;
    }
    public void Result()
    {
        int resultScore = 0;
        switch (sceneMode)
        {
            case SceneMode.Survivor:
                if (playerSetting.health > 0)
                    resultScore = 100;
                else
                    resultScore = 0;
                break;
            case SceneMode.KillNumber:
                resultScore = Mathf.Clamp(50 + killNumber, 50, 100);
                break;
            case SceneMode.Damage:
                if (playerSetting.health > 0)
                    resultScore = 100 - 5*damageCount;
                else
                    resultScore = 0;
                break;
            case SceneMode.GetFlags:
                resultScore = Mathf.Clamp(50 + flagsCount * 10, 50, 100);
                break;
            default:
                resultScore = 0;
                break;
        }
        StartCoroutine(ResultPanel(resultScore));
    }
    public IEnumerator ResultPanel(int score)
    {
        tmpText.text = "Final Score: " + score + "\n";
        if (score == 100)
            tmpText.text += "perfect";
        else if (score >= 90)
            tmpText.text += "excellent";
        else if (score >= 80)
            tmpText.text += "great";
        else if (score >= 50)
            tmpText.text += "good";
        else
            tmpText.text += "bad";
        tmpText.ForceMeshUpdate(true);
        TMP_TextInfo textInfo = tmpText.textInfo;
        TMP_CharacterInfo[] charInfos = textInfo.characterInfo;
        for (int i = 0; i < charInfos.Length; i++)
        {
            SetTextAlpha(tmpText, i, 0);
        }
        Color mainColor = Color.white;
        mainColor.a = 0;
        panelImage.color = mainColor;
        yield return new WaitForSeconds(0.5f);
        for (float counter = 0; counter < displayDuration; counter += Time.deltaTime)
        {
            mainColor.a = counter / displayDuration;
            panelImage.color = mainColor;
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < charInfos.Length; i++)
        {
            if (char.IsWhiteSpace(charInfos[i].character)) continue;
            yield return new WaitForSeconds(displayDelay);
            byte alpha = 0;
            while (true)
            {
                yield return new WaitForFixedUpdate();
                alpha = (byte)Mathf.Min(alpha + 30, 255);
                SetTextAlpha(tmpText, i, alpha);
                if (alpha >= 255) break;
            }
        }

    }
    private void SetTextAlpha(TMP_Text text, int charIndex, byte alpha)
    {
        // charIndex番目の文字のデータ構造体を取得
        TMP_TextInfo textInfo = text.textInfo;
        TMP_CharacterInfo charInfo = textInfo.characterInfo[charIndex];

        // 文字を構成するメッシュ(矩形)を取得
        TMP_MeshInfo meshInfo = textInfo.meshInfo[charInfo.materialReferenceIndex];

        // 矩形なので4頂点
        var rectVerticesNum = 4;
        for (var i = 0; i < rectVerticesNum; ++i)
        {
            // 一文字を構成する矩形の頂点の透明度を変更
            meshInfo.colors32[charInfo.vertexIndex + i].a = alpha;
        }

        // 頂点カラーを変更したことを通知
        text.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
    }
    public void SceneTransition(string nextSceneName)
    {
        SceneManager.LoadScene(nextSceneName);
    }
}

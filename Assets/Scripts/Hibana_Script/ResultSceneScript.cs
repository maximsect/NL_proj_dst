using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class ResultSceneScript : MonoBehaviour
{
    public TextMeshProUGUI resultText;
    public ScoreData scoreData;
    public SceneData sceneData;
    public PlayerData playerData;
    private float sumElapsedTime = 0;
    private int sumNumberOfKill = 0;
    private int sumDamageAmount = 0;
    private float sumScore = 0;

    public void ReturnToTitleScene()
    {
        scoreData.Initialize();
        playerData.ResetValue();
        sceneData.Initialize();
        this.LoadSceneByName("StartScene");
    }
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "ResultScene")
        {
            ScoreCalculation();
            resultText.text =
                "���U�����ԁ@�@�F" + sumElapsedTime +
                "\n�������@�@�@�@�F" + sumNumberOfKill +
                "\n���_���[�W�@�@�F" + sumDamageAmount +
                "\n���X�R�A�@�@�@�F" + sumScore;
        }

    }
    void ScoreCalculation()
    {
        for (int i = 0; i < scoreData.scoreList.Count; i++)
        {
            sumElapsedTime += scoreData.scoreList[i].elapsedTime;
            sumNumberOfKill += scoreData.scoreList[i].numberOfKill;
            sumDamageAmount += scoreData.scoreList[i].damageAmount;
            sumScore += scoreData.scoreList[i].sceneScore;
        }
    }
}

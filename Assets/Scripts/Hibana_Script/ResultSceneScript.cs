using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultSceneScript : MonoBehaviour
{
    public TextMeshProUGUI resultText;
    public ScoreData scoreData;
    public void ReturnToTitleScene()
    {
        this.LoadSceneByName("StartScene");
    }
    void Start()
    {
        resultText.text =
            "���U�����ԁ@�@�F" + scoreData.elapsedTime +
            "\n�������@�@�@�@�F" + scoreData.numberOfKill +
            "\n��_���[�W�@�@�F" + scoreData.damageAmount +
            "\n���X�R�A�@�@�@�F" + scoreData.sumScore;
            
    }
}

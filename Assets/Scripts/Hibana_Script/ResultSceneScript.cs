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
            "総攻略時間　　：" + scoreData.elapsedTime +
            "\n討伐数　　　　：" + scoreData.numberOfKill +
            "\n被ダメージ　　：" + scoreData.damageAmount +
            "\n総スコア　　　：" + scoreData.sumScore;
            
    }
}

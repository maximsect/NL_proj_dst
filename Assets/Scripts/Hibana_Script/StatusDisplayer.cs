using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class StatusDisplayer : MonoBehaviour
{
    public PlayerData playerData;
    public ScoreData scoreData;
    public SceneData sceneData;
    public static StatusDisplayer main1;
    public GameObject displayer;
    private TextMeshProUGUI statusText;

    bool isOn = false;
    void Start()
    {
        if (main1 == null) main1 = this;
        else Destroy(this.gameObject);
        DontDestroyOnLoad(this.gameObject);
        StartCoroutine(MyUpdate());
    }
    IEnumerator MyUpdate()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(0.02f);
            if (Input.GetKey(KeyCode.R))
            {
                isOn = !isOn;
                StatusOpen();
                yield return new WaitForSecondsRealtime(0.3f);
            }
            if (isOn && Input.GetKey(KeyCode.H))
            {
                ReturnToTitleScene();
            }
            if(isOn && Input.GetKey(KeyCode.Q))
            {

            }
        }
    }
    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
      UnityEngine.Application.Quit();
#endif
    }
    public void ReturnToTitleScene()
    {
        Time.timeScale = 1;
        scoreData.Initialize();
        playerData.ResetValue();
        sceneData.Initialize();
        this.LoadSceneByName("StartScene");
    }
    GameObject generated;
    void StatusOpen()
    {
        
        if (isOn)
        {
            generated = Instantiate(displayer) as GameObject;
            generated.transform.parent = GameObject.Find("Canvas").transform;
            generated.transform.localScale = Vector3.one;
            generated.transform.localPosition = Vector3.zero;
            statusText = generated.transform.GetChild(generated.transform.childCount - 1).GetComponent<TextMeshProUGUI>();
            Time.timeScale = 0;
            statusText.text = "MAX HP：" + playerData.maxHp
            + "\nHP：" + playerData.hp
            + "\n移動速度：" + playerData.moveSpeed
            + "\nジャンプ力：" + playerData.jumpSpeed
            + "\nこん棒攻撃力：" + playerData.batAttack
            + "\n槍攻撃力：" + playerData.spearAttack
            + "\n弓攻撃力：" + playerData.bowAttack
            + "\n金槌攻撃力：" + playerData.hammerAttack
            + "\n固有スキル攻撃力：" + playerData.skillAttack;
        }
        else
        {
            if(generated != null)
            Destroy(generated);
            Time.timeScale = 1;
        }
    }
}






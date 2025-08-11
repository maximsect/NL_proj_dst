using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class StageManager : MonoBehaviour
{
    public SceneData sceneData;
    public GameObject stageDecriptionPanel;
    private GameObject generatedPanel;
    private TextMeshProUGUI stageDescription;
    private Image blackScreen;
    public AudioClip successSound;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void MainStart()
    {
        generatedPanel = Instantiate(stageDecriptionPanel, GameObject.Find("Canvas").transform);
        blackScreen = generatedPanel.GetComponent<Image>();
        stageDescription = generatedPanel.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        StartCoroutine(Description());
    }
    IEnumerator Description()
    {
        Time.timeScale = 0;
        stageDescription.text = sceneData.GetDescription();
        yield return StartCoroutine(WaitForClick());
        Time.timeScale = 1;
        blackScreen.color = new Color(0, 0, 0, 0);
        stageDescription.text = "";
    }
    IEnumerator WaitForClick()
    {
        while (true)
        {
            if (Input.GetMouseButtonDown(0) || Input.anyKey)
            {
                yield break;
            }
            yield return null;
        }
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class GameManager : MonoBehaviour
{
    public static GameManager main;
    public ScoreData scoreData;
    public GameObject camera;
    public GameObject player;
    public List<string> playerTag = new List<string>();
    public List<string> enemyTag = new List<string>();
    public List<GameObject> killEffect = new List<GameObject>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        if (main == null) main = this;
    }
    public GameObject GetKillEffect()
    {
        return killEffect[UnityEngine.Random.Range(0, killEffect.Count)];
    }
    public void ChangeScore(int delta)
    {
        scoreData.sumScore += delta;
    }
}
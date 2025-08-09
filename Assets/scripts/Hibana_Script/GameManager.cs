using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager main;
    public GameObject camera;
    [HideInInspector] public GameObject player;
    [HideInInspector] public List<string> playerTag = new List<string>() { "bow", "spear", "bat", "hammer", "arrow", "skillattack" };
    public List<string> enemyTag = new List<string>();
    public List<GameObject> killEffect = new List<GameObject>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        if (main == null) main = this;
        player = GameObject.Find("player");
    }
    public GameObject GetKillEffect()
    {
        return killEffect[UnityEngine.Random.Range(0, killEffect.Count)];
    }
}
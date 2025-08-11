using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager main;
    public PlayerData playerData;
    public static GameObject player;
    public static List<string> playerWeaponTag = new List<string>() { "bow", "spear", "bat", "hammer", "arrow", "skillattack" };
    public static List<string> enemyWeaponTag = new List<string>() { "enemyweapon", "damage_factor", "assign_attack" };
    public static List<string> syllabusLevel = new List<string>() { "ê_", "ëÂïß", "ïß", "ãS", "ëÂãS" };
    //public static List<string> stageMode = new List<string>() { "GetFlag", "LessDamage", "KillEnemy", "Survivor", "NoDamage" };
    public List<GameObject> killEffect = new List<GameObject>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        if (main == null) main = this;
        else Destroy(this.gameObject);
        playerData.OnStartSetting();
        player = GameObject.Find("player");
        if(player == null)
        {
            player = GameObject.Instantiate(PlayerData.main.playerPrefab, Vector3.zero, Quaternion.identity);
        }
        DontDestroyOnLoad(this.gameObject);
    }
    public GameObject GetKillEffect()
    {
        return killEffect[UnityEngine.Random.Range(0, killEffect.Count)];
    }
}
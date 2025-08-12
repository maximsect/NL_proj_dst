using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
public class GameManager : MonoBehaviour
{
    public static GameManager main;
    public PlayerData playerData;
    public static GameObject player;
    public static List<string> playerWeaponTag = new List<string>() { "bow", "spear", "bat", "hammer", "arrow", "skillattack" };
    public static List<string> enemyWeaponTag = new List<string>() { "enemyweapon", "damage_factor", "assign_attack","waveattack", "kawara_red","kawara_blue"};
    public static List<string> syllabusLevel = new List<string>() { "神", "大仏", "仏", "鬼", "大鬼" };
    //public static List<string> stageMode = new List<string>() { "GetFlag", "LessDamage", "KillEnemy", "Survivor", "NoDamage" };
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private AudioSource seSource, bgmSource;
    void OnEnable()
    {
        if (main == null) main = this;
        else Destroy(this.gameObject);
        playerData.OnStartSetting();
        player = GameObject.Find("player");
        if (player == null)
        {
            player = GameObject.Instantiate(PlayerData.main.playerPrefab, Vector3.zero, Quaternion.identity);
        }
        GameObject seObj = GameObject.Instantiate(playerData.seObj, GameObject.Find("Main Camera").transform);
        seObj.transform.localPosition = Vector3.zero;
        seSource = seObj.GetComponent<AudioSource>();
    }
    public void PlayOneShot(AudioClip sound)
    {
        seSource.pitch = UnityEngine.Random.Range(1f, 1.2f);
        seSource.PlayOneShot(sound);
    }
    public GameObject GetKillEffect()
    {
        return playerData.killEffect[UnityEngine.Random.Range(0, playerData.killEffect.Count)];
    }

}

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ScoreData", menuName = "Scriptable Objects/ScoreData")]
public class ScoreData : ScriptableObject
{
    public List<int> scores = new List<int>();
    public int sumScore = 0;
    public float elapsedTime = 0;
    public int numberOfKill = 0;
    public int numberOfDeath = 0;
    public int DamageAmount = 0;
}

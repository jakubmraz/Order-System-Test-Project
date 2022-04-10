using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Mission", menuName = "ScriptableObjects/Mission", order = 1)]
public class MissionSO: ScriptableObject
{
    public string Name;
    public int Chapter;
    public string Description;
    public string Goals;
    public int MoneyReward;
    public int XpReward;
    public string PrefName;

    //1 stands for true if mission is a "do something once" bool-type mission
    public int PrefGoalValue = 1;
    public Sprite Sprite;
}

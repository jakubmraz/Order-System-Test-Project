using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ContractBuilding", menuName = "ScriptableObjects/ContractBuilding", order = 1)]
public class ContractBuildingSO : ScriptableObject
{
    public string BuildingName;
    public List<ItemData> DesiredItems;
}

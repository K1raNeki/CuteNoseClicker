using System;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgrageData", menuName = "Scriptable Objects/UpgrageData")]
public class UpgrageData : ScriptableObject
{
    [Header("Data")]
    public UpgradeType Type;
    public string Name;
    public int[] Prices;
    public int[] Values;
}

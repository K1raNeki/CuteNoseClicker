using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "AnimalData", menuName = "Scriptable Objects/AnimalData")]
public class AnimalData : ScriptableObject
{
    [Header("Names")]
    public string ID;
    public string Name;

    [Header("Settings")]
    public float NeedCare;

    [Header("Visual")]
    public Image Icon;
    public Image MainView;

}

using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "AnimalData", menuName = "Scriptable Objects/AnimalData")]
public class AnimalData : ScriptableObject
{
    [Header("Passport")]
    public string ID;
    public string Name;
    public Sprite Icon;
    public GameObject Prefab;

    [Header("Anim")]
    public AnimatorOverrideController OverrideController;

    [Header("Balance")]
    public float NeedCare;
    public int Attemps = 3;
    public float Penalty = 0.05f;

    [Header("MiniGames")]
    public AnimalMiniGameFactor[] MiniGames;
}

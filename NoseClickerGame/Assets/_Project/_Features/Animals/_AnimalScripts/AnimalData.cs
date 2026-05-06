using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "AnimalData", menuName = "Scriptable Objects/AnimalData")]
public class AnimalData : ScriptableObject
{
    [Header("Passport")]
    public string ID;
    public string Name;
    public Sprite Icon;

    [Header("Anim")]
    public AnimatorOverrideController OverrideController;

    [Header("Balance")]
    public float NeedCare;
    public int Attemps = 3;

    [Header("MiniGames")]
    public AnimalMiniGameFactor[] MiniGames;
}

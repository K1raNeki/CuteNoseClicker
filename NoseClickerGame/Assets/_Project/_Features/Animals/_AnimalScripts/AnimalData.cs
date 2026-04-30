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

    [Header("Balanse")]
    public float NeedCare;
    public float AngryMultiplier;
    public float[] AngryPoints;

}

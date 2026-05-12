using UnityEngine;

[CreateAssetMenu(fileName = "MiniGameData", menuName = "Scriptable Objects/MiniGameData")]
public class MiniGameDataMain : ScriptableObject
{
    [Header("Spawn")]
    public float TimerForSpawr =1;
    public float MinPossibleFactor = 0.9f;
    public float MaxPossibleFactor = 3.2f;

    [Header("Visual")]
    public float LineHight = 4;

    [Header("Prefabs")]
    public GameObject LinePrefab;
    public MiniGamePoint PointPrefab;

}

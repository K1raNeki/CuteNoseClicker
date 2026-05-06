using UnityEngine;

[CreateAssetMenu(fileName = "AnimalMiniGameFactor", menuName = "Scriptable Objects/AnimalMiniGameFactor")]
public class AnimalMiniGameFactor : ScriptableObject
{
    [Header("MiniGameSrange")]
    public float AngryBarPositionX;
    public float StartAngerFill;
    public bool CanInvertLine;
    public float[] InvertPoints;


    [Header("PointsSettings")]
    public float MoveSpeedPoint = 6;
    public float ScoreTaked = 0.1f;
    public PointTypes[] Types;
}

using UnityEngine;

[System.Serializable]
public class GlobalRates
{
    [Header("BaseStates")]
    public float TakedCare { get; private set; } = 1f; // in a animal

    [Header("UpgradesStates")]
    public float MultyClickChanse { get; private set; }
    private readonly int _maxCombo = 10;
    public float CriticalCare { get; private set; }
    private readonly int _critChanse = 30;
    public float AutoClickDelta { get; private set; }

    public int RollMultyClick()
    {
        int round = 1;

        for (int i = 0; i <= _maxCombo; i++)
        {
            if (Random.Range(0.01f, 100) <= MultyClickChanse)
                round++;
            else
                break;
        }

        return round;
    }

    public bool WillBeCrit()
    {
        if (Random.Range(0, 100) <= _critChanse)
            return true;
        else
            return false;
    }

    public void GetUpgrade(UpgradeType type, float value)
    {
        switch (type)
        {
            case UpgradeType.AutoClick:
                AutoClickDelta = value;
                break;

            case UpgradeType.CritClick:
                CriticalCare = value;
                break;

            case UpgradeType.MultyClick:
                MultyClickChanse = value;
                break;

            default:
                Debug.Log("Прокачено что то кроме 3х базовых статов или печенье");
                break;
        }
    }
}

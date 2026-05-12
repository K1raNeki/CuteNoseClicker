using UnityEngine;

[System.Serializable]
public class GlobalRates
{
    [Header("BaseStates")]
    public float TakedCare { get; private set; } // in a animal

    [Header("UpgradesStates")]
    public float MultyClickChanse { get; private set; }
    private int _maxCombo = 10;
    public float CriticalCare { get; private set; }
    public float AutoClickDelta { get; private set; }

    public int RollMultyClick()
    {
        int round = 1;
        float currentChange = MultyClickChanse;

        for (int i = 0; i <= _maxCombo; i++)
        {
            if (Random.Range(0, 100) <= currentChange)
            {
                round++;
                currentChange *= 0.5f;
            }
            else
                break;
        }

        return round;
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
        }
    }
}

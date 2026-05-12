using UnityEngine;

public class EventManager : MonoBehaviour
{
    [Header("LinksMain")]
    public Animal CurrentAnimal;
    public MiniGameMain MiniGameMain;

    [Header("LinksUI")]
    public ProgressBarUI ProgressBarUI;
    public HealthPointsUI HealthPointsUI;
    public UpgradeButtons UpgradeButtons;

    [Header("LinksManagers")]
    public GlobalRates GlobalRates;


    private static bool _exists;


    private void Awake()
    {
        if (_exists)
        {
            Destroy(gameObject);
            return;
        }
        _exists = true;
        DontDestroyOnLoad(gameObject);

        Init();
    }

    private void Init()
    {
        CurrentAnimal.Rates = GlobalRates;

        ProgressBarUI.CurrentAnimal = CurrentAnimal;
        MiniGameMain.CurrentAnimal = CurrentAnimal;
        HealthPointsUI.CurrentAnimal = CurrentAnimal;

        CurrentAnimal.AnimalAgressiveStart += StartMiniGame;
        MiniGameMain.ResultMiniGame += MiniGameEnd;
        HealthPointsUI.HeelthEnded += LooseGame;
        UpgradeButtons.OnUpgrade += GlobalRates.GetUpgrade;
    }

    private void StartMiniGame(bool isStart, AnimalMiniGameFactor config)
    {
        MiniGameMain.StartMiniGame(isStart, config);
        ProgressBarUI.HideBaseProgress(isStart);
    }

    private void MiniGameEnd(bool result)
    {
        CurrentAnimal.CompleteActiveMiniGame(result);
        CurrentAnimal.AgressionStart(false, MiniGameMain.CurrentFactor);
        CurrentAnimal.SwitchStatet(AnimalState.Default);

        if (!result)
        {
            HealthPointsUI.TakeLoose();
        }
    }

    private void LooseGame()
    {
        CurrentAnimal.SwitchStatet(AnimalState.Angry);
        Debug.Log("pososal");
    }
}

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
    public CurrencyUI CurrencyUI;

    [Header("LinksManagers")]
    public GlobalRates GlobalRates;
    public PaymentsManager PaymentsManager;


    // private static bool _exists;
    private static EventManager current;

    private void Awake()
    {
        if (current != null)
            Destroy(current.gameObject);

        // if (_exists)
        // {
        //     Destroy(gameObject);
        //     return;
        // }
        // _exists = true;
        DontDestroyOnLoad(gameObject);

        Init();
    }

    private void Init()
    {
        CurrentAnimal.AnimalAgressiveStart += StartMiniGame;
        MiniGameMain.ResultMiniGame += MiniGameEnd;
        HealthPointsUI.HeelthEnded += LooseGame;

        ProgressBarUI.CurrentAnimal = CurrentAnimal;
        MiniGameMain.CurrentAnimal = CurrentAnimal;
        HealthPointsUI.CurrentAnimal = CurrentAnimal;
        CurrentAnimal.Rates = GlobalRates;

        UpgradeButtons.OnUpgrade += GlobalRates.GetUpgrade;
        CurrentAnimal.AnimalTakeCare += ProgressBarUI.UpdateUIBarProgress;
        CurrentAnimal.AnimalTakeCare += PaymentsManager.AddLoveCurrency;
        PaymentsManager.OnLoveCurrencyTransaction += CurrencyUI.UpdateLoveCurrencyValueUI;
        PaymentsManager.OnKeysTransaction += CurrencyUI.UpdateKeysValueUI;
        CurrencyUI.UpdateKeysValueUI(PaymentsManager.Keys); CurrencyUI.UpdateLoveCurrencyValueUI(PaymentsManager.LoveCurrency);
        CurrentAnimal.AnimalAgressiveStart += ProgressBarUI.UpdateAngryPointCompleted;
    }

    private void StartMiniGame(bool isStart, AnimalMiniGameFactor config)
    {
        MiniGameMain.StartMiniGame(isStart, config);
        ProgressBarUI.HideBaseProgress(isStart);
        UpgradeButtons.HideButtonContainer(isStart);
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

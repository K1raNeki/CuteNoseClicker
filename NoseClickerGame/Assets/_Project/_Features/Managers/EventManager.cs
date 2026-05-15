using System;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private static EventManager _current;


    public static Action<bool> OnOpenMainsContainer;

    private void Awake()
    {
        if (_current != null)
            Destroy(_current.gameObject);
        DontDestroyOnLoad(gameObject);

        PlayMainButton.OnStartGameWithCurrentAnimal += SetupAnimal;

        Init();
    }
    private void SetupAnimal(Animal animal) => CurrentAnimal = animal;
    private void Init()
    {
        CurrentAnimal.AnimalAgressiveStart += StartMiniGame;
        MiniGameMain.ResultMiniGame += MiniGameEnd;
        HealthPointsUI.HeelthEnded += LooseGame;

        ProgressBarUI.CurrentAnimal = CurrentAnimal;
        MiniGameMain.CurrentAnimal = CurrentAnimal;
        HealthPointsUI.CurrentAnimal = CurrentAnimal;
        CurrentAnimal.Rates = GlobalRates;

        CurrentAnimal.AnimalTakeCare += ProgressBarUI.UpdateUIBarProgress;
        CurrentAnimal.CurrentTakedCare += PaymentsManager.AddLoveCurrency;
        CurrentAnimal.AnimalAgressiveStart += ProgressBarUI.UpdateAngryPointCompleted;
        // UpgradeButtons.OnUpgrade += GlobalRates.GetUpgrade;
        // PaymentsManager.OnLoveCurrencyTransaction += CurrencyUI.UpdateLoveCurrencyValueUI;
        // PaymentsManager.OnKeysTransaction += CurrencyUI.UpdateKeysValueUI;
        // CurrencyUI.UpdateKeysValueUI(PaymentsManager.Keys); CurrencyUI.UpdateLoveCurrencyValueUI(PaymentsManager.LoveCurrency);
    }

    private void StartMiniGame(bool isStart, AnimalMiniGameFactor config)
    {
        MiniGameMain.StartMiniGame(isStart, config);
        ProgressBarUI.HideBaseProgress(isStart);
        UpgradeButtons.HideButtonContainer(!isStart);
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

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PaymentsManager.ResetEvent();
        UpgradeButtons.ResetEvent();

        CurrencyUI = FindAnyObjectByType<CurrencyUI>();
        UpgradeButtons = FindAnyObjectByType<UpgradeButtons>();

        if (CurrencyUI != null)
        {
            PaymentsManager.OnLoveCurrencyTransaction += CurrencyUI.UpdateLoveCurrencyValueUI;
            PaymentsManager.OnKeysTransaction += CurrencyUI.UpdateKeysValueUI;

            CurrencyUI.UpdateLoveCurrencyValueUI(PaymentsManager.LoveCurrency);
            CurrencyUI.UpdateKeysValueUI(PaymentsManager.Keys);
        }
        else
            Debug.Log("На сцене нет Канваза Валюты");

        if (UpgradeButtons != null)
        {
            UpgradeButtons.PaymentsManager = PaymentsManager;
            UpgradeButtons.OnUpgrade += GlobalRates.GetUpgrade;
        }
        else
            Debug.Log("На сцене нет Кнопок апгрейда");
    }
    private void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
    private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;
}

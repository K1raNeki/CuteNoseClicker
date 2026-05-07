using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Links")]
    public Animal CurrentAnimal;
    public ProgressBarUI ProgressBarUI;
    public MiniGameMain MiniGameMain;
    public HealthPointsUI HealthPointsUI;

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

    private void Start()
    {
        CurrentAnimal.AnimalAgressiveStart += StartMiniGame;
        MiniGameMain.ResultMiniGame += MiniGameEnd;
        HealthPointsUI.HeelthEnded += LooseGame;
    }

    private void Init()
    {
        ProgressBarUI.CurrentAnimal = CurrentAnimal;
        MiniGameMain.CurrentAnimal = CurrentAnimal;
        HealthPointsUI.CurrentAnimal = CurrentAnimal;
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

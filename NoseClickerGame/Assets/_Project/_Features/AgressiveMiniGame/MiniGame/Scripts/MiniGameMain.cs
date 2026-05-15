using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameMain : MonoBehaviour
{
    [Header("Links")]
    [HideInInspector] public Animal CurrentAnimal;
    [HideInInspector] public AnimalMiniGameFactor CurrentFactor;
    public MiniGameDataMain Data;
    public GameObject MinigameContainer;
    [SerializeField] private Collider2D _barier;
    public Image AgressiveBar;
    [HideInInspector] public GameObject CurrentLine;
    public Image InvertPointPrefab;

    [Header("ObjectPoolin")]
    private PointController _pointController;
    [HideInInspector] public List<MiniGamePoint> BusyObjects = new();
    public List<MiniGamePoint> FreeObjects = new();
    public GameObject FreeObjectsParent;

    [Header("Settings")]
    private MiniGameLineController _miniGameLineController;
    private float _angryScore;
    [HideInInspector] public bool GameIsStart;
    [HideInInspector] public bool LineInvert;
    [HideInInspector] public List<InvertPoint> InvertsPoint = new();


    private void Awake()
    {
        _pointController = new PointController(
            FreeObjects,
            BusyObjects,
            Data.PointPrefab,
            _barier);

        _miniGameLineController = new MiniGameLineController(this);
    }

    private void Start()
    {
        MinigameContainer.SetActive(false);
        MiniGamePoint.OnPointResult += GetPointResult;
        MiniGamePoint.OnPointFinished += _pointController.RecyclePoint;
    }

    private void Update() => _pointController.TimerForSpawn(GameIsStart, Data, CurrentFactor, LineInvert);

    public event Action<bool> ResultMiniGame;

    public void StartMiniGame(bool isStart, AnimalMiniGameFactor config)
    {
        LineInvert = false;
        GameIsStart = isStart;
        CurrentFactor = config;
        _angryScore = CurrentFactor.AngryBarPositionX;
        _barier.gameObject.SetActive(isStart);
        MinigameContainer.SetActive(isStart);

        _miniGameLineController.CreateButtonLines(isStart);
        UpdateUI();
    }

    private void GetPointResult(bool positive, float score)
    {
        _angryScore += positive ? -score : score;

        if (Mathf.Round(_angryScore * 100) / 100 <= 0)
        {
            _angryScore = 0;
            // Debug.Log("Win minigame");
            ResultMiniGame?.Invoke(true);
        }
        else if (_angryScore >= 1)
        {
            _angryScore = 1;
            // Debug.Log("Loose");
            ResultMiniGame?.Invoke(false);
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        AgressiveBar.fillAmount = _angryScore;

        if (!CurrentFactor.CanInvertLine)
            return;
        for (int i = 0; i < CurrentFactor.InvertPoints.Length; i++)
        {
            float tempScore = Mathf.Round(_angryScore * 100) / 100;
            float invertPoint = CurrentFactor.InvertPoints[i];
            if (tempScore >= invertPoint
                && ((tempScore - invertPoint) <= CurrentFactor.ScoreTaked)
                && !InvertsPoint[i].Completed)
            {
                InvertsPoint[i].Completed = true;
                InvertsPoint[i].ImagePoit.color = Color.gray;
                StartCoroutine(_miniGameLineController.InvertLine(1, _barier));
            }
        }
    }

    void OnDestroy()
    {
        MiniGamePoint.OnPointResult -= GetPointResult;
        MiniGamePoint.OnPointFinished -= _pointController.RecyclePoint;
    }

    public class InvertPoint
    {
        public Image ImagePoit;
        public bool Completed;
        public InvertPoint(Image img)=> ImagePoit = img;
    }
}

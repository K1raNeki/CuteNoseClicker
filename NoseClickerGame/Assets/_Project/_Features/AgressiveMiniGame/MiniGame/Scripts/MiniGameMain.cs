using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameMain : MonoBehaviour
{
    [Header("Links")]
    [HideInInspector] public Animal CurrentAnimal;
    [HideInInspector] public AnimalMiniGameFactor CurrentFactor;
    [SerializeField] private MiniGameDataMain _data;
    [SerializeField] private GameObject _minigameContainer;
    [SerializeField] private Collider2D _barier;
    [SerializeField] private Image _agressiveBar;
    private GameObject _currentLine;

    [Header("ObjectPoolin")]
    PointController _pointController;
    private List<MiniGamePoint> _busyObjects = new List<MiniGamePoint>();
    [SerializeField] private List<MiniGamePoint> _freeObjects = new List<MiniGamePoint>();
    [SerializeField] private GameObject _freeObjectsParent;

    [Header("Settings")]
    private float _angryScore;
    private bool _gameIsStart;
    private float _timer;



    private void Awake()
    {
        _pointController = new PointController(
            _freeObjects,
            _busyObjects,
            _data.PointPrefab,
            _barier);
    }

    private void Start()
    {
        MiniGamePoint.OnPointResult += GetPointResult;
        MiniGamePoint.OnPointFinished += _pointController.RecyclePoint;
    }

    private void Update() => _pointController.TimerForSpawn(_gameIsStart, _data, CurrentFactor);

    public event Action<bool> ResultMiniGame;

    public void StartMiniGame(bool isStart, AnimalMiniGameFactor config)
    {
        _gameIsStart = isStart;
        CurrentFactor = config;
        _angryScore = CurrentFactor.AngryBarPositionX;
        _barier.gameObject.SetActive(isStart);
        _minigameContainer.SetActive(isStart);


        CreateButtonLines(isStart);
        UpdateUI();
    }

    private void CreateButtonLines(bool isStart)
    {
        switch (isStart)
        {
            case true:
                if (_currentLine != null)
                    _currentLine.SetActive(true);
                else
                    _currentLine = Instantiate(_data.LinePrefab, _minigameContainer.transform);

                _currentLine.transform.localScale = new Vector2(Camera.main.orthographicSize * 2 * Camera.main.aspect, _data.LineHight);
                break;

            case false:
                _currentLine.SetActive(false);

                foreach (MiniGamePoint point in _busyObjects)
                {
                    _freeObjects.Add(point);
                    point.gameObject.SetActive(false);
                }
                _busyObjects.Clear();
                break;
        }
    }

    private void GetPointResult(bool positive, float score)
    {
        _angryScore += positive ? -score : score;

        if (Mathf.Round(_angryScore * 100) / 100 <= 0)
        {
            _angryScore = 0;
            Debug.Log("Win minigame");
            ResultMiniGame?.Invoke(true);
        }
        else if (_angryScore >= 1)
        {
            _angryScore = 1;
            Debug.Log("Loose");
            ResultMiniGame?.Invoke(false);
        }

        UpdateUI();
    }

    private void UpdateUI() => _agressiveBar.fillAmount = _angryScore;

    void OnDestroy()
    {
        MiniGamePoint.OnPointResult -= GetPointResult;
        MiniGamePoint.OnPointFinished -= _pointController.RecyclePoint;
    }
}

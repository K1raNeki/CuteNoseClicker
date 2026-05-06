using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameMain : MonoBehaviour
{
    [Header("Links")]
    public Animal CurrentAnimal;
    [SerializeField] private MiniGameDataMain _data;
    private AnimalMiniGameFactor _currentFactor;
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



    void Awake()
    {
        _pointController = new PointController(
            _freeObjects,
            _busyObjects,
            _data.PointPrefab,
            _barier);

        CurrentAnimal.AnimalAgressiveStart += CreateButtonLines;
        MiniGamePoint.OnPointResult += GetPointResult;
        MiniGamePoint.OnPointFinished += _pointController.RecyclePoint;

        UpdateUI();
    }

    void Update()
    {
        TimerForSpawn();
    }

    private void CreateButtonLines(bool isStart, AnimalMiniGameFactor config)
    {
        _currentFactor = config;

        _gameIsStart = isStart;
        _barier.gameObject.SetActive(isStart);
        _minigameContainer.SetActive(isStart);
        _angryScore = _currentFactor.AngryBarPositionX;
        UpdateUI();

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
            CurrentAnimal.AgressionStart(false, _currentFactor);
            CurrentAnimal.SwitchStatet(AnimalState.Default);
        }
        else if (_angryScore >= 1)
        {
            _angryScore = 1;
            Debug.Log("Loose");
            CurrentAnimal.AgressionStart(false, _currentFactor);
            CurrentAnimal.SwitchStatet(AnimalState.Default);
        }

        UpdateUI();
    }

    private void TimerForSpawn()
    {
        if (_gameIsStart)
        {
            _timer += _data.SpeedFactorFSpawn * Time.deltaTime;
            if (_timer >= 1)
            {
                _data.SpeedFactorFSpawn = Random.Range(_data.MinPossibleFactor, _data.MaxPossibleFactor);
                _timer = 0;
                _pointController.CreatePoint(_currentFactor.ScoreTaked);
            }
        }
    }

    private void UpdateUI() => _agressiveBar.fillAmount = _angryScore;

    void OnDestroy()
    {
        CurrentAnimal.AnimalAgressiveStart -= CreateButtonLines;
        MiniGamePoint.OnPointResult -= GetPointResult;
        MiniGamePoint.OnPointFinished -= _pointController.RecyclePoint;
    }
}

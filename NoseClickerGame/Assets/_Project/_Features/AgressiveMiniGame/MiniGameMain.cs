using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameMain : MonoBehaviour
{
    [Header("Links")]
    public Animal CurrentAnimal;

    [Header("MiniGamePrefabs")]
    [SerializeField] private GameObject _minigameContainer;
    [SerializeField] private GameObject _linePrefab;
    [SerializeField] private Image _agressiveBar;
    private GameObject _currentLine;
    [SerializeField] private Collider2D _barier;

    [Header("ObjectPoolin")]
    private List<MiniGamePoint> _busyObjects = new List<MiniGamePoint>();
    [SerializeField] private List<MiniGamePoint> _freeObjects = new List<MiniGamePoint>();
    [SerializeField] private GameObject _freeObjectsParent;
    [SerializeField] private MiniGamePoint _pointPrefab;

    [Header("Settings")] //huynya in config
    private float _angryScore;
    private float _lineHight = 4;
    private bool _gameIsStart;
    private float _timer;
    private float _speedTimer = 1;
    private float _minSpeed = 0.9f;
    private float _maxSpeed = 3.2f;


    void Awake()
    {
        CurrentAnimal.AnimalAgressiveStart += CreateButtonLines;

        MiniGamePoint.OnPointResult += GetPointResult;
        MiniGamePoint.PointFinished += RecyclePoint;
    }

    void Start()
    {
        _agressiveBar.fillAmount = _angryScore;
    }

    void Update()
    {
        // stupidTimer
        if (_gameIsStart)
        {
            _timer += _speedTimer * Time.deltaTime;
            if (_timer >= 1)
            {
                _speedTimer = Random.Range(_minSpeed, _maxSpeed);
                _timer = 0;
                CreatePoint();
            }
        }
        // stupidTimer

    }

    private void CreateButtonLines(bool isStart)
    {
        _gameIsStart = isStart;
        _barier.gameObject.SetActive(isStart);

        switch (isStart)
        {
            case true:
                if (_currentLine != null)
                    _currentLine.SetActive(true);
                else
                    _currentLine = Instantiate(_linePrefab, _minigameContainer.transform);

                _currentLine.transform.localScale = new Vector2(Camera.main.orthographicSize * 2 * Camera.main.aspect, _lineHight);
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

    private void CreatePoint()
    {
        Vector3 spawnPos = Camera.main.ViewportToWorldPoint(new Vector3(1.1f, 0.5f, 10f));

        if (_freeObjects.Count == 0)
        {
            MiniGamePoint point = Instantiate(_pointPrefab);
            point.transform.SetParent(_freeObjectsParent.transform);
            point.transform.position = spawnPos;
            point.Init(_barier.transform.position.x, _angryScore);

            _busyObjects.Add(point);
        }
        else
        {
            _freeObjects[0].gameObject.SetActive(true);
            _freeObjects[0].transform.position = spawnPos;
            _freeObjects[0].Init(_barier.transform.position.x, _angryScore);
            _busyObjects.Add(_freeObjects[0]);
            _freeObjects.RemoveAt(0);
        }
    }

    private void RecyclePoint(MiniGamePoint point)
    {
        _busyObjects.Remove(point);

        point.gameObject.SetActive(false);

        _freeObjects.Add(point);
    }

    private void GetPointResult(bool positive, float score)
    {
        _angryScore += positive ? -score : score;

        if (Mathf.Round(_angryScore * 100) / 100 <= 0)
        {
            _angryScore = 0;
            Debug.Log("Win minigame");
            CurrentAnimal.AnimalAgressiveStart.Invoke(false);
            CurrentAnimal.SwitchStatet(AnimalState.Default);
        }
        else if (_angryScore >= 1)
        {
            _angryScore = 1;
            Debug.Log("Loose");
            CurrentAnimal.AnimalAgressiveStart.Invoke(false);
            CurrentAnimal.SwitchStatet(AnimalState.Default);
        }
    }

    void OnDestroy()
    {
        CurrentAnimal.AnimalAgressiveStart -= CreateButtonLines;
        MiniGamePoint.OnPointResult -= GetPointResult;
    }
}

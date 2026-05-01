using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class AgressiveMiniGame : MonoBehaviour
{
    [Header("Links")]
    [SerializeField] private GameObject _minigameContainer;
    [SerializeField] private GameObject _linePrefab;
    [SerializeField] private GameObject _pointPrefab;
    [SerializeField] private float _lineHight;
    public Collider2D Barier;
    private GameObject _currentLine;

    [Header("Settings")]
    private List<GameObject> _points = new List<GameObject>(33);
    private bool _start;
    private float _timer;

    void Awake()
    {
        Animal.AnimalAgressiveStart += CreateButtonLines;
    }

    void Update()
    {
        if (_start)
        {
            _timer += 0.8f * Time.deltaTime;
            if (_timer >= 1)
            {
                _timer = 0;
                CreatePoint();
            }
        }
    }

    private void CreateButtonLines(bool isStart)
    {
        _start = isStart;

        switch (isStart)
        {
            case true:
                _currentLine = Instantiate(_linePrefab, _minigameContainer.transform);
                _linePrefab.transform.localScale = new Vector2(Camera.main.orthographicSize * 2 * Camera.main.aspect, _lineHight);
                break;

            case false:
                Destroy(_currentLine);
                _currentLine = null;

                foreach (GameObject point in _points)
                    Destroy(point);
                _points.Clear();
                break;
        }

    }

    private void CreatePoint()
    {
        Vector3 spawnPos = Camera.main.ViewportToWorldPoint(new Vector3(1.1f, 0.5f, 10f));
        GameObject point = Instantiate(_pointPrefab);
        point.transform.position = spawnPos;
        _points.Add(point);
    }


    void OnDestroy() => Animal.AnimalAgressiveStart -= CreateButtonLines;
}

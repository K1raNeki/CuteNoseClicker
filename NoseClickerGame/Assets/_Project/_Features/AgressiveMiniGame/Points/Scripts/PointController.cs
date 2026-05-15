using System.Collections.Generic;
using UnityEngine;

public class PointController
{
    [Header("Links")]
    private List<MiniGamePoint> _freeObjects = new();
    private List<MiniGamePoint> _busyObjects = new();
    private GameObject _freeObjectsParent;
    private MiniGamePoint _pointPrefab;
    private Collider2D _barier;

    private float _timer;
    

    public PointController(
        List<MiniGamePoint> freeObj,
        List<MiniGamePoint> bussyObj,
        MiniGamePoint prefab,
        Collider2D barier)
    {
        _freeObjects = freeObj;
        _busyObjects = bussyObj;
        _pointPrefab = prefab;
        _barier = barier;
    }

    public void CreatePoint(AnimalMiniGameFactor config, bool isInvert)
    {
        float xSpawnPos = isInvert ? -1.1f : 1.1f;
        Vector3 spawnPos = Camera.main.ViewportToWorldPoint(new Vector3(xSpawnPos, 0.5f, 10f));

        if (_freeObjects.Count == 0)
        {
            MiniGamePoint point = Object.Instantiate(_pointPrefab);
            point.transform.SetParent(_freeObjectsParent.transform);
            point.transform.localPosition = spawnPos;
            point.Init(_barier.transform.position.x, config);

            _busyObjects.Add(point);
        }
        else
        {
            _freeObjects[0].gameObject.SetActive(true);
            _freeObjects[0].transform.position = spawnPos;
            _freeObjects[0].Init(_barier.transform.position.x, config);
            _busyObjects.Add(_freeObjects[0]);
            _freeObjects.RemoveAt(0);
        }
    }

    public void RecyclePoint(MiniGamePoint point)
    {
        _busyObjects.Remove(point);

        point.gameObject.SetActive(false);

        _freeObjects.Add(point);
    }

    public void TimerForSpawn(bool gameStart, MiniGameDataMain data, AnimalMiniGameFactor config, bool invert)
    {
        if (gameStart)
        {
            _timer += data.TimerForSpawr * Time.deltaTime;
            if (_timer >= 1)
            {
                data.TimerForSpawr = UnityEngine.Random.Range(data.MinPossibleFactor, data.MaxPossibleFactor);
                _timer = 0;
                CreatePoint(config, invert);
            }
        }
    }
}

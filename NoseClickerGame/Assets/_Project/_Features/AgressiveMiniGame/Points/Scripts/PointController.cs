using System.Collections.Generic;
using UnityEngine;

public class PointController
{
    private List<MiniGamePoint> _freeObjects;
    private List<MiniGamePoint> _busyObjects;
    private GameObject _freeObjectsParent;
    private MiniGamePoint _pointPrefab;
    private Collider2D _barier;

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

    public void CreatePoint(float angryStrange)
    {
        Vector3 spawnPos = Camera.main.ViewportToWorldPoint(new Vector3(1.1f, 0.5f, 10f));

        if (_freeObjects.Count == 0)
        {
            MiniGamePoint point = Object.Instantiate(_pointPrefab);
            point.transform.SetParent(_freeObjectsParent.transform);
            point.transform.position = spawnPos;
            point.Init(_barier.transform.position.x, angryStrange);

            _busyObjects.Add(point);
        }
        else
        {
            _freeObjects[0].gameObject.SetActive(true);
            _freeObjects[0].transform.position = spawnPos;
            _freeObjects[0].Init(_barier.transform.position.x, angryStrange);
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
}

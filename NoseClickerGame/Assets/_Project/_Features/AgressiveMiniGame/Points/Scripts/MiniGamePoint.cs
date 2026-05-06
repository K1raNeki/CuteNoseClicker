using System;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class MiniGamePoint : MonoBehaviour, IClickable
{
    [Header("Links")]
    private float _failPositionX;
    private float _scoreCorrect;
    private bool isInit;

    SpriteRenderer _renderer;
    private bool _pointIsWin;
    private bool _pointIsLoose;


    void Awake()
    {
        if (_renderer == null) _renderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (!isInit) return;

        MovePoint();

        if ((_pointIsLoose || _pointIsWin) && !_renderer.isVisible)
            PointFinished();
    }

    public void Init(float failPos, float scoreCorrect)
    {
        isInit = true;
        _failPositionX = failPos;
        _scoreCorrect = scoreCorrect;
    }

    public void Interact() => Completed();

    public static event Action<bool, float> OnPointResult;
    public static event Action<MiniGamePoint> OnPointFinished;

    private void Completed()
    {
        _pointIsWin = true;
        OnPointResult?.Invoke(true, _scoreCorrect);
        _renderer.color = Color.green;
    }
    private void Fail()
    {
        _pointIsLoose = true;
        OnPointResult?.Invoke(false, _scoreCorrect);
        _renderer.color = Color.red;
    }

    private void PointFinished()
    {
        gameObject.SetActive(false);
        OnPointFinished?.Invoke(this);
    }

    private void MovePoint()
    {
        transform.localPosition = new Vector2(transform.localPosition.x - 6f * Time.deltaTime, transform.localPosition.y);

        if (transform.localPosition.x <= _failPositionX && !_pointIsLoose && !_pointIsWin)
            Fail();
    }
}
[System.Flags]
public enum PointTypes
{
    Default = 0,
    Double = 1 << 0,
    Hold = 1 << 1,
    Fake = 1 << 2,
    Wave = 1 << 3
}

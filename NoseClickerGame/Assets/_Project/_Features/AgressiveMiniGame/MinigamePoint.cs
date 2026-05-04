using System;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class MiniGamePoint : MonoBehaviour, IClickable
{
    [Header("Links")]
    public float FailPositionX;
    public float ScoreCorrect;
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
            OnPointFinished();
    }

    public void Init(float failPos, float scoreCorrect)
    {
        isInit = true;
        FailPositionX = failPos; ScoreCorrect = scoreCorrect;
    }

    public void Interact() => Completed();

    public static event Action<bool, float> OnPointResult;
    public static event Action<MiniGamePoint> PointFinished;

    private void Completed()
    {
        _pointIsWin = true;
        // Debug.Log($"{this} выебан");
        OnPointResult?.Invoke(true, ScoreCorrect);
        _renderer.color = Color.green;
    }
    private void Fail()
    {
        _pointIsLoose = true;
        // Debug.Log($"{this} проебан");
        OnPointResult?.Invoke(false, ScoreCorrect);
        _renderer.color = Color.red;
    }

    private void OnPointFinished()
    {
        gameObject.SetActive(false);
        PointFinished?.Invoke(this);
    }

    private void MovePoint()
    {
        transform.position = new Vector2(transform.position.x - 6f * Time.deltaTime, transform.position.y);

        if (transform.position.x <= FailPositionX && !_pointIsLoose && !_pointIsWin)
            Fail();
    }
}

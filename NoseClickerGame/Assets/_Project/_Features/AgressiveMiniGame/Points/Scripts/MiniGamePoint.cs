using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class MiniGamePoint : MonoBehaviour, IClickable
{
    [Header("Links")]
    private float _failPositionX;
    private bool isInit;
    private SpriteRenderer _renderer;

    [Header("PointData")]
    private PointTypes _pointType;
    private float _scoreCorrect;
    private float _pointSpeed;
    private bool _pointIsWin;
    private bool _pointIsLoose;

    [Header("PointFlags")]
    private float _lifeTime;
    private float _waveSyn = 8f;
    private bool _doublePointClick;
    private bool _isHolding;
    private float _holdPointTimer;
    private float _neededHoldTime = 0.3f;


    void Awake()
    {
        if (_renderer == null) _renderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (!isInit) return;

        if (_isHolding
        && !_pointIsLoose && !_pointIsWin)
        {
            if (PlayerClick.Instance.IsHoldClick())
            {
                _holdPointTimer += Time.deltaTime;
                if (_holdPointTimer >= _neededHoldTime)
                    Completed(!_pointType.HasFlag(PointTypes.Fake));
            }
            else
            {
                _holdPointTimer = 0;
                _isHolding = false;
            }
        }

        MovePoint();

        if ((_pointIsLoose || _pointIsWin) && !_renderer.isVisible)
        {
            gameObject.SetActive(false);
            OnPointFinished?.Invoke(this);
        }
    }

    public void Init(float failPos, AnimalMiniGameFactor config)
    {
        ResetPoint();
        _failPositionX = failPos;
        _scoreCorrect = config.ScoreTaked;
        _pointSpeed = config.MoveSpeedPoint;

        if (config.Types.Length > 0)
        {
            int index = UnityEngine.Random.Range(0, config.Types.Length);
            _pointType = config.Types[index];
        }
        else
            _pointType = PointTypes.Default;

    }

    public void Interact() => Completed(!_pointType.HasFlag(PointTypes.Fake));

    public static event Action<bool, float> OnPointResult;
    public static event Action<MiniGamePoint> OnPointFinished;

    private void Completed(bool isWin)
    {
        if (isWin && _pointType.HasFlag(PointTypes.Double) && !_doublePointClick)
        {
            _doublePointClick = true;
            return;
        }
        if (isWin && _pointType.HasFlag(PointTypes.Hold) && !_isHolding)
        {
            _isHolding = true;
            return;
        }

        _pointIsWin = isWin;
        _pointIsLoose = !isWin;
        OnPointResult?.Invoke(isWin, _scoreCorrect);
        _renderer.color = isWin ? Color.green : Color.red;

    }

    private void MovePoint()
    {
        _lifeTime += Time.deltaTime;
        float xMove = _pointSpeed * Time.deltaTime;
        float yMove = 0;

        if (_pointType.HasFlag(PointTypes.Wave))
            yMove = Mathf.Cos(_lifeTime * _pointSpeed) * _waveSyn;

        transform.Translate(-xMove, yMove * Time.deltaTime, 0);

        if (transform.localPosition.x < _failPositionX
            && !_pointType.HasFlag(PointTypes.Fake)
            && !_pointIsLoose && !_pointIsWin)
            Completed(false);
    }

    private void ResetPoint()
    {
        _isHolding = false;
        _holdPointTimer = 0;
        _doublePointClick = false;
        _lifeTime = 0;
        _pointIsLoose = false;
        _pointIsWin = false;
        isInit = true;
        _renderer.color = Color.white;
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

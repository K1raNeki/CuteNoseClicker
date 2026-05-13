using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class Animal : MonoBehaviour, IClickable
{
    [Header("Links")]
    public AnimalData Data;
    public AnimalExtensions Extensions { get; private set; }
    [HideInInspector] public GlobalRates Rates;
    private float _currentCare;

    [Header("Body")]
    public PolygonCollider2D[] BodyColliders;
    public PolygonCollider2D NoseCollider;
    private Animator _animator;
    private AnimalState _currentState;

    [Header("Current Indexes")]
    private float _noseMultycare = 1.5f;
    [SerializeField] private float _multyClickDuration = 0.2f;
    private int _currentMiniGame = -1;
    public bool[] IsCompletedMiniGame { get; private set; }
    public AnimalMiniGameFactor[] SortedMiniGames { get; private set; }


    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _animator.runtimeAnimatorController = Data.OverrideController;

        IsCompletedMiniGame = new bool[Data.MiniGames.Length];
        SortedMiniGames = (AnimalMiniGameFactor[])Data.MiniGames.Clone();
        Array.Sort(SortedMiniGames, (x, y) => x.AngryBarPositionX.CompareTo(y.AngryBarPositionX));

        Extensions = new AnimalExtensions(this);

        SwitchStatet(AnimalState.Default);
    }

    private void Update()
    {
        if (Extensions.AutoClickSpawnFlag())
        {
            Debug.Log("AutoTake Care");
            TakeCare(Rates.TakedCare);
        }
    }

    public event Action<float> AnimalTakeCare;
    public event Action<bool, AnimalMiniGameFactor> AnimalAgressiveStart;

    public void Interact()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        float multipierValue = _noseMultycare * Rates.TakedCare;
        float takedCare = NoseCollider.OverlapPoint(mousePos) ? multipierValue : Rates.TakedCare;

        // TakeCare(takedCare);
        StartCoroutine(MultyCare(takedCare, Rates.RollMultyClick()));
    }

    private void TakeCare(float takedCare)
    {
        switch (_currentState)
        {
            case AnimalState.Default:
                break;

            case AnimalState.Angry:
                return;

            case AnimalState.Win:
                return;
        }

        _currentCare = Math.Clamp(_currentCare + takedCare, 0, Data.NeedCare);
        _currentCare += Rates.TakedCare * Extensions.GetCriticalCare();

        CheckedCare();

        AnimalTakeCare?.Invoke(_currentCare);
        // Debug.Log($"У {Data.Name} набито {_currentCare}/{Data.NeedCare}");
    }
    private void CheckedCare()
    {
        if (_currentCare >= Data.NeedCare)
        {
            _currentCare = Data.NeedCare;
            SwitchStatet(AnimalState.Win);
            return;
        }

        for (int i = 0; i < SortedMiniGames.Length; i++)
        {
            if ((_currentCare / Data.NeedCare) >= SortedMiniGames[i].AngryBarPositionX
            && !IsCompletedMiniGame[i]
            && _currentState != AnimalState.Angry)
            {
                _currentMiniGame = i;
                AgressionStart(true, Data.MiniGames[i]);
                break;
            }
        }
    }

    public void CompleteActiveMiniGame(bool success)
    {
        if (success && _currentMiniGame != -1)
            IsCompletedMiniGame[_currentMiniGame] = true;
        else if (!success)
        {
            _currentCare = Data.NeedCare * SortedMiniGames[_currentMiniGame].AngryBarPositionX * (1f - Data.Penalty);
            AnimalTakeCare?.Invoke(_currentCare);
        }

        _currentMiniGame = -1;
        SwitchStatet(AnimalState.Default);
    }

    public void AgressionStart(bool isAngry, AnimalMiniGameFactor config) => Extensions.AgressionStart(isAngry, config, AnimalAgressiveStart);

    public void SwitchStatet(AnimalState state)
    {
        _currentState = state;

        foreach (AnimalState value in Enum.GetValues(typeof(AnimalState)))
            _animator.ResetTrigger(value.ToString());

        int index = (int)state;
        for (int i = 0; i < BodyColliders.Length; i++)
        {
            BodyColliders[i].gameObject.SetActive(i == index && i != (int)AnimalState.Angry);
        }
        _animator.SetTrigger(state.ToString());
    }

    private IEnumerator MultyCare(float takedCare, float count)
    {
        for (int i = 0; i < count; i++)
        {
            TakeCare(takedCare);
            yield return new WaitForSeconds(_multyClickDuration);
        }
    }
}

public enum AnimalState { Default, Angry, Win };
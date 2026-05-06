using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class Animal : MonoBehaviour, IClickable
{
    [Header("Links")]
    public AnimalData Data;
    public PolygonCollider2D[] BodyColliders;
    public PolygonCollider2D NoseCollider;
    private Animator _animator;
    private AnimalState _currentState;

    [Header("Current Indexes")]
    private float _currentCare;
    private bool[] _isCompletedMiniGame;
    private AnimalMiniGameFactor[] _sortedMiniGames;


    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _animator.runtimeAnimatorController = Data.OverrideController;

        _isCompletedMiniGame = new bool[Data.MiniGames.Length];
        _sortedMiniGames = (AnimalMiniGameFactor[])Data.MiniGames.Clone();
        Array.Sort(_sortedMiniGames, (x, y) => x.AngryBarPositionX.CompareTo(y.AngryBarPositionX));

        SwitchStatet(AnimalState.Default);
    }

    public event Action<float> AnimalTakeCare;
    public event Action<bool, AnimalMiniGameFactor> AnimalAgressiveStart;

    public void Interact()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        float multiplier = NoseCollider.OverlapPoint(mousePos) ? 2f : 1f;

        TakeCare(multiplier);
    }

    public void TakeCare(float takedCare = 1)
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

        CheckedCare();

        AnimalTakeCare?.Invoke(_currentCare);
        Debug.Log($"У {Data.Name} набито {_currentCare}/{Data.NeedCare}");
    }
    private void CheckedCare()
    {
        if (_currentCare >= Data.NeedCare)
        {
            _currentCare = Data.NeedCare;
            SwitchStatet(AnimalState.Win);
            return;
        }

        for (int i = 0; i < _sortedMiniGames.Length; i++)
        {
            if ((_currentCare / Data.NeedCare) >= _sortedMiniGames[i].AngryBarPositionX
            && !_isCompletedMiniGame[i]
            && _currentState != AnimalState.Angry)
            {
                _isCompletedMiniGame[i] = true;
                AgressionStart(true, Data.MiniGames[i]);
            }
        }
    }

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

    public void AgressionStart(bool isAngry, AnimalMiniGameFactor config)
    {
        if (isAngry)
            SwitchStatet(AnimalState.Angry);
        else
            SwitchStatet(AnimalState.Default);

        AnimalAgressiveStart?.Invoke(isAngry, config);
    }

}

public enum AnimalState { Default, Angry, Win };
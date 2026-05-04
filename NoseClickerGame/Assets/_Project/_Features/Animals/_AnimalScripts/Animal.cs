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

    [Header("Current Indexes")] // in a config
    public float AngryScoreAwake = 0.4f;
    public float MiniGameScore;
    private AnimalState _currentState;
    private bool[] _trigerredPoints;
    private float[] _sortedAngryPoints;
    private float _currentCare;


    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _animator.runtimeAnimatorController = Data.OverrideController;

        _trigerredPoints = new bool[Data.AngryPoints.Length];
        _sortedAngryPoints = (float[])Data.AngryPoints.Clone();
        Array.Sort(_sortedAngryPoints);

        SwitchStatet(AnimalState.Default);
    }

    private void Start()
    {
        // MinigameBar.MiniaGameEnd += ResultMiniGame;
    }

    public event Action<float> AnimalTakeCare;
    public Action<bool> AnimalAgressiveStart;

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
                takedCare *= -Data.AngryMultiplier;
                break;

            case AnimalState.Win:
                return;
        }

        _currentCare = Math.Clamp(_currentCare + takedCare, 0, Data.NeedCare);

        CheckedCare(_currentCare, Data.NeedCare, Data.AngryPoints);


        AnimalTakeCare?.Invoke(_currentCare);
        Debug.Log($"У {Data.Name} набито {_currentCare}/{Data.NeedCare}");
    }
    private void CheckedCare(float currentCare, float neededCare, float[] angryPoints)
    {
        if (_currentCare >= Data.NeedCare)
        {
            _currentCare = Data.NeedCare;
            SwitchStatet(AnimalState.Win);
            return;
        }

        for (int i = 0; i < _sortedAngryPoints.Length; i++)
        {
            if (!_trigerredPoints[i]
                && _currentCare / Data.NeedCare >= _sortedAngryPoints[i]
                && _currentState != AnimalState.Angry)
            {
                _trigerredPoints[i] = true;
                AgressionStart(true);
            }
        }
    }

    private void AgressionStart(bool isAngry)
    {
        if (isAngry)
            SwitchStatet(AnimalState.Angry);
        else
            SwitchStatet(AnimalState.Default);

        AnimalAgressiveStart?.Invoke(isAngry);
    }

    public void SwitchStatet(AnimalState state)
    {
        _currentState = state;

        foreach (AnimalState value in Enum.GetValues(typeof(AnimalState)))
            _animator.ResetTrigger(value.ToString());

        int index = (int)state;
        for (int i = 0; i < BodyColliders.Length; i++)
        {
            BodyColliders[i].gameObject.SetActive(i == index);
        }
        _animator.SetTrigger(state.ToString());
    }

}

public enum AnimalState { Default, Angry, Win };
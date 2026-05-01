using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Animal : MonoBehaviour
{
    [Header("Links")]
    public AnimalData Data;
    public PolygonCollider2D[] BodyColliders;
    public PolygonCollider2D NoseCollider;
    private Animator _animator;

    [Header("Current Indexes")]
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

    public static event Action<float> AnimalTakeCare;
    public static event Action<bool> AnimalAgressiveStart;

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
                StartCoroutine(AgressionStart(12f));
            }
        }
    }

    private IEnumerator AgressionStart(float dur)
    {
        AnimalAgressiveStart?.Invoke(true);
        SwitchStatet(AnimalState.Angry);
        yield return new WaitForSeconds(dur);
        AnimalAgressiveStart?.Invoke(false);
        SwitchStatet(AnimalState.Default);
    }

    private void SwitchStatet(AnimalState state)
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
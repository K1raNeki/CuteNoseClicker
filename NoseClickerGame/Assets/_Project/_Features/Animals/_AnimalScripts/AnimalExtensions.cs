using System;
using System.Collections;
using UnityEngine;

public class AnimalExtensions
{
    private Animal _animal;
    private float _timerForCare;

    public AnimalExtensions(Animal animal) => _animal = animal;

    public bool IsGameCompleted(AnimalMiniGameFactor config)
    {
        int index = Array.IndexOf(_animal.SortedMiniGames, config);
        if (index != -1)
            return _animal.IsCompletedMiniGame[index];

        return false;
    }

    public void AgressionStart(bool isAngry, AnimalMiniGameFactor config, Action<bool, AnimalMiniGameFactor> action)
    {
        _animal.SwitchStatet(isAngry ? AnimalState.Angry : AnimalState.Default);

        action?.Invoke(isAngry, config);
    }

    public bool AutoClickSpawnFlag()
    {
        if (_animal.Rates.AutoClickDelta > 0)
        {
            _timerForCare += Time.deltaTime;
            if (_timerForCare >= _animal.Rates.AutoClickDelta)
            {
                _timerForCare = 0;
                return true;
            }
        }
        return false;
    }

    public float GetCriticalCare()
    {
        if (_animal.Rates.WillBeCrit())
        {
            return _animal.Rates.CriticalCare;
        }
        else
            return 0;
    }

}

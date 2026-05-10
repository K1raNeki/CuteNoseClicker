using System;

public class AnimalExtensions
{
    private Animal _animal;

    public AnimalExtensions(Animal animal) => _animal = animal;

    public bool IsGameCompleted(AnimalMiniGameFactor config)
    {
        int index = Array.IndexOf(_animal.SortedMiniGames, config);
        if (index != -1)
            return _animal.IsCompletedMiniGame[index];

        return false;
    }

}

using System;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [Header("Links")]
    [SerializeField] private Image _progressBar;
    [SerializeField] private Animal _currentAnimal;


    void Start()
    {
        _progressBar.fillAmount = 0;

        _currentAnimal.AnimalTakeCare += UpdateUIBar;
    }

    private void UpdateUIBar(float impact)
    {
        _progressBar.fillAmount = impact / _currentAnimal.NeedCare;
    }

}

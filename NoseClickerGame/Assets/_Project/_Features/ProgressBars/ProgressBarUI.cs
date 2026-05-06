using System;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [Header("LinksMain")]
    [SerializeField] private Image _progressBar;

    [Header("LinksAngryPoints")]
    [SerializeField] private Image _progressBarContainer;
    [SerializeField] private Image _angryPointPrefab;
    [SerializeField] private Animal _currentAnimal;
    [SerializeField] Vector2 _anfryPointSize;


    void Start()
    {
        _progressBar.fillAmount = 0;
        SpawnAngryPoints();
        _currentAnimal.AnimalTakeCare += UpdateUIBarProgress;

    }

    private void UpdateUIBarProgress(float impact)
    {
        _progressBar.fillAmount = impact / _currentAnimal.Data.NeedCare;
    }

    private void SpawnAngryPoints()
    {
        float[] points = new float[_currentAnimal.Data.MiniGames.Length];
        for(int i = 0; i < _currentAnimal.Data.MiniGames.Length; i++)
        {
            points[i] = _currentAnimal.Data.MiniGames[i].AngryBarPositionX;
        }

        foreach (float point in points)
        {
            Image angryPoint = Instantiate(_angryPointPrefab, _progressBarContainer.transform);
            RectTransform rect = angryPoint.rectTransform;

            rect.pivot = new Vector2(0, 0.5f);

            rect.anchorMax = new Vector2(point, 0.5f);
            rect.anchorMin = new Vector2(point, 0.5f);
            
            rect.anchoredPosition = Vector2.zero;
            rect.localScale = Vector3.one;

            rect.sizeDelta = _anfryPointSize;
        }
    }


    void OnDestroy() => _currentAnimal.AnimalTakeCare -= UpdateUIBarProgress;
}

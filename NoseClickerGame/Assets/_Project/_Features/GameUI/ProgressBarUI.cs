using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [Header("LinksMain")]
    [SerializeField] private Image _progressBar;
    [HideInInspector] public Animal CurrentAnimal;

    [Header("LinksAngryPoints")]
    [SerializeField] private Image _progressBarContainer;
    [SerializeField] private Image _angryPointPrefab;
    [SerializeField] Vector2 _anfryPointSize;
    private List<AngryPointContainerUI> _pointsList = new();


    private void Start()
    {
        CurrentAnimal.AnimalTakeCare += UpdateUIBarProgress;
        CurrentAnimal.AnimalAgressiveStart += UpdateAngryPointCompleted;
        UpdateUIBarProgress(0);
        SpawnAngryPoints();
    }

    public void HideBaseProgress(bool show) => _progressBarContainer.gameObject.SetActive(!show);

    public void UpdateUIBarProgress(float impact) => _progressBar.fillAmount = impact / CurrentAnimal.Data.NeedCare;

    private void UpdateAngryPointCompleted(bool isAngry, AnimalMiniGameFactor config)
    {
        if (config == null) return;

        var point = _pointsList.Find(p => p.XPos == config.AngryBarPositionX);

        if (point != null)
        {
            if (isAngry)
                point.PointImage.color = Color.red;

            else
                point.PointImage.color = CurrentAnimal.Extensions.IsGameCompleted(config) ? Color.green : Color.grey;
        }
    }

    private void SpawnAngryPoints()
    {
        foreach (var p in _pointsList) if (p.PointImage != null) Destroy(p.PointImage.gameObject);
        _pointsList.Clear();

        for (int i = 0; i < CurrentAnimal.Data.MiniGames.Length; i++)
        {
            _pointsList.Add(
                new AngryPointContainerUI(
                    Instantiate(_angryPointPrefab, _progressBarContainer.transform),
                    CurrentAnimal.Data.MiniGames[i].AngryBarPositionX));
        }

        foreach (AngryPointContainerUI point in _pointsList)
        {
            RectTransform rect = point.PointImage.rectTransform;

            rect.pivot = new Vector2(0, 0.5f);

            rect.anchorMax = new Vector2(point.XPos, 0.5f);
            rect.anchorMin = new Vector2(point.XPos, 0.5f);

            rect.anchoredPosition = Vector2.zero;
            rect.localScale = Vector3.one;

            rect.sizeDelta = _anfryPointSize;
        }
    }

    private void OnDestroy()
    {
        CurrentAnimal.AnimalTakeCare -= UpdateUIBarProgress;
        CurrentAnimal.AnimalAgressiveStart -= UpdateAngryPointCompleted;
    }

    private class AngryPointContainerUI
    {
        public Image PointImage;
        public float XPos;

        public AngryPointContainerUI(Image img, float xPos)
        {
            PointImage = img; XPos = xPos;
        }
    }
}

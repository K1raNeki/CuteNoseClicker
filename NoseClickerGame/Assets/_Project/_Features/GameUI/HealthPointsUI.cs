using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthPointsUI : MonoBehaviour
{
    [Header("Links")]
    [HideInInspector] public Animal CurrentAnimal;
    [SerializeField] private Image _heartPrefab;
    [SerializeField] private Transform _heartContainer;
    private List<Heart> _herts = new();

    private void Start()
    {
        ArrangeHearts();
    }

    public event Action HeelthEnded;

    public void TakeLoose()
    {
        foreach (Heart heart in _herts)
        {
            if (heart.HeartValue)
            {
                heart.HeartPrefab.color = Color.grey;
                heart.HeartValue = false;
                {
                    if (_herts.IndexOf(heart) == _herts.Count - 1)
                        HeelthEnded?.Invoke();
                }
                break;
            }
        }
    }

    private void ArrangeHearts()
    {
        for (int i = 0; i < CurrentAnimal.Data.Attemps; i++)
        {
            Heart heart = new Heart(Instantiate(_heartPrefab, _heartContainer));
            _herts.Add(heart);
        }
    }

    private class Heart
    {
        public Image HeartPrefab;
        public bool HeartValue = true;

        public Heart(Image img)
        {
            HeartPrefab = img;
        }
    }
}

using System;
using UnityEngine;
using UnityEngine.UI;

public class Animal : MonoBehaviour, IAnimal
{
    [Header("Links")]
    [SerializeField] private AnimalData _data;

    private float _currentCare;
    public float NeedCare { get; private set; }


    private void Awake()
    {
        NeedCare = _data.NeedCare;
    }

    public void Interact()
    {
        TakeCare();
    }

    public event Action<float> AnimalTakeCare;

    public void TakeCare()
    {
        _currentCare ++;
        
        if(_currentCare >= NeedCare) 
            _currentCare = NeedCare;

        AnimalTakeCare?.Invoke(_currentCare);

        Debug.Log($"У {_data.Name} набито {_currentCare}/{NeedCare}");
        
    }

}

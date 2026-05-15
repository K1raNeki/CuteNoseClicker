using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UI;

public class ChoiseAnimalMainButton : MonoBehaviour
{
    [Header("Links")]
    public Button ThisButton
    {
        get
        {
            if (_tempButton == null) _tempButton = GetComponent<Button>();
            return _tempButton;
        }
    }
    private Button _tempButton;

    [Header("Container")]
    [SerializeField] private GameObject _choiseContainer;
    [SerializeField] private Transform _animalsIconsContainer;
    [SerializeField] private GameObject _animalIconPrefab;
    [SerializeField] private Button _closeContainer;
    public AnimalData[] AllAnimals = new AnimalData[1];
    private List<GameObject> _createsAnimal = new();
    private bool _isOpenContainer;


    public static event Action<Animal> OnSetapCurrentAnimal;

    private void Awake()
    {
        EventManager.OnOpenMainsContainer += CloseThisButton;
        PlayMainButton.OnNullAnimal += OpenChoiseContainer;

        _choiseContainer.gameObject.SetActive(false);

        ThisButton.onClick.AddListener(() => OpenChoiseContainer(_isOpenContainer));
        _closeContainer.onClick.AddListener(() => OpenChoiseContainer(_isOpenContainer));
    }

    private void OpenChoiseContainer(bool isOpen)
    {
        _choiseContainer.SetActive(!isOpen);
        _isOpenContainer = !isOpen;

        CreateAnimalIcons();

        EventManager.OnOpenMainsContainer?.Invoke(isOpen);
    }

    private void CreateAnimalIcons()
    {
        if (_createsAnimal.Count != 0) return;

        for (int i = 0; i < AllAnimals.Length; i++)
        {
            int index = i;

            GameObject newAnimalIcon = Instantiate(_animalIconPrefab, _animalsIconsContainer);
            if (newAnimalIcon.TryGetComponent<Image>(out Image img))
            {
                img.sprite = AllAnimals[i].Icon;
                img.color = Color.white;
            }

            if (newAnimalIcon.TryGetComponent<Button>(out Button butt))
            {
                butt.onClick.AddListener(() => ThisAnimalCurrent(index));
            }

            _createsAnimal.Add(newAnimalIcon);
        }
    }

    private void ThisAnimalCurrent(int count)
    {
        if (AllAnimals[count].Prefab.gameObject.TryGetComponent<Animal>(out Animal animal))
        {
            OpenChoiseContainer(_isOpenContainer);
            OnSetapCurrentAnimal?.Invoke(animal);
        }
    }

    private void CloseThisButton(bool open) =>
        gameObject.SetActive(open);
}

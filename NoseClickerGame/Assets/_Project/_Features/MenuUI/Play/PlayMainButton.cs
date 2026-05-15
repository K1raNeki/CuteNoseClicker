using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using Unity.VisualScripting;

[RequireComponent(typeof(Button))]
public class PlayMainButton : MonoBehaviour
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
    [HideInInspector] public Animal CurrentAnimal;
    [SerializeField] private Image IconAnimal;


    private void Awake()
    {
        ChoiseAnimalMainButton.OnSetapCurrentAnimal += SetapAnimal;
        EventManager.OnOpenMainsContainer += CloseThisButton;
        ThisButton.onClick.AddListener(PlayGame);
    }

    public static event Action<bool> OnNullAnimal;
    public static event Action<Animal> OnStartGameWithCurrentAnimal;

    public void PlayGame()
    {
        if (CurrentAnimal == null)
        {
            OnNullAnimal?.Invoke(false);
            return;
        }

        OnStartGameWithCurrentAnimal?.Invoke(CurrentAnimal);
        SceneManager.LoadScene(1);
    }

    private void SetapAnimal(Animal animal)
    {
        CurrentAnimal = animal;
        IconAnimal.sprite = CurrentAnimal.Data.Icon;
    }

    private void CloseThisButton(bool open) =>
        gameObject.SetActive(open);
}

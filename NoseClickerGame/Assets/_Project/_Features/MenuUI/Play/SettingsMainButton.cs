using UnityEngine;
using UnityEngine.UI;

public class SettingsMainButton : MonoBehaviour
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
    [SerializeField] private GameObject SettingsContainer;
    [SerializeField] private Button CloseContainer;
    private bool _isOpenContainer;



    private void Awake()
    {
        EventManager.OnOpenMainsContainer += CloseThisButton;

        SettingsContainer.gameObject.SetActive(false);

        ThisButton.onClick.AddListener(() => OpenSettingsContainer(_isOpenContainer));
        CloseContainer.onClick.AddListener(() => OpenSettingsContainer(_isOpenContainer));
    }

    private void OpenSettingsContainer(bool isOpen)
    {
        SettingsContainer.SetActive(!isOpen);
        _isOpenContainer = !isOpen;

        EventManager.OnOpenMainsContainer?.Invoke(isOpen);
    }

    private void CloseThisButton(bool open) =>
        gameObject.SetActive(open);
}

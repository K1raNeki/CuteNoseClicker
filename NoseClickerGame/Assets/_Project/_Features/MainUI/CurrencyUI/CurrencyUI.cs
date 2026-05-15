using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyUI : MonoBehaviour
{
    [Header("UIElements")]
    [SerializeField] private TextMeshProUGUI _loveCurrency;
    [SerializeField] private TextMeshProUGUI _keys;


    public void UpdateLoveCurrencyValueUI(float value) => _loveCurrency.text = value.ToString() + " money";
    public void UpdateKeysValueUI(float value) => _keys.text = value.ToString() + " keys";

}

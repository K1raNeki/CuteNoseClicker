using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButtons : MonoBehaviour
{
    [HideInInspector] public PaymentsManager PaymentsManager;

    [Header("LinksButtons")]
    [SerializeField] private UpgradeButtonUI MultyClickB;
    [SerializeField] private UpgradeButtonUI CritClickB;
    [SerializeField] private UpgradeButtonUI AutoClickB;
    [SerializeField] private UpgradeButtonUI CoockieB;

    [Header("Counters")]
    private Dictionary<UpgrageData, int> _upgradesMap = new();
    [SerializeField] private UpgrageData[] _upgradesData;


    public event Action<UpgradeType, float> OnUpgrade;

    private void Awake() => Init();

    public void UpdateState(UpgradeButtonUI button)
    {
        foreach (var entry in _upgradesMap)
        {
            if (entry.Key.Type == UpgradeType.Coockie)
            {
                float price = entry.Key.Prices[1];
                if (PaymentsManager.CanPay(price, PaymentsManager.LoveCurrency))
                {
                    PaymentsManager.SpendLoveCurrency(price);
                }
                else
                {
                    Debug.Log("мало деняк");
                    return;
                }
                _upgradesMap[entry.Key]++;

                button.UpdateButton(price, _upgradesMap[entry.Key], 1);
                return;
            }

            if (entry.Key.Type == button.Type)
            {
                if (entry.Value >= entry.Key.Values.Length - 1)
                {
                    Debug.Log($"Max level for {button.Type}");
                    return;
                }

                float price = entry.Key.Prices[_upgradesMap[entry.Key]];
                if (PaymentsManager.CanPay(price, PaymentsManager.LoveCurrency))
                {
                    PaymentsManager.SpendLoveCurrency(price);
                }
                else
                {
                    Debug.Log("мало деняк");
                    return;
                }
                _upgradesMap[entry.Key]++;

                float newPrice = entry.Key.Prices[_upgradesMap[entry.Key]];
                float newValue = entry.Key.Values[_upgradesMap[entry.Key]];
                button.UpdateButton(newPrice, newValue, entry.Value);
                OnUpgrade?.Invoke(button.Type, newValue);
                return;
            }
        }
    }

    public void HideButtonContainer(bool hide) => gameObject.SetActive(hide);

    public void ResetEvent() => OnUpgrade = null;

    private void Init()
    {
        _upgradesMap.Clear(); ;
        foreach (UpgrageData data in _upgradesData)
            _upgradesMap.Add(data, 0);

        SetupButton(MultyClickB, UpgradeType.MultyClick);
        SetupButton(CritClickB, UpgradeType.CritClick);
        SetupButton(AutoClickB, UpgradeType.AutoClick);
        SetupButton(CoockieB, UpgradeType.Coockie);
    }
    private void SetupButton(UpgradeButtonUI ui, UpgradeType type)
    {
        ui.Type = type;
        ui.ThisButton.onClick.AddListener(() => UpdateState(ui));

        foreach (var entry in _upgradesMap)
        {
            if (entry.Key.Type == type)
            {
                ui.UpdateButton(entry.Key.Prices[0], entry.Key.Values[0], 0, entry.Key.Name);
                OnUpgrade?.Invoke(ui.Type, entry.Key.Values[0]);
                break;
            }
        }
    }
}
public enum UpgradeType
{
    MultyClick, CritClick, AutoClick, Coockie
}
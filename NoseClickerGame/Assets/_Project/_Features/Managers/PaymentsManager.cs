using System;
using UnityEngine;

[System.Serializable]
public class PaymentsManager
{
    [Header("Currency")]
    public float LoveCurrency { get; private set; }
    public float Keys { get; private set; }

    public event Action<float> OnLoveCurrencyTransaction;
    public event Action<float> OnKeysTransaction;

    public void AddLoveCurrency(float rise)
    {
        LoveCurrency += rise;
        OnLoveCurrencyTransaction?.Invoke(LoveCurrency);
    }
    public void SpendLoveCurrency(float price)
    {
        if (CanPay(price, LoveCurrency))
            LoveCurrency -= price;
        else
            Debug.Log($"Нехватает {price - LoveCurrency} сердечек");

        OnLoveCurrencyTransaction?.Invoke(LoveCurrency);
    }

    public void AddKeys(int rise)
    {
        Keys += rise;
        OnKeysTransaction?.Invoke(Keys);
    }
    public void SpendKeys(int price)
    {
        if (CanPay(price, Keys))
            Keys -= price;
        else
            Debug.Log($"Нехватает {price - Keys} ключей");

        OnKeysTransaction?.Invoke(Keys);
    }

    public bool CanPay(float needfullCurrensy, float realCurrensy) => needfullCurrensy <= realCurrensy;

    public void ResetEvent()
    {
        OnLoveCurrencyTransaction = null;
        OnKeysTransaction = null;
    }
}

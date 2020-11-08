using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallet : MonoBehaviour
{
    public static PlayerWallet instance;
    public Action<CurrencyType, uint> CurrencyAmountChange;

    private Dictionary<CurrencyType, uint> wallet;


    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            wallet = new Dictionary<CurrencyType, uint>(Enum.GetNames(typeof(CurrencyType)).Length);
            CurrencyAmountChange += ChangingCurrencyAmount;
        }
    }
    private void Start()
    {
        LoadCurrencyAmount();
    }

    #region WalletOperations ------------------------------------------------------------------------------------------

    public void AddCurrencyAmount(CurrencyType type, uint cash)
    {
        wallet[type] = cash;
    }

    public void Buy(CurrencyType type, uint price, Action OnSuccess, Action onDecline)
    {
        if (wallet[type] >= price)
        {
            wallet[type] -= price;
            OnSuccess.Invoke();
        }
        else
        {
            onDecline.Invoke();
        }
    }

    #endregion

    #region Saving ----------------------------------------------------------------------------------------------------

    /// <summary>
    /// Get local saved amount currency value
    /// </summary>
    public void LoadCurrencyAmount()
    {

    }

    /// <summary>
    /// Save amount value on local device
    /// </summary>
    public void SaveCurrencyAmount()
    {

    }

    /// <summary>
    /// Сохранение изменений
    /// </summary>
    /// <param name="type"></param>
    /// <param name="amount"></param>
    public void ChangingCurrencyAmount(CurrencyType type, uint amount)=>SaveCurrencyAmount();

    #endregion
}
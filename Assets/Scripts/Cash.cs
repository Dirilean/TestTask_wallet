using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WalletModule;

public class Cash : MonoBehaviour
{
    public Text[] textCurrency;

    void Start()
    {
        PlayerWallet.instance.savingModule.needSaving += ChangeUI;
    }

    void ChangeUI(Dictionary<CurrencyType, uint> dictionary)
    {
        for (int i = 0; i < textCurrency.Length; i++)
        {
            textCurrency[i].text = dictionary[(CurrencyType)i].ToString();
        }
    }
}

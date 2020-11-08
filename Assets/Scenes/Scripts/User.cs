using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User : MonoBehaviour
{
    private void Start()
    {
        PlayerWallet.instance.AddCurrencyAmount(CurrencyType.gold, 10);
        PlayerWallet.instance.Buy(CurrencyType.gold, 5, delegate { print("Покупка проведена успешно"); }, delegate { print("Недостаточно средств"); });
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WalletModule;

public class User : MonoBehaviour
{
    WaitForSeconds waitingTime = new WaitForSeconds(3f);
    private IEnumerator Start()
    {
        yield return waitingTime;
        PlayerWallet.instance.AddCurrencyAmount(CurrencyType.gold, 17);
        PlayerWallet.instance.AddCurrencyAmount(CurrencyType.diamond, 52);
        yield return waitingTime;
        PlayerWallet.instance.Buy(CurrencyType.gold, 10, delegate { print("Покупка проведена успешно"); }, delegate { print("Недостаточно средств"); });
        yield return waitingTime;
        PlayerWallet.instance.Buy(CurrencyType.diamond, 10, delegate { print("Покупка проведена успешно"); }, delegate { print("Недостаточно средств"); });
        yield return waitingTime;
        PlayerWallet.instance.AddCurrencyAmount(CurrencyType.silver, 36);
    }
}


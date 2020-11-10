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
        //yield return waitingTime;
        //PlayerWallet.instance.Buy(CurrencyType.gold, 10, delegate { print("Покупка проведена успешно"); }, delegate { print("Недостаточно средств"); });
        //yield return waitingTime;
        //PlayerWallet.instance.Buy(CurrencyType.diamond, 10, delegate { print("Покупка проведена успешно"); }, delegate { print("Недостаточно средств"); });
        //yield return waitingTime;
        //PlayerWallet.instance.AddCurrencyAmount(CurrencyType.silver, 36);

        //var str=( WalletSerialization.DictionaryToString());
        // SoapFormatter sf = new SoapFormatter();
        //using (Stream stream = File.Create("test.txt"))
        //{
        //    sf.Serialize(stream, PlayerWallet.instance);
        //}
        //Dictionary<CurrencyType, uint> pairs1;
        //using (Stream stream = File.OpenRead("test.txt"))
        //{
        //    pairs1 = (Dictionary<CurrencyType, uint>)sf.Deserialize(stream);
        //}    

        //string str = WalletSerialization.DictionaryToString(PlayerWallet.instance.Wallet);
        //Dictionary<CurrencyType, uint> w = new Dictionary<CurrencyType, uint>();
        //w = WalletSerialization.StringToDictionary(str);
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WalletModule;

public class User : MonoBehaviour
{
    PlayerWallet wallet;
    public void Start()
    {
        wallet = PlayerWallet.instance;
    }
    public void AddGold(int amount)
    {
        wallet.AddCurrencyAmount(CurrencyType.gold, (uint)amount);
    }
    public void AddDiamond(int amount)
    {
        wallet.AddCurrencyAmount(CurrencyType.diamond, (uint)amount);
    }
    public void AddSilver(int amount)
    {
        wallet.AddCurrencyAmount(CurrencyType.silver, (uint)amount);
    }

    public void BuyByGold(int amount)
    {
        wallet.Buy(CurrencyType.gold, (uint)amount, () => Debug.Log("you buy item by " + amount), () => Debug.LogError("You cant buy it. It too expensive"));
    }
    public void BuyByDiamond(int amount)
    {
        wallet.Buy(CurrencyType.diamond, (uint)amount, () => Debug.Log("you buy item by " + amount), () => Debug.LogError("You cant buy it. It too expensive"));
    }
    public void BuyBySilver(int amount)
    {
        wallet.Buy(CurrencyType.silver, (uint)amount, () => Debug.Log("you buy item by " + amount), () => Debug.LogError("You cant buy it. It too expensive"));
    }

    public bool SaveInFile
    {
        get{ return wallet.savingModule.SavingInFile; }
        set{ wallet.savingModule.SavingInFile = value; }
    }
    public bool SaveInBinFile
    {
        get { return wallet.savingModule.SavingInBinFile; }
        set { wallet.savingModule.SavingInBinFile = value; }
    }
    public bool SaveInPlayerPrefs
    {
        get { return wallet.savingModule.SavingInPlayerPrefs; }
        set { wallet.savingModule.SavingInPlayerPrefs = value; }
    }

    public void ClearSavedData()
    {
        SavingWalletData.ClearAllSavedData();
    }


    public void CreateAndLoadNewWallet(int loadWay)
    {
        wallet.CreateWallet((SavingWalletData.LoadingPreset)loadWay);
    }
}


﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WalletModule;
using System.IO;

public class User : MonoBehaviour
{
    PlayerWallet wallet;
    public Text[] textCurrency;
    public Dropdown loadingWay;
    public Toggle toggle_playerPrefs;
    public Toggle toggle_file;
    public Toggle toggle_binFile;
    public Toggle toggle_server;


    public IEnumerator Start()
    {
        wallet = PlayerWallet.instance;
        wallet.savingModule.changeWalletValue += ChangeWalletValue;
        ChangeWalletValue(wallet.Wallet);
        yield return null;
        toggle_playerPrefs.isOn = wallet.savingModule.SavingInPlayerPrefs;
        toggle_file.isOn = wallet.savingModule.SavingInFile;
        toggle_binFile.isOn = wallet.savingModule.SavingInBinFile;
    }
    void ChangeWalletValue(Dictionary<CurrencyType, uint> dictionary)
    {
        for (int i = 0; i < textCurrency.Length; i++)
        {
            textCurrency[i].text = dictionary[(CurrencyType)i].ToString();
        }
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
    public bool SaveOnServer
    {
        get { return wallet.savingModule.SavingOnServer; }
        set { wallet.savingModule.SavingOnServer = value; }
    }

    public void ClearSavedData()
    {
        SavingWalletData.ClearAllSavedData();
    }

    public void OpenFile()
    {
        wallet.savingModule.OpenFile();
    }    
    public void OpenBinFile()
    {
        wallet.savingModule.OpenBinFile();
    }

    public void CreateAndLoadNewWallet(int loadWay)
    {
        StartCoroutine(CreateNewWallet(loadWay));
    }
    IEnumerator CreateNewWallet(int loadWay)
    {
        wallet.savingModule.changeWalletValue -= ChangeWalletValue;
        Destroy(wallet);

        yield return null;
        GameObject newWallet = GameObject.Find("Wallet");
        if (newWallet == null) newWallet = new GameObject("Wallet");
        wallet = newWallet.AddComponent<PlayerWallet>();
        wallet.savingModule = new SavingWalletData(toggle_playerPrefs.isOn, toggle_file.isOn, toggle_binFile.isOn, toggle_server.isOn, (SavingWalletData.LoadingPreset)loadWay);
        wallet.savingModule.changeWalletValue += ChangeWalletValue;
        ChangeWalletValue(wallet.Wallet);

        toggle_playerPrefs.isOn = wallet.savingModule.SavingInPlayerPrefs;
        toggle_file.isOn = wallet.savingModule.SavingInFile;
        toggle_binFile.isOn = wallet.savingModule.SavingInBinFile;
        toggle_binFile.isOn = wallet.savingModule.SavingOnServer;
    }
    public void CreateWalletButton()
    {
        CreateAndLoadNewWallet(loadingWay.value);
    }
}


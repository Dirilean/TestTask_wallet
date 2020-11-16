using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WalletModule;

public class CreateWalletAgain : MonoBehaviour
{
    public void CreateWallet()
    {
        if (PlayerWallet.instance!=null)
        Destroy(PlayerWallet.instance.gameObject);
        GameObject newWallet = new GameObject("newWallet");
        newWallet.AddComponent<PlayerWallet>();
    }
}

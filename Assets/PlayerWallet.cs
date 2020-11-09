using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace WalletModule
{
    public class PlayerWallet : MonoBehaviour
    {
        //Public fields ------------------------------------------------------------------------------------------
        public static PlayerWallet instance;
        public Action<CurrencyType, uint> changingCurrencyAmount;
        public Dictionary<CurrencyType, uint> Wallet { get; private set; }

        //Settings -----------------------------------------------------------------------------------------------
        [Header("Saving Settings")]
        public bool saveInFile = true;
        public bool binFormatForSave=false;
        [Space]
        public bool saveOnServer = true;
        [Header("Debug settings")]
        public bool debug=true;

        void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
                Wallet = new Dictionary<CurrencyType, uint>();
                for (int i = 0; i < Enum.GetNames(typeof(CurrencyType)).Length; i++)
                {
                    Wallet.Add((CurrencyType)i, 0);
                }
                if (debug) print("Created: " + WalletCash());
                changingCurrencyAmount += ChangingCurrencyAmount;
            }
        }

        private void Start()
        {
            LoadCurrencyAmount();
        }

        #region WalletOperations ------------------------------------------------------------------------------------------

        /// <summary>
        /// adding money to the player's game wallet
        /// </summary>
        /// <param name="type">Currency type</param>
        /// <param name="amount">Count of added money</param>
        public void AddCurrencyAmount(CurrencyType type, uint amount)
        {
            Wallet[type] += amount;
            if (debug) print(amount+" "+type.ToString()+" added: " + WalletCash());
            SaveCurrencyAmount();
        }

        /// <summary>
        /// removing money to the player's game wallet
        /// </summary>
        /// <param name="type">currency type</param>
        /// <param name="price">count of money for buy</param>
        /// <param name="OnSuccess">action after a successful purchase</param>
        /// <param name="OnDecline">action after rejecting</param>
        public void Buy(CurrencyType type, uint price, Action OnSuccess, Action OnDecline)
        {
            if (Wallet[type] >= price)
            {
                Wallet[type] -= price;
                OnSuccess.Invoke();
                SaveCurrencyAmount();
            }
            else
            {
                OnDecline.Invoke();
            }
        }


        #endregion

        #region Saving ----------------------------------------------------------------------------------------------------

        /// <summary>
        /// Get local saved amount currency value
        /// </summary>
        public void LoadCurrencyAmount()
        {
            for (int i = 0; i < Enum.GetNames(typeof(CurrencyType)).Length; i++)
            {
                Wallet[(CurrencyType)i] = Convert.ToUInt32(PlayerPrefs.GetString(((CurrencyType)i).ToString(),"2"));
            }
            if (debug) print("Loaded: "+ WalletCash());  
        }

        /// <summary>
        /// Save amount value on local device and other devices by wallet settings
        /// </summary>
        public void SaveCurrencyAmount()
        {
            foreach (KeyValuePair<CurrencyType, uint> cur in Wallet)
            {
                PlayerPrefs.SetString(cur.Key.ToString(), cur.Value.ToString());
            }
            if (saveInFile) SaveInFile();
            if (saveOnServer) SaveOnServer();
            if (debug) print("Saved: " + WalletCash());
        }

        public void SaveInFile()
        {

        }
        public void SaveOnServer()
        {
            StartCoroutine(Server.Post(Server.Api.walletSaving, JsonUtility.ToJson(Wallet),
                delegate (UnityWebRequest www) { print(www); },
                delegate { print("serverError"); }));
        }

        /// <summary>
        /// Сохранение изменений
        /// </summary>
        /// <param name="type"></param>
        /// <param name="amount"></param>
        public void ChangingCurrencyAmount(CurrencyType type, uint amount) => SaveCurrencyAmount();

        /// <summary>
        /// Current cash amount
        /// </summary>
        /// <returns></returns>
        public string WalletCash()
        {
            return (String.Join(", ", Wallet));
        }
        #endregion

    }

}
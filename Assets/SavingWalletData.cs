using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace WalletModule
{
    public class SavingWalletData
    {
        public Action<Dictionary<CurrencyType,uint>> needSaving;


        public bool debug=true;
        private const string filePath = "D://testSaving.txt";
        private const string filePathBin = "D://testBinSaving.txt";
        public SavingWalletData()
        {
            needSaving += SaveInPlayerPrefs;
            needSaving += SaveInFile;
            needSaving += SaveBinaryInFile;
        }

        #region Loading ------------------------------------------------------------------------------------
        /// <summary>
        /// Get PlayerPrefs saving
        /// </summary>
        public void LoadCurrencyAmount(out Dictionary<CurrencyType,uint> loadedWallet)
        {
            loadedWallet = new Dictionary<CurrencyType, uint>();
            for (int i = 0; i < Enum.GetNames(typeof(CurrencyType)).Length; i++)
            {
                loadedWallet[(CurrencyType)i] = Convert.ToUInt32(PlayerPrefs.GetString(((CurrencyType)i).ToString(), "2"));
            }
            if (debug) Debug.Log("Loaded from PlayerPrefs");
        }

        /// <summary>
        /// get file saving
        /// </summary>
        /// <param name="loadedWallet"></param>
        public void LoadFromFile(out Dictionary<CurrencyType, uint> loadedWallet)
        {
            loadedWallet = new Dictionary<CurrencyType, uint>();
            using (FileStream fstream = File.OpenRead(filePath))
            {
                byte[] array = new byte[fstream.Length];
                fstream.Read(array, 0, array.Length);
                string textFromFile = System.Text.Encoding.Default.GetString(array);
                loadedWallet = WalletSerialization.StringToDictionary(textFromFile);
                Debug.Log("Loaded from file "+filePath);
            }
        }

        public void LoadingFromBinary(out Dictionary<CurrencyType, uint> loadedWallet)
        {
            loadedWallet = new Dictionary<CurrencyType, uint>();
            using (BinaryReader reader = new BinaryReader(File.Open(filePathBin, FileMode.Open)))
            {
                // пока не достигнут конец файла
                // считываем каждое значение из файла
                while (reader.PeekChar() > -1)
                {
                    //loadedWallet.Add(reader.ReadString();)
                }
            }
        }
        #endregion


        #region Saving ---------------------------------------------------------------------------------------
        /// <summary>
        /// Save wallet value in PlayerPrefs
        /// </summary>
        public static void SaveInPlayerPrefs(Dictionary<CurrencyType, uint> currentWallet)
        {
            foreach (KeyValuePair<CurrencyType, uint> cur in currentWallet)
            {
                PlayerPrefs.SetString(cur.Key.ToString(), cur.Value.ToString());
            }
            Debug.Log("Save in PlayerPrefs");
        }

        /// <summary>
        /// Save wallet in file
        /// </summary>
        /// <param name="currentWallet"></param>
        public static void SaveInFile(Dictionary<CurrencyType, uint> currentWallet)
        {
            using (FileStream fstream = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                // преобразуем строку в байты
                byte[] array = System.Text.Encoding.Default.GetBytes(WalletSerialization.DictionaryToString(currentWallet));
                // запись массива байтов в файл
                fstream.Write(array, 0, array.Length);
                Debug.Log("Save in file "+filePath);
            }
        }

        public static void SaveBinaryInFile(Dictionary<CurrencyType, uint> currentWallet)
        {
            //Не работает
            using (BinaryWriter writer = new BinaryWriter(File.Open(filePathBin, FileMode.OpenOrCreate)))
            {
                // преобразуем строку в байты
                byte[] array = System.Text.Encoding.Default.GetBytes(WalletSerialization.DictionaryToString(currentWallet));
                writer.Write(array, 0, array.Length);
                Debug.Log("Save in file " + filePathBin);
            }
        }

        /// <summary>
        /// Save on server
        /// </summary>
        public static void SaveOnServer(Dictionary<CurrencyType, uint> currentWallet)
        {
            //StartCoroutine(Server.Post(Server.Api.walletSaving, JsonUtility.ToJson(PlayerWallet.instance.Wallet),
            //    delegate (UnityWebRequest www) { Debug.Log(www); },
            //    delegate { Debug.LogError("serverError"); }));
        }
        #endregion
    }
}

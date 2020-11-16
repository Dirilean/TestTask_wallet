using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

namespace WalletModule
{
    public class SavingWalletData
    {
        public Action<Dictionary<CurrencyType,uint>> needSaving;

        public bool debug=true;
        private const string filePath = "D://testSaving.txt";
        private const string filePathBin = "D://testBinSaving.txt";

        public bool SavingInPlayerPrefs
        {
            get { return needSaving.GetInvocationList().Any(x => x.Method.Name == "SaveInPlayerPrefs"); }
            set
            {
                if (value)
                    needSaving += SaveInPlayerPrefs;
                else
                    needSaving -= SaveInPlayerPrefs;
            }
        }
        public bool SavingInFile
        {
            get { return needSaving.GetInvocationList().Any(x => x.Method.Name == "SaveInFile"); }
            set
            {
                if (value)
                    needSaving += SaveInFile;
                else
                    needSaving -= SaveInFile;
            }
        }
        public bool SavingInBinFile
        {
            get { return needSaving.GetInvocationList().Any(x => x.Method.Name == "SaveBinaryInFile"); }
            set
            {
                if (value)
                    needSaving += SaveBinaryInFile;
                else
                    needSaving -= SaveBinaryInFile;
            }
        }


        public delegate void load(out Dictionary<CurrencyType, uint> loadedWallet);
        public load LoadWay { get; private set; }
        public enum LoadingPreset
        {
            PlayerPrefs,
            File,
            BinFile
        }

        public SavingWalletData(bool _savingInPlayerPrefs=true, bool _savingInFile=false, bool _savingInBinFile=false, LoadingPreset loadedPreset=LoadingPreset.PlayerPrefs)
        {
            SavingInPlayerPrefs = _savingInPlayerPrefs;
            SavingInFile = _savingInFile;
            SavingInBinFile = _savingInBinFile;

            switch(loadedPreset)
            {
                case LoadingPreset.PlayerPrefs: LoadWay = LoadPlayerPrefs; break;
                case LoadingPreset.File: LoadWay = LoadFromFile; break;
                case LoadingPreset.BinFile: LoadWay = LoadingFromBinary; break;
            }
        }

        #region Loading ------------------------------------------------------------------------------------

        /// <summary>
        /// Get PlayerPrefs saving
        /// </summary>
        void LoadPlayerPrefs(out Dictionary<CurrencyType,uint> loadedWallet)
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
        void LoadFromFile(out Dictionary<CurrencyType, uint> loadedWallet)
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

        void LoadingFromBinary(out Dictionary<CurrencyType, uint> loadedWallet)
        {
            loadedWallet = new Dictionary<CurrencyType, uint>();
            using (FileStream fs = File.OpenRead(filePath))
            using (BinaryReader reader = new BinaryReader(fs))
            {
                // Get count.
                int count = reader.ReadInt32();
                // Read in all pairs.
                for (int i = 0; i < count; i++)
                {
                    CurrencyType key = (CurrencyType)Enum.Parse(typeof(CurrencyType), reader.ReadString());
                    uint value = uint.Parse(reader.ReadString());
                    loadedWallet.Add(key, value);
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
            using (FileStream fs = File.OpenWrite(filePathBin))
            using (BinaryWriter writer = new BinaryWriter(fs))
            {
                // Put count.
                writer.Write(currentWallet.Count);
                // Write pairs.
                foreach (var pair in currentWallet)
                {
                    writer.Write((byte)pair.Key);
                    writer.Write(pair.Value);
                }
            }
            Debug.Log("Save binary in file " + filePathBin);
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

        public static void ClearPlayerPrefs()
        {
            for (int i = 0; i < Enum.GetNames(typeof(CurrencyType)).Length; i++)
            {
                PlayerPrefs.DeleteKey(((CurrencyType)i).ToString());
            }
            Debug.Log("PlayerPrefs with currency was cleaned");
        }
        public static void ClearFile()
        {
            File.Delete(filePath);
            Debug.Log("File on " + filePath + " was deleted");
        }
        public static void ClearBinFile()
        {
            File.Delete(filePathBin);
            Debug.Log("File on " + filePathBin + " was deleted");
            
        }

        public static void ClearAllSavedData()
        {
            ClearPlayerPrefs();
            ClearFile();
            ClearBinFile();
        }
    }
}

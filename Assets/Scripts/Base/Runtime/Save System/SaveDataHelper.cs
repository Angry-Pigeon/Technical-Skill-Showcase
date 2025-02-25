using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Tool_Development.SerializableScriptableObject.Scripts
{
    public enum SaveType
    {
        EncryptedJson,
        Json,
        PlayerPrefs,
        EncryptedAsyncJson,
    }
    [Serializable]
    public class SaveDataHelper : SerializedResourcedScriptableObject<SaveDataHelper>
    {

        [OdinSerialize]
        [InlineEditor()]
        public SerializedSaveData saveData;

        private readonly string firstPass = "word";
        private readonly string secondPass = "word2";
        [Button]
        public virtual void Save(SerializedSaveData data = null)
        {
            if (data != null) saveData = data;
            if (saveData == null)
            {
                Debug.LogError("Save Data is null");
                return;
            }
            switch (saveData.SaveType)
            {
                case SaveType.EncryptedJson:
                    SaveEncryptedJson();
                    break;
                case SaveType.Json:
                    SaveJson();
                    break;
                case SaveType.PlayerPrefs:
                    SavePlayerPrefs();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(saveData.SaveType), saveData.SaveType, null);
            }
        }

        [Button]
        public virtual void Load(SerializedSaveData data = null)
        {
            if (data != null) saveData = data;
            if (saveData == null)
            {
                Debug.LogError("Save Data is null");
                return;
            }
            switch (saveData.SaveType)
            {
                case SaveType.EncryptedJson:
                    LoadEncryptedJson();
                    break;
                case SaveType.Json:
                    LoadJson();
                    break;
                case SaveType.PlayerPrefs:
                    LoadPlayerPrefs();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(saveData.SaveType), saveData.SaveType, null);
            }
        }

        [Button]
        public virtual void Delete(SerializedSaveData data = null)
        {
            if (data != null) saveData = data;
            if (saveData == null)
            {
                Debug.LogError("Save Data is null");
                return;
            }

            switch (saveData.SaveType)
            {
                case SaveType.EncryptedJson:
                    DeleteEncryptedJson();
                    break;
                case SaveType.Json:
                    DeleteJson();
                    break;
                case SaveType.PlayerPrefs:
                    DeletePlayerPrefs();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(saveData.SaveType), saveData.SaveType, null);
            }
        }
        

        [Button]
        public virtual void DeleteAll(SerializedSaveData data = null)
        {
            if (data != null) saveData = data;
            if (saveData == null)
            {
                Debug.LogError("Save Data is null");
                return;
            }
            DeleteEncryptedJson();
            DeleteJson();
            DeletePlayerPrefs();
        }

        #region EncryptedJson

        protected void SaveEncryptedJson()
        {
            try
            {
                Directory.CreateDirectory(System.IO.Path.GetDirectoryName(GetFullPath()));

                string dataStore = JsonUtility.ToJson(saveData, true);
                dataStore = EncryptDecrypt(dataStore);
                using (FileStream stream = new FileStream(GetFullPath(), FileMode.Create))
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.Write(dataStore);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        protected async Task SaveEncryptedJsonAsync()
        {
            try
            {
                Directory.CreateDirectory(System.IO.Path.GetDirectoryName(GetFullPath()));

                string dataStore = JsonUtility.ToJson(saveData, true);
                dataStore = EncryptDecrypt(dataStore);

                using (FileStream stream = new FileStream(GetFullPath(), FileMode.Create, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true))
                {
                    using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8))
                    {
                        await writer.WriteAsync(dataStore);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Error saving encrypted JSON: {e.Message}");
            }
        }


        protected void LoadEncryptedJson()
        {
            if (File.Exists(GetFullPath()))
            {
                try
                {
                    string dataToLoad = "";
                    using (FileStream stream = new FileStream(GetFullPath(), FileMode.Open))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            dataToLoad = reader.ReadToEnd();
                        }
                    }

                    JsonUtility.FromJsonOverwrite(EncryptDecrypt(dataToLoad), saveData);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            else
            {
                
            }
        }

        protected async Task LoadEncryptedJsonAsync()
        {
            if (File.Exists(GetFullPath()))
            {
                try
                {
                    string dataToLoad;
                    using (FileStream stream = new FileStream(GetFullPath(), FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true))
                    {
                        using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                        {
                            dataToLoad = await reader.ReadToEndAsync();
                        }
                    }

                    JsonUtility.FromJsonOverwrite(EncryptDecrypt(dataToLoad), saveData);
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error loading encrypted JSON: {e.Message}");
                }
            }
            else
            {
                Debug.LogWarning("Encrypted JSON file not found.");
            }
        }




        protected virtual string EncryptDecrypt(string data)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                char character = (char)(data[i] ^ firstPass[i % firstPass.Length] ^ secondPass[i % secondPass.Length]);
                result.Append(character);
            }
            return result.ToString();
        }


        protected virtual void DeleteEncryptedJson()
        {
            if (File.Exists(GetFullPath()))
            {
                File.Delete(GetFullPath());
            }
        }

        #endregion

        #region Json

        protected virtual void SaveJson()
        {
            string json = JsonUtility.ToJson(saveData);
            File.WriteAllText(GetFullPath(), json);
        }

        protected virtual void LoadJson()
        {
            if (File.Exists(GetFullPath()))
            {
                string json = File.ReadAllText(GetFullPath());
                JsonUtility.FromJsonOverwrite(json, saveData);
            }
            else
            {
                Debug.LogError($"Save file not found at {GetFullPath()}");
                SaveJson();
            }
        }

        protected virtual void DeleteJson()
        {
            if (File.Exists(GetFullPath()))
            {
                File.Delete(GetFullPath());
            }
        }

        #endregion

        #region PlayerPrefs

        protected void SavePlayerPrefs()
        {
            string json = JsonUtility.ToJson(this);
            PlayerPrefs.SetString(GetFullName(), json);
        }

        protected void LoadPlayerPrefs()
        {
            if (PlayerPrefs.HasKey(GetFullName()))
            {
                string json = PlayerPrefs.GetString(GetFullName());
                JsonUtility.FromJsonOverwrite(json, this);
            }
            else
            {
                Debug.LogError($"Save file not found at {GetFullName()}");
                SavePlayerPrefs();
            }
        }

        protected virtual void DeletePlayerPrefs()
        {
            if (PlayerPrefs.HasKey(GetFullName()))
            {
                PlayerPrefs.DeleteKey(GetFullName());
            }
        }

        #endregion



        protected virtual string GetExtension()
        {
            switch (saveData.SaveType)
            {
                case SaveType.EncryptedJson:
                    return ".encjson";
                case SaveType.Json:
                    return ".json";
                case SaveType.PlayerPrefs:
                    return "";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected virtual string GetFullName()
        {
            return $"{saveData.SaveName}{GetExtension()}";
        }
        
        protected virtual string GetFullPath()
        {
            return System.IO.Path.Combine(Application.persistentDataPath, GetFullName());
        }
        
    }
}
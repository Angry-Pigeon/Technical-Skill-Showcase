using System;
using System.Collections.Generic;
using Herkdes.EnumCreator;
using UnityEngine;
using UnityEngine.Serialization;
namespace Tool_Development.SerializableScriptableObject.Scripts
{
    [CreateAssetMenu(fileName = "NewSaveData", menuName = "Save/New Save Data Dictionary", order = 0)]
    public class DictionarySaveType : SerializedSaveData
    {
        public Dictionary<string, string> DataDictionary;

        public override void CreateEnums()
        {
            var keys = new string[DataDictionary.Keys.Count];
            var i = 0;
            foreach (var key in DataDictionary.Keys)
            {
                keys[i] = key.ToString();
                i++;
            }
            #if UNITY_EDITOR
            EnumCreator.CreateEnum(SaveName, keys, false, Prefix, Suffix);

            #endif
            base.CreateEnums();
        }

        public override Enum GetEnum()
        {
            return SaveEnum;
        }

        public override T GetData<T>(Enum saveType, T defaultValue = default)
        {
            string key = saveType.ToString();
            if (DataDictionary.TryGetValue(key, out string value1))
            {
                T value = (T)Convert.ChangeType(value1, typeof(T));
                return value;
            }
            else
            {
                DataDictionary.Add(key, defaultValue.ToString());
                Debug.LogError($"Key {key} does not exist in {SaveName}, creating it with default value {defaultValue}");
                markedForEnumCreation = true;
                return defaultValue;
            }
        }

        public override T SetData<T>(Enum saveType, T value)
        {
            string key = saveType.ToString();
            if (DataDictionary.ContainsKey(key))
            {
                DataDictionary[key] = value.ToString();
            }
            else
            {
                DataDictionary.Add(key, value.ToString());
            }

            return value;
        }

        public override T AddData<T>(Enum saveType, T value)
        {
            string key = saveType.ToString();
            if (DataDictionary.ContainsKey(key))
            {
                T temp = GetData<T>(saveType);
                temp = (T)(object)(Convert.ToInt32(temp) + Convert.ToInt32(value));
                SetData(saveType, temp);
            }
            else
            {
                DataDictionary.Add(key, value.ToString());
            }

            return GetData<T>(saveType);
        }

        public override T SubtractData<T>(Enum saveType, T value)
        {
            string key = saveType.ToString();
            if (DataDictionary.ContainsKey(key))
            {
                T temp = GetData<T>(saveType);
                temp = (T)(object)(Convert.ToInt32(temp) - Convert.ToInt32(value));
                SetData(saveType, temp);
            }
            else
            {
                DataDictionary.Add(key, value.ToString());
            }

            return GetData<T>(saveType);
        }

        public override void ClearData()
        {
            if (DataDictionary != null && DataDictionary.Count > 0)
            {
                List<string> keys = new List<string>();
                foreach (var key in DataDictionary.Keys)
                {
                    keys.Add(key);
                }

                DataDictionary = new Dictionary<string, string>();
                foreach (var key in keys)
                {
                    DataDictionary.Add(key, "0");
                }
            }
            
            Save();

        }


    }
}
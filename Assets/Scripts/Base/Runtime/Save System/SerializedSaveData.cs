using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Herkdes.EnumCreator;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
namespace Tool_Development.SerializableScriptableObject.Scripts
{
    public enum SaveDataEnumExample
    {
        Example1,
        Example2,
        Example3
    }
    [CreateAssetMenu(fileName = "NewSaveData", menuName = "Save/New Save Data", order = 0)]
    [Serializable]
    public class SerializedSaveData : SerializedScriptableObject
    {
        
        [field: FoldoutGroup("Save Data")]
        [field: SerializeField]
        public SaveType SaveType { get; private set; }
        [field: FoldoutGroup("Save Data")]
        [field: SerializeField]
        public string SaveName { get; protected set; }
        [field: FoldoutGroup("Save Data")]
        [field: SerializeField]
        public string Prefix { get; private set; }
        [field: FoldoutGroup("Save Data")]
        [field: SerializeField]
        public string Suffix { get; private set; }
        [field: FoldoutGroup("Save Data")]
        [field: SerializeField]
        public Enum SaveEnum { get; private set; }
        
        protected bool markedForEnumCreation = false;
        
        [Button]
        public virtual void Save()
        {
            SaveDataHelper.Instance().Save(this);
        }

        [Button]
        public virtual void Load()
        {
            SaveDataHelper.Instance().Load(this);
        }

        [Button]
        public virtual void Delete()
        {
            SaveDataHelper.Instance().Delete(this);
        }

        [Button]
        public virtual void ClearData()
        {
            
        }

        [Button]
        public virtual void CreateEnums()
        {
            
        }
        public void OnEditor()
        {
            if (markedForEnumCreation)
            {
                Load();
                CreateEnums();
                Save();
            }
        }
        
        public virtual Enum GetEnum()
        {
            return null;
        }

        #if UNITY_EDITOR
        
        public virtual string GetEnumTypeName()
        {
            return EnumCreator.GenerateEnumName(SaveName, Prefix, Suffix);
        }
        
        #endif

        public virtual T GetData<T>(Enum saveType, T defaultValue = default)
        {
            return default;
        }
        
        public virtual T SetData<T>(Enum saveType, T value)
        {
            return default;
        }
        
        public virtual T AddData<T>(Enum saveType, T value)
        {
            return default;
        }
        
        public virtual T SubtractData<T>(Enum saveType, T value)
        {
            return default;
        }
        
        public virtual T GetDataSet<T>()
        {
            return default;
        }
    }
}
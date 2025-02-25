using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Win32;
using Sirenix.OdinInspector;
using UnityEngine;
namespace Editor.Utility
{
    [Serializable]
    public class PlayerPrefsEditor
    {
        [BoxGroup("PlayerPrefs", Order = 1)]
        [TableList]
        public List<PlayerPrefsShowcase> PlayerPrefsList = new List<PlayerPrefsShowcase>();

        public PlayerPrefsEditor()
        {
            Refresh();
        }
        
        [BoxGroup("Functions", Order = 0)]
        [Button]
        public void SaveAll()
        {
            foreach (var playerPref in PlayerPrefsList)
            {
                playerPref.Save();
            }
        }
        
        [BoxGroup("Functions", Order = -1)]
        [Button]
        public void Refresh()
        {
            PlayerPrefsList.Clear();
            List<PlayerPrefsShowcase> unitySpecificPrefs = new List<PlayerPrefsShowcase>();
            List<PlayerPrefsShowcase> userPrefs = new List<PlayerPrefsShowcase>();

            Dictionary<string, object> allPrefs = GetAllPlayerPrefs();
            foreach (var kvp in allPrefs)
            {
                // Clean up the key by removing any suffix that starts with "_h" followed by numbers
                string cleanKey = System.Text.RegularExpressions.Regex.Replace(kvp.Key, @"_h\d+$", "");

                // Check if the value is of a readable type (int, float, or string)
                if (kvp.Value is int || kvp.Value is float || kvp.Value is string)
                {
                    PlayerPrefsShowcase showcase = new PlayerPrefsShowcase
                    {
                        Key = cleanKey,
                        Value = kvp.Value.ToString()
                    };

                    // Separate Unity-specific keys
                    if (cleanKey.StartsWith("unity."))
                    {
                        unitySpecificPrefs.Add(showcase);
                    }
                    else
                    {
                        userPrefs.Add(showcase);
                    }
                }
                else
                {
                    Debug.LogWarning($"Skipping unreadable PlayerPrefs key: {cleanKey} with value type: {kvp.Value.GetType()}");
                }
            }

            // Add user-defined keys first, then Unity-specific ones
            PlayerPrefsList.AddRange(userPrefs);
            PlayerPrefsList.AddRange(unitySpecificPrefs);
        }






        public static Dictionary<string, object> GetAllPlayerPrefs()
        {
            Dictionary<string, object> allPrefs = new Dictionary<string, object>();

#if UNITY_EDITOR_WIN
            // Get company and product name from PlayerSettings
            string companyName = Application.companyName;
            string productName = Application.productName;

            // Path to the registry key
            string registryPath = $@"Software\Unity\UnityEditor\{companyName}\{productName}";
            Debug.Log($"Attempting to access PlayerPrefs at registry path: {registryPath}");

            // Open the registry key
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(registryPath))
            {
                if (key != null)
                {
                    foreach (string valueName in key.GetValueNames())
                    {
                        object value = key.GetValue(valueName);

                        allPrefs[valueName] = value;
                        // Check the type and add it to the dictionary
                        if (value is int)
                            allPrefs[valueName] = value;
                        else if (value is float)
                            allPrefs[valueName] = value;
                        else if (value is string)
                            allPrefs[valueName] = value;
                        else
                            Debug.LogWarning($"Unknown PlayerPrefs type for key: {valueName}");
                    }
                }
                else
                {
                    Debug.LogWarning("No PlayerPrefs found in the registry. Check the company and product names.");
                }
            }

#elif UNITY_EDITOR_OSX
    // Path to the .plist file
    string plistPath = $"~/Library/Preferences/unity.{Application.companyName}.{Application.productName}.plist";
    plistPath = plistPath.Replace("~", System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile));
    Debug.Log($"Attempting to access PlayerPrefs at plist path: {plistPath}");

    if (File.Exists(plistPath))
    {
        try
        {
            var plist = (IDictionary)Plist.readPlist(plistPath);
            foreach (DictionaryEntry entry in plist)
            {
                string key = entry.Key.ToString();
                allPrefs[key] = entry.Value;
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error reading plist file: {e.Message}");
        }
    }
    else
    {
        Debug.LogWarning("PlayerPrefs plist file not found. Check the path and product/company name.");
    }
#endif

            return allPrefs;
        }

    }
}

[Serializable]
public class PlayerPrefsShowcase
{
    public string Key;
    public string Value;
    
    public void Save()
    {
        if (int.TryParse(Value, out int intValue))
        {
            PlayerPrefs.SetInt(Key, intValue);
        }
        else if (float.TryParse(Value, out float floatValue))
        {
            PlayerPrefs.SetFloat(Key, floatValue);
        }
        else
        {
            PlayerPrefs.SetString(Key, Value);
        }
    }
}
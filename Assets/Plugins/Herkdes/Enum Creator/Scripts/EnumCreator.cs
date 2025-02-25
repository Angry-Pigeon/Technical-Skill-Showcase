using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

// MIT License
//
// Copyright (c) 2023 [Angry Pigeon]
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

namespace Herkdes.EnumCreator {
    public static class EnumCreator {
#if UNITY_EDITOR
        
        /// <summary>
        /// Base path for the enum storage
        /// </summary>
        public const string BasePath = "Assets/Resources/Base/EnumStorage/";

        /// <summary>
        /// Create an enum with the given user input
        /// </summary>
        /// <param name="enumName"></param>
        /// <param name="enumMembers"></param>
        /// <param name="hasFlags"></param>
        /// <param name="enumPrefix"></param>
        /// <param name="enumSuffix"></param>
        /// <param name="memberPrefix"></param>
        /// <param name="memberSuffix"></param>
        public static void CreateEnum(string enumName, string[] enumMembers, bool hasFlags = false, string enumPrefix = "", string enumSuffix = "", string memberPrefix = "", string memberSuffix = "")
        {
            enumPrefix = string.IsNullOrEmpty(enumPrefix) ? "Enum_" : enumPrefix;
            enumSuffix = enumSuffix.Insert(0, string.IsNullOrEmpty(enumSuffix) ? "" : "_");
            memberPrefix += string.IsNullOrEmpty(memberPrefix) ? "" : "_";
            memberSuffix = memberSuffix.Insert(0, string.IsNullOrEmpty(memberSuffix) ? "" : "_");

            string enumFileName = enumPrefix + enumName + ".cs";
            string flagAttribute = hasFlags ? "using System;\n[Flags]\n" : "";
            string fullName = flagAttribute + "public enum " + enumPrefix + enumName + $"{enumSuffix}" + "{";
            string writePath = BasePath + enumFileName;

            if (enumMembers.Length > 0)
            {
                for (int i = 0; i < enumMembers.Length; i++)
                {
                    string member = enumMembers[i];
                    string value = hasFlags ? " = 1 << " + i : "";
                    fullName += " " + $"{memberPrefix}" + member + $"{memberSuffix}" + value;

                    if (i != enumMembers.Length - 1)
                        fullName += ",";
                }
                fullName += "}";
            }
            else fullName += "}";
            File.WriteAllText(writePath, fullName);
            AssetDatabase.Refresh();
        }
        
        public static string GenerateEnumName(string enumName, string enumPrefix, string enumSuffix)
        {
            enumPrefix = string.IsNullOrEmpty(enumPrefix) ? "Enum_" : enumPrefix;
            enumSuffix = enumSuffix.Insert(0, string.IsNullOrEmpty(enumSuffix) ? "" : "_");
            return enumPrefix + enumName + enumSuffix;
        }
        
        /// <summary>
        /// Create an enum with the given user input
        /// </summary>
        /// <param name="user"></param>
        public static void CreateEnum(this EnumCreatorUser user)
        {
            user = user.Validate();

            string enumFileName = user.EnumPrefix + user.EnumName + ".cs";
            string flagAttribute = user.hasFlags ? "using System;\n[Flags]\n" : "";
            string fullName = flagAttribute + "public enum " + user.EnumPrefix + user.EnumName + $"{user.EnumSuffix}" + "{";
            string basePath = BasePath;
            
            if (!string.IsNullOrWhiteSpace(user.Folder))
            {
                FindAndDeleteAssetByName(user.EnumPrefix + user.EnumName);
                basePath = user.Folder + "/";
            }
            else
            {
                ValidatePath((basePath + enumFileName).Remove((basePath + enumFileName).Length -3));
            }
            
            string writePath = basePath + enumFileName;

            if (user.EnumMembers.Length > 0)
            {
                for (int i = 0; i < user.EnumMembers.Length; i++)
                {
                    string member = user.EnumMembers[i];
                    string value = user.hasFlags ? " = 1 << " + i : "";
                    fullName += " " + $"{user.MemberPrefix}" + member + $"{user.MemberSuffix}" + value;

                    if (i != user.EnumMembers.Length - 1)
                        fullName += ",";
                }
                fullName += "}";
            }
            else fullName += "}";
            File.WriteAllText(writePath, fullName);
            AssetDatabase.Refresh();
        }
        
        public static void FindAndDeleteAssetByName(this string name, string searchFolder = "Assets/Resources/Base")
        {
            string path = GetAssetPathByName(name, searchFolder);
            if (path == null)
                return;
            AssetDatabase.DeleteAsset(path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        
        public static string GetAssetPathByName(this string name, string searchFolder = "Assets/Resources/Base")
        {
            string[] folders = new string[] { searchFolder };
            string[] assets = AssetDatabase.FindAssets(name, folders);
            if (assets.Length == 0)
            {
                return null;
            }
            string path = AssetDatabase.GUIDToAssetPath(assets[0]);
            return path;
        }
        
        private static void ValidatePath(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        #endif

        
        /// <summary>
        /// Finds the prefix that ends with "_" and delete everything before the "_"
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetPrefix(this string name) {

            var prefix = name.Split('_')[0];
            return prefix;
        }

        /// <summary>
        /// Find the suffix that starts with "_" and delete everything after the "_"
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetSuffix(this string name) {
            var suffix = name.Split('_')[1];
            return suffix;
        }

        /// <summary>
        /// Validate the input and return the correct prefix and suffix
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static EnumCreatorUser Validate(this EnumCreatorUser user)
        {
            if (!string.IsNullOrEmpty(user.EnumPrefix))
            {
                if (!user.EnumPrefix.HasUnderScore())
                {
                    user.EnumPrefix += "_";
                }
            }
            else
            {
                user.EnumPrefix += "Enum_";
            }
            
            if (!string.IsNullOrEmpty(user.EnumSuffix))
            {
                if (!user.EnumSuffix.HasUnderScore())
                {
                    user.EnumSuffix = "_" + user.EnumSuffix;
                }
            }
            else
            {
                user.EnumSuffix = "";
            }

            return user;
        }

        /// <summary>
        /// Check if the string contains an underscore
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool HasUnderScore(this string name) {
            bool hasUnderScore = name.Contains("_");
            return hasUnderScore;
        }
        /// <summary>
        /// Get the enum name from the enum type, and remove the prefix and suffix
        /// </summary>
        /// <param name="item">Enum input that you want to clear</param>
        /// <param name="HasPrefix">If the item had prefix. Default value : true </param>
        /// <param name="HasSuffix">If the item had a suffix. Default value : false </param>
        /// <typeparam name="T">Return type of the enum you input</typeparam>
        /// <returns></returns>
        public static string Clear<T>(this T item, bool HasPrefix = true, bool HasSuffix = false) where T : Enum {
            string name = item.ToString();
            var indexes = new List<int>();
            for (int i = 0; i < name.Length; i++) {
                if (name[i].ToString() == "_") {
                    indexes.Add(i);
                }
            }
            switch (indexes.Count) {
                case <= 0:
                    return name;
                case 1: {
                    if (HasPrefix) {
                        //Remove everything before the index
                        name = name.Remove(0, indexes[0] + 1);
                    }
                    break;
                }
                case >= 2 when HasPrefix & HasSuffix:
                    name = name.Remove(0, indexes[0] + 1);
                    indexes[1] = indexes[1] - indexes[0] - 1;
                    name = name.Remove(indexes[1], name.Length - indexes[1]);
                    return name;
                case >= 2 when HasPrefix:
                    name = name.Remove(0, indexes[0] + 1);
                    return name;
                case >= 2 when HasSuffix:
                    name = name.Remove(indexes.Last(), name.Length - indexes[1]);
                    return name;
            }
            return name;
        }
        /// <summary>
        /// Turns the string into an enum, if it exists
        /// </summary>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T ToEnum<T>(this string value) where T : Enum
        {
            return (T)Enum.Parse(typeof(T), value);
        }
        
        /// <summary>
        /// Turns the string into an enum, if it exists
        /// </summary>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryToEnum<T>(this string value, out T result) where T : struct, Enum
        {
            if (Enum.TryParse<T>(value, out T parsedValue))
            {
                result = parsedValue;
                return true;
            }
            else
            {
                result = default(T);
                return false;
            }
        }
        
        /// <summary>
        /// Checks if the enum contains the flag
        /// </summary>
        /// <param name="value"></param>
        /// <param name="flag"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool Contains<T>(this T value, T flag) where T : Enum
        {
            long valueLong = Convert.ToInt64(value);
            long flagLong = Convert.ToInt64(flag);

            return (valueLong & flagLong) == flagLong;
        }
        
        /// <summary>
        /// Removes the flag from the enum
        /// </summary>
        /// <param name="value"></param>
        /// <param name="flag"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T RemoveFlag<T>(this T value, T flag) where T : Enum
        {
            long valueLong = Convert.ToInt64(value);
            long flagLong = Convert.ToInt64(flag);
            long newValueLong = valueLong & ~flagLong;
            return (T)Enum.ToObject(typeof(T), newValueLong);
        }
        
        /// <summary>
        /// Adds the flag to the enum
        /// </summary>
        /// <param name="value"></param>
        /// <param name="flag"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T AddFlag<T>(this T value, T flag) where T : Enum
        {
            long valueLong = Convert.ToInt64(value);
            long flagLong = Convert.ToInt64(flag);
            long newValueLong = valueLong | flagLong;
            return (T)Enum.ToObject(typeof(T), newValueLong);
        }
        
        public static List<T> GetFlags<T>(this T value) where T : Enum
        {
            List<T> flags = new List<T>();
            foreach (T flag in Enum.GetValues(typeof(T)))
            {
                if (value.Contains(flag))
                {
                    flags.Add(flag);
                }
            }
            return flags;
        }

    }
}
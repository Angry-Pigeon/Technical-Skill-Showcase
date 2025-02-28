using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Base {
    
    public enum BlendMode {
        Opaque,
        Cutout,
        Fade,
        Transparent
    }
    public static class Base_Extensions {

        #region Math Extentions

        public static bool IsBetweenRange(this float value, float min, float max) {
            return value >= min & value <= max;
        }

        public static float Round(float value, int digits) {
            var mult = Mathf.Pow(10.0f, digits);
            return Mathf.Round(value * mult) / mult;
        }

        public static float RoundTo(this float value) {
            return Mathf.Round(value / 100) * 100;
        }

        public static float RoundTo(this float value, float to = 1) {
            return Mathf.Round(value / to) * to;
        }
        
        public static float RoundTo(this float value, float to = 1, bool ceiling = false) {
            float floor = Mathf.Floor(value / to) * to;
            float ceil = Mathf.Ceil(value / to) * to;

            if (ceiling && value - floor <= ceil - value) {
                return ceil;
            } else {
                return floor + to;
            }
        }
        
        public static float RoundTo(this int value) {
            return Mathf.Round(value / 100) * 100;
        }

        public static float RoundTo(this int value, float to = 1) {
            return Mathf.Round(value / to) * to;
        }

        public static float Multi(this float value, float multiplier) {
            return value * multiplier;
        }


        public static float Remap(this float value, float from1, float to1, float from2, float to2, bool clamp = true) {
            float x = (value - from1) / (to1 - from1) * (to2 - from2) + from2;
            if (from2 < to2)
                return !clamp ? x : Mathf.Clamp(x, from2, to2);
            else
                return !clamp ? x : Mathf.Clamp(x, to2, from2);
        }

        public static float Remap(this int value, float from1, float to1, float from2, float to2, bool clamp = true) {
            float x = (value - from1) / (to1 - from1) * (to2 - from2) + from2;

            if (from2 < to2)
                return !clamp ? x : Mathf.Clamp(x, from2, to2);
            else
                return !clamp ? x : Mathf.Clamp(x, to2, from2);
        }
        
        // public static float Remap(this float value, float inMin, float inMax, float outMin, float outMax)
        // {
        //     // Avoid division by zero
        //     if (Mathf.Approximately(inMax, inMin))
        //     {
        //         Debug.LogWarning("inMax and inMin are too close or equal, returning outMin");
        //         return outMin;
        //     }
        //     return (value - inMin) / (inMax - inMin) * (outMax - outMin) + outMin;
        // }



        public static float ClampAngle(float angle, float min, float max) {
            angle = Mathf.Repeat(angle, 360);
            min = Mathf.Repeat(min, 360);
            max = Mathf.Repeat(max, 360);
            var inverse = false;
            var tmin = min;
            var tangle = angle;
            if (min > 180) {
                inverse = !inverse;
                tmin -= 180;
            }

            if (angle > 180) {
                inverse = !inverse;
                tangle -= 180;
            }

            var result = !inverse ? tangle > tmin : tangle < tmin;
            if (!result)
                angle = min;

            inverse = false;
            tangle = angle;
            var tmax = max;
            if (angle > 180) {
                inverse = !inverse;
                tangle -= 180;
            }

            if (max > 180) {
                inverse = !inverse;
                tmax -= 180;
            }

            result = !inverse ? tangle < tmax : tangle > tmax;
            if (!result)
                angle = max;
            return angle;
        }
        
        public static float GetPercentage(this Vector2 range, float value) {
            //Return the percentage of the value between the range, and convert it to a 0-1 range
            return (value - range.x) / (range.y - range.x);
        }
        
        public static float GetPercentage(this Vector2Int range, float value) {
            //Return the percentage of the value between the range, and convert it to a 0-1 range
            return (value - range.x) / (range.y - range.x);
        }

        #endregion

        #region Vector3 Extensions

        public static Vector3 GetWorldPosition(Ray Ray, LayerMask Mask) {
            RaycastHit hit;
            if (Physics.Raycast(Ray, out hit, Mathf.Infinity, Mask)) return hit.point;
            return Vector3.zero;
        }

        public static Vector3 GetWorldPosition(this Vector3 Vector3, Camera Camera, LayerMask Mask) {
            var ray = Camera.ScreenPointToRay(Vector3);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, Mask)) return hit.point;
            return Vector3.zero;
        }

        public static Transform GetWorldObject(this Vector3 Vector3, Camera Camera, LayerMask Mask) {
            var ray = Camera.ScreenPointToRay(Vector3);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, Mask)) return hit.collider.transform;
            return null;
        }

        public static Vector3 GetHitPosition(this Vector3 MainObject, Vector3 ObjectToPush, float yMinus = 0, float force = 1) {
            var _temp = MainObject;
            _temp.y -= yMinus;
            return (ObjectToPush - _temp) * force;
        }

        public static Vector3 FindCenterInGroup<T>(this IEnumerable<T> ObjectGroup) where T : MonoBehaviour {
            MonoBehaviour[] objects = ObjectGroup.ToArray();
            if (objects.Length == 0)
                return Vector3.zero;
            if (objects.Length == 1)
                return objects[0].transform.position;
            var bounds = new Bounds(objects[0].transform.position, Vector3.zero);
            for (var i = 1; i < objects.Length; i++) {
                if (objects[i] != null)
                    bounds.Encapsulate(objects[i].transform.position);
            }

            return bounds.center;
        }
        
        public static Vector3 Remap(this Vector3 value, Vector3 inMin, Vector3 inMax, Vector3 outMin, Vector3 outMax)
        {
            float x = value.x.Remap(inMin.x, inMax.x, outMin.x, outMax.x);
            float y = value.y.Remap(inMin.y, inMax.y, outMin.y, outMax.y);
            float z = value.z.Remap(inMin.z, inMax.z, outMin.z, outMax.z);
            return new Vector3(x, y, z);
        }
        
        public static Vector3 RemapToFloat(this float value, float inMin, float inMax, Vector3 outMin, Vector3 outMax)
        {
            float x = value.Remap(inMin, inMax, outMin.x, outMax.x);
            float y = value.Remap(inMin, inMax, outMin.y, outMax.y);
            float z = value.Remap(inMin, inMax, outMin.z, outMax.z);
            return new Vector3(x, y, z);
        }


        #endregion

        #region Transform Extensions

        public static MonoBehaviour GetOrAddComponent(this Transform transform, Type type) {
            var component = transform.GetComponent(type);
            if (component == null) {
                component = transform.gameObject.AddComponent(type);
            }
            return component as MonoBehaviour;
        }

        public static MonoBehaviour[] GetAllChildComponents(this Transform transform, Type type) {
            var components = new List<MonoBehaviour>();
            foreach (Transform child in transform) {
                var component = child.GetComponent(type);
                if (component != null) {
                    components.Add(component as MonoBehaviour);
                }
            }
            return components.ToArray();
        }

        public static void DestroyAllChildComponents(this Transform transform, Type type) {
            foreach (Transform child in transform) {
                var component = child.GetComponent(type);
                if (component != null) {
                    Object.Destroy(component);
                }
            }
        }

        public static MonoBehaviour[] GetChildren(this Transform transform) {
            var children = new List<MonoBehaviour>();
            foreach (Transform child in transform) {
                children.Add(child.GetComponent<MonoBehaviour>());
            }
            return children.ToArray();
        }

        public static List<Transform> GetAllChildren(Transform go) {
            List<Transform> list = new List<Transform>();
            return GetChildrenHelper(go, list);
        }

        private static List<Transform> GetChildrenHelper(Transform go, List<Transform> list) {
            if (go == null || go.transform.childCount == 0) {
                return list;
            }
            foreach (Transform t in go.transform) {
                list.Add(t);
                GetChildrenHelper(t, list);
            }
            return list;
        }



        public static void DestroyAllChildren(this Transform transform) {
            if (transform.childCount <= 0) return;
            for (int i = transform.childCount - 1; i >= 0; i--) {
#if UNITY_EDITOR
                if (!Application.isPlaying)
                    GameObject.DestroyImmediate(transform.GetChild(i).gameObject);
#endif
                if (Application.isPlaying)
                    GameObject.Destroy(transform.GetChild(i).gameObject);
            }
        }

        public static void ResizeObject(this Transform objToEnlarge, float Size) {
            objToEnlarge.localScale = new Vector3(Size, Size, Size);
        }

        #endregion

        #region GameObject Extensions

        public static T InstantiateB<T>(this T ObjectToInstantiate) where T : MonoBehaviour {
            var ReturnObject = GameObject.Instantiate(ObjectToInstantiate);
            return ReturnObject;
        }

        public static T InstantiateB<T>(this T ObjectToInstantiate, Vector3 Position) where T : MonoBehaviour {
            var ReturnObject = GameObject.Instantiate(ObjectToInstantiate);
            ReturnObject.transform.position = Position;
            return ReturnObject;
        }

        public static T InstantiateB<T>(this T ObjectToInstantiate, Vector3 Position, Quaternion Rotation) where T : MonoBehaviour {
            var ReturnObject = GameObject.Instantiate(ObjectToInstantiate);
            ReturnObject.transform.position = Position;
            ReturnObject.transform.rotation = Rotation;
            return ReturnObject;
        }

        public static bool CheckLayer(this GameObject GameObject, LayerMask LayerMask) {
            return LayerMask == (LayerMask | (1 << GameObject.layer));
        }

        /// <summary>
        /// Takes in a GameObject and copies it, needs a material to ghost it
        /// </summary>
        /// <param name="ObjectToCopy"></param>
        /// <param name="GhostMaterial"></param>
        /// <param name="Parent"></param>
        /// <returns></returns>
        public static GameObject CreateGhostObject(this GameObject ObjectToCopy, Material GhostMaterial, Transform Parent = null) {

            GameObject ghostObject = GameObject.Instantiate(ObjectToCopy, Parent);

            List<GameObject> meshes = new List<GameObject>();
            foreach (var x in ghostObject.GetComponentsInChildren<MeshRenderer>()) {
                meshes.Add(x.gameObject);
            }

            for (int i = 0; i < meshes.Count; i++) {
                meshes[i].TryGetComponent(out Renderer rends);
                Material[] materials = new Material[rends.sharedMaterials.Length];
                for (int x = 0; x < rends.sharedMaterials.Length; x++) {
                    materials[x] = GhostMaterial;
                }
                rends.sharedMaterials = materials;
            }

            return ghostObject;
        }

        #endregion

        #region String Extensions

        public static bool IsNullOrEmpty(this string String) {
            return String == null | String.Length <= 0 | String == "";
        }

        public static bool IsAllLetters(this string s) {
            foreach (var c in s)
                if (!char.IsLetter(c))
                    return false;
            return true;
        }

        public static bool IsAllDigits(this string s) {
            foreach (var c in s)
                if (!char.IsDigit(c))
                    return false;
            return true;
        }

        public static bool IsAllLettersOrDigits(this string s) {
            foreach (var c in s)
                if (!char.IsLetterOrDigit(c))
                    return false;
            return true;
        }

        public static float IsFloat(this string s) {
            if (s.IsAllDigits())
                return float.Parse(s);
            return 0;
        }

        public enum StringViabilityStatus {
            Viable,
            Null,
            Incomplete,
            HasDigits
        }

        public static StringViabilityStatus IsVaibleForSave(this string obj) {
            if (obj.Length <= 3 && !(obj == null || obj == "Null" || string.IsNullOrEmpty(obj)))
                return StringViabilityStatus.Incomplete;
            if (obj == null || obj == "Null" || string.IsNullOrEmpty(obj)) return StringViabilityStatus.Null;
            if (obj.Any(char.IsDigit)) return StringViabilityStatus.HasDigits;
            // if(obj.Any(char.spa))
            return StringViabilityStatus.Viable;
        }

        public static string MakeViable(this string obj) {
            switch (obj.IsVaibleForSave()) {
                case StringViabilityStatus.Viable:
                    return obj;
                case StringViabilityStatus.Null:
                    Debug.Log("Name was " + obj.IsVaibleForSave());
                    return "";
                case StringViabilityStatus.Incomplete:
                    Debug.Log(obj + " Was " + obj.IsVaibleForSave());
                    return obj + "_Completed_Part";
                case StringViabilityStatus.HasDigits:
                    Debug.Log(obj + " " + obj.IsVaibleForSave());
                    var newObj = obj.Where(t => !char.IsDigit(t)).ToArray();
                    var newName = new string(newObj);
                    if (newName.Where(t => char.IsLetter(t)).ToArray().Length <= 3) newName = "";
                    return newName;
            }

            return null;
        }

        #endregion

        #region AssetDatabase Extensions

        /// <summary>
        /// Only works on editor
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> FindAssetsByType<T>() where T : UnityEngine.Object {
#if UNITY_EDITOR
            List<T> assets = new List<T>();
            string[] guids = AssetDatabase.FindAssets(string.Format("t:{0}", typeof(T)));
            for (int i = 0; i < guids.Length; i++) {
                string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
                T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
                if (asset != null) {
                    assets.Add(asset);
                }
            }

            return assets;
#else
            return null;
#endif
        }

        public static List<string> FindAssetsPathByType<T>() where T : UnityEngine.Object {
#if UNITY_EDITOR
            List<string> assets = new List<string>();
            string[] guids = AssetDatabase.FindAssets(string.Format("t:{0}", typeof(T)));
            for (int i = 0; i < guids.Length; i++) {
                string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
                T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
                if (asset != null) {
                    assets.Add(assetPath);
                }
            }

            return assets;
#else
            return null;
#endif
        }

        public static string FindAssetParentPath(this string originalPath) {
#if UNITY_EDITOR
            return Directory.GetParent(originalPath)?.ToString().Replace("\\", "/");
#else
            return null;
#endif
        }

        public static string FindAssetPath<T>() where T : UnityEngine.Object {
#if UNITY_EDITOR

            string assetPath = "";
            string[] guids = AssetDatabase.FindAssets(string.Format("t:{0}", typeof(T)));
            for (int i = 0; i < guids.Length; i++) {
                assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
                T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
                if (asset != null) {
                    assetPath = assetPath.Replace("\\", "/");
                }
            }

            return assetPath;

#else
            return null;
#endif
        }

        #endregion

        #region Coroutine Extensions

        #endregion

        #region IList Extensions

        /// <summary>
        /// Get random element from list or array. Returns null if list is empty.
        /// </summary>
        /// <param name="list"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetRandom<T>(this IList<T> list) {
            return list[UnityEngine.Random.Range(0, list.Count)];
        }

        /// <summary>
        /// Returns a shuffled copy of the list.
        /// </summary>
        /// <param name="list"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> Shuffle<T>(this IList<T> list) {
            List<T> shuffled = new List<T>(list);
            for (int i = 0; i < shuffled.Count; i++) {
                int randomIndex = UnityEngine.Random.Range(0, shuffled.Count);
                (shuffled[i], shuffled[randomIndex]) = (shuffled[randomIndex], shuffled[i]);
            }
            return shuffled;
        }

        /// <summary>
        /// Returns a value between the desired amount and the max amount.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="desired"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static int GetMax<T>(this IList<T> list, int desired = -1) {
            return desired == -1 ? list.Count : Mathf.Max(desired, list.Count);
        }

        public static bool IsNullOrEmpty<T>(this IList<T> list) => list == null || list.Count == 0;

        #endregion

        #region Collider Extensions

        public static Vector3 GetRandomPoint(this Collider collider) {
            return new Vector3(
                Random.Range(collider.bounds.min.x, collider.bounds.max.x),
                Random.Range(collider.bounds.min.y, collider.bounds.max.y),
                Random.Range(collider.bounds.min.z, collider.bounds.max.z)
            );
        }

        public static Vector3 GetRandomPoint(this Collider collider, float extends) {
            return new Vector3(
                Random.Range(collider.bounds.min.x, collider.bounds.max.x) * extends,
                Random.Range(collider.bounds.min.y, collider.bounds.max.y) * extends,
                Random.Range(collider.bounds.min.z, collider.bounds.max.z) * extends
            );
        }

        #endregion

        #region Color Extensions

        public static Color SetAlpha(this Color color, float alpha) {
            color.a = alpha;
            return color;
        }
        
        public static Color Increase (this Color color, float amount) {
            return new Color(color.r + amount, color.g + amount, color.b + amount, color.a);
        }
        
        public static Color Decrease (this Color color, float amount) {
            return new Color(color.r - amount, color.g - amount, color.b - amount, color.a);
        }
        
        public static Color Inversed (this Color color) {
            return new Color(1 - color.r, 1 - color.g, 1 - color.b, color.a);
        }
        
        public static Color Inversed (this Color color, float alpha) {
            return new Color(1 - color.r, 1 - color.g, 1 - color.b, alpha);
        }
        
        public static Color Inversed (this Color color, Color color2) {
            return new Color(color2.r - color.r, color2.g - color.g, color2.b - color.b, color.a);
        }
        
        public static Color Inversed (this Color color, Color color2, float alpha) {
            return new Color(color2.r - color.r, color2.g - color.g, color2.b - color.b, alpha);
        }
        
        public static Color Combine (this Color color, Color color2) {
            return new Color(color.r + color2.r, color.g + color2.g, color.b + color2.b, color.a);
        }
        
        public static Color Combine (this Color color, Color color2, float alpha) {
            return new Color(color.r + color2.r, color.g + color2.g, color.b + color2.b, alpha);
        }
        
        public static Color Combine(this Color color, params Color[] colors) {
            return colors.Aggregate(color, (current, c) => current.Combine(c));
        }
        
        public static Color Combine(this Color color, Color color2, float amount, float alpha) {
            return new Color(color.r + color2.r * amount, color.g + color2.g * amount, color.b + color2.b * amount, alpha);
        }
        

        #endregion

        #region Material Extensions

        public static void ChangeRenderMode(this Material standardShaderMaterial, BlendMode blendMode)
        {
            switch (blendMode)
            {
                case BlendMode.Opaque:
                    standardShaderMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                    standardShaderMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                    standardShaderMaterial.SetInt("_ZWrite", 1);
                    standardShaderMaterial.DisableKeyword("_ALPHATEST_ON");
                    standardShaderMaterial.DisableKeyword("_ALPHABLEND_ON");
                    standardShaderMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    standardShaderMaterial.renderQueue = -1;
                    break;
                case BlendMode.Cutout:
                    standardShaderMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                    standardShaderMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                    standardShaderMaterial.SetInt("_ZWrite", 1);
                    standardShaderMaterial.EnableKeyword("_ALPHATEST_ON");
                    standardShaderMaterial.DisableKeyword("_ALPHABLEND_ON");
                    standardShaderMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    standardShaderMaterial.renderQueue = 2450;
                    break;
                case BlendMode.Fade:
                    standardShaderMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                    standardShaderMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    standardShaderMaterial.SetInt("_ZWrite", 0);
                    standardShaderMaterial.DisableKeyword("_ALPHATEST_ON");
                    standardShaderMaterial.EnableKeyword("_ALPHABLEND_ON");
                    standardShaderMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    standardShaderMaterial.renderQueue = 3000;
                    break;
                case BlendMode.Transparent:
                    standardShaderMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                    standardShaderMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    standardShaderMaterial.SetInt("_ZWrite", 0);
                    standardShaderMaterial.DisableKeyword("_ALPHATEST_ON");
                    standardShaderMaterial.DisableKeyword("_ALPHABLEND_ON");
                    standardShaderMaterial.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                    standardShaderMaterial.renderQueue = 3000;
                    break;
            }
        }

        #endregion

        #region Animation Extensions
        
        public static string[] GetClipNames(this Animator animator) {
            return animator.runtimeAnimatorController.animationClips.Select(clip => clip.name).ToArray();
        }
        
        public static string[] GetClipHash(this Animator animator) {
            string[] clipHashes = animator.runtimeAnimatorController.animationClips.Select(clip => Animator.StringToHash(clip.name).ToString()).ToArray();
            return clipHashes;
        }

        public static void PlayClip(this Animator animator, string name, float crossFade = 0f, int layer = 0) {
            animator.CrossFade(name, crossFade, layer);
        }
        
        public static void PlayClip(this Animator animator, int name, float crossFade = 0f, int layer = 0) {
            animator.CrossFade(name, crossFade, layer);
        }
        
        public static void PlayClipFixedTime(this Animator animator, string name, float crossFade = 0f, int layer = 0) {
            animator.CrossFadeInFixedTime(name, crossFade, layer);
        }

#if UNITY_EDITOR
        public static void CreateAnimationEnums(this Animator animator, string suffix = "", string prefix = "") {

        }
        
        [MenuItem("Base/Tools/Editor Functions/Func1 %g")]
        private static void UseEditorFunc1() {
            IEditorShortcutUser user = null;

            if (Selection.activeGameObject != null) {
                user = Selection.activeGameObject.GetComponent<IEditorShortcutUser>();
            }
            else {
                user = Selection.activeObject as IEditorShortcutUser;
            }
            user?.Func1();
        }
        
        [MenuItem("Base/Tools/Editor Functions/Func2 %b")]
        private static void UseEditorFunc2() {
            IEditorShortcutUser user = null;

            if (Selection.activeGameObject != null) {
                user = Selection.activeGameObject.GetComponent<IEditorShortcutUser>();
            }
            else {
                user = Selection.activeObject as IEditorShortcutUser;
            }
            user?.Func2();
        }
        
        [MenuItem("Base/Tools/Editor Functions/Func3 %n")]
        private static void UseEditorFunc3() {
            IEditorShortcutUser user = null;

            if (Selection.activeGameObject != null) {
                user = Selection.activeGameObject.GetComponent<IEditorShortcutUser>();
            }
            else {
                user = Selection.activeObject as IEditorShortcutUser;
            }
            user?.Func3();
        }
        
        [MenuItem("Base/Tools/Editor Functions/Go To Prefabs %4")]
        private static void GoToPrefabs() {
            EditorUtility.FocusProjectWindow();
            string path =  "Assets/Prefabs";
 
            Object obj = AssetDatabase.LoadAssetAtPath(path, typeof(Object));

            AssetDatabase.OpenAsset(obj);
        }
#endif
        #endregion

        #region Animation Curve Extensions
        
        public static AnimationCurve ScaleCurve(this AnimationCurve curve, float maxX, float maxY)
        {
            AnimationCurve scaledCurve = new AnimationCurve();
            for (int i = 0; i < curve.keys.Length; i++)
            {
                Keyframe keyframe = curve.keys[i];
                keyframe.value = curve.keys[i].value * maxY;
                keyframe.time = curve.keys[i].time * maxX;
                keyframe.inTangent = curve.keys[i].inTangent * maxY / maxX;
                keyframe.outTangent = curve.keys[i].outTangent * maxY / maxX;
                
                scaledCurve.AddKey(keyframe);
            }
            return scaledCurve;
        }

        public static AnimationCurve NormalizeCurve(this AnimationCurve curve, float maxX, float maxY)
        {
            AnimationCurve normalizedCurve = new AnimationCurve();
            for (int i = 0; i < curve.keys.Length; i++)
            {
                Keyframe keyframe = curve.keys[i];
                keyframe.value = curve.keys[i].value / maxY;
                keyframe.time = curve.keys[i].time / maxX;
                keyframe.inTangent = curve.keys[i].inTangent / maxY * maxX;
                keyframe.outTangent = curve.keys[i].outTangent / maxY * maxX;

                normalizedCurve.AddKey(keyframe);
            }
            return normalizedCurve;
        }
        
        #endregion
        
        
    }
    
    public interface IEditorShortcutUser {
        public void Func1() {
            Debug.Log("Func1");
        }
        public void Func2() {
            Debug.Log("Func2");
        }
        public void Func3() {
            Debug.Log("Func3");
        }
    }
}
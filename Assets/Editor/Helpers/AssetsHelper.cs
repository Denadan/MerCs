using UnityEngine;
using UnityEditor;
using System.IO;

namespace Mercs.Editor
{
    public static class AssetsHelper
    {
        public static void CreateAssets<T>()
            where T : ScriptableObject
        {
            T asset = ScriptableObject.CreateInstance<T>();

            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (path == "")
            {
                path = "Assets";
            }
            else if (Path.GetExtension(path) != "")
            {
                path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
            }

            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/New " + typeof(T).ToString() + ".asset");

            AssetDatabase.CreateAsset(asset, assetPathAndName);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
        }

        [MenuItem("Assets/Create/MerCs/Faction")]
        public static void CreateFaction()
        {
            CreateAssets<Faction>();
        }

        [MenuItem("Assets/Create/MerCs/Pilot")]
        public static void CreatePilot()
        {
            CreateAssets<PilotInfo>();
        }

        [MenuItem("Assets/Create/MerCs/Merc")]
        public static void CreatMerc()
        {
            CreateAssets<MercInfo>();
        }
    }
    
}
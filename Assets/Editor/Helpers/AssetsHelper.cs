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

        public static void CreateCustomAsset<T>(T asset)
            where T : ScriptableObject
        {
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

        [MenuItem("Assets/Create/MerCs/Unit/Merc")]
        public static void CreatMerc()
        {
            var t = ScriptableObject.CreateInstance<UnitTemplate>();
            t.SetType(UnitType.MerC);
            CreateCustomAsset(t);
        }
        [MenuItem("Assets/Create/MerCs/Unit/Tank")]
        public static void CreatTank()
        {
            var t = ScriptableObject.CreateInstance<UnitTemplate>();
            t.SetType(UnitType.Tank);
            CreateCustomAsset(t);
        }
        [MenuItem("Assets/Create/MerCs/Unit/Turret")]
        public static void CreatTurret()
        {
            var t = ScriptableObject.CreateInstance<UnitTemplate>();
            t.SetType(UnitType.Turret);
            CreateCustomAsset(t);
        }
        [MenuItem("Assets/Create/MerCs/Unit/Vehicle")]
        public static void CreatVehicle()
        {
            var t = ScriptableObject.CreateInstance<UnitTemplate>();
            t.SetType(UnitType.Vehicle);
            CreateCustomAsset(t);
        }


        [MenuItem("Assets/Create/MerCs/Module/WeaponTemplate")]
        public static void CreateWeaponTemplate()
        {

            CreateAssets<WeaponTemplate>();
        }
        [MenuItem("Assets/Create/MerCs/Module/Weapon")]
        public static void CreateWeapon()
        {
            CreateAssets<Weapon>();
        }
    }

}
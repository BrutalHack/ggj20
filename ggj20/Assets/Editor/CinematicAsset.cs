using com.BrutalHack.GlobalGameJam20.DataModels;
using UnityEditor;

namespace com.BrutalHack.GlobalGameJam20.Editor
{
    public class CinematicAsset
    {
        [MenuItem("Assets/Create/DataModels/CinematicModel")]
        public static void CreateAsset()
        {
            ScriptableObjectUtility.CreateAsset<CinematicModel>();
        } 
    }
}
using UnityEngine;

namespace com.BrutalHack.GlobalGameJam20.DataModels
{
    public class CinematicModel : ScriptableObject
    {
        [FMODUnity.EventRef] public string[] sounds;

        public string[] texts;

        public void OnValidate()
        {
            if (sounds.Length != texts.Length)
            {
                Debug.LogError(
                    $"sounds and texts size mismatch: sounds size: {sounds.Length}; texts size: {sounds.Length}", this);
            }
        }
    }
}
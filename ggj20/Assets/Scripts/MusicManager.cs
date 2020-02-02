using System;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace com.BrutalHack.GlobalGameJam20
{
    public class MusicManager : MonoBehaviour
    {
        [EventRef] [SerializeField] private string ambientMusicEventName;

        [EventRef] [SerializeField] private string acceptanceMusicEventName;
        private EventInstance eventInstance;
        private const string VoiceParameterName = "Voice";

        // Start is called before the first frame update
        private void Start()
        {
            eventInstance = RuntimeManager.CreateInstance(ambientMusicEventName);

            eventInstance.start();
        }

        public void SetVoice(bool active)
        {
            eventInstance.setParameterByName(VoiceParameterName, Convert.ToInt32(active));
        }
    }
}
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
        private EventInstance acceptanceMusicInstance;
        private const string VoiceParameterName = "Voice";
        private const string AngerDistParameterName = "Zorn Dist";
        private const string DepressionDistParameterName = "Depr Dist";
        private const string DepressionEndParameterName = "Depr End";

        // Start is called before the first frame update
        private void Start()
        {
            eventInstance = RuntimeManager.CreateInstance(ambientMusicEventName);
            acceptanceMusicInstance = RuntimeManager.CreateInstance(acceptanceMusicEventName);

            eventInstance.start();
        }

        public void SetVoice(bool active)
        {
            eventInstance.setParameterByName(VoiceParameterName, Convert.ToInt32(active));
        }

        public void SetAnger(bool active)
        {
            eventInstance.setParameterByName(VoiceParameterName, Convert.ToInt32(active));
        }
        
        public void SetDepression(bool active)
        {
            eventInstance.setParameterByName(VoiceParameterName, Convert.ToInt32(active));
        }
        
        public void SetDepressionEnd(bool active)
        {
            eventInstance.setParameterByName(VoiceParameterName, Convert.ToInt32(active));
        }

        public void PlayEndMusic()
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            acceptanceMusicInstance.start();
        }
    }
}
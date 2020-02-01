using System;
using System.Threading.Tasks;
using com.BrutalHack.GlobalGameJam20.DataModels;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace com.BrutalHack.GlobalGameJam20
{
    public class Cinematic : MonoBehaviour
    {
        [SerializeField] private CinematicModel model;
        [SerializeField] private int nextLinePosition;
        private CinematicUiController cinematicUiController;
        private bool isPlaying;
        private EventInstance currentEvent;

        // Start is called before the first frame update
        async void Start()
        {
            cinematicUiController =
                GameObject.FindWithTag("CinematicUiController").GetComponent<CinematicUiController>();

            cinematicUiController.Show();
            await Task.Delay(TimeSpan.FromSeconds(0.5));
            currentEvent = StartNextLine();
        }

        // Update is called once per frame
        void Update()
        {
            if (isPlaying)
            {
                HandleCinematic();
            }
        }

        private void HandleCinematic()
        {
            currentEvent.getPlaybackState(out var result);
            if (result == PLAYBACK_STATE.STOPPED && nextLinePosition == model.texts.Length)
            {
                Debug.Log($"Cinematic {model.name} is complete. LinePosition: {nextLinePosition}");
                cinematicUiController.Hide();
                isPlaying = false;
            }
            else if (result == PLAYBACK_STATE.STOPPED)
            {
                currentEvent = StartNextLine();
            }
        }

        private EventInstance StartNextLine()
        {
            if (nextLinePosition < model.texts.Length)
            {
                var eventInstance = RuntimeManager.CreateInstance(model.sounds[nextLinePosition]);
                eventInstance.start();
                cinematicUiController.ShowLine(model.texts[nextLinePosition]);
                isPlaying = true;
                nextLinePosition++;
                return eventInstance;
            }

            throw new InvalidOperationException(
                $"lineposition {nextLinePosition} is greater than {model.name} length {model.sounds.Length}");
        }
    }
}
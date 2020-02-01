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
        [SerializeField] private PlayerMovement playerMovement;
        [SerializeField] private Rigidbody2D playerRigidBody;
        private CinematicUiController cinematicUiController;
        private bool isPlaying;
        private EventInstance currentEvent;
        private const double CinematicStartDelay = 1.5;
        private const double CinematicEndDelay = 0.5;

        // Start is called before the first frame update
        async void Start()
        {
            cinematicUiController =
                GameObject.FindWithTag("CinematicUiController").GetComponent<CinematicUiController>();
            playerMovement = FindObjectOfType<PlayerMovement>();
            playerRigidBody = playerMovement.GetComponent<Rigidbody2D>();
        }

        async Task PlayCinematic()
        {
            cinematicUiController.Show();
            playerMovement.enabled = false;
            playerRigidBody.velocity = Vector2.zero;
            await Task.Delay(TimeSpan.FromSeconds(CinematicStartDelay));
            currentEvent = StartNextLine();
        }

        // Update is called once per frame
        private async Task FinishCinematic()
        {
            Debug.Log($"Cinematic {model.name} is complete. LinePosition: {nextLinePosition}");
            cinematicUiController.Hide();
            isPlaying = false;
            await Task.Delay(TimeSpan.FromSeconds(CinematicEndDelay));
            playerMovement.enabled = true;
        }

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
                FinishCinematic();
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
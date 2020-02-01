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
        private const double cinematicStartDelay = 2.0;
        private const double cinematicEndDelay = 1.0;

        public delegate void AfterCinematicFinished();

        public AfterCinematicFinished onCinematicFinishedEvent;
        private MusicManager musicManager;

        // Start is called before the first frame update
        private async void Start()
        {
            cinematicUiController =
                GameObject.FindWithTag("CinematicUiController").GetComponent<CinematicUiController>();
            playerMovement = FindObjectOfType<PlayerMovement>();
            playerRigidBody = playerMovement.GetComponent<Rigidbody2D>();
            musicManager = FindObjectOfType<MusicManager>();

            // await Task.Delay(TimeSpan.FromSeconds(5));
            // await PlayCinematicAsync();
        }

        // Update is called once per frame
        public async Task PlayCinematicAsync()
        {
            musicManager.SetVoice(true);
            cinematicUiController.Show();
            playerMovement.enabled = false;
            playerRigidBody.velocity = Vector2.zero;
            await Task.Delay(TimeSpan.FromSeconds(cinematicStartDelay));
            currentEvent = StartNextLine();
        }

        private async Task FinishCinematic()
        {
            musicManager.SetVoice(false);
            Debug.Log($"Cinematic {model.name} is complete. LinePosition: {nextLinePosition}");
            cinematicUiController.Hide();
            isPlaying = false;
            await Task.Delay(TimeSpan.FromSeconds(cinematicEndDelay));
            playerMovement.enabled = true;
            onCinematicFinishedEvent?.Invoke();
        }

        async void Update()
        {
            if (isPlaying)
            {
                await HandleCinematic();
            }
        }

        private async Task HandleCinematic()
        {
            currentEvent.getPlaybackState(out var result);
            if (result == PLAYBACK_STATE.STOPPED && nextLinePosition == model.texts.Length)
            {
                await FinishCinematic();
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
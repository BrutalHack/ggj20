using System;
using System.Threading.Tasks;
using com.BrutalHack.GlobalGameJam20.DataModels;
using FMOD.Studio;
using FMODUnity;
using Pathfinding;
using UnityEngine;

namespace com.BrutalHack.GlobalGameJam20
{
    public class Cinematic : MonoBehaviour
    {
        [SerializeField] private CinematicModel model;
        [SerializeField] private int nextLinePosition;
        private PlayerMovement playerMovement;
        private Rigidbody2D playerRigidBody;
        private AIPath playerAiPath;
        private CinematicUiController cinematicUiController;
        private bool isPlaying;
        private EventInstance currentEvent;
        private const double cinematicStartDelay = 2.0;
        private const double cinematicEndDelay = 1.0;

        public delegate void AfterCinematicFinished();

        public AfterCinematicFinished onCinematicFinishedEvent;
        private MusicManager musicManager;
        private static readonly int Intro = Animator.StringToHash("Intro");
        private static readonly int Outro = Animator.StringToHash("Outro");
        private static readonly int Sun = Animator.StringToHash("Sun");
        private Animator skyAnimator;
        private bool startNextLine;
        [SerializeField] private bool isDone;

        private void Awake()
        {
            cinematicUiController =
                GameObject.FindWithTag("CinematicUiController").GetComponent<CinematicUiController>();
            playerMovement = FindObjectOfType<PlayerMovement>();
            playerRigidBody = playerMovement.GetComponent<Rigidbody2D>();
            playerAiPath = playerMovement.GetComponent<AIPath>();
            musicManager = FindObjectOfType<MusicManager>();
            skyAnimator = GameObject.FindWithTag("Sky").GetComponent<Animator>();
        }

        // Update is called once per frame
        public async Task PlayCinematicAsync()
        {
            playerMovement.ForceIdle();
            musicManager.SetVoice(true);
            cinematicUiController.Show();
            playerMovement.enabled = false;
            playerRigidBody.velocity = Vector2.zero;
            await Task.Delay(TimeSpan.FromSeconds(cinematicStartDelay));
            isPlaying = true;
        }

        private async Task FinishCinematic()
        {
            musicManager.SetVoice(false);
            Debug.Log($"Cinematic {model.name} is complete. LinePosition: {nextLinePosition}");
            cinematicUiController.Hide();
            isDone = true;
            isPlaying = false;
            await Task.Delay(TimeSpan.FromSeconds(cinematicEndDelay));
            playerMovement.enabled = true;

            onCinematicFinishedEvent?.Invoke();
        }

        private async void Update()
        {
            Debug.Log("Update IsPlaying: " + isPlaying);
            if (!isPlaying || isDone)
            {
                return;
            }

            await HandleCinematic();
        }

        private async Task HandleCinematic()
        {
            currentEvent.getPlaybackState(out var result);
            Debug.Log(result);
            if (result == PLAYBACK_STATE.STOPPED && nextLinePosition == model.texts.Length)
            {
                await FinishCinematic();
            }
            else if (result == PLAYBACK_STATE.STOPPED)
            {
                currentEvent = await StartNextLine();
            }
        }

        private async Task<EventInstance> StartNextLine()
        {
            if (nextLinePosition < model.texts.Length)
            {
                isPlaying = false;
                Debug.Log("IsPlaying: " + isPlaying);
                Debug.Log("Line: " + model.texts[nextLinePosition]);
                if (model.texts[nextLinePosition].Equals("CMD:Center", StringComparison.OrdinalIgnoreCase))
                {
                    var aStarTarget = GameObject.FindWithTag("AStarTarget").transform;
                    aStarTarget.position = new Vector3(0, -3, 0);
                    playerAiPath.enabled = true;
                    await Task.Delay(TimeSpan.FromSeconds(3.0));
                    playerAiPath.enabled = false;
                    isPlaying = true;
                    nextLinePosition++;
                    return new EventInstance();
                }

                if (model.texts[nextLinePosition].Equals("CMD:Sun", StringComparison.OrdinalIgnoreCase))
                {
                    isPlaying = false;
                    skyAnimator.ResetTrigger(Intro);
                    skyAnimator.ResetTrigger(Outro);
                    skyAnimator.SetTrigger(Sun);
                    isPlaying = true;
                    nextLinePosition++;
                    return new EventInstance();
                }

                if (model.texts[nextLinePosition].Equals("CMD:Blendin", StringComparison.OrdinalIgnoreCase))
                {
                    isPlaying = false;
                    skyAnimator.SetTrigger(Intro);
                    skyAnimator.ResetTrigger(Outro);
                    skyAnimator.ResetTrigger(Sun);
                    await Task.Delay(TimeSpan.FromSeconds(1.0));
                    isPlaying = true;
                    nextLinePosition++;
                    return new EventInstance();
                }

                if (model.texts[nextLinePosition].Equals("CMD:Outro", StringComparison.OrdinalIgnoreCase))
                {
                    isPlaying = false;
                    skyAnimator.ResetTrigger(Intro);
                    skyAnimator.SetTrigger(Outro);
                    skyAnimator.ResetTrigger(Sun);
                    await Task.Delay(TimeSpan.FromSeconds(4.0));
                    //TODO Roll credits
                    //Remain off
                    isPlaying = false;
                    nextLinePosition++;
                    return new EventInstance();
                }

                var eventInstance = RuntimeManager.CreateInstance(model.sounds[nextLinePosition]);
                eventInstance.start();
                cinematicUiController.ShowLine(model.texts[nextLinePosition]);
                Debug.Log("Played Audio");
                isPlaying = true;
                nextLinePosition++;
                return eventInstance;
            }

            throw new InvalidOperationException("Called while not ready " + nextLinePosition);
        }
    }
}
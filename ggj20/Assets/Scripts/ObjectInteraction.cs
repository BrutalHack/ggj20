using System;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace com.BrutalHack.GlobalGameJam20
{
    [RequireComponent(typeof(Cinematic))]
    public class ObjectInteraction : MonoBehaviour
    {
        [FMODUnity.EventRef] public string wanderingWhispers;
        public Sprite sprite;
        public ObjectEnum objectEnum;
        [HideInInspector] public bool done;
        private Cinematic _cinematic;
        private InteractionManager _interactionManager;
        private SpriteRenderer _spriteRenderer;
        private EventInstance eventInstance;

        private void Awake()
        {
            _cinematic = GetComponent<Cinematic>();
            _interactionManager = GameObject.FindWithTag("InteractionManager").GetComponent<InteractionManager>();
            _spriteRenderer = GetComponentInParent<SpriteRenderer>();
            if (wanderingWhispers.Length > 0)
            {
                eventInstance = RuntimeManager.CreateInstance(wanderingWhispers);
            }

            var child = GetComponentInChildren<ObjectProximity>();
            if (child != null)
            {
                child.objectEnum = objectEnum;
            }
            else
            {
                Debug.LogError(
                    $"{nameof(ObjectInteraction)} {objectEnum} has no child with component {typeof(ObjectProximity)}");
            }
        }

        public void UpdateParentSprite()
        {
            _spriteRenderer.sprite = sprite;
            if (wanderingWhispers.Length > 0)
            {
                eventInstance.start();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            OnInteraction();
        }

        private async void OnInteraction()
        {
            if (wanderingWhispers.Length > 0)
            {
                eventInstance.setParameterByName("Cinematic", 1);
            }

            var musicManager = FindObjectOfType<MusicManager>();

            done = true;
            switch (objectEnum)
            {
                case ObjectEnum.DollLight:
                    break;
                case ObjectEnum.DollDark:
                    musicManager.SetAnger(true);
                    break;
                case ObjectEnum.SandboxLight:
                    break;
                case ObjectEnum.SandboxDark:
                    break;
                case ObjectEnum.PuddleLight:
                    break;
                case ObjectEnum.PuddleDark:
                    break;
                case ObjectEnum.BenchLight:
                    break;
                case ObjectEnum.BenchDark:
                    musicManager.SetDepression(true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            _cinematic.onCinematicFinishedEvent += AfterInteraction;
            await _cinematic.PlayCinematicAsync();
        }

        private void AfterInteraction()
        {
            var musicManager = FindObjectOfType<MusicManager>();

            switch (objectEnum)
            {
                case ObjectEnum.DollLight:
                    break;
                case ObjectEnum.DollDark:
                    musicManager.SetAnger(false);
                    break;
                case ObjectEnum.SandboxLight:
                    break;
                case ObjectEnum.SandboxDark:
                    break;
                case ObjectEnum.PuddleLight:
                    break;
                case ObjectEnum.PuddleDark:
                    break;
                case ObjectEnum.BenchLight:
                    break;
                case ObjectEnum.BenchDark:
                    musicManager.SetDepression(false);
                    musicManager.SetDepressionEnd(false);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            musicManager.PlayEndMusic();
            _cinematic.onCinematicFinishedEvent -= AfterInteraction;
            gameObject.SetActive(false);
            _interactionManager.NextPhase();
        }
    }
}
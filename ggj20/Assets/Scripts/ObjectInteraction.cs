﻿using System;
using UnityEngine;

namespace com.BrutalHack.GlobalGameJam20
{
    [RequireComponent(typeof(Cinematic))]
    public class ObjectInteraction : MonoBehaviour
    {
        public bool done;
        private Cinematic _cinematic;
        private InteractionManager _interactionManager;

        private void Awake()
        {
            _cinematic = GetComponent<Cinematic>();
            _interactionManager = GameObject.FindWithTag("InteractionManager").GetComponent<InteractionManager>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            OnInteraction();
        }

        private async void OnInteraction()
        {
            gameObject.SetActive(false);
            done = true;

            _cinematic.onCinematicFinishedEvent += AfterInteraction;
            await _cinematic.PlayCinematicAsync();
        }

        private void AfterInteraction()
        {
            _cinematic.onCinematicFinishedEvent -= AfterInteraction;
            _interactionManager.NextPhase();
        }
    }
}

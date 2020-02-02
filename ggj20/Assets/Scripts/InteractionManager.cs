using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace com.BrutalHack.GlobalGameJam20
{
    public class InteractionManager : MonoBehaviour
    {
        [SerializeField] private Cinematic introCinematic;
        [SerializeField] private Cinematic outroCinematic;
        public List<ObjectInteraction> darkObjects;
        public List<ObjectInteraction> lightObjects;
        private int _nextDarkPhaseCounter = 0;
        private bool _lightPhase = false;

        private async void Awake()
        {
            if (darkObjects.Any())
            {
                darkObjects.ForEach(o => o.gameObject.SetActive(false));
            }
            else
            {
                Debug.LogError($"Dark objects are missing in {nameof(InteractionManager)}");
            }

            if (lightObjects.Any())
            {
                lightObjects.ForEach(o => o.gameObject.SetActive(false));
            }
            else
            {
                Debug.LogError($"Light objects are missing in {nameof(InteractionManager)}");
            }

            introCinematic.onCinematicFinishedEvent += FirstPhase;
            await Task.Delay(TimeSpan.FromSeconds(0.2));
            await introCinematic.PlayCinematicAsync();
        }

        private void FirstPhase()
        {
            darkObjects[_nextDarkPhaseCounter].gameObject.SetActive(true);
            darkObjects[_nextDarkPhaseCounter].UpdateParentSprite();
            _nextDarkPhaseCounter++;
        }

        public async Task NextPhase()
        {
            if (_lightPhase)
            {
                await EvaluateWinCondition();
                return;
            }

            if (_nextDarkPhaseCounter < darkObjects.Count)
            {
                darkObjects[_nextDarkPhaseCounter].gameObject.SetActive(true);
                darkObjects[_nextDarkPhaseCounter].UpdateParentSprite();
                _nextDarkPhaseCounter++;
                return;
            }

            _lightPhase = true;
            lightObjects.ForEach(o => o.gameObject.SetActive(true));
            lightObjects.ForEach(o => o.UpdateParentSprite());
        }

        private async Task EvaluateWinCondition()
        {
            if (lightObjects.Exists(o => !o.done))
            {
                return;
            }

            outroCinematic.onCinematicFinishedEvent += OutroFinished;
            await outroCinematic.PlayCinematicAsync();
        }

        private void OutroFinished()
        {
            
        }
    }
}
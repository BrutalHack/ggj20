using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace com.BrutalHack.GlobalGameJam20
{
    public class InteractionManager : MonoBehaviour
    {
        public List<ObjectInteraction> darkObjects;
        public List<ObjectInteraction> lightObjects;
        private int _darkPhaseCounter = 0;
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
            await NextPhase();
        }

        public async Task NextPhase()
        {
            if (_lightPhase)
            {
                await EvaluateWinCondition();
                return;
            }

            if (_darkPhaseCounter < darkObjects.Count)
            {
                darkObjects[_darkPhaseCounter].gameObject.SetActive(true);
                _darkPhaseCounter++;
                return;
            }

            _lightPhase = true;
            lightObjects.ForEach(o => o.gameObject.SetActive(true));
        }

        private async Task EvaluateWinCondition()
        {
            if (lightObjects.Exists(o => !o.done))
            {
                return;
            }

            var cinematic = GetComponent<Cinematic>();
            cinematic.onCinematicFinishedEvent += OutroFinished;
            await cinematic.PlayCinematicAsync();

        }

        private void OutroFinished()
        {
            //TODO Make Outro here
        }
    }
}
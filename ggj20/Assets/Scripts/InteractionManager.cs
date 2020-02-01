using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace com.BrutalHack.GlobalGameJam20
{
    public class InteractionManager : MonoBehaviour
    {
        public List<ObjectInteraction> darkObjects;
        public List<ObjectInteraction> lightObjects;
        private int _darkPhaseCounter = 0;
        private bool _lightPhase = false;

        private void Awake()
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
            NextPhase();
        }

        public void NextPhase()
        {
            if (_lightPhase)
            {
                EvaluateWinCondition();
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

        private void EvaluateWinCondition()
        {
            if (lightObjects.Exists(o => !o.done))
            {
                return;
            }

            Debug.Log("Win logic !!!");
        }
    }
}
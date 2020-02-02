using UnityEngine;

namespace com.BrutalHack.GlobalGameJam20
{
    [RequireComponent(typeof(Cinematic))]
    public class ObjectInteraction : MonoBehaviour
    {
        public Sprite sprite;
        public ObjectEnum objectEnum;
        [HideInInspector] public bool done;
        private Cinematic _cinematic;
        private InteractionManager _interactionManager;
        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _cinematic = GetComponent<Cinematic>();
            _interactionManager = GameObject.FindWithTag("InteractionManager").GetComponent<InteractionManager>();
            _spriteRenderer = GetComponentInParent<SpriteRenderer>();

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
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            OnInteraction();
        }

        private async void OnInteraction()
        {
            done = true;
            _cinematic.onCinematicFinishedEvent += AfterInteraction;
            await _cinematic.PlayCinematicAsync();
        }

        private void AfterInteraction()
        {
            _cinematic.onCinematicFinishedEvent -= AfterInteraction;
            gameObject.SetActive(false);
            _interactionManager.NextPhase();
        }
    }
}
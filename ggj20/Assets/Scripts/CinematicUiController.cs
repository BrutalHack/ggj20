using TMPro;
using UnityEngine;

namespace com.BrutalHack.GlobalGameJam20
{
    public class CinematicUiController : MonoBehaviour
    {
        [SerializeField] private TMP_Text tmpText;
        private Animator animator;
        private static readonly int ShowTrigger = Animator.StringToHash("show");
        private static readonly int HideTrigger = Animator.StringToHash("hide");

        // Start is called before the first frame update
        void Awake()
        {
            animator = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void Show()
        {
            animator.ResetTrigger(HideTrigger);
            animator.SetTrigger(ShowTrigger);
        }

        public void Hide()
        {
            animator.ResetTrigger(ShowTrigger);
            animator.SetTrigger(HideTrigger);
        }

        public void ShowLine(string modelText)
        {
            tmpText.text = modelText;
        }
    }
}
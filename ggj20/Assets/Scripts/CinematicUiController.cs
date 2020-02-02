using Cinemachine;
using TMPro;
using UnityEngine;
using static System.String;

namespace com.BrutalHack.GlobalGameJam20
{
    public class CinematicUiController : MonoBehaviour
    {
        [SerializeField] private TMP_Text tmpText;
        [SerializeField] private Animator cinemachineAnimator;
        private Animator animator;
        private static readonly int ShowTrigger = Animator.StringToHash("show");
        private static readonly int HideTrigger = Animator.StringToHash("hide");
        private static readonly int ZoomOut = Animator.StringToHash("ZoomOut");
        private static readonly int ZoomIn = Animator.StringToHash("ZoomIn");

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
            cinemachineAnimator.ResetTrigger(ZoomOut);
            cinemachineAnimator.SetTrigger(ZoomIn);
        }

        public void Hide()
        {
            tmpText.text = Empty;
            animator.ResetTrigger(ShowTrigger);
            animator.SetTrigger(HideTrigger);
            cinemachineAnimator.ResetTrigger(ZoomIn);
            cinemachineAnimator.SetTrigger(ZoomOut);
        }

        public void ShowLine(string modelText)
        {
            tmpText.text = modelText;
        }
    }
}
using UnityEngine;
using UnityEngine.UI;

namespace UIScripts
{
    public class UIViewUpdateManager : MonoBehaviour
    {
        [SerializeField] private Slider timerSlider;
        [SerializeField] private GameObject targetObject;

        private void Start()
        {
            var hoverInfo = new UIEffects.HoverFloatInfo(1.2f, 1.6f, 20f, 100f);
            var shakeInfo = new UIEffects.RotateShakeInfo(5f,0.25f,0.4f);
            var pulseInfo = new UIEffects.PulseInOutInfo(0.12f, 0.4f, 1.5f, 3);
            
            UIEffects.RotateShakeEffect.StartRotateShake(targetObject,shakeInfo);
            UIEffects.PulseInOutEffect.StartPulsingInOut(targetObject,pulseInfo);
            UIEffects.HoverFloatEffect.StartHoverFloat(targetObject, hoverInfo);
        }
        
        private void InitializeTimer(float maxDuration)
        {
            timerSlider.maxValue = maxDuration;
            timerSlider.value = maxDuration;
        }

        private void SetTimer(float duration)
        {
            timerSlider.value = duration;
        }
    }
}

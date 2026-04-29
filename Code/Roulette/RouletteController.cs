using System.Collections;
using UnityEngine;

namespace CIW.Code.Roulette
{
    public class RouletteController : MonoBehaviour
    {
        [SerializeField] RouletteSystem rouletteSystem;
        [SerializeField] RectTransform wheelTransform;

        [SerializeField] float angleOffset = 157.5f; 

        bool _isSpin = false;

        public void OnSpinButtonClicked()
        {
            if (_isSpin) return;

            RouletteReward result = rouletteSystem.Spin(out int targetIndex);
            StartCoroutine(SpinRoutine(targetIndex, result));
        }

        private IEnumerator SpinRoutine(int targetIndex, RouletteReward reward)
        {
            _isSpin = true;
    
            float segmentAngle = 360f / 8f;
            
            float targetAngle = (targetIndex * segmentAngle) + (segmentAngle / 2f); 
            float fullSpins = 360f * 5f; 
            float finalTargetRotation = -(fullSpins + targetAngle + angleOffset);
    
            float duration = 3f;
            float elapsed = 0f;
            float startAngle = wheelTransform.eulerAngles.z;
    
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;
                float curve = 1f - Mathf.Pow(1f - t, 3f); 
    
                float currentAngle = Mathf.Lerp(startAngle, finalTargetRotation, curve);
                wheelTransform.rotation = Quaternion.Euler(0, 0, currentAngle);
                yield return null;
            }
    
            wheelTransform.rotation = Quaternion.Euler(0, 0, finalTargetRotation % 360f);
            _isSpin = false;
            rouletteSystem.GiveReward(reward);
        }
    }
}


using UnityEngine;
using UnityEngine.UI;

namespace CIW.Code.Roulette
{
    public class RouletteSetup : MonoBehaviour
    {
        [SerializeField] RouletteSystem system;
        [SerializeField] GameObject iconPrefab;
        [SerializeField] Transform iconsParent;
        [SerializeField] float distanceFromCenter = 150f;

        private void Start()
        {
            SetupWheel();
        }

        [ContextMenu("Setup Wheel")]
        private void SetupWheel()
        {
            foreach (Transform child in iconsParent) {
                DestroyImmediate(child.gameObject);
            }

            var rewards = system.RouletteTable.rewards;
            int count = rewards.Count;
            float angleStep = 360f / count;

            float startAngle = 225f;

            for (int i = 0; i < count; i++)
            {
                GameObject iconGo = Instantiate(iconPrefab, iconsParent);
                iconGo.name = $"Icon_{i}";

                float currentAngle = startAngle + (i * angleStep);
                float radian = currentAngle * Mathf.Deg2Rad; 
                float x = Mathf.Cos(radian) * distanceFromCenter;
                float y = Mathf.Sin(radian) * distanceFromCenter;

                RectTransform rect = iconGo.GetComponent<RectTransform>();
                rect.anchoredPosition = new Vector2(x, y);

                iconGo.transform.rotation = Quaternion.identity;

                if (iconGo.TryGetComponent(out Image img)) {
                    img.raycastTarget = false; 
                    if (rewards[i].itemData != null)
                        img.sprite = rewards[i].itemData.visualData.icon;
                }
            }
        }
    }
}

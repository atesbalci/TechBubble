using UnityEngine;

namespace TechBubble.Views
{
    public class DeadlineScreenIndicator : MonoBehaviour
    {
        [SerializeField] private RectTransform arrow;
        
        public RectTransform RectTransform => (RectTransform) transform;

        public void RefreshViewportPosition(Vector2 viewportPosition)
        {
            float rotation;
            if (viewportPosition.x < 0.001f)
            {
                rotation = 90f;
            }
            else if (viewportPosition.x > 0.999f)
            {
                rotation = 270f;
            }
            else if (viewportPosition.y < 0.001f)
            {
                rotation = 180f;
            }
            else
            {
                rotation = 0f;
            }
            
            arrow.localEulerAngles = new Vector3(0f, 0f, rotation);
        }
    }
}
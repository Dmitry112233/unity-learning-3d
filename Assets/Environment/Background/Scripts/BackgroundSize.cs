using Camera.Scripts;
using UnityEngine;

namespace Environment.Background.Scripts
{
    public class BackgroundSize : MonoBehaviour
    {
        private void Start()
        {
            InitializeBounds();
        }

        private void InitializeBounds()
        {
            var bounds = CameraBounds.Instance;
            float width = bounds.Width;
            float height = bounds.Height;
            
            float quadHeight = width * 2 > height ? width * 2 : height;

            transform.localScale = new Vector3(width, quadHeight, 1);
        }
    }
}
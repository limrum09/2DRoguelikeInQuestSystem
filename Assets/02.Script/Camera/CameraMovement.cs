using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Completed
{
    public class CameraMovement : MonoBehaviour
    {
        private Vector3 velocity = Vector3.zero;
        private ExternalCallPlayer instance;
        private BoxCollider2D boardCollider;
        private Camera camera;

        private float xMin;
        private float xMax;
        private float yMin;
        private float yMax;
        private float camX;
        private float camY;
        private float camOrthsize;
        private float cameraRatio;

        [SerializeField]
        private float dampTime = 1.0f;
        // Start is called before the first frame update
        void Start()
        {
            instance = ExternalCallPlayer.Instance;

            boardCollider = GameManager.instance.BoardCollider;

            xMin = boardCollider.bounds.min.x;
            xMax = boardCollider.bounds.max.x;
            yMin = boardCollider.bounds.min.x;
            yMax = boardCollider.bounds.max.y;

            camera = GetComponent<Camera>();
        }

        private void Update()
        {
            Vector2 playerPos = instance.CurrentPlayerPos;
            CameraPos(playerPos);
        }

        private void CameraPos(Vector2 pos) => CameraPos(pos.x, pos.y);

        private void CameraPos(float posX, float posY)
        {
            if (camera == null)
            {
                Debug.LogError("No Camera"); 
                return;
            }
                
            if (boardCollider == null)
            {
                Debug.LogError("No Board's Collider"); 
                return;
            }

            camX = Mathf.Clamp(posX, xMin + 9.14f, xMax - 9.14f);
            camY = Mathf.Clamp(posY, yMin + 5.0f, yMax - 5.0f);
            Vector3 destination = new Vector3(camX, camY, this.transform.position.z);

            this.transform.position = Vector3.Lerp(this.transform.position, destination, dampTime);
        }
    }
}

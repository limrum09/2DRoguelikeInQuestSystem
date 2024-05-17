using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Completed
{
    public class ExternalCallPlayer : MonoBehaviour
    {
        #region Event
        public delegate void PlayerMoveHandler(float posX, float posY);
        #endregion

        [SerializeField]
        private Player player;

        private static ExternalCallPlayer instance;
        public static ExternalCallPlayer Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<ExternalCallPlayer>();
                }

                return instance;
            }
        }

        public float posX;
        public float posY;

        public Vector2 CurrentPlayerPos
        {
            get
            {
                posX = this.transform.position.x;
                posY = this.transform.position.y;

                return new Vector2(posX, posY);
            }
            set 
            {
                onPlayerMoved?.Invoke(this.transform.position.x, this.transform.position.y);
            }
        }

        public event PlayerMoveHandler onPlayerMoved;


        private void Start()
        {
            
        }
        public void GetReward(int rewardPoint)
        {
            player.GetReward(rewardPoint);
        }

        private void PlayerMoving(int posX, int posY) => onPlayerMoved?.Invoke(posX, posY);
    }
}

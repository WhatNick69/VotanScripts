using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerBehaviour;

namespace VotanLibraries
{
	public class LibraryPlayerPosition 
        : MonoBehaviour
	{
		[SerializeField]
        private static GameObject player;
        private static Transform mainCameraPlayerTransform;
		private static PlayerConditions playerConditions;
        private static Transform playerPoint, playerRightPoint,
			playerLeftPoint, playerFacePoint, playerBackPoint; // точки персонажа
        public static System.Random rnd = new System.Random();

        public static PlayerConditions PlayerConditions
        {
            get
            {
                return playerConditions;
            }

            set
            {
                playerConditions = value;
            }
        }

        public static Transform MainCameraPlayerTransform
        {
            get
            {
                return mainCameraPlayerTransform;
            }

            set
            {
                mainCameraPlayerTransform = value;
            }
        }

        public static Vector3 GetPlayerPoint(int index) // возвращает точки персонажа игрока
		{
            switch (index)
            {
                case 0:
                    return playerRightPoint.position;
                case 1:
                    return playerLeftPoint.position;
                case 2:
                    return playerFacePoint.position;
                case 3:
                    return playerBackPoint.position;
            }
			return Vector3.zero;
		}

		private void Start()
		{
			player = GameObject.FindWithTag("Player");
            mainCameraPlayerTransform = GameObject.FindWithTag("MainCamera").transform;
            playerRightPoint = player.GetComponent<PlayerAttack>().PlayerPosition(0);
			playerLeftPoint = player.GetComponent<PlayerAttack>().PlayerPosition(1);
			playerFacePoint = player.GetComponent<PlayerAttack>().PlayerPosition(2);
			playerBackPoint = player.GetComponent<PlayerAttack>().PlayerPosition(3);
			playerConditions = player.GetComponent<PlayerConditions>();
		}
	}

}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerBehaviour;

namespace VotanLibraries
{
	public class LibraryPlayerPosition : MonoBehaviour
	{
		[SerializeField]
		GameObject player;
		public static PlayerConditions playerConditions;
		protected static Transform playerPoint, playerRightPoint,
			playerLeftPoint, playerFacePoint, playerBackPoint; // точки персонажа

		

		public static Vector3 GetPlayerPoint(int index) // возвращает точки персонажа игрока
		{
			if (index == 0) return playerRightPoint.position;
			if (index == 1) return playerLeftPoint.position;
			if (index == 3) return playerFacePoint.position;
			if (index == 4) return playerBackPoint.position;
			return Vector3.zero;
		}

		private void Start()
		{
			player = GameObject.FindWithTag("Player");
			playerRightPoint = player.GetComponent<PlayerAttack>().PlayerPosition(0);
			playerLeftPoint = player.GetComponent<PlayerAttack>().PlayerPosition(1);
			playerFacePoint = player.GetComponent<PlayerAttack>().PlayerPosition(2);
			playerBackPoint = player.GetComponent<PlayerAttack>().PlayerPosition(3);
			playerConditions = player.GetComponent<PlayerConditions>();
		}
	}

}


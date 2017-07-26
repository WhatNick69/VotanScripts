using AbstractBehaviour;
using PlayerBehaviour;
using System.Collections.Generic;
using UnityEngine;

namespace VotanGameplay
{
    /// <summary>
    /// Хранит в себе всех игроков
    /// </summary>
    public class AllPlayerManager
        : MonoBehaviour
    {
        private static bool isGameOver;

        /// <summary>
        /// Лист с игроками
        /// </summary>
        private static List<GameObject> playerList;

        /// <summary>
        /// Свойство для получения ссылки на лист с игроками
        /// </summary>
        public static List<GameObject> PlayerList
        {
            get
            {
                return playerList;
            }

            set
            {
                playerList = value;
            }
        }

        public static bool IsGameOver
        {
            get
            {
                return isGameOver;
            }

            set
            {
                isGameOver = value;
            }
        }

        private void Awake()
        {
            playerList = new List<GameObject>();
            isGameOver = false;
        }

        /// <summary>
        /// Добавить игрока в лист
        /// </summary>
        /// <param name="player"></param>
        public static void AddPlayerToPlayerList(GameObject player)
        {
            playerList.Add(player);
        }

        //[Command]
        public static void CmdAddEnemyToList(GameObject enemy)
        {
            RpcAddEnemyToList(enemy);
        }

        //[ClientRpc]
        private static void RpcAddEnemyToList(GameObject enemy)
        {
            StaticStorageWithEnemies.AddToList(enemy.GetComponent<AbstractEnemy>());
        }

        /// <summary>
        /// Проверить лист на мертвых игроков
        /// </summary>
        public static void CheckList()
        {
            for (int i = 0;i<playerList.Count;i++)
                if (!playerList[i].GetComponent<PlayerConditions>().IsAlive)
                    playerList.Remove(playerList[i]);

            CheckByGameOver();
        }

        /// <summary>
        /// Проверить, завершилась ли игра
        /// </summary>
        private static void CheckByGameOver()
        {
            foreach (GameObject player in playerList)
                if (player.GetComponent<PlayerConditions>().IsAlive) return;

            isGameOver = true;
        }
    }
}

using AbstractBehaviour;
using PlayerBehaviour;
using System.Collections.Generic;
using UnityEngine;

namespace GameBehaviour
{
    /// <summary>
    /// Хранит в себе всех игроков
    /// </summary>
    public class AllPlayerManager
        : MonoBehaviour
    {
        private static List<GameObject> playerList 
            = new List<GameObject>();

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

        public static void CheckList()
        {
            Debug.Log(playerList.Count);
            for (int i = 0;i<playerList.Count;i++)
                if (!playerList[i].GetComponent<PlayerConditions>().IsAlive)
                    playerList.Remove(playerList[i]);
        }
    }
}

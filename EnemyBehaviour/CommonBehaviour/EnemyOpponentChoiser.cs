using PlayerBehaviour;
using UnityEngine;
using VotanLibraries;
using GameBehaviour;
using VotanGameplay;

namespace EnemyBehaviour
{
    /// <summary>
    /// Класс, который позволяет врагу выбирать оппонента в виде игрока
    /// </summary>
    public class EnemyOpponentChoiser
    : MonoBehaviour
    {     
        private PlayerConditions playerConditionsTarget;

        public PlayerConditions PlayerConditionsTarget
        {
            get
            {
                return playerConditionsTarget;
            }

            set
            {
                playerConditionsTarget = value;
            }
        }

        /// <summary>
        /// Получить случайного игрока из списка
        /// </summary>
        /// <returns></returns>
        public Transform GetRandomTransformOfPlayer()
        {
            GameObject obj;
            if (CheckListOfPlayer())
            {
                    obj = AllPlayerManager.PlayerList[LibraryStaticFunctions
                        .rnd.Next(0, AllPlayerManager.PlayerList.Count)];
                playerConditionsTarget = 
                    obj.GetComponent<PlayerConditions>();
                return obj.transform;
            }
            else
            {
                return null;
            }
        }

        //метод для баланса

        /// <summary>
        /// Проверить, есть ли кто живой в листе игроков
        /// </summary>
        /// <returns></returns>
        private bool CheckListOfPlayer()
        {
            foreach (GameObject player in AllPlayerManager.PlayerList)
            {
                if (player.GetComponent<PlayerConditions>().IsAlive) return true;
            }

            return false;
        }
    }
}

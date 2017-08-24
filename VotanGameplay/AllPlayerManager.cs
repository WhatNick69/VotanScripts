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
        #region Переменные
        /// <summary>
        /// Лист с игроками
        /// </summary>
        private static List<GameObject> playerList;
        private static List<PlayerComponentsControl> playerComponentsList;
        #endregion

        #region Свойства
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

        public static List<PlayerComponentsControl> PlayerComponentsList
        {
            get
            {
                return playerComponentsList;
            }

            set
            {
                playerComponentsList = value;
            }
        }

        /// <summary>
        /// Вернуть компонент игрока через индекс
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static PlayerComponentsControl GetPlayerComponents(int index)
        {
            return playerComponentsList[index];
        }
        #endregion

        /// <summary>
        /// Инициализация
        /// </summary>
        private void Awake()
        {
            playerList = new List<GameObject>();
            playerComponentsList = new List<PlayerComponentsControl>();
        }

        /// <summary>
        /// Добавить игрока в лист
        /// </summary>
        /// <param name="player"></param>
        public static void AddPlayerToPlayerList(GameObject player)
        {
            playerList.Add(player);
            playerComponentsList.Add
                (player.GetComponent<PlayerComponentsControl>());
        }

        /// <summary>
        /// Проверить лист на мертвых игроков
        /// </summary>
        public static void CheckList()
        {
            for (int i = 0; i < playerList.Count; i++)
            {
                if (!playerComponentsList[i].PlayerConditions.IsAlive)
                {
                    playerList.Remove(playerList[i]);
                    playerComponentsList.Remove(playerComponentsList[i]);
                }
            }
            CheckByGameOver();
        }

        /// <summary>
        /// Проверить, завершилась ли игра
        /// </summary>
        private static void CheckByGameOver()
        {
            for (int i = 0;i<playerList.Count;i++)
            {
                if (playerComponentsList[i].PlayerConditions.IsAlive) return;
            }

            GameManager.IsGameOver = true;
        }

        /// <summary>
        /// Запуск эффекта грозы для всех игроков
        /// </summary>
        public static void FireLightingEffectInAllPlayers()
        {
            for (int i = 0; i < playerList.Count; i++)
            {
                playerComponentsList[i].PlayerVisualEffects.FireLightingEffect();
            }
        }
    }
}

﻿using CraftSystem;
using UnityEngine;
using VotanLibraries;

namespace ShopSystem
{
    /// <summary>
    /// Деньги и гемы пользователя, а также 
    /// дерево и железо (если потребуется)
    /// </summary>
    public class UserResources
        : MonoBehaviour
    {
        #region Переменные
        private PlayerStats playerStats;
        private static bool onLoad;
        public long money;
        public long gems;
        #endregion

        #region Свойства
        public long Money
        {
            get
            {
                return money;
            }

            set
            {
                money = value;
                playerStats.RefreshUserMoney(money);
                SaveUserResources();
            }
        }

        public long Gems
        {
            get
            {
                return gems;
            }

            set
            {
                gems = value;
                playerStats.RefreshUserGems(gems);
                SaveUserResources();
            }
        }
        #endregion

        /// <summary>
        /// ========================== Инициализация ==========================
        /// </summary>
        private void Awake()
        {
            playerStats = GameObject.Find("ImportantComponents").GetComponent<PlayerStats>();
            LoadUserResources();

            if (!onLoad)
            {
                DontDestroyOnLoad(this);
                onLoad = true;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Обновить ресурсы
        /// </summary>
        public void RefreshResources()
        {
            if (playerStats==null)
                playerStats = GameObject.Find("ImportantComponents").GetComponent<PlayerStats>();

            playerStats.RefreshUserGems(gems);
            playerStats.RefreshUserMoney(money);
        }

        /// <summary>
        /// Достаточно ли у нас денег и золота для покупки предмета
        /// </summary>
        /// <param name="tempMoney"></param>
        /// <param name="tempGems"></param>
        /// <returns></returns>
        public bool IsEnoughGemNMoney(long tempMoney,long tempGems)
        {
            if ((gems-tempGems) >= 0 && (money - tempMoney) >= 0) return true;
            else return false;
        }

        /// <summary>
        /// Загрузить локальные данные
        /// </summary>
        public void LoadUserResources()
        {
            string str = PlayerPrefs.GetString("playerResources");
            if (str == null || str == "")
            {
                PlayerPrefs.SetString("playerResources", "5000_3");
                LoadUserResources();
            }
            else
            {
                int[] resources = LibraryObjectsWorker.StringSplitter(str, '_');
                money = resources[0];
                gems = resources[1];
            }
        }

        /// <summary>
        /// Сохранить локальные данные
        /// </summary>
        /// <param name="money"></param>
        /// <param name="gems"></param>
        public void SaveUserResources(long money=0, long gems=0)
        {
            if (money > 0) this.money += money;
            if (gems > 0) this.gems += money;
            PlayerPrefs.SetString("playerResources", (this.money+"_"+this.gems).ToString());
        }
    }
}

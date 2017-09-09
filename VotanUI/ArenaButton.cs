using ShopSystem;
using UnityEngine;
using UnityEngine.UI;
using VotanUI;

namespace CraftSystem
{
    /// <summary>
    /// Выбрать арену
    /// </summary>
    public class ArenaButton 
        : MonoBehaviour
    {
        #region Переменные и ссылки
        [SerializeField, Tooltip("Изображение арены")]
        private Image arenaImage;
        [SerializeField,Tooltip("Вознаграждения")]
        private int gemsReward,goldReward;
        [SerializeField,Tooltip("Объекты вознаграждения")]
        private Text gemsRewardText, goldRewardText;
        [SerializeField, Tooltip("Эта карта имеет режим хардкора?")]
        private bool isHardcoreMode;
        [SerializeField,Tooltip("Есть ли мини игра у этого уровня?")]
        private bool isHaveMiniGame;
        [SerializeField, Tooltip("Объект завершенной локации")]
        private GameObject completedScene;
        [SerializeField, Tooltip("Изображение хардкор-режима")]
        private GameObject hardcoreImage;
        private bool isCompleted;
        [SerializeField,Tooltip("Кнопка с мини игрой")]
        private GameObject miniGameButton;

        private int sceneNumber;
        private MainWindow mainWindow;
        private ArmorCraft armorCraft;
        private WeaponCraft weaponCraft;
        private ItemsSkillsCraft itemSkillsCraft;
        private UserResources userResources;
        #endregion

        #region Свойства
        public int GemsReward
        {
            get
            {
                return gemsReward;
            }

            set
            {
                gemsReward = value;
            }
        }

        public int GoldReward
        {
            get
            {
                return goldReward;
            }

            set
            {
                goldReward = value;
            }
        }

        public bool IsHardcoreMode
        {
            get
            {
                return isHardcoreMode;
            }

            set
            {
                isHardcoreMode = value;
            }
        }

        public bool IsCompleted
        {
            get
            {
                return isCompleted;
            }

            set
            {
                isCompleted = value;
            }
        }

        public int SceneNumber
        {
            get
            {
                return sceneNumber;
            }

            set
            {
                sceneNumber = value;
            }
        }
        #endregion

        /// <summary>
        /// Запуск арены
        /// </summary>
        public void StartArena()
        {
            if (userResources == null)
                userResources = GameObject.Find("GetPrefabs").GetComponent<UserResources>();
            userResources.GoldBonus = goldReward;
            userResources.GemsBonus = gemsReward;

            armorCraft.PlayArenaArmor();
            weaponCraft.PlayArenaWeapon();
            mainWindow.InitialisationArena(sceneNumber);
            itemSkillsCraft.ClearItemWindow();
            mainWindow.SelectBuyItemsWindow();
        }

        /// <summary>
        /// Инициализация
        /// </summary>
        public void Initialisation(MainWindow mainWindow,ArmorCraft armorCraft
            , WeaponCraft weaponCraft, ItemsSkillsCraft itemSkillsCraft)
        {
            this.mainWindow = mainWindow;
            this.armorCraft = armorCraft;
            this.weaponCraft = weaponCraft;
            this.itemSkillsCraft = itemSkillsCraft;

            // Если уровень завершен - мы не получим гемов, а золото в 4 раза меньше
            if (isCompleted)
            {
                Color col = new Color(1, 1, 1, 0.5f);
                gemsReward = 0;
                goldReward /= 4;
                completedScene.SetActive(true);
                hardcoreImage.GetComponent<Image>().color = col;
                hardcoreImage.transform.GetChild(0).GetComponent<Text>().color = col;
                arenaImage.color = col;
            }

            // Активация объектов, в зависимости от количества гемов
            if (gemsReward == 0)
            {
                gemsRewardText.transform.parent.gameObject.SetActive(false);
            }
            else
            {
                gemsRewardText.transform.parent.gameObject.SetActive(true);
                gemsRewardText.text = "+" + gemsReward;
            }

            // Активация объектов, в зависимости от количества золота
            if (goldReward == 0)
            {
                goldRewardText.transform.parent.gameObject.SetActive(false);
            }
            else
            {
                goldRewardText.transform.parent.gameObject.SetActive(true);
                goldRewardText.text = "+" + goldReward;
            }

            // Если это режим хардкора - включаем соответствующую картинку
            if (isHardcoreMode)
                hardcoreImage.SetActive(true);
            else
                hardcoreImage.SetActive(false);

            if (isCompleted && isHaveMiniGame)
                miniGameButton.SetActive(true);
            else
                miniGameButton.SetActive(false);
        }
    }
}

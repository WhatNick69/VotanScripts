using UnityEngine;
using UnityEngine.UI;
using VotanLibraries;

namespace CraftSystem
{
    /// <summary>
    /// Система инвентаря пользователя
    /// </summary>
    public class Inventory 
        : MonoBehaviour
    {
        #region Переменные
        [SerializeField, Tooltip("Оружие")]
        private GameObject weapon;
        [SerializeField, Tooltip("Броня")]
        private GameObject armor;
        [SerializeField, Tooltip("Кнопка экипировки")]
        private Button equipButton;

        private ArmorCraft armorCraftComponent;
        private WeaponCraft weaponCraftComponent;
        private ItemsSkillsCraft itemsSkillsCraft;

        public ItemsSkillsCraft ItemsSkillsCraft
        {
            get
            {
                return itemsSkillsCraft;
            }

            set
            {
                itemsSkillsCraft = value;
            }
        }

        public WeaponCraft WeaponCraftComponent
        {
            get
            {
                return weaponCraftComponent;
            }

            set
            {
                weaponCraftComponent = value;
            }
        }

        public ArmorCraft ArmorCraftComponent
        {
            get
            {
                return armorCraftComponent;
            }

            set
            {
                armorCraftComponent = value;
            }
        }
        #endregion

        #region Работа с окнами
        public void WeaponWindow()
        {
            armor.SetActive(false);
            weapon.SetActive(true);
        }

        public void ArmorWindow()
        {
            weapon.SetActive(false);
            armor.SetActive(true);
        }
        #endregion

        /// <summary>
        /// =================== Инициализация ===================
        /// </summary>
        private void Start()
        {
            armorCraftComponent = GetComponent<ArmorCraft>();
            weaponCraftComponent = GetComponent<WeaponCraft>();
            itemsSkillsCraft = GetComponent<ItemsSkillsCraft>();
        }

        /// <summary>
        /// Загрузить лист шлемов
        /// </summary>
        public void LoadHelmetsList()
        {
            if (armorCraftComponent == null) Start();

            armorCraftComponent.HelmetWindow();
        }

        /// <summary>
        /// Загрузить лист оружия
        /// </summary>
        public void LoadWeaponsList()
        {
            if (weaponCraftComponent == null) Start();

            weaponCraftComponent.WeaponWindow();
        }


        public static int[] LoadInventoryNumbers()
        {
            string str = PlayerPrefs.GetString("inventoryNumbers");
            if (str == null || str == "")
            {
                // шлем, щит, кираса, оружие, скилл1, скилл2, скилл3
                PlayerPrefs.SetString("inventoryNumbers", "0_0_0_0_255_255_255");
            }
            return LibraryObjectsWorker.StringSplitter(str, '_');
        }

        public static void SaveInventoryNumber(int position, int number)
        {
            string str = PlayerPrefs.GetString("inventoryNumbers");
            int[] saveInventory = LibraryObjectsWorker.StringSplitter(str, '_');
            saveInventory[position] = number;

            string save = null;
            for (int i = 0;i<saveInventory.Length;i++)
            {
                save += saveInventory[i] + "_";
            }
            PlayerPrefs.SetString("inventoryNumbers",save);
        }
    }
}

using UnityEngine;

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
        private ArmorCraft armorCraftComponent;
        private WeaponCraft weaponCraftComponent;
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
    }
}

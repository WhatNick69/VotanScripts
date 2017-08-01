using UnityEngine;

namespace CraftSystem
{
    /// <summary>
    /// Система инвентаря пользователя
    /// </summary>
    public class Inventory 
        : MonoBehaviour
    {
        [SerializeField, Tooltip("Оружие")]
        private GameObject weapon;
        [SerializeField, Tooltip("Броня")]
        private GameObject armor;
        [SerializeField, Tooltip("Крафт брони. Компонент")]
        private ArmorCraft armorCraftComponent;
        [SerializeField,Tooltip("Крафт оружия. Компонент")]
        private WeaponCraft weaponCraftComponent;

        private void Start()
        {
            armorCraftComponent = GetComponent<ArmorCraft>();
            weaponCraftComponent = GetComponent<WeaponCraft>();
        }

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

        public void LoadHelmetsList()
        {
            if (armorCraftComponent == null) Start();

            armorCraftComponent.HelmetWindow();
        }

        public void LoadWeaponsList()
        {
            if (weaponCraftComponent == null) Start();

            weaponCraftComponent.HeadWindow();
        }
    }
}

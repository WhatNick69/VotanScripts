using PlayerBehaviour;
using UnityEngine;

namespace CraftSystem
{
    /// <summary>
    /// Описывает гем
    /// </summary>
    public class Gem
        : MonoBehaviour
    {
        #region Переменные
        [SerializeField]
        private DamageType damageTypeGem;
        [SerializeField, Range(1, 100f)]
        private float gemPower;
        [SerializeField]
        private string weaponName;
        #endregion

        #region Свойства
        public DamageType DamageTypeGem
        {
            get
            {
                return damageTypeGem;
            }

            set
            {
                damageTypeGem = value;
            }
        }

        public float GemPower
        {
            get
            {
                return gemPower;
            }

            set
            {
                gemPower = value;
            }
        }

        public string WeaponName
        {
            get
            {
                return weaponName;
            }

            set
            {
                weaponName = value;
            }
        }
        #endregion
    }
}

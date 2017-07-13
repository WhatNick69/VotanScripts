using UnityEngine;

namespace CraftSystem
{
    /// <summary>
    /// Наконечник оружия
    /// </summary>
    public class Head 
        : MonoBehaviour
    {
        #region Переменные
        [SerializeField, Range(1, 75f),Tooltip("Вес головной части")]
        private float headWeight;
        [SerializeField,Range(1,100f),Tooltip("Базовое значение урона оружием")]
        private int damageBase;
        [SerializeField, Range(0, 25f), Tooltip("Бонус к скорости вращения за счет головной части")]
        private float bonusSpinSpeedFromHead;

        [SerializeField, Range(0, 1)] // 0 рубящая, 1 дробящая
        private int headType;
        [SerializeField]
        private string weaponName;
        #endregion

        #region Свойства
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

        public int HeadType
        {
            get
            {
                return headType;
            }

            set
            {
                headType = value;
            }
        }

        public float BonusSpinSpeedFromHead
        {
            get
            {
                return bonusSpinSpeedFromHead;
            }

            set
            {
                bonusSpinSpeedFromHead = value;
            }
        }

        public float HeadWeight
        {
            get
            {
                return headWeight;
            }

            set
            {
                headWeight = value;
            }
        }

        public int DamageBase
        {
            get
            {
                return damageBase;
            }

            set
            {
                damageBase = value;
            }
        }
        #endregion
    }
}

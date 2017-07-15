﻿using PlayerBehaviour;
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
        [SerializeField,Tooltip("Тип атаки камня")]
        private DamageType damageTypeGem;
        [SerializeField, Range(1, 100f),Tooltip("Сила камня")]
        private float gemPower;
        [SerializeField,Tooltip("Имя камня")]
        private string gemName;
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

        public string GemName
        {
            get
            {
                return gemName;
            }

            set
            {
                gemName = value;
            }
        }
        #endregion
    }
}

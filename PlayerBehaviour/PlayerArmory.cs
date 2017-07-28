using AbstractBehaviour;
using System.Collections.Generic;
using UnityEngine;
using VotanLibraries;
using GameBehaviour;
using VotanInterfaces;

namespace PlayerBehaviour
{
    /// <summary>
    /// Абстрактный класс, который описывает броню объекта
    /// </summary>
    public class PlayerArmory 
        : AbstractObjectConditions
    {
        #region Переменные
        [SerializeField,Tooltip("Шлем")]
        private PartArmoryManager helmet;
        [SerializeField, Tooltip("Части кирасы. Первый элемент - главный")]
        private List<PartArmoryManager> kirasaParts;
        [SerializeField, Tooltip("Части щита. Первый элемент - главный")]
        private List<PartArmoryManager> shieldParts;

        [SerializeField, Tooltip("Позиции")]
        private List<Transform> armoryPosition;

        private bool isHelmetDeactive;
        private bool isKirasaDeactive;
        private bool isShieldDeactive;
        private float kirasaPartArmory;
        private float shieldPartArmory;

        public float tempArmory;

        private PlayerComponentsControl playerComponentsControl;
        #endregion

        #region Свойства и доступы
        public PartArmoryManager Helmet
        {
            get
            {
                return helmet;
            }

            set
            {
                helmet = value;
            }
        }

        public List<PartArmoryManager> KirasaParts
        {
            get
            {
                return kirasaParts;
            }

            set
            {
                kirasaParts = value;
            }
        }

        public List<PartArmoryManager> ShieldParts
        {
            get
            {
                return shieldParts;
            }

            set
            {
                shieldParts = value;
            }
        }
        #endregion

        /// <summary>
        /// Инициализация
        /// </summary>
        private void Start() // --------------главный метод--------------------
        {
            IsAlive = true;
            colorChannelGreen = 1;
            colorChannelRed = 0.5f;
            circleHealthUI.color = new Color(colorChannelRed, 0,colorChannelGreen);
            initialisatedHealthValue = healthValue;

            kirasaPartArmory = 0.4f / kirasaParts.Count;
            shieldPartArmory = 0.4f / (shieldParts.Count-1);

            //SwitchPositionInCuirassList();
            playerComponentsControl = GetComponent<PlayerComponentsControl>();
            SwitchPositionInShieldList();
        }

        public Transform GetMyParent(int i)
        {
            return armoryPosition[i];
        }

        /// <summary>
        /// Ставит на нулевую позицию наплечник
        /// </summary>
        public void SwitchPositionInCuirassList()
        {
            int tempNumber = 0;
            PartArmoryManager tempGameObject = KirasaParts[0] ;
            for (int i = 0;i<kirasaParts.Count;i++)
            {
                if (!kirasaParts[i].IsActivePart)
                {
                    tempNumber = i;
                    break;
                }
            }
            kirasaParts[0] = kirasaParts[tempNumber];
            kirasaParts[tempNumber] = tempGameObject;
        }

        /// <summary>
        /// Ставит на нулевую позицию наплечник
        /// </summary>
        public void SwitchPositionInShieldList()
        {
            int tempNumber = 0;
            PartArmoryManager tempGameObject = ShieldParts[0];
            for (int i = 0; i < shieldParts.Count; i++)
            {
                if (!shieldParts[i].IsActivePart)
                {
                    tempNumber = i;
                    break;
                }
            }
            shieldParts[0] = ShieldParts[tempNumber];
            shieldParts[tempNumber] = tempGameObject;
        }

        /// <summary>
        /// Снизить показатель брони
        /// </summary>
        /// <param name="value"></param>
        public void DecreaseArmoryLevel(float value)
        {
            if (healthValue > 0)
            {
                healthValue += value;
                RefreshHealthCircle();
            }
            else
            {
                IsAlive = false;
                healthValue = 0;
                RefreshHealthCircle();
            }
        }

        /// <summary>
        /// Обновить бар брони
        /// </summary>
        public override void RefreshHealthCircle()
        {
            float a = circleHealthUI.fillAmount;
            circleHealthUI.fillAmount = healthValue / initialisatedHealthValue;
            a -= circleHealthUI.fillAmount;
            tempArmory += a;
            CheckArmoryLevel();
        }

        /// <summary>
        /// Проверить уровень брони
        /// </summary>
        private void CheckArmoryLevel()
        {
            // если шлем целый
            if (!isHelmetDeactive && !isShieldDeactive && !isKirasaDeactive)
            {
                if (tempArmory >= 0.2f)
                {
                    isHelmetDeactive = true;
                    playerComponentsControl.
                        PlayerSounder.PlayAnyDestroyArmoryAudio
                        (helmet.NumberPosition);
                    helmet.FireEvent();
                    tempArmory -= 0.2f;
                }
                else
                {
                    return;
                }
            }

            // если щит целый
            if (!isShieldDeactive && isHelmetDeactive && !isKirasaDeactive)
            {
                if (tempArmory >= shieldPartArmory)
                {
                    while (tempArmory >= shieldPartArmory)
                    {
                        int a = 0;
                        if (shieldParts.Count > 1)
                        {
                            a = LibraryStaticFunctions.rnd.Next(1, shieldParts.Count);
                        }

                        playerComponentsControl.
                            PlayerSounder.PlayAnyDestroyArmoryAudio
                            (shieldParts[a].NumberPosition);
                        shieldParts[a].FireEvent();
                        shieldParts.RemoveAt(a);
                        tempArmory -= shieldPartArmory;

                        if (shieldParts.Count == 1)
                        {
                            isShieldDeactive = true;
                            break;
                        }
                    }
                }
            }

            // если броня целая
            if (!isKirasaDeactive && isHelmetDeactive && isShieldDeactive)
            {
                if (tempArmory >= kirasaPartArmory)
                {
                    while (tempArmory >= kirasaPartArmory)
                    {
                        int a;
                        if (kirasaParts.Count >= 1)
                        {
                            a = LibraryStaticFunctions.rnd.Next(0, kirasaParts.Count);
                        }
                        else
                        {
                            a = 0;
                            isKirasaDeactive = true;
                            CheckArmorCorrectly();
                            break;
                        }

                        playerComponentsControl.
                            PlayerSounder.PlayAnyDestroyArmoryAudio
                            (kirasaParts[a].NumberPosition);
                        kirasaParts[a].FireEvent();
                        kirasaParts.RemoveAt(a);
                        tempArmory -= kirasaPartArmory;
                    }
                }
            }
        }

        /// <summary>
        /// Проверка листа кирасы на пустоту
        /// </summary>
        private void CheckArmorCorrectly()
        {
            if (kirasaParts.Count != 0)
            {
                Debug.Log("Коррекция");
                foreach (PartArmoryManager part in kirasaParts)
                {
                    part.FireEvent();
                    kirasaParts.Remove(part);
                }
            }
        }
    }

    /// <summary>
    /// Тип брони. Перечислитель.
    /// Шлем, кираса или щит.
    /// </summary>
    public enum ArmoryClass
    {
        Helmet, Cuirass,Shield
    }

	/// <summary>
    /// Позиция брони. Перечислитель
    /// </summary>
    public enum ArmoryPosition
	{
		Helmet, Shield, Cuirass, LeftShoulder, RightShoulder, LeftBallast, RightBallast
	}
}

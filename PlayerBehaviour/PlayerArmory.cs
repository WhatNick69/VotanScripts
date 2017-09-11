using AbstractBehaviour;
using System.Collections.Generic;
using UnityEngine;
using VotanLibraries;
using GameBehaviour;

namespace PlayerBehaviour
{
    /// <summary>
    /// Абстрактный класс, который описывает броню объекта
    /// </summary>
    public class PlayerArmory 
        : AbstractObjectConditions
    {
        #region Переменные
        //[SerializeField,Tooltip("Шлем")]
        private PartArmoryManager helmet;
        //[SerializeField, Tooltip("Части кирасы. Первый элемент - главный")]
        private List<PartArmoryManager> kirasaParts = new List<PartArmoryManager>();
        //[SerializeField, Tooltip("Части щита. Первый элемент - главный")]
        private List<PartArmoryManager> shieldParts = new List<PartArmoryManager>();

        [SerializeField, Tooltip("Позиции")]
        private List<Transform> armoryPosition;
        [SerializeField, Tooltip("Вес брони. Общий")]
        private float armoryWeight;

        private float helmetWeight;
        private float shieldWeight;
        private float cuirasseWeight;
        private float shieldWeightPart;
        private float cuirasseWeightPart;

        private bool isHelmetDeactive;
        private bool isKirasaDeactive;
        private bool isShieldDeactive;
        private float kirasaPartArmory;
        private float shieldPartArmory;

        private float tempArmory;
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

        public float ArmoryWeight
        {
            get
            {
                return armoryWeight;
            }

            set
            {
                armoryWeight = value;
                OverrideWeaponParametersDependenceArmoryWeight();
                playerComponentsControl.PlayerController.
                    OverridePlayerControllerParametersDependenceArmoryWeight();
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

            cuirasseWeightPart = cuirasseWeight / kirasaParts.Count; // вес элемента кирасы
            shieldWeightPart = shieldWeight / shieldParts.Count; // вес элемента щита

            SwitchPositionInShieldList();
        }

        /// <summary>
        /// Переопределить параметры оружия в зависимисти от 
        /// веса брони на персонаже/оставшегося веса брони.
        /// </summary>
        private void OverrideWeaponParametersDependenceArmoryWeight()
        {
            playerComponentsControl.PlayerWeapon.SetSpinSpeed
                (LibraryStaticFunctions.TotalSpinSpeed(ArmoryWeight));
        }

        /// <summary>
        /// Задать общий вес брони и вес элементов по отдельности.
        /// </summary>
        /// <param name="helmetWeight">Шлем</param>
        /// <param name="shieldWeight">Щит</param>
        /// <param name="cuirasseWeight">Кираса</param>
        public void SetArmoryWeight(float helmetWeight,float shieldWeight,float cuirasseWeight)
        {
            this.helmetWeight = helmetWeight;
            this.shieldWeight = shieldWeight;
            this.cuirasseWeight = cuirasseWeight;

            armoryWeight = LibraryStaticFunctions.TotalWeight(this.helmetWeight
                , this.shieldWeight
                , this.cuirasseWeight);
        }

        /// <summary>
        /// Получить точку-родитель
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
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

                    ArmoryWeight -= helmetWeight;
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
                            a = Random.Range(1, shieldParts.Count);
                        }

                        playerComponentsControl.
                            PlayerSounder.PlayAnyDestroyArmoryAudio
                            (shieldParts[a].NumberPosition);
                        shieldParts[a].FireEvent();
                        shieldParts.RemoveAt(a);

                        tempArmory -= shieldPartArmory;
                        ArmoryWeight -= shieldWeightPart;

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
                            a = Random.Range(0, kirasaParts.Count);
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
                        ArmoryWeight -= cuirasseWeightPart;
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
                foreach (PartArmoryManager part in kirasaParts)
                {
                    part.FireEvent();
                    kirasaParts.Remove(part);
                }
            }
        }

        public override IEnumerator<float> DieState()
        {
            yield break;
        }
    }

    /// <summary>
    /// Тип брони. Перечислитель.
    /// Шлем, кираса или щит.
    /// </summary>
    public enum ArmoryClass
    {
        Helmet,
        Cuirass,
        Shield
    }

	/// <summary>
    /// Позиция брони. Перечислитель
    /// </summary>
    public enum ArmoryPosition
	{
		Helmet,
        Shield,
        Cuirass,
        LeftShoulder,
        RightShoulder,
        LeftBallast,
        RightBallast
	}
}

using MovementEffects;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;
using VotanInterfaces;

namespace PlayerBehaviour
{
    /// <summary>
    /// Менеджер по управлению предметами игрока, а также его умениями
    /// </summary>
    public class PlayerHUDManager
        : MonoBehaviour
    {
        #region Переменные и ссылки инвентаря
        [Header("Основа инвентаря")]
        [SerializeField, Tooltip("Цвет незаряженных индикаторов")]
        private Color unActiveIndicatorColor;
        [SerializeField, Tooltip("Цвет готовых индикаторов")]
        private Color activeIndicatorColor;
        [SerializeField, Tooltip("Кнопка, нажатие по которой приводит к закрытию инвентарей")]
        private Button outsideButton;

        [Header("Левый инвентарь")]
        [SerializeField, Tooltip("Левая кнопка")]
        private RectTransform leftButton;
        [SerializeField, Tooltip("Левый хранитель зелий")]
        private RectTransform leftItemsParent;
        [SerializeField, Tooltip("Левый хранитель позиций")]
        private RectTransform leftPositionsParent;
        [SerializeField, Tooltip("Левые индикаторы")]
        private RectTransform leftIndicators;
        private RectTransform[] arrayPositionsForItems;
        private Button[] arrayButtonItems;
        private RectTransform[] arrayItems;
        private Image[] arrayLeftIndicators;

        [Header("Правый инвентарь")]
        [SerializeField, Tooltip("Правая кнопка")]
        private RectTransform rightButton;
        [SerializeField, Tooltip("Правый хранитель умений")]
        private RectTransform rightSkillsParent;
        [SerializeField, Tooltip("Правый хранитель позиций")]
        private RectTransform rightPositionsParent;
        [SerializeField, Tooltip("Правые индикаторы")]
        private RectTransform rightIndicators;
        private RectTransform[] arrayPositionsForSkills;
        private Button[] arrayButtonSkills;
        private RectTransform[] arraySkills;
        private Image[] arrayRightIndicators;

        private Coroutine rightCoroutine;
        private Coroutine leftCoroutine;

        private bool isRightInventoryOpen;
        private bool isLeftInventoryOpen;
        #endregion

        #region Содержимое инвентаря
        private IItem firstItem, secondItem, thirdItem;
        private ISkill firstSkill, secondSkill, thirdSkill;
        private PlayerComponentsControl playerComponentsControl;
        private bool isActiveItemButton = true;
        private bool isActiveSkillButton = true;

        private string animationIndicator = "HUDIndicatorEnabled";
        #endregion

        #region Свойства
        public IItem FirstItem
        {
            get
            {
                return firstItem;
            }

            set
            {
                firstItem = value;
            }
        }

        public IItem SecondItem
        {
            get
            {
                return secondItem;
            }

            set
            {
                secondItem = value;
            }
        }

        public IItem ThirdItem
        {
            get
            {
                return thirdItem;
            }

            set
            {
                thirdItem = value;
            }
        }

        public ISkill FirstSkill
        {
            get
            {
                return firstSkill;
            }

            set
            {
                firstSkill = value;
            }
        }

        public ISkill SecondSkill
        {
            get
            {
                return secondSkill;
            }

            set
            {
                secondSkill = value;
            }
        }

        public ISkill ThirdSkill
        {
            get
            {
                return thirdSkill;
            }

            set
            {
                thirdSkill = value;
            }
        }

        public RectTransform LeftItemsParent
        {
            get
            {
                return leftItemsParent;
            }

            set
            {
                leftItemsParent = value;
            }
        }

        public RectTransform RightSkillsParent
        {
            get
            {
                return rightSkillsParent;
            }

            set
            {
                rightSkillsParent = value;
            }
        }
        #endregion

        /// <summary>
        /// Инициализация
        /// </summary>
        private void Start()
        {
            playerComponentsControl = GetComponent<PlayerComponentsControl>();
            InitialisationsLeftArrays();
            InitialisationRightArrays();
        }

        #region Левый инвентарь: предметы
        /// <summary>
        /// Инициализация левого инвентаря
        /// </summary>
        private void InitialisationsLeftArrays()
        {
            arrayItems = new RectTransform[leftItemsParent.transform.childCount];
            arrayButtonItems = new Button[arrayItems.Length];
            arrayPositionsForItems = new RectTransform[leftPositionsParent.transform.childCount];
            arrayLeftIndicators = new Image[leftIndicators.transform.childCount];

            for (int i = 0; i < arrayItems.Length; i++)
            {
                arrayItems[i] = leftItemsParent.GetChild(i).GetComponent<RectTransform>();
                arrayButtonItems[i] = arrayItems[i].GetComponent<Button>();
            }

            for (int i = 0; i < arrayPositionsForItems.Length; i++)
                arrayPositionsForItems[i] = leftPositionsParent.GetChild(i).GetComponent<RectTransform>();

            for (int i = 0; i < arrayLeftIndicators.Length; i++)
            {
                arrayLeftIndicators[i] = leftIndicators.GetChild(i).GetComponent<Image>();
                //arrayLeftIndicators[i].GetComponent<Animation>()[animationIndicator].speed = 0.5f;
            }
        }

        /// <summary>
        /// Инициализация предмета в левый инвентарь
        /// </summary>
        /// <param name="thisItem"></param>
        /// <returns></returns>
        public int InitialisationThisItemToInventory(IItem thisItem)
        {
            if (firstItem == null)
            {
                firstItem = thisItem;
                return 0;
            }
            else if (secondItem == null)
            {
                secondItem = thisItem;
                return 1;
            }
            else if (thirdItem == null)
            {
                thirdItem = thisItem;
                return 2;
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// Установить позицию левому индикатору
        /// </summary>
        /// <param name="indicatorPosition"></param>
        public void SetPositionToLeftIndicator(int indicatorPosition)
        {
            if (arrayLeftIndicators == null) Start();

            arrayLeftIndicators[indicatorPosition].GetComponent<RectTransform>().anchoredPosition =
                new Vector2(0, -35 + (35 * indicatorPosition));
        }

        /// <summary>
        /// Установить позицию индикатору левого инвентаря
        /// </summary>
        /// <param name="indicatorPosition"></param>
        /// <param name="active"></param>
        public void TellItemIndicator(int indicatorPosition, bool active)
        {
            if (active)
            {
                if (arrayLeftIndicators[indicatorPosition].color.r != activeIndicatorColor.r)
                {
                    arrayLeftIndicators[indicatorPosition].color = activeIndicatorColor;
                    arrayLeftIndicators[indicatorPosition].GetComponent<Animation>().enabled = true;
                }
            }
            else
            {
                if (arrayLeftIndicators[indicatorPosition].color.r == activeIndicatorColor.r)
                {
                    arrayLeftIndicators[indicatorPosition].color = unActiveIndicatorColor;
                    arrayLeftIndicators[indicatorPosition].GetComponent<Animation>().enabled = false;
                }
               
            }
        }

        /// <summary>
        /// Открыть левый инвентарь
        /// </summary>
        public void OnClickOpenLeftInventory()
        {
            if (leftCoroutine != null)
                StopCoroutine(leftCoroutine);

            if (!isLeftInventoryOpen)
            {
                playerComponentsControl.PlayerHUDAudioStorage.PlaySoundSwipeInventory(true);
                isLeftInventoryOpen = true;
                leftCoroutine =
                    StartCoroutine(CoroutineForMoveInventoryWindow(true, true));
            }
            else
            {
                playerComponentsControl.PlayerHUDAudioStorage.PlaySoundSwipeInventory(false);
                isLeftInventoryOpen = false;
                leftCoroutine =
                    StartCoroutine(CoroutineForMoveInventoryWindow(true, false));
            }
        }

        /// <summary>
        /// Установить состояние для левых кнопок
        /// </summary>
        /// <param name="active"></param>
        private void SetActiveForLeftButtons(bool active)
        {
            for (int i = 0; i < arrayButtonItems.Length; i++)
            {
                arrayButtonItems[i].enabled = active;
            }
        }

        /// <summary>
        /// Установить состояние для левых индикаторов
        /// </summary>
        /// <param name="active"></param>
        private void SetActiveForLeftIndicators(bool active)
        {
            for (int i = 0; i < arrayLeftIndicators.Length; i++)
            {
                arrayLeftIndicators[i].enabled = active;
            }
        }

        /// <summary>
        /// Зажечь событие кнопки инвентаря зельев
        /// </summary>
        /// <param name="numberButton"></param>
        public void FireItem(int numberButton)
        {
            switch (numberButton)
            {
                case 0:
                    if (firstItem.PlayerComponentsControlInstance == null)
                        firstItem.PlayerComponentsControlInstance = playerComponentsControl;
                    firstItem.FireEventItem();
                    break;
                case 1:
                    if (secondItem.PlayerComponentsControlInstance == null)
                        secondItem.PlayerComponentsControlInstance = playerComponentsControl;
                    secondItem.FireEventItem();
                    break;
                case 2:
                    if (thirdItem.PlayerComponentsControlInstance == null)
                        thirdItem.PlayerComponentsControlInstance = playerComponentsControl;
                    thirdItem.FireEventItem();
                    break;
            }
        }
        #endregion

        #region Правый инвентарь: умения
        /// <summary>
        /// Инициализация правого инвентаря
        /// </summary>
        private void InitialisationRightArrays()
        {
            arraySkills = new RectTransform[rightSkillsParent.transform.childCount];
            arrayButtonSkills = new Button[arraySkills.Length];
            arrayPositionsForSkills = new RectTransform[rightPositionsParent.transform.childCount];
            arrayRightIndicators = new Image[rightIndicators.transform.childCount];

            for (int i = 0; i < arraySkills.Length; i++)
            {
                arraySkills[i] = rightSkillsParent.GetChild(i).GetComponent<RectTransform>();
                arrayButtonSkills[i] = arraySkills[i].GetComponent<Button>();
            }

            for (int i = 0; i < arrayPositionsForSkills.Length; i++)
                arrayPositionsForSkills[i] = rightPositionsParent.GetChild(i).GetComponent<RectTransform>();

            for (int i = 0; i < arrayRightIndicators.Length; i++)
            {
                arrayRightIndicators[i] = rightIndicators.GetChild(i).GetComponent<Image>();
                //arrayRightIndicators[i].GetComponent<Animation>()[animationIndicator].speed = 0.5f;
            }
        }

        /// <summary>
        /// Инициализация умения в инвентарь
        /// </summary>
        /// <param name="thisSkill"></param>
        /// <returns></returns>
        public int InitialisationThisSkillToInventory(ISkill thisSkill)
        {
            if (firstSkill == null)
            {
                firstSkill = thisSkill;
                TellSkillIndicator(0, true);
                return 0;
            }
            else if (secondSkill == null)
            {
                secondSkill = thisSkill;
                TellSkillIndicator(1, true);
                return 1;
            }
            else if (thirdSkill == null)
            {
                thirdSkill = thisSkill;
                TellSkillIndicator(2, true);
                return 2;
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// Установить позицию правому индикатору
        /// </summary>
        /// <param name="indicatorPosition"></param>
        public void SetPositionToRightIndicator(int indicatorPosition)
        {
            arrayRightIndicators[indicatorPosition].GetComponent<RectTransform>().anchoredPosition =
                new Vector2(0, -35 + (35 * indicatorPosition));
        }

        /// <summary>
        /// Установить позицию индикатору правого инвентаря
        /// </summary>
        /// <param name="indicatorPosition"></param>
        /// <param name="active"></param>
        public void TellSkillIndicator(int indicatorPosition, bool active)
        {
            if (active)
            {
                if (arrayRightIndicators[indicatorPosition].color.r != activeIndicatorColor.r)
                {
                    arrayRightIndicators[indicatorPosition].color = activeIndicatorColor;
                    arrayRightIndicators[indicatorPosition].GetComponent<Animation>().enabled = true;
                }
            }
            else
            {
                if (arrayRightIndicators[indicatorPosition].color.r == activeIndicatorColor.r)
                {
                    arrayRightIndicators[indicatorPosition].color = unActiveIndicatorColor;
                    arrayRightIndicators[indicatorPosition].GetComponent<Animation>().enabled = false;
                }
            }
        }

        /// <summary>
        /// Открыть правый инвентарь
        /// </summary>
        public void OnClickOpenRightInventory()
        {
            if (rightCoroutine != null)
                StopCoroutine(rightCoroutine);

            if (!isRightInventoryOpen)
            {
                playerComponentsControl.PlayerHUDAudioStorage.PlaySoundSwipeInventory(true);
                isRightInventoryOpen = true;
                rightCoroutine =
                    StartCoroutine(CoroutineForMoveInventoryWindow(false, true));
            }
            else
            {
                playerComponentsControl.PlayerHUDAudioStorage.PlaySoundSwipeInventory(false);
                isRightInventoryOpen = false;
                rightCoroutine =
                    StartCoroutine(CoroutineForMoveInventoryWindow(false, false));
            }
        }

        /// <summary>
        /// Зажечь событие кнопки инвентаря умений
        /// </summary>
        /// <param name="numberButton"></param>
        public void FireSkill(int numberButton)
        {
            switch (numberButton)
            {
                case 0:
                    if (firstSkill.PlayerComponentsControlInstance == null)
                        firstSkill.PlayerComponentsControlInstance = playerComponentsControl;
                    firstSkill.FireEventSkill();
                    break;
                case 1:
                    if (secondSkill.PlayerComponentsControlInstance == null)
                        secondSkill.PlayerComponentsControlInstance = playerComponentsControl;
                    secondSkill.FireEventSkill();
                    break;
                case 2:
                    if (thirdSkill.PlayerComponentsControlInstance == null)
                        thirdSkill.PlayerComponentsControlInstance = playerComponentsControl;
                    thirdSkill.FireEventSkill();
                    break;
            }
        }

        /// <summary>
        /// Установить состояние для левых индикаторов
        /// </summary>
        /// <param name="active"></param>
        private void SetActiveForRightIndicators(bool active)
        {
            for (int i = 0; i < arrayRightIndicators.Length; i++)
            {
                arrayRightIndicators[i].enabled = active;
            }
        }

        /// <summary>
        /// Установить состояние для левых кнопок
        /// </summary>
        /// <param name="active"></param>
        private void SetActiveForRightButtons(bool active)
        {
            for (int i = 0; i < arrayButtonSkills.Length; i++)
            {
                arrayButtonSkills[i].enabled = active;
            }
        }
        #endregion

        #region Методы, общие для инвентарей
        /// <summary>
        /// Корутина для движения окна с инвентарем
        /// </summary>
        /// <param name="inventory">Компонент объекта для движения</param>
        /// <param name="destinationCor">Конечная координата движения</param>
        /// <returns></returns>
        private IEnumerator<float> CoroutineForMoveInventoryWindow
            (bool isLeftInventory, bool isOpen)
        {
            int i = 0;
            int iterations = 0;
            float cachedTime;

            if (isLeftInventory)
            {
                Transform cashedTransformItem = arrayItems[i];
                if (isOpen)
                {
                    Vector2 cashedTransformPosition = arrayPositionsForItems[i].transform.position;
                    Vector3 scaleVector = new Vector3(1, 1, 1);

                    SetActiveForLeftIndicators(false);
                    while (Mathf.Abs(Vector2.Distance(cashedTransformItem.position, cashedTransformPosition)) >= 1)
                    {
                        i++;
                        cachedTime = Time.deltaTime * i;
                        for (iterations = 0; iterations < arrayItems.Length; iterations++)
                        {
                            arrayItems[iterations].localScale =
                                Vector3.Lerp(arrayItems[iterations].localScale,
                                scaleVector, cachedTime);

                            arrayItems[iterations].anchoredPosition =
                                Vector2.Lerp(arrayItems[iterations].anchoredPosition,
                                arrayPositionsForItems[iterations].anchoredPosition, cachedTime);
                        }
                        yield return Timing.WaitForOneFrame;
                    }
                    SetActiveForLeftButtons(true);
                }
                else
                {
                    Vector2 cashedTransformPosition = leftItemsParent.transform.position;

                    SetActiveForLeftButtons(false);
                    while (Mathf.Abs(Vector2.Distance(cashedTransformItem.position, cashedTransformPosition)) >= 3)
                    {
                        i++;
                        cachedTime = Time.deltaTime * i;
                        for (iterations = 0; iterations < arrayItems.Length; iterations++)
                        {
                            arrayItems[iterations].localScale =
                                Vector3.Lerp(arrayItems[iterations].localScale,
                                Vector3.zero, cachedTime);

                            arrayItems[iterations].anchoredPosition =
                                Vector2.Lerp(arrayItems[iterations].anchoredPosition,
                                Vector2.zero, cachedTime);
                        }
                        yield return Timing.WaitForOneFrame;
                    }
                    SetActiveForLeftIndicators(true);
                }
            }
            else
            {
                Transform cashedTransformItem = arraySkills[i];
                if (isOpen)
                {
                    Vector2 cashedTransformPosition = arrayPositionsForSkills[i].transform.position;
                    Vector3 scaleVector = new Vector3(1, 1, 1);

                    SetActiveForRightIndicators(false);
                    while (Mathf.Abs(Vector2.Distance(cashedTransformItem.position, cashedTransformPosition)) >= 1)
                    {
                        i++;
                        cachedTime = Time.deltaTime * i;
                        for (iterations = 0; iterations < arraySkills.Length; iterations++)
                        {
                            arraySkills[iterations].localScale =
                                Vector3.Lerp(arraySkills[iterations].localScale,
                                scaleVector, cachedTime);

                            arraySkills[iterations].anchoredPosition =
                                Vector2.Lerp(arraySkills[iterations].anchoredPosition,
                                arrayPositionsForSkills[iterations].anchoredPosition, cachedTime);
                        }
                        yield return Timing.WaitForOneFrame;
                    }
                    SetActiveForRightButtons(true);
                }
                else
                {
                    Vector2 cashedTransformPosition = rightSkillsParent.transform.position;

                    SetActiveForRightButtons(false);
                    while (Mathf.Abs(Vector2.Distance(cashedTransformItem.position, cashedTransformPosition)) >= 3)
                    {
                        i++;
                        cachedTime = Time.deltaTime * i;
                        for (iterations = 0; iterations < arraySkills.Length; iterations++)
                        {
                            arraySkills[iterations].localScale =
                                Vector3.Lerp(arraySkills[iterations].localScale,
                                Vector3.zero, cachedTime);

                            arraySkills[iterations].anchoredPosition =
                                Vector2.Lerp(arraySkills[iterations].anchoredPosition,
                                Vector2.zero, cachedTime);
                        }
                        yield return Timing.WaitForOneFrame;
                    }
                    SetActiveForRightIndicators(true);
                }
            }
        }

        /// <summary>
        /// Закрыть все окна
        /// </summary>
        public void OnClickCloseAllWindows()
        {
            bool flag = false;

            if (isLeftInventoryOpen)
            {
                flag = true;
                if (leftCoroutine != null)
                    StopCoroutine(leftCoroutine);

                isLeftInventoryOpen = false;
                leftCoroutine =
                    StartCoroutine(CoroutineForMoveInventoryWindow(true, false));
            }

            if (isRightInventoryOpen)
            {
                flag = true;
                if (rightCoroutine != null)
                    StopCoroutine(rightCoroutine);

                isRightInventoryOpen = false;
                rightCoroutine =
                    StartCoroutine(CoroutineForMoveInventoryWindow(false, false));
            }

            if (flag)
                playerComponentsControl.PlayerHUDAudioStorage.PlaySoundSwipeInventory(false);
        }

        /// <summary>
        /// Отключать инвентари во время игры
        /// </summary>
        /// <returns></returns>
        private IEnumerator<float> EventPlayMode()
        {
            while (playerComponentsControl.PlayerConditions.IsAlive)
            {
                if (Joystick.IsPlaying)
                {
                    OnClickCloseAllWindows();
                }
                yield return Timing.WaitForSeconds(1);
            }
        }
        #endregion
    }

    /// <summary>
    /// Перечислитель для классов вещей
    /// </summary>
    public enum PlayerItems
    {
        HealthItem,
        SpeedItem,
        PowerItem
    }
}

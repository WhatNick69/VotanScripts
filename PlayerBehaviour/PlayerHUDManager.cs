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
        private RectTransform[] arrayItemTransform;
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
        private RectTransform[] arraySkillTransform;
        private Image[] arrayRightIndicators;

        private Coroutine rightCoroutine;
        private Coroutine leftCoroutine;

        private bool isRightInventoryOpen;
        private bool isLeftInventoryOpen;
        #endregion

        #region Содержимое инвентаря
        private IItem[] itemsList;
        private ISkill[] skillsList;
        private PlayerComponentsControl playerComponentsControl;
        private bool isActiveItemButton = true;
        private bool isActiveSkillButton = true;

        private string animationIndicator;
        private Color alphaColor;
        #endregion

        #region Свойства
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
            InitialisationStartArrays();
            InitialisationsLeftInventory();
            InitialisationRightInventory();
        }

        /// <summary>
        /// Инициализация начальных массивов со ссылками
        /// </summary>
        private void InitialisationStartArrays()
        {
            animationIndicator = "HUDIndicatorEnabled";
            alphaColor = new Color(0.5f, 0.5f, 0.5f, 0);

            InitialisationAllIndicators();
            InitialisationAllPositions();
        }

        #region Левый инвентарь: предметы
        /// <summary>
        /// Удалить ссылку на интерфейс предмета из инвентаря.
        /// </summary>
        /// <param name="number"></param>
        public void DeleteItemInterfaceReference(IItem item)
        {
            for (int i = 0; i < itemsList.Length; i++)
            {
                if (item == itemsList[i])
                {
                    itemsList[i] = null;
                    arrayItemTransform[i] = null;
                    arrayLeftIndicators[i].color = alphaColor;
                    return;
                }
            }
        }

        /// <summary>
        /// Возвращает число ненулевых 
        /// элементов в инвентаре предметов
        /// </summary>
        /// <returns></returns>
        public int GetCountOfItems()
        {
            int counter = 0;
            for (int i = 0; i < itemsList.Length; i++)
                if (itemsList[i] != null) counter++;

            return counter;
        }

        /// <summary>
        /// Метод для проверки, содержится ли данный
        /// предмет в левом инвентаре и в случае положительного
        /// результата - мы повышаем его количество на величину,
        /// которая содержится в передаваемом предмете
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool EqualsItemInLeftInventory(IItem item)
        {
            for (int i = 0; i < itemsList.Length; i++)
            {
                if (itemsList[i] != null)
                {
                    if (itemsList[i].ItemType == item.ItemType
                        && itemsList[i].ItemQuality == item.ItemQuality)
                    {
                        itemsList[i].ItemCount += item.ItemCount;
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Добавить предмет в инвентарь
        /// </summary>
        /// <param name="item"></param>
        public void AddItem(GameObject item)
        {
            for (int i = 0; i < itemsList.Length; i++)
            {
                if (itemsList[i] == null)
                {
                    itemsList[i] = item.GetComponent<IItem>();
                    arrayItemTransform[i] = item.GetComponent<RectTransform>();
                    itemsList[i].Starter(i);
                    RefreshVisibleItemsInLeftInventory(i);
                    return;
                }
            }
        }

        /// <summary>
        /// Если открыто окно и у нас появился 
        /// новый предмет в левом инвентаре -  
        /// незамедлительно отображаем его.
        /// </summary>
        /// <param name="numberInLeftInventory"></param>
        private void RefreshVisibleItemsInLeftInventory(int numberInLeftInventory)
        {
            if (isLeftInventoryOpen)
            {
                arrayItemTransform[numberInLeftInventory].localScale 
                    = new Vector3(1, 1, 1);
                arrayItemTransform[numberInLeftInventory].anchoredPosition =
                    arrayPositionsForItems[numberInLeftInventory].anchoredPosition;
            }
        }

        /// <summary>
        /// Добавить умение в инвентарь
        /// </summary>
        /// <param name="skill"></param>
        public void AddSkill(GameObject skill)
        {
            for (int i = 0; i < skillsList.Length; i++)
            {
                if (skillsList[i] == null)
                {
                    skillsList[i] = skill.GetComponent<ISkill>();
                    arraySkillTransform[i] = skill.GetComponent<RectTransform>();
                    skillsList[i].Starter(i);
                    return;
                }
            }
        }

        /// <summary>
        /// Инициализация левого инвентаря
        /// </summary>
        private void InitialisationsLeftInventory()
        {
            itemsList = new IItem[arrayLeftIndicators.Length];
            arrayItemTransform = new RectTransform[itemsList.Length];

            for (int i = 0; i < itemsList.Length; i++)
            {
                if (leftItemsParent.childCount > i)
                { 
                    itemsList[i] = leftItemsParent.GetChild(i).GetComponent<IItem>();
                    itemsList[i].Starter(i);
                    arrayItemTransform[i] = leftItemsParent.GetChild(i).GetComponent<RectTransform>();
                }
            }

            if (GetCountOfItems()==0) isLeftInventoryOpen = false;
        }

        /// <summary>
        /// Включить левые индикаторы
        /// </summary>
        public void EnableLeftIndicators()
        {
            if (GetCountOfItems()==1)
            {
                for (int i = 0; i < arrayLeftIndicators.Length; i++)
                    arrayLeftIndicators[i].enabled = true;
            }
        }

        /// <summary>
        /// Установить позицию левому индикатору
        /// </summary>
        /// <param name="indicatorPosition"></param>
        public void SetPositionToLeftIndicator(int indicatorPosition)
        {
            if (arrayLeftIndicators == null) InitialisationStartArrays();

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

            if (GetCountOfItems() > 0)
            {
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
            else
            {
                playerComponentsControl.PlayerHUDAudioStorage.PlaySoundImpossibleClick();
            }
        }

        /// <summary>
        /// Установить состояние для левых кнопок
        /// </summary>
        /// <param name="active"></param>
        private void SetActiveForLeftButtons(bool active)
        {
            for (int i = 0; i < arrayItemTransform.Length; i++)
            {
                if (arrayItemTransform[i] != null)
                {
                    arrayItemTransform[i].GetComponent<Button>().enabled = active;
                }
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
        public void FireItem(IItem item)
        {
            for (int i = 0;i<itemsList.Length;i++)
            {
                if (itemsList[i]==item)
                {
                    if (itemsList[i].PlayerComponentsControlInstance == null)
                        itemsList[i].PlayerComponentsControlInstance = playerComponentsControl;
                    itemsList[i].FireEventItem();
                }
            }
        }
        #endregion

        #region Правый инвентарь: умения
        /// <summary>
        /// Удалить ссылку на интерфейс умения из инвентаря.
        /// </summary>
        /// <param name="number"></param>
        public void DeleteSkillInterfaceReference(ISkill skill)
        {
            for (int i = 0; i < skillsList.Length; i++)
            {
                if (skill == skillsList[i])
                {
                    skillsList[i] = null;
                    arraySkillTransform[i] = null;
                    arrayRightIndicators[i].color = alphaColor;
                    return;
                }
            }
        }

        /// <summary>
        /// Возвращает число ненулевых 
        /// элементов в инвентаре умений
        /// </summary>
        /// <returns></returns>
        public int GetCountOfSkills()
        {
            int counter = 0;
            for (int i = 0; i < skillsList.Length; i++)
                if (skillsList[i] != null) counter++;

            return counter;
        }

        /// <summary>
        /// Инициализация правого инвентаря
        /// </summary>
        private void InitialisationRightInventory()
        {
            skillsList = new ISkill[arrayRightIndicators.Length];
            arraySkillTransform = new RectTransform[skillsList.Length];

            for (int i = 0; i < skillsList.Length; i++)
            {
                if (rightSkillsParent.childCount > i)
                {
                    skillsList[i] = rightSkillsParent.GetChild(i).GetComponent<ISkill>();
                    skillsList[i].Starter(i);
                    arraySkillTransform[i] = rightSkillsParent.GetChild(i).GetComponent<RectTransform>();
                }
            }

            if (GetCountOfSkills()==0) isRightInventoryOpen = false;
        }
 
        /// <summary>
        /// Установить позицию правому индикатору
        /// </summary>
        /// <param name="indicatorPosition"></param>
        public void SetPositionToRightIndicator(int indicatorPosition)
        {
            if (arrayRightIndicators == null) InitialisationStartArrays();

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

            if (GetCountOfSkills() > 0)
            {
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
            else
            {
                playerComponentsControl.PlayerHUDAudioStorage.PlaySoundImpossibleClick();
            }
        }

        /// <summary>
        /// Зажечь событие кнопки инвентаря умений
        /// </summary>
        /// <param name="numberButton"></param>
        public void FireSkill(ISkill skill)
        {
            for (int i = 0; i < skillsList.Length; i++)
            {
                if (skillsList[i] == skill)
                {
                    if (skillsList[i].PlayerComponentsControlInstance == null)
                        skillsList[i].PlayerComponentsControlInstance = playerComponentsControl;
                    skillsList[i].FireEventSkill();
                }
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
            for (int i = 0; i < arraySkillTransform.Length; i++)
            {
                if (arraySkillTransform[i] != null)
                {
                    arraySkillTransform[i].GetComponent<Button>().enabled = active;
                }
            }
        }
        #endregion

        #region Методы, общие для инвентарей
        /// <summary>
        /// Скрыть индикаторы
        /// </summary>
        public void InitialisationAllIndicators()
        {           
            arrayLeftIndicators = new Image[leftIndicators.transform.childCount];
            arrayRightIndicators = new Image[rightIndicators.transform.childCount];

            for (int i = 0; i < arrayLeftIndicators.Length; i++)
            {
                arrayLeftIndicators[i] = leftIndicators.GetChild(i).GetComponent<Image>();
                arrayRightIndicators[i] = rightIndicators.GetChild(i).GetComponent<Image>();
            }
        }

        /// <summary>
        /// Инициализация всех позиций
        /// </summary>
        private void InitialisationAllPositions()
        {
            arrayPositionsForItems = new RectTransform[leftPositionsParent.transform.childCount];
            for (int i = 0; i < arrayPositionsForItems.Length; i++)
                arrayPositionsForItems[i] =
                    leftPositionsParent.GetChild(i).GetComponent<RectTransform>();

            arrayPositionsForSkills = new RectTransform[rightPositionsParent.transform.childCount];
            for (int i = 0; i < arrayPositionsForSkills.Length; i++)
                arrayPositionsForSkills[i] =
                    rightPositionsParent.GetChild(i).GetComponent<RectTransform>();
        }

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
                Transform cashedTransformItem = arrayItemTransform[i];
                if (isOpen)
                {
                    Vector2 cashedTransformPosition = arrayPositionsForItems[i].transform.position;
                    Vector3 scaleVector = new Vector3(1, 1, 1);

                    SetActiveForLeftIndicators(false);
                    while (Mathf.Abs(Vector2.Distance(cashedTransformItem.position, cashedTransformPosition)) >= 1)
                    {
                        i++;
                        cachedTime = Time.deltaTime * i;
                        for (iterations = 0; iterations < arrayItemTransform.Length; iterations++)
                        {
                            if (arrayItemTransform[iterations] != null)
                            {
                                arrayItemTransform[iterations].localScale =
                                    Vector3.Lerp(arrayItemTransform[iterations].localScale,
                                    scaleVector, cachedTime);

                                arrayItemTransform[iterations].anchoredPosition =
                                    Vector2.Lerp(arrayItemTransform[iterations].anchoredPosition,
                                    arrayPositionsForItems[iterations].anchoredPosition, cachedTime);
                            }
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
                        for (iterations = 0; iterations < arrayItemTransform.Length; iterations++)
                        {
                            if (arrayItemTransform[iterations] != null)
                            {
                                arrayItemTransform[iterations].localScale =
                                Vector3.Lerp(arrayItemTransform[iterations].localScale,
                                Vector3.zero, cachedTime);

                                arrayItemTransform[iterations].anchoredPosition =
                                    Vector2.Lerp(arrayItemTransform[iterations].anchoredPosition,
                                    Vector2.zero, cachedTime);
                            }
                        }
                        yield return Timing.WaitForOneFrame;
                    }
                    SetActiveForLeftIndicators(true);
                }
            }
            else
            {
                Transform cashedTransformItem = arraySkillTransform[i];
                if (isOpen)
                {
                    Vector2 cashedTransformPosition = arrayPositionsForSkills[i].transform.position;
                    Vector3 scaleVector = new Vector3(1, 1, 1);

                    SetActiveForRightIndicators(false);
                    while (Mathf.Abs(Vector2.Distance
                        (cashedTransformItem.position, cashedTransformPosition)) >= 1)
                    {
                        i++;
                        cachedTime = Time.deltaTime * i;
                        for (iterations = 0; iterations < arraySkillTransform.Length; iterations++)
                        {
                            if (arraySkillTransform[iterations] != null)
                            {
                                arraySkillTransform[iterations].localScale =
                                Vector3.Lerp(arraySkillTransform[iterations].localScale,
                                scaleVector, cachedTime);

                                arraySkillTransform[iterations].anchoredPosition =
                                    Vector2.Lerp(arraySkillTransform[iterations].anchoredPosition,
                                    arrayPositionsForSkills[iterations].anchoredPosition, cachedTime);
                            }
                        }
                        yield return Timing.WaitForOneFrame;
                    }
                    SetActiveForRightButtons(true);
                }
                else
                {
                    Vector2 cashedTransformPosition = rightSkillsParent.transform.position;

                    SetActiveForRightButtons(false);
                    while (Mathf.Abs(Vector2.Distance
                        (cashedTransformItem.position, cashedTransformPosition)) >= 3)
                    {
                        i++;
                        cachedTime = Time.deltaTime * i;
                        for (iterations = 0; iterations < arraySkillTransform.Length; iterations++)
                        {
                            if (arraySkillTransform[iterations] != null)
                            {
                                arraySkillTransform[iterations].localScale =
                                Vector3.Lerp(arraySkillTransform[iterations].localScale,
                                Vector3.zero, cachedTime);

                                arraySkillTransform[iterations].anchoredPosition =
                                    Vector2.Lerp(arraySkillTransform[iterations].anchoredPosition,
                                    Vector2.zero, cachedTime);
                            }
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

                if (GetCountOfItems()>0)
                {
                    isLeftInventoryOpen = false;
                    leftCoroutine =
                        StartCoroutine(CoroutineForMoveInventoryWindow(true, false));
                }
            }

            if (isRightInventoryOpen)
            {
                flag = true;
                if (rightCoroutine != null)
                    StopCoroutine(rightCoroutine);

                if (GetCountOfSkills() > 0)
                {
                    isRightInventoryOpen = false;
                    rightCoroutine =
                        StartCoroutine(CoroutineForMoveInventoryWindow(false, false));
                }
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
}

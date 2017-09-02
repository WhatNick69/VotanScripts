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

        [Header("Инвентарь предметов")]
        [SerializeField, Tooltip("Кнопка предметов")]
        private RectTransform itemButton;
        [SerializeField, Tooltip("Хранитель зелий")]
        private RectTransform itemsParent;
        [SerializeField, Tooltip("Хранитель позиций")]
        private RectTransform itemPositionsParent;
        [SerializeField, Tooltip("Индикаторы предметов")]
        private RectTransform itemIndicators;
        private RectTransform[] arrayPositionsForItems;
        private RectTransform[] arrayItemTransform;
        private Image[] arrayItemIndicators;

        [Header("Инвентарь умений")]
        [SerializeField, Tooltip("Кнопка умений")]
        private RectTransform skillButton;
        [SerializeField, Tooltip("Хранитель умений")]
        private RectTransform skillsParent;
        [SerializeField, Tooltip("Хранитель позиций")]
        private RectTransform skillPositionsParent;
        [SerializeField, Tooltip("Индикаторы умений")]
        private RectTransform skillIndicators;
        private RectTransform[] arrayPositionsForSkills;
        private RectTransform[] arraySkillTransform;
        private Image[] arraySkillIndicators;

        private Coroutine skillCoroutine;
        private Coroutine itemCoroutine;

        private bool isSkillInventoryOpen;
        private bool isItemInventoryOpen;
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
        public RectTransform ItemsParent
        {
            get
            {
                return itemsParent;
            }

            set
            {
                itemsParent = value;
            }
        }

        public RectTransform SkillsParent
        {
            get
            {
                return skillsParent;
            }

            set
            {
                skillsParent = value;
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
            InitialisationsItemInventory();
            InitialisationSkillInventory();
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
                    arrayItemIndicators[i].color = alphaColor;
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
        public bool EqualsItemInItemInventory(IItem item)
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
                    RefreshVisibleItemsInItemInventory(i);
                    return;
                }
            }
        }

        /// <summary>
        /// Если открыто окно и у нас появился 
        /// новый предмет в левом инвентаре -  
        /// незамедлительно отображаем его.
        /// </summary>
        /// <param name="numberInInventory"></param>
        private void RefreshVisibleItemsInItemInventory(int numberInInventory)
        {
            if (isItemInventoryOpen)
            {
                arrayItemTransform[numberInInventory].localScale 
                    = new Vector3(1, 1, 1);
                arrayItemTransform[numberInInventory].anchoredPosition =
                    arrayPositionsForItems[numberInInventory].anchoredPosition;
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
        private void InitialisationsItemInventory()
        {
            itemsList = new IItem[arrayItemIndicators.Length];
            arrayItemTransform = new RectTransform[itemsList.Length];

            for (int i = 0; i < itemsList.Length; i++)
            {
                if (itemsParent.childCount > i)
                { 
                    itemsList[i] = itemsParent.GetChild(i).GetComponent<IItem>();
                    itemsList[i].Starter(i);
                    arrayItemTransform[i] = itemsParent.GetChild(i).GetComponent<RectTransform>();
                }
            }

            if (GetCountOfItems()==0) isItemInventoryOpen = false;
        }

        /// <summary>
        /// Включить левые индикаторы
        /// </summary>
        public void EnableItemIndicators()
        {
            if (GetCountOfItems()==1)
            {
                for (int i = 0; i < arrayItemIndicators.Length; i++)
                    arrayItemIndicators[i].enabled = true;
            }
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
                if (arrayItemIndicators[indicatorPosition].color.r != activeIndicatorColor.r)
                {
                    arrayItemIndicators[indicatorPosition].color = activeIndicatorColor;
                    arrayItemIndicators[indicatorPosition].GetComponent<Animation>().enabled = true;
                }
            }
            else
            {          
                if (arrayItemIndicators[indicatorPosition].color.r == activeIndicatorColor.r)
                {
                    arrayItemIndicators[indicatorPosition].color = unActiveIndicatorColor;
                    arrayItemIndicators[indicatorPosition].GetComponent<Animation>().enabled = false;
                }
               
            }
        }

        /// <summary>
        /// Открыть все инвентари
        /// </summary>
        public void OnClickOpenAllInventories()
        {
            if (itemCoroutine != null)
                StopCoroutine(itemCoroutine);
            if (skillCoroutine != null)
                StopCoroutine(skillCoroutine);

            if (GetCountOfItems() > 0)
            {
                if (!isItemInventoryOpen)
                {
                    playerComponentsControl.PlayerHUDAudioStorage.PlaySoundSwipeInventory(true);
                    isItemInventoryOpen = true;
                    itemCoroutine =
                        StartCoroutine(CoroutineForMoveInventoryWindow(true, true));
                }
                else
                {
                    playerComponentsControl.PlayerHUDAudioStorage.PlaySoundSwipeInventory(false);
                    isItemInventoryOpen = false;
                    itemCoroutine =
                        StartCoroutine(CoroutineForMoveInventoryWindow(true, false));
                }
            }
            if (GetCountOfSkills() > 0)
            {
                if (!isSkillInventoryOpen)
                {
                    playerComponentsControl.PlayerHUDAudioStorage.PlaySoundSwipeInventory(true);
                    isSkillInventoryOpen = true;
                    skillCoroutine =
                        StartCoroutine(CoroutineForMoveInventoryWindow(false, true));
                }
                else
                {
                    playerComponentsControl.PlayerHUDAudioStorage.PlaySoundSwipeInventory(false);
                    isSkillInventoryOpen = false;
                    skillCoroutine =
                        StartCoroutine(CoroutineForMoveInventoryWindow(false, false));
                }
            }

            if (GetCountOfSkills() == 0 && GetCountOfItems() == 0)
                playerComponentsControl.PlayerHUDAudioStorage.PlaySoundImpossibleClick();
        }

        /// <summary>
        /// Установить состояние для левых кнопок
        /// </summary>
        /// <param name="active"></param>
        private void SetActiveForItemButtons(bool active)
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
        private void SetActiveForItemIndicators(bool active)
        {
            for (int i = 0; i < arrayItemIndicators.Length; i++)
            {
                arrayItemIndicators[i].enabled = active;
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
                    arraySkillIndicators[i].color = alphaColor;
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
        private void InitialisationSkillInventory()
        {
            skillsList = new ISkill[arraySkillIndicators.Length];
            arraySkillTransform = new RectTransform[skillsList.Length];

            for (int i = 0; i < skillsList.Length; i++)
            {
                if (skillsParent.childCount > i)
                {
                    skillsList[i] = skillsParent.GetChild(i).GetComponent<ISkill>();
                    skillsList[i].Starter(i);
                    arraySkillTransform[i] = skillsParent.GetChild(i).GetComponent<RectTransform>();
                }
            }

            if (GetCountOfSkills()==0) isSkillInventoryOpen = false;
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
                if (arraySkillIndicators[indicatorPosition].color.r != activeIndicatorColor.r)
                {
                    arraySkillIndicators[indicatorPosition].color = activeIndicatorColor;
                    arraySkillIndicators[indicatorPosition].GetComponent<Animation>().enabled = true;
                }
            }
            else
            {
                if (arraySkillIndicators[indicatorPosition].color.r == activeIndicatorColor.r)
                {
                    arraySkillIndicators[indicatorPosition].color = unActiveIndicatorColor;
                    arraySkillIndicators[indicatorPosition].GetComponent<Animation>().enabled = false;
                }
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
        private void SetActiveForSkillIndicators(bool active)
        {
            for (int i = 0; i < arraySkillIndicators.Length; i++)
            {
                arraySkillIndicators[i].enabled = active;
            }
        }

        /// <summary>
        /// Установить состояние для левых кнопок
        /// </summary>
        /// <param name="active"></param>
        private void SetActiveForSkillButtons(bool active)
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
            arrayItemIndicators = new Image[itemIndicators.transform.childCount];
            arraySkillIndicators = new Image[skillIndicators.transform.childCount];

            for (int i = 0; i < arrayItemIndicators.Length; i++)
            {
                arrayItemIndicators[i] = itemIndicators.GetChild(i).GetComponent<Image>();
                arraySkillIndicators[i] = skillIndicators.GetChild(i).GetComponent<Image>();
            }
        }

        /// <summary>
        /// Инициализация всех позиций
        /// </summary>
        private void InitialisationAllPositions()
        {
            arrayPositionsForItems = new RectTransform[itemPositionsParent.transform.childCount];
            for (int i = 0; i < arrayPositionsForItems.Length; i++)
                arrayPositionsForItems[i] =
                    itemPositionsParent.GetChild(i).GetComponent<RectTransform>();

            arrayPositionsForSkills = new RectTransform[skillPositionsParent.transform.childCount];
            for (int i = 0; i < arrayPositionsForSkills.Length; i++)
                arrayPositionsForSkills[i] =
                    skillPositionsParent.GetChild(i).GetComponent<RectTransform>();
        }

        /// <summary>
        /// Корутина для движения окна с инвентарем
        /// </summary>
        /// <param name="isItemInventory"></param>
        /// <param name="isOpen"></param>
        /// <returns></returns>
        private IEnumerator<float> CoroutineForMoveInventoryWindow
            (bool isItemInventory, bool isOpen)
        {
            int i = 0;
            int iterations = 0;
            int iteration = 0;
            float cachedTime;

            if (isItemInventory)
            {
                RectTransform cashedTransformItem = null;
                for (iteration = 0; iteration < arrayItemTransform.Length; iteration++)
                {
                    if (arrayItemTransform[iteration] != null)
                    {
                        cashedTransformItem = arrayItemTransform[iteration];
                        break;
                    }                     
                }
                if (cashedTransformItem == null) yield break;

                if (isOpen)
                {
                    Vector2 cashedTransformPosition = 
                        arrayPositionsForItems[iteration].position;
                    Vector3 scaleVector = new Vector3(1, 1, 1);

                    SetActiveForItemIndicators(false);
                    while (Vector2.Distance
                        (cashedTransformItem.position, cashedTransformPosition) >= 1)
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
                                    Vector2.Lerp(arrayItemTransform[iterations].
                                    anchoredPosition,
                                    arrayPositionsForItems[iterations]
                                    .anchoredPosition, cachedTime);
                            }
                        }
                        yield return Timing.WaitForOneFrame;
                    }
                    SetActiveForItemButtons(true);
                }
                else
                {
                    Vector2 cashedTransformPosition = itemsParent.position;

                    SetActiveForItemButtons(false);
                    while (Vector2.Distance
                        (cashedTransformItem.position, cashedTransformPosition) >= 3)
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
                    SetActiveForItemIndicators(true);
                }
            }
            else
            {
                RectTransform cashedTransformItem = null;
                for (iteration = 0; iteration < arraySkillTransform.Length; iteration++)
                {
                    if (arraySkillTransform[iteration] != null)
                    {
                        cashedTransformItem = arraySkillTransform[iteration];
                        break;
                    }
                }
                if (cashedTransformItem == null) yield break;

                if (isOpen)
                {
                    Vector2 cashedTransformPosition = 
                        arrayPositionsForSkills[iteration].position;
                    Vector3 scaleVector = new Vector3(1, 1, 1);

                    SetActiveForSkillIndicators(false);
                    while (Vector2.Distance
                        (cashedTransformItem.position, cashedTransformPosition) >= 1)
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
                                    Vector2.Lerp(arraySkillTransform[iterations]
                                    .anchoredPosition,
                                    arrayPositionsForSkills[iterations]
                                    .anchoredPosition, cachedTime);
                            }
                        }
                        yield return Timing.WaitForOneFrame;
                    }
                    SetActiveForSkillButtons(true);
                }
                else
                {
                    Vector2 cashedTransformPosition = skillsParent.position;

                    SetActiveForSkillButtons(false);
                    while (Vector2.Distance
                        (cashedTransformItem.position, cashedTransformPosition) >= 3)
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
                    SetActiveForSkillIndicators(true);
                }
            }
        }

        /// <summary>
        /// Закрыть все окна
        /// </summary>
        public void OnClickCloseAllWindows()
        {
            bool flag = false;

            if (isItemInventoryOpen)
            {
                flag = true;
                if (itemCoroutine != null)
                    StopCoroutine(itemCoroutine);

                if (GetCountOfItems()>0)
                {
                    isItemInventoryOpen = false;
                    itemCoroutine =
                        StartCoroutine(CoroutineForMoveInventoryWindow(true, false));
                }
            }

            if (isSkillInventoryOpen)
            {
                flag = true;
                if (skillCoroutine != null)
                    StopCoroutine(skillCoroutine);

                if (GetCountOfSkills() > 0)
                {
                    isSkillInventoryOpen = false;
                    skillCoroutine =
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

using MovementEffects;
using System;
using UnityEngine;
using VotanInterfaces;
using VotanLibraries;
using System.Collections.Generic;
using VotanGameplay;
using PlayerBehaviour;

namespace GameBehaviour
{
    /// <summary>
    /// Класс, который случайным образом выбрасывает предмет/умение
    /// </summary>
    public class RandomInGameDrop
        : MonoBehaviour
    {
        #region Переменные
        [SerializeField,Tooltip("Этот объект генерит только предметы?")]
        private bool onlyItems;
        [SerializeField, Tooltip("Компонент-звук")]
        private AudioSource audioSource;
        private BoxCollider boxCollider;
        private SpriteRenderer rendering;
        private ItemType itemType;
        private ItemQuality itemQuality;

        private const string prefixHealthItem = "ItemHealth_";
        private const string prefixPowerItem = "ItemPower_";
        private const string prefixSpeedItem = "ItemSpeed_";

        private const string prefixHealthSkill = "SkillHealth_";
        private const string prefixPowerSkill = "SkillPower_";
        private const string prefixSpeedSkill = "SkilSpeed_";

        private bool isItemActive;
        [SerializeField]
        private GameObject item;
        IItem iItem;
        private Vector3 localComponentStartRotation;
        #endregion

        /// <summary>
        /// Инициализация
        /// </summary>
        private void Start()
        {
            rendering = GetComponent<SpriteRenderer>();
            isItemActive = true;
            boxCollider = GetComponent<BoxCollider>();
            localComponentStartRotation = transform.localEulerAngles;

            RandomChoice();
        }

        /// <summary>
        /// Случайный дроп
        /// </summary>
        private void RandomChoice()
        {
            itemType = (ItemType)UnityEngine.Random.Range
                (0, Enum.GetNames(typeof(ItemType)).Length);
            itemQuality = LibraryStaticFunctions.RandomDropItemQuality();
            itemType = ItemType.HealthItem;
            itemQuality = ItemQuality.Lite;

            switch (itemType)
            {
                case ItemType.HealthItem:
                    item = (GameObject)Resources.Load
                        ("Prefabs/Items/Health/"+prefixHealthItem+itemQuality);
                    break;
                case ItemType.SpeedItem:
                    item = (GameObject)Resources.Load
                        ("Prefabs/Items/Speed/" + prefixSpeedItem + itemQuality);
                    break;
                case ItemType.PowerItem:
                    item = (GameObject)Resources.Load
                        ("Prefabs/Items/Power/" + prefixPowerItem + itemQuality);
                    break;
            }
            iItem = item.GetComponent<IItem>(); // получаем интерфейс
        }

        /// <summary>
        /// Установить звук поднятия предмета в зависимости от предмета
        /// </summary>
        private void SetAudioDependenceItemQuality()
        {
            object[] audio;
            if ((int)itemQuality >= 4)
            {
                audio = Resources.LoadAll("Sounds/Common/PickUps/Paper");
                audioSource.clip = audio[UnityEngine.Random.Range(0, audio.Length)] 
                    as AudioClip;
            }
            else
            {
                audio = Resources.LoadAll("Sounds/Common/PickUps/Bottles");
                audioSource.clip = audio[UnityEngine.Random.Range(0, audio.Length)]
                    as AudioClip;
            }
        }

        /// <summary>
        /// Запуск рендеринга.
        /// </summary>
        public void StartRendering()
        {
            rendering.enabled = true;
            rendering.sprite = iItem.ItemImage.sprite;
            Timing.RunCoroutine(CoroutineForDetectPlayer());
        }

        /// <summary>
        /// Запуск корутин.
        /// </summary>
        public void StartCoroutines()
        {
            if (boxCollider != null) boxCollider.enabled = false;
            Timing.RunCoroutine(CoroutineForItemScale());
            Timing.RunCoroutine(CoroutineForRotate());
        }

        /// <summary>
        /// Корутина на поворот предмета
        /// </summary>
        /// <returns></returns>
        private IEnumerator<float> CoroutineForRotate()
        {
            Vector3 rotateVector = new Vector3(0, 10, 0);
            while (isItemActive)
            {
                if (this == null) yield break;

                transform.Rotate(rotateVector);
                yield return Timing.WaitForSeconds(0.05f);
            }
        }

        /// <summary>
        /// Корутина на увеличение размера предмета.
        /// </summary>
        /// <returns></returns>
        private IEnumerator<float> CoroutineForItemScale()
        {
            Vector3 localScaleVector = new Vector3(0.75f, 0.75f, 0.75f);
            Vector3 translateVector = new Vector3(0, 0.5f, 0);
            transform.localEulerAngles = localComponentStartRotation;

            int i = 0;
            while (this != null
                && transform.localScale.x <= 0.7f
                && isItemActive)
            {
                i++;
                transform.localScale = 
                    Vector3.Lerp(transform.localScale, localScaleVector, Time.deltaTime*i);
                transform.Translate(translateVector*Time.deltaTime * i);
                yield return Timing.WaitForSeconds(0.05f);
            }
        }

        /// <summary>
        /// Корутин на обнаружение игрока в зоне досягаемости.
        /// </summary>
        /// <returns></returns>
        private IEnumerator<float> CoroutineForDetectPlayer()
        {
            PlayerComponentsControl tempComponentsControl;
            int tempCallback;

            while (isItemActive)
            {
                yield return Timing.WaitForSeconds(1);
                if (this == null) yield break;

                for (int i = 0;i<AllPlayerManager.PlayerList.Count;i++)
                {
                    tempComponentsControl = AllPlayerManager.GetPlayerComponents(i);
                    if (Vector3.Distance(transform.position,
                        tempComponentsControl.PlayerModel.position) < 2)
                    {
                        tempCallback = PlayerMayGetItem(tempComponentsControl);
                        if (tempCallback == 0)
                        {
                            isItemActive = false;
                            ProcedureForPlayerItem(tempComponentsControl);
                        }
                        // Раскомментировать на случай использования 
                        // количества для каждого предмета
                        //else if (tempCallback == 1)
                        //{
                        //    isItemActive = false;
                        //}
                    }
                }
            }
            rendering.enabled = false;
            PlayPickUpSound();
            yield return Timing.WaitForSeconds(2);
            Destroy(gameObject);
        }

        /// <summary>
        /// 1 - нашли подобный предмет и увеличили его количество.
        /// 0 - подобного предмета не было найдено в инвентаре и мы его добавляем.
        /// -1 - невозможно добавить предмет.
        /// </summary>
        /// <param name="plComponents"></param>
        /// <returns></returns>
        private int PlayerMayGetItem(PlayerComponentsControl plComponents)
        {
            // Раскомментировать на случай использования количества для каждого предмета
            //if (plComponents.PlayerHUDManager.EqualsItemInLeftInventory(iItem))
            //    return 1;

            if (plComponents.PlayerHUDManager.GetCountOfItems() != 3)
            {
                return 0;
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// Процедура для предмета.
        /// </summary>
        /// <param name="plComponents"></param>
        private void ProcedureForPlayerItem(PlayerComponentsControl plComponents)
        {       
            iItem.PlayerComponentsControlInstance = plComponents; // инициализируем компонент-ссылку
            GameObject objectNew = Instantiate(item);

            // даем родителя
            objectNew.transform.SetParent(plComponents.PlayerHUDManager.ItemsParent); 
            objectNew.transform.localPosition = Vector3.zero;  // обнуляем позицию

            plComponents.PlayerHUDManager.AddItem(objectNew);
            plComponents.PlayerHUDManager.EnableItemIndicators();
        }

        /// <summary>
        /// Проиграть звук взятия предмета
        /// </summary>
        private void PlayPickUpSound()
        {
            SetAudioDependenceItemQuality();
            audioSource.volume = 1;
            audioSource.pitch = LibraryStaticFunctions.GetRangeValue(1, 0.1f);
            audioSource.Play();
        }
    }
}

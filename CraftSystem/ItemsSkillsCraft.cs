using MovementEffects;
using ShopSystem;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VotanInterfaces;
using VotanUI;

namespace CraftSystem
{
    /// <summary>
    /// Крафт зелий и умений
    /// </summary>
    public class ItemsSkillsCraft
        : MonoBehaviour
    {
        #region Переменные
        [SerializeField]
        private GameObject itemRepository;
        [SerializeField]
        private GameObject skillsRepository;
        [SerializeField, Tooltip("кнопка скила")]
        private GameObject itemSkill;
        [SerializeField, Tooltip("кнопка итема")]
        private GameObject itemItm;
        [SerializeField]
        GameObject skillWindow;
        [SerializeField]
        Text textCost;
        [SerializeField]
        Button playButton;
        Image imagePlayButton;

        private GameObject skillGamObj;
        private GameObject itemGamObj;

        string skillPrefix = "Prefabs/Skills/";
        string itemPrefix = "Prefabs/Items/";

        private List<ISkill> skillList;
        private List<IItem> itemList;

        [SerializeField, Tooltip("Закинуть сюда все зелья")]
        private List<GameObject> itemArray;

        private int itemItemNumberOne;
        private int itemItemNumberTwo;
        private int itemItemNumberThree;

        private int skillItemNumberOne;
        private int skillItemNumberTwo;
        private int skillItemNumberThree;

        ItemSkillPrefabs itemSkillPrefabs;
        PlayerStats playerStats;
        UserResources userResources;

        ScrollRect scrollRectSkillRepository;
        ScrollRect scrollRectItemRepository;

        private long tempPrice;
        private Color originalColor;
        private Color nonActiveImageColorButton;
        #endregion

        #region Свойства и работа с окнами
        public void CloseAllWindows()
        {
            skillWindow.SetActive(false);
        }

        public void WeaponWindow()
        {
            skillWindow.SetActive(false);
        }

        public void SkillWindow()
        {
            skillWindow.SetActive(true);
        }

        public void ItemWindow()
        {
            skillWindow.SetActive(false);
        }

        public void ClearItemWindow()
        {
            itemItemNumberOne = -1;
            itemItemNumberTwo = -1;
            itemItemNumberThree = -1;

            playerStats.SetItemImg(0);
            playerStats.SetItemImg(1);
            playerStats.SetItemImg(2);

            tempPrice = 0;
            textCost.text = "Cost: " + tempPrice;
            textCost.color = originalColor;
        }
        #endregion

        /// <summary>
        /// Инициализация
        /// </summary>
        private void Awake()
        {
            itemSkillPrefabs = GameObject.Find("GetPrefabs").GetComponent<ItemSkillPrefabs>();
            userResources = GameObject.Find("GetPrefabs").GetComponent<UserResources>();
            playerStats = GetComponent<PlayerStats>();

            skillList = new List<ISkill>();
            itemList = new List<IItem>();
            skillItemNumberOne = -1;
            skillItemNumberTwo = -1;
            skillItemNumberThree = -1;
            itemItemNumberOne = -1;
            itemItemNumberTwo = -1;
            itemItemNumberThree = -1;

            textCost.text = "Cost: " + tempPrice;
            originalColor = textCost.color;
            imagePlayButton = playButton.transform.GetChild(0).GetComponent<Image>();
            nonActiveImageColorButton = new Color(0, 0, 0, 0.5f);

            Timing.RunCoroutine(SkillCorutine());
            Timing.RunCoroutine(ItemsCorutine());
        }

        /// <summary>
        /// Установить номер умения предмета
        /// </summary>
        /// <param name="numberButton"></param>
        public void SetSkillItemNumber(int numberButton)
        {
            if (skillItemNumberOne == -1)
            {
                MenuSoundManager.PlaySoundStatic(1);
                skillItemNumberOne = numberButton;
                playerStats.SetSkillImg(skillList[numberButton].SkillImage.sprite, 0);
            }
            else if (skillItemNumberTwo == -1)
            {
                MenuSoundManager.PlaySoundStatic(1);
                skillItemNumberTwo = numberButton;
                playerStats.SetSkillImg(skillList[numberButton].SkillImage.sprite, 1);
            }
            else if (skillItemNumberThree == -1)
            {
                MenuSoundManager.PlaySoundStatic(1);
                skillItemNumberThree = numberButton;
                playerStats.SetSkillImg(skillList[numberButton].SkillImage.sprite, 2);
            }
        }

        /// <summary>
        /// Установить номер предмета
        /// </summary>
        /// <param name="numberButton"></param>
        public void SetItemNumber(int numberButton)
        {
            if (itemItemNumberOne == -1)
            {
                MenuSoundManager.PlaySoundStatic(1);
                itemItemNumberOne = numberButton;
                playerStats.SetItemImg(itemList[numberButton].ItemImage.sprite, 0);
                MoveItemToCart(itemList[numberButton]);
            }
            else if (itemItemNumberTwo == -1)
            {
                MenuSoundManager.PlaySoundStatic(1);
                itemItemNumberTwo = numberButton;
                playerStats.SetItemImg(itemList[numberButton].ItemImage.sprite, 1);
                MoveItemToCart(itemList[numberButton]);
            }
            else if (itemItemNumberThree == -1)
            {
                MenuSoundManager.PlaySoundStatic(1);
                itemItemNumberThree = numberButton;
                playerStats.SetItemImg(itemList[numberButton].ItemImage.sprite, 2);
                MoveItemToCart(itemList[numberButton]);
            }
        }

        /// <summary>
        /// Вызывать для удаления умения из 1й ячейки 
        /// </summary>
        public void RemoveSkillOne()
        {
            skillItemNumberOne = -1;
            playerStats.SetSkillImg(0);
        }

        /// <summary>
        /// Вызывать для удаления умения из 2й ячейки 
        /// </summary>
        public void RemoveSkillTwo()
        {
            skillItemNumberTwo = -1;
            playerStats.SetSkillImg(1);
        }

        /// <summary>
        /// Вызывать для удаления умения из 3й ячейки 
        /// </summary>
        public void RemoveSkillThree()
        {
            skillItemNumberThree = -1;
            playerStats.SetSkillImg(2);
        }

        /// <summary>
        ///  Вызывать для удаления зелья из 1й ячейки 
        /// </summary>
        public void RemoveItemOne()
        {
            DeleteItemFromCart(itemList[itemItemNumberOne]);
            itemItemNumberOne = -1;
            playerStats.SetItemImg(0);
        }

        /// <summary>
        ///  Вызывать для удаления зелья из 2й ячейки 
        /// </summary>
        public void RemoveItemTwo()
        {
            DeleteItemFromCart(itemList[itemItemNumberTwo]);
            itemItemNumberTwo = -1;
            playerStats.SetItemImg(1);
        }

        /// <summary>
        /// Вызывать для удаления зелья из 3й ячейки 
        /// </summary>
        public void RemoveItemThree()
        {
            DeleteItemFromCart(itemList[itemItemNumberThree]);
            itemItemNumberThree = -1;
            playerStats.SetItemImg(2);
        }

        /// <summary>
        /// Передача предметов и умений игроку
        /// </summary>
        public void PlayArenaItems()
        {
            userResources.Money -= tempPrice;
            tempPrice = 0;

            if (skillItemNumberOne >= 0)
                itemSkillPrefabs.FirstSkill = (GameObject)Resources.Load(skillPrefix + skillItemNumberOne);
            if (skillItemNumberTwo >= 0)
                itemSkillPrefabs.SecondSkill = (GameObject)Resources.Load(skillPrefix + skillItemNumberTwo);
            if (skillItemNumberThree >= 0)
                itemSkillPrefabs.ThirdSkill = (GameObject)Resources.Load(skillPrefix + skillItemNumberThree);
            if (itemItemNumberOne >= 0)
                itemSkillPrefabs.FirstItem = itemArray[itemItemNumberOne];
            if (itemItemNumberTwo >= 0)
                itemSkillPrefabs.SecondItem = itemArray[itemItemNumberTwo];
            if (itemItemNumberThree >= 0)
                itemSkillPrefabs.ThirdItem = itemArray[itemItemNumberThree];
        }

        /// <summary>
        /// Добавить вещь в корзину
        /// </summary>
        /// <param name="item"></param>
        public void MoveItemToCart(IItem item)
        {
            tempPrice += item.PriceGold;
            textCost.text = "Cost: " + tempPrice;
            if (userResources.Money - tempPrice < 0)
            {
                playButton.interactable = false;
                imagePlayButton.color = 
                textCost.color = Color.red;
                imagePlayButton.color = nonActiveImageColorButton;
            }
            else
            {
                if (textCost.color==Color.red)
                {
                    textCost.color = originalColor;
                    playButton.interactable = true;
                    imagePlayButton.color = Color.black;
                }
            }
        }

        /// <summary>
        /// Удалить вещь из корзины
        /// </summary>
        /// <param name="item"></param>
        public void DeleteItemFromCart(IItem item)
        {
            tempPrice -= item.PriceGold;
            textCost.text = "Cost: " + tempPrice;
            if (userResources.Money - tempPrice < 0)
            {
                playButton.interactable = false;
                imagePlayButton.color =
                textCost.color = Color.red;
                imagePlayButton.color = nonActiveImageColorButton;
            }
            else
            {
                if (textCost.color == Color.red)
                {
                    textCost.color = originalColor;
                    playButton.interactable = true;
                    imagePlayButton.color = Color.black;
                }
            }
        }

        /// <summary>
        /// Корутина для загрузки умений
        /// </summary>
        /// <returns></returns>
        private IEnumerator<float> SkillCorutine()
        {
            object[] tempObjects = Resources.LoadAll(skillPrefix);

            for (int i = 0; i < tempObjects.Length; i++)
            {
                GameObject skillGamObj = (GameObject)tempObjects[i];
                skillList.Add(skillGamObj.GetComponent<ISkill>());
                GameObject item = Instantiate(itemSkill);
                ItemOrSkillButton button = item.GetComponent<ItemOrSkillButton>();
                button.SetItemSkillsCraft(this);
                button.SetNumber(i);
                button.NameSkill.text = skillList[i].SkillName;
                button.TutorialSkill.text = skillList[i].SkillTutorial;
                button.SetImage(skillList[i].SkillImage.sprite);
                item.transform.SetParent(skillsRepository.transform, false);
            }
            scrollRectSkillRepository =
                skillsRepository.transform.parent.GetComponent<ScrollRect>();
            yield return Timing.WaitForSeconds(0);
        }

        /// <summary>
        /// Корутина для загрудки предметов
        /// </summary>
        /// <returns></returns>
        private IEnumerator<float> ItemsCorutine()
        {
            int count = itemArray.Count;

            for (int i = 0; i < count; i++)
            {
                GameObject itemGamObj = itemArray[i];
                itemList.Add(itemGamObj.GetComponent<IItem>());
                GameObject item = Instantiate(itemItm);
                ItemOrSkillButton button = item.GetComponent<ItemOrSkillButton>();
                button.SetItemSkillsCraft(this);
                button.SetNumber(i);
                button.NameSkill.text = itemList[i].ItemName;
                button.TutorialSkill.text = itemList[i].ItemTutorial;
                button.SetImage(itemList[i].ItemImage.sprite);
                item.transform.SetParent(itemRepository.transform, false);
            }
            scrollRectSkillRepository =
                skillsRepository.transform.parent.GetComponent<ScrollRect>();
            yield return Timing.WaitForSeconds(0);
        }
    }
}

using MovementEffects;
using ShopSystem;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VotanInterfaces;
using VotanUI;
using VotanLibraries;

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
        string skillPostfix = "_Skill";

        private List<ISkill> skillList;
        private List<IItem> itemList;

        [SerializeField, Tooltip("Закинуть сюда все зелья")]
        private List<GameObject> itemArray;

        private IRepositoryObject[] skillInventoryElements;

        private int itemItemNumberOne;
        private int itemItemNumberTwo;
        private int itemItemNumberThree;

        private int skillItemNumberOne;
        private int skillItemNumberTwo;
        private int skillItemNumberThree;

        Shop shop;
        ItemSkillPrefabs itemSkillPrefabs;
        Inventory inventory;
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

            inventory.ArmorCraftComponent.CloseAllWindows();
            inventory.WeaponCraftComponent.CloseAllWindows();

            playerStats.SkillPage();
            scrollRectSkillRepository.horizontalNormalizedPosition = 0;
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
        private void Start()
        {
            itemSkillPrefabs = GameObject.Find("GetPrefabs").GetComponent<ItemSkillPrefabs>();
            itemSkillPrefabs.ClearAllItems();
            itemSkillPrefabs.ClearAllSkills();
            userResources = GameObject.Find("GetPrefabs").GetComponent<UserResources>();
            playerStats = GetComponent<PlayerStats>();
            inventory = GetComponent<Inventory>();
            shop = GetComponent<Shop>();

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

                Inventory.SaveInventoryNumber(4, skillItemNumberOne);
            }
            else if (skillItemNumberTwo == -1)
            {
                MenuSoundManager.PlaySoundStatic(1);
                skillItemNumberTwo = numberButton;
                playerStats.SetSkillImg(skillList[numberButton].SkillImage.sprite, 1);

                Inventory.SaveInventoryNumber(5, skillItemNumberTwo);
            }
            else if (skillItemNumberThree == -1)
            {
                MenuSoundManager.PlaySoundStatic(1);
                skillItemNumberThree = numberButton;
                playerStats.SetSkillImg(skillList[numberButton].SkillImage.sprite, 2);

                Inventory.SaveInventoryNumber(6, skillItemNumberThree);
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
            if (itemItemNumberOne != -1)
                DeleteItemFromCart(itemList[itemItemNumberOne]);
            itemItemNumberOne = -1;
            playerStats.SetItemImg(0);
            CheckMoney();
        }

        /// <summary>
        ///  Вызывать для удаления зелья из 2й ячейки 
        /// </summary>
        public void RemoveItemTwo()
        {
            if (itemItemNumberTwo != -1)
                DeleteItemFromCart(itemList[itemItemNumberTwo]);
            itemItemNumberTwo = -1;
            playerStats.SetItemImg(1);
            CheckMoney();
        }

        /// <summary>
        /// Вызывать для удаления зелья из 3й ячейки 
        /// </summary>
        public void RemoveItemThree()
        {
            if (itemItemNumberThree != -1)
                DeleteItemFromCart(itemList[itemItemNumberThree]);
            itemItemNumberThree = -1;
            playerStats.SetItemImg(2);
            CheckMoney();
        }

        /// <summary>
        /// Передача предметов и умений игроку
        /// </summary>
        public void PlayArenaItems()
        {
            userResources.Money -= tempPrice;
            tempPrice = 0;

            if (skillItemNumberOne >= 0)
                itemSkillPrefabs.FirstSkill = (GameObject)Resources.Load(skillPrefix + skillItemNumberOne + skillPostfix);
            if (skillItemNumberTwo >= 0)
                itemSkillPrefabs.SecondSkill = (GameObject)Resources.Load(skillPrefix + skillItemNumberTwo + skillPostfix);
            if (skillItemNumberThree >= 0)
                itemSkillPrefabs.ThirdSkill = (GameObject)Resources.Load(skillPrefix + skillItemNumberThree + skillPostfix);
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
            CheckMoney();
        }

        /// <summary>
        /// Проверить деньги пользователя
        /// </summary>
        private void CheckMoney()
        {
            if (!CheckEmptyItems())
            {
                if (userResources.Money - tempPrice < 0)
                {
                    playButton.interactable = false;
                    textCost.color = Color.red;
                    imagePlayButton.color = nonActiveImageColorButton;
                }
                else
                {
                    textCost.color = originalColor;
                    playButton.interactable = true;
                    imagePlayButton.color = Color.black;
                }
            }
            else
            {
                textCost.color = originalColor;
                playButton.interactable = true;
                imagePlayButton.color = Color.black;
            }
        }

        /// <summary>
        /// Проверить, все ли предметы пустые
        /// </summary>
        /// <returns></returns>
        private bool CheckEmptyItems()
        {
            return itemItemNumberOne == -1 
                && itemItemNumberTwo == -1 
                && itemItemNumberThree == -1 
                    ? true : false;
        } 

        /// <summary>
        /// Отключить все элементы инвентаря с подсветкой
        /// </summary>
        public void DisableListHighlightingInventory()
        {
            for (int i = 0; i < skillInventoryElements.Length; i++)
                skillInventoryElements[i].HighlightingControl(false, false);
        }

        /// <summary>
        /// Удалить вещь из корзины
        /// </summary>
        /// <param name="item"></param>
        public void DeleteItemFromCart(IItem item)
        {
            tempPrice -= item.PriceGold;
            textCost.text = "Cost: " + tempPrice;
            CheckMoney();
        }

        /// <summary>
        /// Проверить сохранение умений на пустоту
        /// </summary>
        /// <returns></returns>
        private string CheckEmptySkillLocalSave()
        {
            string str = PlayerPrefs.GetString("skillArray");
            if (str == null || str == "")
            {
                str = "";
                PlayerPrefs.SetString("skillArray", str);
            }
            return str;
        }

        /// <summary>
        /// Обновить окно умений
        /// </summary>
        public void RestartSkillWindow()
        {
            for (int i = 0; i < skillsRepository.transform.childCount; i++)
                Destroy(skillsRepository.transform.GetChild(i).gameObject);

            skillList = new List<ISkill>();
            Timing.RunCoroutine(SkillCorutine());
        }

        /// <summary>
        /// Загрузить окно инвентаря
        /// </summary>
        /// <returns></returns>
        private bool LoadArmorInventory()
        {
            int[] armorNumbers = Inventory.LoadInventoryNumbers();
            if (armorNumbers.Length == 0)
            {
                return false;
            }
            else
            {
                if (armorNumbers[4] != 255)
                {
                    Debug.Log(armorNumbers[4]);
                    skillItemNumberOne = armorNumbers[4];
                    playerStats.SetSkillImg(skillList[skillItemNumberOne].SkillImage.sprite, 0);
                }
                if (armorNumbers[5] != 255)
                {
                    skillItemNumberTwo = armorNumbers[5];
                    playerStats.SetSkillImg(skillList[skillItemNumberTwo].SkillImage.sprite, 1);
                }
                if (armorNumbers[6] != 255)
                {
                    skillItemNumberThree = armorNumbers[6];
                    playerStats.SetSkillImg(skillList[skillItemNumberThree].SkillImage.sprite, 2);
                }

                return true;
            }
        }

        /// <summary>
        /// Корутина для загрузки умений
        /// </summary>
        /// <returns></returns>
        private IEnumerator<float> SkillCorutine()
        {
            string str = CheckEmptySkillLocalSave();
            int[] elements = LibraryObjectsWorker.StringSplitter(str, '_');
            object[] tempObjects = new object[elements.Length];
            skillInventoryElements = new IRepositoryObject[tempObjects.Length];
            for (int i = 0; i < elements.Length; i++)
                tempObjects[i] = Resources.Load(skillPrefix + elements[i] + skillPostfix);

            GameObject skillGamObj;
            GameObject item;
            ItemOrSkillButton button;
            for (int i = 0; i < tempObjects.Length; i++)
            {
                skillGamObj = (GameObject)tempObjects[i];
                item = Instantiate(itemSkill);
                button = item.GetComponent<ItemOrSkillButton>();

                skillList.Add(skillGamObj.GetComponent<ISkill>());
                skillInventoryElements[i] = button;

                button.SetItemSkillsCraft(this);
                button.SetShop(shop);
                button.SetNumber(i);

                button.NameSkill.text = skillList[i].SkillName;
                button.SetImage(skillList[i].SkillImage.sprite);
                button.TutorialSkill = skillList[i].SkillTutorial;
                button.MoneyCost.text = (skillList[i].MoneyCost / 4).ToString(); ;

                item.transform.SetParent(skillsRepository.transform, false);
            }
            scrollRectSkillRepository =
                skillsRepository.transform.parent.GetComponent<ScrollRect>();

            while (!LoadArmorInventory())
            {
                yield return Timing.WaitForSeconds(0.5f);
            }
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
                GameObject item = Instantiate(itemItm);
                ItemOrSkillButton button = item.GetComponent<ItemOrSkillButton>();

                itemList.Add(itemGamObj.GetComponent<IItem>());

                button.SetItemSkillsCraft(this);
                button.SetNumber(i);

                button.NameSkill.text = itemList[i].ItemName;
                button.TutorialSkill = itemList[i].ItemTutorial;
                button.TutorialText.text = button.TutorialSkill;
                button.MoneyCost.text = itemList[i].PriceGold.ToString();
                button.SetImage(itemList[i].ItemImage.sprite);
                item.transform.SetParent(itemRepository.transform, false);
            }
            scrollRectSkillRepository =
                skillsRepository.transform.parent.GetComponent<ScrollRect>();
            yield return Timing.WaitForSeconds(0);
        }
    }
}

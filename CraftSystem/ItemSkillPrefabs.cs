using UnityEngine;

namespace CraftSystem
{
    /// <summary>
    /// Хранит в себе предметы и умения, которыми игрок
    /// может воспользоваться во время игры.
    /// 
    /// Объекты были сериализированы для того, чтобы в качестве примера можно было
    /// накинуть префабы. Ты уже знаешь, как работать: у каждого объекта есть свойство,
    /// через которое можно его проинициализировать.
    /// </summary>
    public class ItemSkillPrefabs
        : MonoBehaviour
    {
        #region Переменные и объекты
        [SerializeField]
        private GameObject firstItem, secondItem, thirdItem;
        [SerializeField]
        private GameObject firstSkill, secondSkill, thirdSkill;
        private static bool onLoad;
        #endregion

        #region Свойства
        public GameObject FirstItem
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

        public GameObject SecondItem
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

        public GameObject ThirdItem
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

        public GameObject FirstSkill
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

        public GameObject SecondSkill
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

        public GameObject ThirdSkill
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
        #endregion

        #region Методы
        /// <summary>
        /// Инициализация
        /// </summary>
        private void Awake()
        {
            if (!onLoad)
            {
                DontDestroyOnLoad(this);
                onLoad = true;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void ClearAllItems()
        {
            FirstItem = null;
            SecondItem = null;
            ThirdItem = null;
        }

        public void ClearAllSkills()
        {
            FirstSkill = null;
            SecondSkill = null;
            ThirdSkill = null;
        }
        #endregion
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CraftSystem
{
    public class SkillInShop 
        : MonoBehaviour
    {
        [SerializeField, Tooltip("Стоимость умения в золоте")]
        private long moneyCost;
        [SerializeField, Tooltip("Стоимость умения в гемах")]
        private long gemsCost;
        [SerializeField, Tooltip("Изображение умения")]
        private Image skillImage;
        [SerializeField, Tooltip("Название умения")]
        private string skillName;
        [SerializeField, Tooltip("Описание умения")]
        private string skillTutorial;

        public string SkillTutorial
        {
            get
            {
                return skillTutorial;
            }

            set
            {
                skillTutorial = value;
            }
        }

        public string SkillName
        {
            get
            {
                return skillName;
            }

            set
            {
                skillName = value;
            }
        }

        public Image SkillImage
        {
            get
            {
                return skillImage;
            }

            set
            {
                skillImage = value;
            }
        }

        public long GemsCost
        {
            get
            {
                return gemsCost;
            }

            set
            {
                gemsCost = value;
            }
        }

        public long MoneyCost
        {
            get
            {
                return moneyCost;
            }

            set
            {
                moneyCost = value;
            }
        }
    }
}

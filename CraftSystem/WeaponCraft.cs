using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CraftSystem
{
    /// <summary>
    /// Система крафта оружия
    /// </summary>
    public class WeaponCraft 
        : MonoBehaviour
    {
        #region Переменные
        [SerializeField]
        private int gripType = 0;
        private int headType = 0;
        private int gemType = 0;
        [SerializeField]
        private Text gripTextNumber;
        [SerializeField]
        private Text headTextNumber;
        [SerializeField]
        private Text gemTextNumber;

        private static GameObject grip;
        private static GameObject head;
        private static GameObject gem;
        private string[] loadGripList;
        private string[] loadHeadList;
        private string[] loadGemList;
        #endregion

        #region Свойства
        public GameObject GetGripPrafab()
        {
            return grip;
        }

        public GameObject GetHeadPrafab()
        {
            return head;
        }

        public GameObject GetGemPrafab()
        {
            return gem;
        }
        #endregion

        public void PlayArena()
        {
            gem = (GameObject)Resources.Load(loadGemList[gemType]);
            head = (GameObject)Resources.Load(loadHeadList[headType]);
            grip = (GameObject)Resources.Load(loadGripList[gripType]);
            SceneManager.LoadScene(1);
        }

        public void HeadTypeButton(int x)
        {
            headType += x;
            headTextNumber.text = headType.ToString();
        }

        public void GripTypeButton(int x)
        {
            gripType += x;
            gripTextNumber.text = gripType.ToString();
        }

        public void GemTypeButton(int x)
        {
            gemType += x;
            gemTextNumber.text = gemType.ToString();
        }

        private void FixedUpdate()
        {
            gripTextNumber.text = gripType.ToString();
            headTextNumber.text = headType.ToString();
            gemTextNumber.text = gemType.ToString();
        }

        private void Start()
        {
            DontDestroyOnLoad(this);
            loadGemList = new string[] 
            {
                "Prefabs/Weapon/Gem/000_FrostGem",
                "Prefabs/Weapon/Gem/001_FireGem",
                "Prefabs/Weapon/Gem/002_PowerfulGem",
                "Prefabs/Weapon/Gem/003_ElectricGem"
            };
            loadHeadList = new string[] 
            {
                "Prefabs/Weapon/Head/000_Axe",
                "Prefabs/Weapon/Head/001_Hammer",
                "Prefabs/Weapon/Head/002_Halberd"
            };
            loadGripList = new string[] 
            {
                "Prefabs/Weapon/Grip/000_ShortGrip",
                "Prefabs/Weapon/Grip/001_MidleGrip",
                "Prefabs/Weapon/Grip/002_LondGrip"
            };
        }
    }
}

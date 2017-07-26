using UnityEngine;

namespace CraftSystem
{
    /// <summary>
    /// Объект игрока, который никогда не погибает.
    /// Передает оружие.
    /// </summary>
    public class WeaponPrefabs 
        : MonoBehaviour
    {
        private static bool onLoad;
        GameObject grip;
        GameObject head;
        GameObject gem;

        private void Start()
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

        public GameObject Grip
        {
            get { return grip; }
            set { grip = value; }
        }

        public GameObject Head
        {
            get { return head; }
            set { head = value; }
        }

        public GameObject Gem
        {
            get { return gem; }
            set { gem = value; }
        }
    }
}

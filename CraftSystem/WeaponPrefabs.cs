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
        GameObject weapon;

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

        public GameObject Weapon
        {
            get { return weapon; }
            set { weapon = value; }
        }
    }
}

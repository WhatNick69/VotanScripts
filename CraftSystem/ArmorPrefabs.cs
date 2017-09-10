using UnityEngine;

namespace CraftSystem
{
    /// <summary>
    /// Объект игрока, который никогда не погибает.
    /// Передает броню.
    /// </summary>
    public class ArmorPrefabs 
        : MonoBehaviour
    {
        private static bool onLoad;
        GameObject cuirass;
        GameObject helmet;
        GameObject shield;


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

        public GameObject Cuirass
        {
            get { return cuirass; }
            set { cuirass = value; }
        }

        public GameObject Helmet
        {
            get { return helmet; }
            set { helmet = value; }
        }

        public GameObject Shield
        {
            get { return shield; }
            set { shield = value; }
        }
    }
}

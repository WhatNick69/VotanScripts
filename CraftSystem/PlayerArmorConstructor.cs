using UnityEngine;

namespace CraftSystem
{
    /// <summary>
    /// Конструктор брони
    /// </summary>
    public class PlayerArmorConstructor 
        : MonoBehaviour
    {
        /// <summary>
        /// Инициализация
        /// </summary>
        private void Awake()
        {
            Instantiate(GameObject.Find("GetArmorPrefabs").
                GetComponent<ArmorPrefabs>().Shield);
            Instantiate(GameObject.Find("GetArmorPrefabs").
                GetComponent<ArmorPrefabs>().Cuirass);
            Instantiate(GameObject.Find("GetArmorPrefabs").
                GetComponent<ArmorPrefabs>().Helmet);
        }
    }
}

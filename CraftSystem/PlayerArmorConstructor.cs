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
            Instantiate(GameObject.Find("GetPrefabs").
                GetComponent<ArmorPrefabs>().Shield);
            Instantiate(GameObject.Find("GetPrefabs").
                GetComponent<ArmorPrefabs>().Cuirass);
            Instantiate(GameObject.Find("GetPrefabs").
                GetComponent<ArmorPrefabs>().Helmet);
        }
    }
}

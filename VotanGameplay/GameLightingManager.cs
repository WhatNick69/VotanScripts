using UnityEngine;

namespace VotanGameplay
{
    /// <summary>
    /// Менеджер молний
    /// </summary>
    public class GameLightingManager
        :   MonoBehaviour
    {
        private static GameLighting[] arrayLightings;

        /// <summary>
        /// Инициализация
        /// </summary>
        private void Start()
        {
            arrayLightings = new GameLighting[transform.childCount];
            for (int i = 0; i < transform.childCount; i++)
                arrayLightings[i] = transform.GetChild(i).GetComponent<GameLighting>();
        }

        /// <summary>
        /// Включить несколько молний
        /// </summary>
        /// <param name="count"></param>
        public static void FireSomeLights(int count)
        {
            int number;
            if (count > arrayLightings.Length) count = arrayLightings.Length;
            for (int i = count;i>0;i--)
            {
                number = Random.Range(0, arrayLightings.Length);
                arrayLightings[number].FireLighting();
            }
        }
    }
}

using UnityEngine;

namespace PlayerBehaviour
{
    /// <summary>
    /// Менеджер костей игрока
    /// </summary>
    public class PlayerBonesManager
        : MonoBehaviour
    {
        [SerializeField,Tooltip("Массив костей игрока")]
        private Transform[] bonesArray;

        /// <summary>
        /// Возвращает ближайшую кость
        /// </summary>
        /// <param name="intoObject"></param>
        /// <returns></returns>
        public Transform GetClosestBone(Vector3 intoObjectPosition)
        {
            float distance = float.MaxValue;
            float tempDistance = float.MaxValue;
            int res = 0;
            for (int i = 0;i<bonesArray.Length;i++)
            {
                tempDistance = Vector3.Distance(intoObjectPosition, bonesArray[i].position);
                if (tempDistance < distance)
                {
                    distance = tempDistance;
                    res = i;
                }
            }
            return bonesArray[res];
        }
    }
}

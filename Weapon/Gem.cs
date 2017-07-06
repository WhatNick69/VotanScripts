using UnityEngine;

namespace PlayerBehaviour
{
    public class Gem
        : MonoBehaviour
    {

        [SerializeField]
        DamageType dt;
        [SerializeField, Range(1, 100)]
        int gemPower;
        [SerializeField]
        string weaponName;

        public DamageType GetDamageType()
        {
            return dt;
        }

        public int GetGemPower()
        {
            return gemPower;
        }

        public string GetName()
        {
            return weaponName;
        }
    }
}

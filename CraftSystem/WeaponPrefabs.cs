using UnityEngine;

namespace CraftSystem
{
    public class WeaponPrefabs : MonoBehaviour
    {
        GameObject grip;
        GameObject head;
        GameObject gem;


        private void Start()
        {
            DontDestroyOnLoad(this);
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

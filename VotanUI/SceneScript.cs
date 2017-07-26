using UnityEngine;

namespace VotanUI
{
    public class SceneScript 
        : MonoBehaviour
    {

        [SerializeField]
        private int arenaNumber;
        [SerializeField]
        string arenaName;

        public string ArenaName
        {
            get
            {
                return arenaName;
            }
        }

        public int Arena
        {
            get
            {
                return arenaNumber;
            }
        }
    }
}

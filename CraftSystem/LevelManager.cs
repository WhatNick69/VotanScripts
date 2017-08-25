using UnityEngine;

namespace CraftSystem
{
    public class LevelManager 
        : MonoBehaviour {

		[SerializeField]
		GameObject level_1;
		[SerializeField]
		GameObject level_2;
		[SerializeField]
		GameObject level_3;
		[SerializeField]
		GameObject level_4;

		public GameObject GetItemLevel(int level)
		{
			switch (level)
			{
				case 0:
					return level_1;
				case 1:
					return level_2;
				case 2:
					return level_3;
				case 3:
					return level_4;
				default:
					return level_1;
			}
		}
	}
}

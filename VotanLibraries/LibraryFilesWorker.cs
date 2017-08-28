using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace VotanLibraries
{
    /// <summary>
    /// Позволяет производить операции над файлами и объектами.
    /// </summary>
    public class LibraryObjectsWorker
        : MonoBehaviour
    {
        /// <summary> 
        /// Глубокий поиск объекта во всей иерархии 
        /// </summary> 
        /// <param name="target"></param> 
        /// <param name="name"></param> 
        /// <returns></returns> 
        public static Transform DeepFind(Transform target, string name)
        {
            if (target.name.Equals(name)) return target;

            for (int i = 0; i < target.childCount; ++i)
            {
                var result = DeepFind(target.GetChild(i), name);

                if (result != null) return result;
            }
            return null;
        }

        /// <summary>
        /// Разделяет строку на подстроки.
        /// </summary>
        /// <param name="source">Входная строка, которую нужно разделять</param>
        /// <param name="splitter">Символ разделитель</param>
        /// <returns></returns>
        public static int[] StringSplitter(string source, char splitter)
        {
            //инициализация
			string[] tempArray = source.Split(splitter);
            List<int> destArray = new List<int>();
			//разделение
			for (int i = 0; i < tempArray.Length; i++)
				if (!Regex.IsMatch(tempArray[i], "[a-z]"))
				{
					if(tempArray[i] != "")
					destArray.Add(Convert.ToInt32(tempArray[i]));
				}
			//вывод
			int[] resArray = new int[destArray.Count];
			for (int i = 0; i < destArray.Count; i++)
			{
				resArray[i] = destArray[i];
			}
			
            return resArray;
        }
    }
}

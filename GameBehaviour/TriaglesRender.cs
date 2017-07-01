using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameBehaviour
{
    /// <summary>
    /// Рассматриваем лестницу
    /// </summary>
    public class TriaglesRender 
        : MonoBehaviour
    {
        [SerializeField]
        private Transform player;
        [SerializeField]
        private Transform A, B,C,A1,B1,C1;
        [SerializeField]
        private Transform stairs;
        private float AB, AC, BC, AA1;

        private float a, b, c,d, e, f;
        private float S;
        private float H;
        private float sootn;
        private static float Y;

<<<<<<< HEAD
	public float AB, AC, BC, AA1;
	

	float a, b, c;
 	float d, e, f;
	public float S;
	public float H;
	float sootn;
	public static float Y;
=======
        /// <summary>
        /// Проверяет принадлежность точки "X" к плосскости заданной точками y,x,w
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="w"></param>
        /// <returns></returns>
        private bool InPlane(Vector3 x, Vector3 y, Vector3 z, Vector3 w)
        {
            Vector3 playerPoint = x;
            Vector3 PV1 = y;
            Vector3 PV2 = z;
            Vector3 PV3 = w;

            a = (PV1.x - playerPoint.x) * (PV2.z - PV1.z) - (PV2.x - PV1.x) * (PV1.z - playerPoint.z);
            b = (PV2.x - playerPoint.x) * (PV3.z - PV2.z) - (PV3.x - PV2.x) * (PV2.z - playerPoint.z);
            c = (PV3.x - playerPoint.x) * (PV1.z - PV3.z) - (PV1.x - PV3.x) * (PV3.z - playerPoint.z);
>>>>>>> 7e66f6989a7f187f026345d005e2e6d2e4847a03

            return ((a >= 0 && b >= 0 && c >= 0) || (a <= 0 && b <= 0 && c <= 0));
        }

        /// <summary>
        /// Расчитывает нужную высоту позиции персонажа по Y
        /// </summary>
        private void HightOnY()
        {
            if (InPlane(player.position, B.position, A1.position, A.position) || InPlane(player.position, B1.position, B.position, A1.position))
            {
                d = Vector3.Distance(player.position, A.position);
                e = Vector3.Distance(player.position, A1.position);
                f = (d + e + AA1) / 2;
                S = Mathf.Sqrt(f * (f - AA1) * (f - d) * (f - e));
                H = (2 * S) / AA1;
                sootn = H / AC;
                Y = BC * sootn;
            }
        }

        /// <summary>
        /// Возвращает нужную высоту по Y
        /// </summary>
        /// <returns></returns>
        public static float GetHightOnY()
        {
            return Y;
        }

        /// <summary>
        /// Инициализация
        /// </summary>
        private void Start()
        {
            AB = Vector3.Distance(A.position, B.position);
            AC = Vector3.Distance(A.position, C.position);
            BC = Vector3.Distance(C.position, B.position);
            AA1 = Vector3.Distance(A.position, A1.position);
        }

<<<<<<< HEAD
		b = (PV2.x - playerPoint.x) * (PV3.z - PV2.z) - (PV3.x - PV2.x) * (PV2.z - playerPoint.z);

		c = (PV3.x - playerPoint.x) * (PV1.z - PV3.z) - (PV1.x - PV3.x) * (PV3.z - playerPoint.z);

		return ((a >= 0 && b >= 0 && c >= 0) || (a <= 0 && b <= 0 && c <= 0));
	}

	/// <summary>
	/// Расчитывает нужную высоту позиции персонажа по Y
	/// </summary>
	private void HightOnY()
	{
		if (InPlane(player.position, B.position, A1.position, A.position) || InPlane(player.position, B1.position, B.position, A1.position))
		{
			d = Vector3.Distance(player.position, A.position);
			e = Vector3.Distance(player.position, A1.position);
			f = (d + e + AA1) / 2;
			S = Mathf.Sqrt(f * (f - AA1) * (f - d) * (f - e));
			H = (2 * S) / AA1;
			sootn = H / AC;
			Y = BC * sootn;
			
		}
		
	}

	/// <summary>
	/// Возвращает нужную высоту по Y
	/// </summary>
	/// <returns></returns>
	public static float GetHightOnY()
	{
		return Y;
	}

	void Start ()
	{
		AB = Vector3.Distance(A.position, B.position);
		AC = Vector3.Distance(A.position, C.position);
		BC = Vector3.Distance(C.position, B.position);
		AA1 = Vector3.Distance(A.position, A1.position);
	}
	
	
	void FixedUpdate ()
	{
		HightOnY();
	}
=======
        /// <summary>
        /// Таймовое обновление
        /// </summary>
        private void FixedUpdate()
        {
            if (Vector3.Distance(stairs.position,player.position) <= 2)
                HightOnY();
        }
    }
>>>>>>> 7e66f6989a7f187f026345d005e2e6d2e4847a03
}

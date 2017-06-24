using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PhisicsLibrary : MonoBehaviour {

	// Первые две переменные задают положение оружия(отрезок), их можно вводить в любой последовательности 
	// Следующие две переменные задают положение врага(отрезок), их так же  можно вводить в любой последовательности 
	// Тк идет рассчет проекций на плоскость, то положение объектов по Y никак не влияет на разультат
	// На выхоже мы получаем true если отрезки пересеклись, в противном случае получаем false
	// Что бы не было проблем с рассчетом столкновений 2х параллельных отрезков, лучше вызвать функцию еще раз, 
	//но уже сдругими точками задающими положение врага 
	public bool Vector3Crossing(Vector3 x, Vector3 y, Vector3 z, Vector3 w) 
	{
		Vector3 PV1 = x;
		Vector3 PV2 = y;
		Vector3 EV3 = z;
		Vector3 EV4 = w;

		float a = (PV1.x - PV2.x) * (EV4.z - EV3.z) - (PV1.z - PV2.z) * (EV4.x - EV3.x);
		float b = (PV1.x - EV3.x) * (EV4.z - EV3.z) - (PV1.z - EV3.z) * (EV4.x - EV3.x);
		float c = (PV1.x - PV2.x) * (PV1.z - EV3.z) - (PV1.z - PV2.z) * (PV1.x - EV3.x);

		float ta = b / a;
		float tb = c / a;

		return (ta >= 0 && ta <= 1 && tb >= 0 && tb <= 1);
	}

	// Пререгрузка функции, которая возвращает точку столкновения 2х отрезков
	public Vector3 Vector3Crossing(Vector3 x, Vector3 y, Vector3 z, Vector3 w, bool AttackPoint)
	{
		Vector3 PV1 = x;
		Vector3 PV2 = y;
		Vector3 EV3 = z;
		Vector3 EV4 = w;

		float a = (PV1.x - PV2.x) * (EV4.z - EV3.z) - (PV1.z - PV2.z) * (EV4.x - EV3.x);
		float b = (PV1.x - EV3.x) * (EV4.z - EV3.z) - (PV1.z - EV3.z) * (EV4.x - EV3.x);
		float c = (PV1.x - PV2.x) * (PV1.z - EV3.z) - (PV1.z - PV2.z) * (PV1.x - EV3.x);

		float ta = b / a;
		float tb = c / a;

		if (ta >= 0 && ta <= 1 && tb >= 0 && tb <= 1)
		{
			float A = x.x + ta * (y.x - x.x);
			float B = x.z + ta * (y.z - x.z);

			return new Vector3(A, 2, B);
		}
		else return Vector3.zero;
	}
}

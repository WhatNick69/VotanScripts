using UnityEngine;
using System.Collections.Generic;
using MovementEffects;
using VotanInterfaces;
using VotanLibraries;

namespace PlayerBehaviour
{
	/// <summary>
	/// Интерфейс для описания оружия
	/// </summary>
	public interface IWeapon
	{
		#region Переменные
		/// <summary>
		/// Величина временной брони
		/// </summary>
		float TempPhysicDefence { get; }

		/// <summary>
		/// Добавить временную защиту в результате
		/// попадания по врагу земельным ударом
		/// </summary>
		/// <param name="value"></param>
		void AddTempPhysicDefence(float value);

		/// <summary>
		/// Скорость вращения оружием
		/// </summary>
		float SpinSpeed { get; set; }

		/// <summary>
		/// Булева переменная, которая показывает
		/// можно ли вновь получить защиту в результате
		/// попадания по врагу земляной атакой
		/// </summary>
		bool IsMayToGetPhysicDefence { get; set; }

		/// <summary>
		/// Получить игрока
		/// </summary>
		IPlayerBehaviour GetPlayer { get; }

		/// <summary>
		/// Значения урона оружием
		/// </summary>
		float Damage { get; set; }

		/// <summary>
		/// Величина критического удара в процентах
		/// </summary>
		float CritChanceStrenght { get; set; }

		/// <summary>
		/// Тип атаки
		/// </summary>
		GemType GemType { get; set; }

		/// <summary>
		/// Тип оружия (режущее, дробящее)
		/// </summary>
		WeaponType WeaponType { get; set; }

		/// <summary>
		/// Сила камня
		/// </summary>
		float GemPower { get; set; }

		/// <summary>
		/// Оригинальная скорость оружия
		/// </summary>
		float OriginalSpinSpeed { get; set; }

		/// <summary>
		/// Трэил-лента оружия
		/// </summary>
		TrailRenderer TrailRenderer { get; set; }
		#endregion

		/// <summary>
		/// Инициализация
		/// </summary>
		void Start();

		/// <summary>
		/// Задаем урон оружию, величину защиты, тип атаки, 
		/// скорость вращения и весь оружия.
		/// Урон оружия влияет на урон, который оно наносит.
		/// Величина защиты влияет на величину урона, которую
		/// оружие может поглатить.
		/// Тип атаки оружия: физика, электричество, огонь или мороз.
		/// Скорость вращения влияет на скорость вращения оружием.
		/// Вес оружия влияет на скорость его замедления при ударе врага.
		/// </summary>
		/// <param name="damage"></param>
		/// <param name="defenceValue"></param>
		/// <param name="attackType"></param>
		/// <param name="spinSpeed"></param>
		/// <param name="weight"></param>
		/// <param name="gemPower"></param>
		void SetWeaponParameters(float damage, float critChance, GemType attackType,
			TrailRenderer trailRenderer, float gemPower, WeaponType weaponType);

		/// <summary>
		/// Снижаем скорость вращения при попадании по врагу
		/// </summary>
		void WhileTime();

		/// <summary>
		/// Установить скорость вращения оружием
		/// </summary>
		/// <param name="spinSpeed"></param>
		void SetSpinSpeed(float spinSpeed);

		/// <summary>
		/// Корутина, для осуществления замедления оружия игрока
		/// </summary>
		/// <returns></returns>
		IEnumerator<float> CoroutineDoSlowMotionSpinSpeed();

		/// <summary>
		/// Установить цвет трэил-ленты оружия
		/// </summary>
		void SetColorTrailWeapon();

		/// <summary>
		/// Выбросить оружие из рук игрока оплсе его смерти
		/// </summary>
		void EventKillWeapon();
	}

	/// <summary>
	/// Класс, описывающий поведение оружия
	/// </summary>
	public class PlayerWeapon
		: MonoBehaviour, IWeapon
	{
		#region Переменные
		[SerializeField, Tooltip("Урон от оружия"), Range(1, 10000f)]
		private float damage;
		[SerializeField, Tooltip("Величина критического урона (в процентах)"), Range(100, 1000)]
		private float critChanceStrenght;
		[SerializeField, Tooltip("Тип атаки")]
		private GemType gemType;
		[SerializeField, Tooltip("Тип оружия")]
		private WeaponType weaponType;
		[SerializeField, Tooltip("Скорость вращения"), Range(10, 100f)]
		private float spinSpeed;
		[SerializeField, Tooltip("Сила камня"), Range(1, 100f)]
		private float gemPower;
		[SerializeField, Tooltip("Временная защита")]
		private float tempDefence;
		private float originalSpinSpeed;
		[SerializeField, Tooltip("Задержка перед возвращением скорости"), Range(0.1f, 3)]
		private float speedReturnLatency;
		[SerializeField, Tooltip("Хранитель компонентов")]
		private PlayerComponentsControl playerComponentsControl;

		private PlayerController playerController;
		private bool isSlowMotion;
		private bool isMayToGetPhysicDefence;
		private float tempAngleForSound;
		private int coroutinesCount;
		private TrailRenderer trailRenderer;

		private Rigidbody rigidbodyOfWeapon;
		private BoxCollider[] boxColliderOfWeaponArray;
		private PlayerArmory playerArmory;
		#endregion

		#region Свойства
		public float Damage
		{
			get
			{
				return damage;
			}

			set
			{
				damage = value;
			}
		}

		public GemType GemType
		{
			get
			{
				return gemType;
			}

			set
			{
				gemType = value;
			}
		}

		public float SpinSpeed
		{
			get
			{
				return spinSpeed;
			}

			set
			{
				if (value > originalSpinSpeed)
					value = originalSpinSpeed;
				else if (value < 0)
					value = 0;
				spinSpeed = value;

				playerComponentsControl.PlayerAnimationsController.
					SetSpeedAnimationByRunSpeed(0.25f + spinSpeed * 0.005f);
				PlaySpinSpeedAudio();
			}
		}

		private void PlaySpinSpeedAudio()
		{
			tempAngleForSound += spinSpeed / 2;
			if (Mathf.Abs(tempAngleForSound) >= 80)
			{
				playerComponentsControl.PlayerSounder.PlaySpinAudio
					(SpinSpeed, false);
				tempAngleForSound = 0;
			}
		}

		public float GemPower
		{
			get
			{
				return gemPower;
			}

			set
			{
				if (value > 100)
					value = 100;
				else if (value < 1)
					value = 1;
				gemPower = value;
			}
		}

		public float OriginalSpinSpeed
		{
			get
			{
				return originalSpinSpeed;
			}

			set
			{
				if (value > 100)
					value = 100;
				else if (value < 0)
					value = 0;
				originalSpinSpeed = value;
			}
		}

		public IPlayerBehaviour GetPlayer
		{
			get
			{
				return playerComponentsControl;
			}
		}

		public TrailRenderer TrailRenderer
		{
			get
			{
				return trailRenderer;
			}

			set
			{
				trailRenderer = value;
			}
		}

		public float TempPhysicDefence
		{
			get
			{
				return tempDefence;
			}
		}

		public bool IsMayToGetPhysicDefence
		{
			get
			{
				return isMayToGetPhysicDefence;
			}

			set
			{
				if (value)
					Timing.RunCoroutine(CoroutineForMayTempPhysicDefence());
				isMayToGetPhysicDefence = value;
			}
		}

		public WeaponType WeaponType
		{
			get
			{
				return weaponType;
			}

			set
			{
				weaponType = value;
			}
		}

		public bool IsSlowMotion
		{
			get
			{
				return isSlowMotion;
			}

			set
			{
				isSlowMotion = value;
			}
		}

		public float CritChanceStrenght
		{
			get
			{
				return critChanceStrenght;
			}

			set
			{
				critChanceStrenght = value;
			}
		}
		#endregion

		/// <summary>
		/// Задать характеристики оружия через урон, 
		/// скорость вращения, тип камня, трэил, 
		/// силу камня, и тип оружия.
		/// </summary>
		/// <param name="damage"></param>
		/// <param name="spinSpeed"></param>
		/// <param name="gemType"></param>
		/// <param name="trailRenderer"></param>
		/// <param name="gemPower"></param>
		/// <param name="weaponType"></param>
		public void SetWeaponParameters(float damage, float critChance, GemType gemType,
			TrailRenderer trailRenderer, float gemPower,
			WeaponType weaponType = WeaponType.Cutting)
		{
			this.damage = damage;
			critChanceStrenght = critChance;
			this.gemType = gemType;
			this.gemPower = gemPower;
			this.trailRenderer = trailRenderer;
			this.weaponType = weaponType;

			SetColorTrailWeapon();
		}

		/// <summary>
		/// Установить скорость вращения оружием
		/// </summary>
		/// <param name="spinSpeed"></param>
		public void SetSpinSpeed(float spinSpeed)
		{
			this.spinSpeed = spinSpeed;
			originalSpinSpeed = spinSpeed;
		}

		/// <summary>
		/// Инициализация
		/// </summary>
		public void Start()
		{
			playerComponentsControl.PlayerWeapon = this;
			playerController = playerComponentsControl.PlayerController;
			isMayToGetPhysicDefence = true;

			rigidbodyOfWeapon = GetComponent<Rigidbody>();
			boxColliderOfWeaponArray = GetComponents<BoxCollider>();

			playerArmory = GetPlayer.PlayerArmory;
		}

		/// <summary>
		/// Выбросить оружие из рук игрока после его смерти
		/// </summary>
		public void EventKillWeapon()
		{
			DetachWeapon();
			EnableActiveRigidbodyOfWeapon();
		}

		/// <summary>
		/// Отсоединить оружие
		/// </summary>
		private void DetachWeapon()
		{
			rigidbodyOfWeapon.transform.parent = null;
		}

		/// <summary>
		/// Включить просчет физики
		/// </summary>
		private void EnableActiveRigidbodyOfWeapon()
		{
			foreach (BoxCollider collider in boxColliderOfWeaponArray)
				collider.enabled = true;

			rigidbodyOfWeapon.useGravity = true;
			rigidbodyOfWeapon.detectCollisions = true;
			rigidbodyOfWeapon.constraints = RigidbodyConstraints.None;
			rigidbodyOfWeapon.AddForce(new Vector3(LibraryStaticFunctions.GetPlusMinusValue(75),
				UnityEngine.Random.Range(40, 100),
				LibraryStaticFunctions.GetPlusMinusValue(75)));
		}

		/// <summary>
		/// Временно снижаем скорость вращения 
		/// оружием при попадании по врагу
		/// </summary>
		public void WhileTime()
		{
			isSlowMotion = true;
			Timing.RunCoroutine(CoroutineDoSlowMotionSpinSpeed());
			coroutinesCount++;
		}

		/// <summary>
		/// Корутин на замедление вращения
		/// </summary>
		/// <returns></returns>
		public IEnumerator<float> CoroutineDoSlowMotionSpinSpeed()
		{
			float spSpeed = LibraryStaticFunctions.CalculateSpinSpeedSlowMotionValue
				(originalSpinSpeed, playerArmory.ArmoryWeight);

			float partOfSpeed = spSpeed / 20;
			if (spinSpeed / originalSpinSpeed >= 0.75f)
				playerComponentsControl.PlayerCameraSmooth.DoNoize(spinSpeed / originalSpinSpeed);

			playerController.MaxSpinSpeed -= spSpeed;
			yield return Timing.WaitForSeconds(speedReturnLatency);

			for (int i = 0; i < 20; i++)
			{
				playerController.MaxSpinSpeed += partOfSpeed;
				yield return Timing.WaitForSeconds(Time.deltaTime);
			}
			coroutinesCount--;
			if (coroutinesCount == 0)
				isSlowMotion = false;
		}

		/// <summary>
		/// Установить цвет трэил-ленты,
		/// время жизни трэила (1 - 2 секунды)
		/// и ширину ленты (0.2 - 0.3),
		/// в зависимости от силы камня
		/// </summary>
		public void SetColorTrailWeapon()
		{
			Color color = LibraryStaticFunctions.GetColorFromGemPower(GemPower, gemType);
			if (color == Color.white)
			{
				TrailRenderer.enabled = false;
				return;
			}
			//TrailRenderer.time = 1 + GemPower / 300; // время жизни молнии. от 1 до 1.33 секунды
			TrailRenderer.startWidth = 0.2f + GemPower / 2000; // размер молнии. от 0.2 до 0.25
			TrailRenderer.startColor = color;
			TrailRenderer.endColor = color;
		}

		/// <summary>
		/// Добавить временную защиту
		/// </summary>
		/// <param name="value"></param>
		public void AddTempPhysicDefence(float value)
		{
			Timing.RunCoroutine(CoroutineForDisableTempDefence(value));
		}

		/// <summary>
		/// Корутина для отключения временной защиты
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		private IEnumerator<float> CoroutineForDisableTempDefence(float value)
		{
			tempDefence += value;
			if (tempDefence > 100) tempDefence = 100;
			yield return Timing.WaitForSeconds(1);
			for (int i = 0; i < 10; i++)
			{
				tempDefence -= value / 10;
				if (tempDefence < 0.01f)
				{
					tempDefence = 0;
					yield break;
				}
				yield return Timing.WaitForSeconds(0.05f);
			}
		}

		/// <summary>
		/// Ждать окончания промежутка времени, между получением следующей
		/// земельной защиты
		/// </summary>
		/// <returns></returns>
		private IEnumerator<float> CoroutineForMayTempPhysicDefence()
		{
			isMayToGetPhysicDefence = false;
			yield return Timing.WaitForSeconds
				(LibraryStaticFunctions.HowMuchWaitForPhysicAttack(GemPower));
			isMayToGetPhysicDefence = true;
		}
	}

	/// <summary>
	/// Типы атаки. Перечисление
	/// 
	/// Frozen - замораживающая атака
	/// Fire - огненная атака
	/// Powerful - физическая атака
	/// Electric - электрическая атака
	/// </summary>
	public enum GemType
	{
		None, Frozen, Fire, Powerful, Electric
	}

	/// <summary>
	/// Тип оружия. Перечисление.
	/// 
	/// Crushing - дробящее оружие.
	/// Cutting - режущее оружие
	/// </summary>
	public enum WeaponType
	{
		Crushing, Cutting
	}

	/// <summary>
	/// Длина рукояти. Перечисление
	/// </summary>
	public enum LenghtGrip
	{
		Long, Middle, Short
	}
}

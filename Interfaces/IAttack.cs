using AbstractBehaviour;
using PlayerBehaviour;
using System.Collections.Generic;
using UnityEngine;

/*
 * Интерфейсы данного скрипта реализуют функционал атаки
 * для таких объектов, как общий объект (который является
 * родителем для более частных потомков - врага или игрока), 
 * а также более частных - игрока или врага.
 */
namespace VotanInterfaces
{

    /// <summary>
    /// Интерфейс для реализации общего поведения атаки объекта
    /// </summary>
    public interface IVotanObjectAttack
    {
        /// <summary>
        /// Установить позицию персонажа
        /// </summary>
        /// <param name="index"></param>
        /// <param name="tr"></param>
        void SetPlayerPoint(int index, Transform tr);

        /// <summary>
        /// Возвращает позиции персонажа
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        Transform PlayerPosition(int index);

        /// <summary>
        /// Радиус атаки
        /// Расстояние между персонажем игрока и врагом</summary>
        /// <param name="Player"></param>
        /// <param name="Enemy"></param>
        /// <returns></returns>
        float AttackRange(Vector3 Player, Vector3 Enemy);

        /// <summary>
        /// Точка атаки
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <returns></returns>
        Vector3 AttackPoint(Transform X, Transform Y);
    }

    /// <summary>
    /// Интерфейс для реализации атаки персонажем
    /// </summary>
    public interface IPlayerAttack
    {
        /// <summary>
        /// Инициализация
        /// </summary>
        void Start();

        /// <summary>
        /// Запуск корутинов
        /// </summary>
        void StartCoroutines();

        /// <summary>
        /// Корутина на атаку
        /// </summary>
        /// <returns></returns>
        IEnumerator<float> CoroutineForAttackUpdate();

        /// <summary>
        /// Корутина на осуществление урона по врагу
        /// </summary>
        /// <returns></returns>
        IEnumerator<float> CoroutineMayDoDamage();

        /// <summary>
        /// Атакуем врага
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="dmgType"></param>
        void AttackToEnemy(float damage, GemType dmgType,bool isSuperAttack);
    }

    /// <summary>
    /// Интерфейс для реализации атаки врагом
    /// </summary>
    public interface IEnemyAttack
    {
        /// <summary>
        /// Рестарт атаки врага
        /// </summary>
        void RestartEnemyAttack();

        /// <summary>
        /// Корутина для нанесения урона по персонажу
        /// </summary>
        /// <returns></returns>
        IEnumerator<float> CoroutineMayDoDamage();

        /// <summary>
        /// Атакуем персонажа
        /// </summary>
        /// <returns></returns>
        bool AttackToPlayer();
    }
}

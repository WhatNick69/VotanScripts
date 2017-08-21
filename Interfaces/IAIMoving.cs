using PlayerBehaviour;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace VotanInterfaces
{
    /// <summary>
    /// Интерфейс для реализации движения врага
    /// </summary>
    public interface IAIMoving
    {
        /// <summary>
        /// Рестартировать компонент, отвеющий за способность противника двигаться
        /// </summary>
        void RestartEnemyMove();

        /// <summary>
        /// Включить агент
        /// </summary>
        void EnableAgent();

        /// <summary>
        /// Выключить агент
        /// </summary>
        void DisableAgent();

        /// <summary>
        /// Включить возможность смотреть на игрока
        /// </summary>
        void EnableMayableLookAtPlayer();

        /// <summary>
        /// Отключить возможность смотреть на игрока
        /// </summary>
        void DisableMayableLookAtPlayer();

        /// <summary>
        /// Переменная которая говорит, чтобы враг 
        /// поворачивался в сторону игрока.
        /// Полезно для боссов, чьи атаки очень сильны и длительны.
        /// </summary>
        /// <returns></returns>
        bool IsLookingAtPlayer { get; set; }

        /// <summary>
        /// Предупредительная дистанция до атаки
        /// </summary>
        float PreDistanceForAttack {get;set;}

        /// <summary>
        /// Скорость агента
        /// </summary>
        float AgentSpeed { get; set; }

        /// <summary>
        /// Скорость поворота для агента
        /// </summary>
        float RotationSpeed { get; set; }

        /// <summary>
        /// НавМешАгент врага
        /// </summary>
        NavMeshAgent Agent { get; set; }

        /// <summary>
        /// Игрок, до которого должен добраться враг
        /// </summary>
        Transform PlayerObjectTransformForFollow { get; set; }

        /// <summary>
        /// Компонент игрока - PlayerCollision
        /// </summary>
        PlayerCollision PlayerCollisionComponent { get; set; }

        /// <summary>
        /// Буль, достиг ли враг цели
        /// </summary>
        bool IsStopped { get; set; }

        /// <summary>
        /// Зависимость скорости воспроизведения 
        /// анимации врага от его скорости движения
        /// </summary>
        void DependenceAnimatorSpeedOfVelocity();

        /// <summary>
        /// Проверить, достиг ли враг точки назначения 
        /// </summary>
        /// <param name="isRandom"></param>
        /// <returns></returns>
        bool CheckStopped(bool isRandom = false);

        /// <summary>
        /// Установить новую скорость движения для НавМешАгента
        /// </summary>
        /// <param name="newValue"></param>
        /// <param name="newRotSpeed"></param>
        void SetNewSpeedOfNavMeshAgent(float newValue, float newRotSpeed = 0);

        /// <summary>
        /// Получить игрока и его компонент
        /// </summary>
        /// <returns></returns>
        bool GetPlayerAndComponent();

        /// <summary>
        /// Метод для инициализации
        /// </summary>
        void Start();

        /// <summary>
        /// Установить случайную позицию для передвижения
        /// </summary>
        void SetRandomPosition();

        /// <summary>
        /// Смотреть на персонажа
        /// </summary>
        void LookAtPlayerObject();

        /// <summary>
        /// Корутина для поворота врага вследом за игроком. 
        /// Используется, когда враг останавливается (достигает пункта назначения)
        /// </summary>
        /// <returns></returns>
        IEnumerator<float> CoroutineForRotating();

        /// <summary>
        /// Корутина для поиска нового объекта, 
        /// который выступает в роли следующего пункта назначения
        /// </summary>
        /// <returns></returns>
        IEnumerator<float> CoroutineForSearchingByPlayerObject();
    }

    /// <summary>
    /// Расширяющий интерфейс для описания конечной фазы
    /// движения (предсмертной) для первого босса (возможно,
    /// что данное поведение может быть использовано и для
    /// других боссов).
    /// </summary>
    public interface IFirstBossMove
    {
        /// <summary>
        /// Установить состояние движения к пункту смерти
        /// </summary>
        void SetDeadPosition();

        /// <summary>
        /// Процедура смены состояния босса после достижения
        /// им пункта смертного назначения.
        /// </summary>
        void DeadPositionDestination();

        /// <summary>
        /// КОрутина для движения босса до пункта смерти.
        /// </summary>
        /// <returns></returns>
        IEnumerator<float> CoroutineForMoveToDeadPosition();

        /// <summary>
        /// Корутина, которая посылает босса за пределы
        /// башни (выкидывает его).
        /// </summary>
        /// <returns></returns>
        IEnumerator<float> CoroutineForFlyBoss();

        /// <summary>
        /// Метод, который запускает корутину CoroutineForFlyBoss()
        /// </summary>
        void GoOutEnemy();
    }
}

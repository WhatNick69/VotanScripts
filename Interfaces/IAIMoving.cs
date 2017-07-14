using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Интерфейс для реализации движения врага
/// </summary>
public interface IAIMoving
{
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

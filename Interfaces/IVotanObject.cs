using EnemyBehaviour;
using UnityEngine;

/*
 * Интерфейсы данного скрипта реализуют 
 * общую логику поведения
 * таких объектов, как общий объект, что 
 * является родителей для
 * таких потомков, как игрок, либо враг.
 */
namespace VotanInterfaces
{
    /// <summary>
    /// Интерфейс для реализации общего 
    /// поведения объекта
    /// </summary>
    public interface IVotanObjectBehaviour
    {

    }

    /// <summary>
    /// Интерфейс для реализации поведения 
    /// игрока
    /// </summary>
    public interface IPlayerBehaviour
    {

    }

    /// <summary>
    /// Интерфейс для реализации поведения 
    /// врага
    /// </summary>
    public interface IEnemyBehaviour
    {
        EnemyAttack EnemyAttack { get; set; }
        EnemyAnimationsController EnemyAnimationsController { get; set; }
        IEnemyConditions EnemyConditions { get; set; }
        EnemyOpponentChoiser EnemyOpponentChoiser { get; set; }

        Vector3 ReturnPosition(int child);
    }
}


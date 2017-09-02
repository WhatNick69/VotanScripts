using AbstractBehaviour;
using GameBehaviour;
using MovementEffects;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.CrossPlatformInput;
using VotanGameplay;
using VotanInterfaces;
using VotanLibraries;

namespace EnemyBehaviour
{
    /// <summary>
    /// Описывает поведение босса с первой локации
    /// </summary>
    public class EnemyFirstBoss
        : KnightEnemy, IBoss
    {
        #region Переменные
        [SerializeField,Tooltip("Дистанция для интро"),Range(0,10)]
        private float distanceToIntro;
        [SerializeField, Tooltip("Время фокуса камеры на боссе"), Range(0, 10)]
        private float timeToFocus;

        IFirstBossMove iFirstBossMove;
        FirstBossAttack firstBossAttack;
        private float distanceForFirstAttack;
        private float distanceForKneeAttack;
        private float distanceForSamuraAttack;
        private float distanceForGolfAttack;
        private float tempDistance;

        private int currentAttackState;
        private bool isIntroEnded;
        #endregion

        #region Свойства
        public IFirstBossMove IFirstBossMove
        {
            get
            {
                return iFirstBossMove;
            }

            set
            {
                iFirstBossMove = value;
            }
        }

        public bool IsIntroEnded
        {
            get
            {
                return isIntroEnded;
            }

            set
            {
                isIntroEnded = value;
            }
        }
        #endregion

        /// <summary>
        /// Инициализация
        /// </summary>
        public override void Awake()
        {
            AbstractObjectSounder =
                GetComponent<AbstractSoundStorage>();
            EnemyOpponentChoiser =
                GetComponent<EnemyOpponentChoiser>();
            EnemyAnimationsController =
                GetComponent<EnemyAnimationsController>();
            firstBossAttack =
                GetComponent<FirstBossAttack>();
            EnemyAttack = firstBossAttack;
            EnemyConditions =
                GetComponent<EnemyConditions>();
            EnemyMove =
                GetComponent<EnemyMove>();
            IFirstBossMove =
                GetComponent<IFirstBossMove>();
            DownInterfaceRotater =
                GetComponent<DownInterfaceRotater>();

            IceEffect =
                LibraryObjectsWorker.DeepFind(transform, "IceStack")
                .GetComponent<IIceEffect>();
            ElectricEffect =
                LibraryObjectsWorker.DeepFind(transform, "ElectricStack")
                .GetComponent<IElectricEffect>();
            Physicffect =
                LibraryObjectsWorker.DeepFind(transform, "PhysicStack")
                .GetComponent<IPhysicEffect>();
            ScoreAddingEffect =
                LibraryObjectsWorker.DeepFind(transform, "ScoreStack")
                .GetComponent<IScoreAddingEffect>();
            FireEffect =
                LibraryObjectsWorker.DeepFind(transform, "FireStack")
                .GetComponent<IFireEffect>();

            gameObject.SetActive(false);
        }

        /// <summary>
        /// Перезагрузить босса
        /// </summary>
        public override void RestartEnemy()
        {
            enemyConditions.RestartEnemyConditions(); // рестарт состояний врага
            firstBossAttack.RestartEnemyAttack(); // рестарт способности врага к битве
            enemyMove.RestartEnemyMove(); // рестарт способносит врага к движению
            downInterfaceRotater.RestartDownInterfaceRotater(); // рестарт поворота интерфейса
            enemyAnimationsController.RestartEnemyAnimationsController(); // рестарт аниматора

            distanceForFirstAttack = enemyMove.Agent.stoppingDistance;
            distanceForKneeAttack = distanceForFirstAttack / 2;
            distanceForGolfAttack = distanceForFirstAttack / 3;

            distanceForSamuraAttack = distanceForFirstAttack / 1.25f;

            movingSpeed = EnemyMove.AgentSpeed / 5;

            FireEffect.RestartFire();

            Timing.RunCoroutine(UpdateAttackState());
        }

        /// <summary>
        /// Отключить инто
        /// </summary>
        public void DisableIntro()
        {
            isIntroEnded = true;
        }

        /// <summary>
        /// Проиграть корутину для босса
        /// </summary>
        public void PlayIntro()
        {
            Timing.RunCoroutine(CoroutineForBossIntro());
        }
    
        /// <summary>
        /// Корутина для интро босса
        /// </summary>
        /// <returns></returns>
        public IEnumerator<float> CoroutineForBossIntro()
        {
            yield return Timing.WaitForSeconds(0.1f);
            EnemyAnimationsController.SetState(8, true);

            Joystick.IsBlock = false;
            Vector3 destVector = GameObject.FindGameObjectWithTag("Player").transform.position;
            transform.localPosition = new Vector3(transform.localPosition.x, -0.259f, transform.localPosition.z);
            transform.localEulerAngles = new Vector3(0, 180, 0);
            destVector.y = transform.position.y;
            Vector3 doorPosition = DynamicGameobjectsManager.GetObjectTransform("Door").position;
            NavMeshAgent navMesh = GetComponent<NavMeshAgent>();
            bool isFired = false;

            EnemyAnimationsController.SetSpeedAnimationByRunSpeed(0.1f);
     
            while (!isIntroEnded)
            {
                transform.position = Vector3.Lerp(transform.position, destVector, Time.deltaTime/2);
                if (navMesh.enabled)
                    navMesh.enabled = false;
                if (!isFired)
                {
                    if (Vector3.Distance(centerOfEnemy.position, doorPosition) < distanceToIntro)
                    {
                        DynamicGameobjectsManager.FireEventDynamicObject("Door");
                        isFired = true;
                        EnemyAnimationsController.DisableAllStates();
                    }
                }
                yield return Timing.WaitForSeconds(Time.deltaTime);
            }
            DynamicGameobjectsManager.FireEventDynamicObject("Box");

            GameLightingManager.FireSomeLights(10);
            EnemyCreator.SendToPlayersCallBossCame(transform,timeToFocus);
            yield return Timing.WaitForSeconds(timeToFocus+1);
            RestartEnemy();
        }

        /// <summary>
        /// Обновлять дистанцию между боссом и игроком
        /// </summary>
        /// <returns></returns>
        private void UpdateDistanceBetweenThisAndPlayer()
        {
            if (EnemyMove.PlayerObjectTransformForFollow 
            && enemyAttack.IsMayToPlayAttackAnimation)
            {
                EnemyAnimationsController.DisableAllStates();
                tempDistance = Vector3.Distance
                    (transform.position, EnemyMove.PlayerObjectTransformForFollow.position);
                if (tempDistance <= distanceForGolfAttack)
                {
                    currentAttackState = 0;
                }
                else if (tempDistance <= distanceForKneeAttack)
                {
                    currentAttackState = 1;
                }
                else if (tempDistance <= distanceForSamuraAttack)
                {
                    currentAttackState = 2;
                }
                else if (tempDistance <= distanceForFirstAttack)
                {
                    currentAttackState = 3;
                }
            }
        }

        /// <summary>
        /// Обычная атака сверху вниз.
        /// </summary>
        private void FirstAttackMethod()
        {
            firstBossAttack.EventStartAttackAnimation(true);
            if (firstBossAttack.AttackToPlayer(true))
            {
                AbstractObjectSounder.PlayWeaponHitAudio
                    (EnemyOpponentChoiser.PlayerConditionsTarget.
                    GetDamage(EnemyAttack.DmgEnemy));
                EnemyAnimationsController.DisableAllStates();
                EnemyAnimationsController.SetSpeedAnimationByRunSpeed(0.5f);
            }
            else
            {
                EnemyAnimationsController.SetSpeedAnimationByRunSpeed(0.1f);
            }
        }

        /// <summary>
        /// Длинная атака. 
        /// Со спадом на колено.
        /// </summary>
        private void KneeAttackMethod()
        {
            firstBossAttack.EventStartNockbackAnimation(true);
            if (firstBossAttack.AttackToPlayer(false))
            {
                AbstractObjectSounder.PlayWeaponHitAudio
                    (EnemyOpponentChoiser.PlayerConditionsTarget.
                    GetDamage(EnemyAttack.DmgEnemy));

                EnemyMove.PlayerCollisionComponent.AddDamageForceToPlayer
                    (firstBossAttack.StartKneePoint.forward,true);
                EnemyAnimationsController.DisableAllStates();
                EnemyAnimationsController.SetSpeedAnimationByRunSpeed(0.5f);
            }
            else
            {
                EnemyAnimationsController.SetSpeedAnimationByRunSpeed(0.1f);
            }
        }

        /// <summary>
        /// Крученый удар.
        /// </summary>
        private void SamuraAttackMethod()
        {
            firstBossAttack.EventStartSamuraAnimation(true);
            if (firstBossAttack.AttackToPlayer(true))
            {
                AbstractObjectSounder.PlayWeaponHitAudio
                    (EnemyOpponentChoiser.PlayerConditionsTarget.
                    GetDamage(EnemyAttack.DmgEnemy));
                EnemyAnimationsController.DisableAllStates();
                EnemyAnimationsController.SetSpeedAnimationByRunSpeed(0.5f);
            }
            else
            {
                EnemyAnimationsController.SetSpeedAnimationByRunSpeed(0.1f);
            }
        }

        /// <summary>
        /// Удар, откидывающий персонажа.
        /// </summary>
        private void GolfAttackMethod()
        {
            firstBossAttack.EventStartGolfAnimation(true);
            if (firstBossAttack.AttackToPlayer(true))
            {
                AbstractObjectSounder.PlayWeaponHitAudio
                    (EnemyOpponentChoiser.PlayerConditionsTarget.
                    GetDamage(EnemyAttack.DmgEnemy));

                EnemyMove.PlayerCollisionComponent.AddDamageForceToPlayer
                    (firstBossAttack.SwordOfEnemy.forward);
                EnemyAnimationsController.DisableAllStates();
                EnemyAnimationsController.SetSpeedAnimationByRunSpeed(0.5f);
            }
            else
            {
                EnemyAnimationsController.SetSpeedAnimationByRunSpeed(0.1f);
            }
        }

        /// <summary>
        /// Обновлять тип атаки босса
        /// </summary>
        /// <returns></returns>
        public override IEnumerator<float> UpdateAttackState()
        {
            //yield return Timing.WaitForSeconds(1);
            while (EnemyConditions.IsAlive)
            {
                if (EnemyMove.IsStopped)
                {
                    if (EnemyMove.PlayerObjectTransformForFollow)
                    {
                        UpdateDistanceBetweenThisAndPlayer();
                        switch (currentAttackState)
                        {
                            case 0:
                                GolfAttackMethod();
                                break;
                            case 1:
                                KneeAttackMethod();
                                break;
                            case 2:
                                SamuraAttackMethod();
                                break;
                            case 3:
                                FirstAttackMethod();
                                break;
                        }
                    }
                    else
                    {
                       EnemyAnimationsController.DisableAllStates();
                       EnemyAnimationsController.SetSpeedAnimationByRunSpeed(0.1f);
                    }
                }
                else
                {
                    if (!firstBossAttack.IsMayToDamage)
                    {
                        EnemyAnimationsController.SetState(0, true);
                        EnemyAnimationsController.SetState(1, false);
                        EnemyAnimationsController.SetState(2, false);
                        EnemyAnimationsController.SetState(3, false);
                        EnemyAnimationsController.SetState(4, false);

                        if (!EnemyConditions.IsFrozen)
                            EnemyMove.DependenceAnimatorSpeedOfVelocity();
                    }
                }
                yield return Timing.WaitForSeconds(refreshLatency);
            }
        }
    }
}

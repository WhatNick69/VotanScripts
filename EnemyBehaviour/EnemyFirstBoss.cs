using GameBehaviour;
using UnityEngine;
using UnityEngine.AI;
using VotanInterfaces;
using VotanLibraries;

namespace EnemyBehaviour
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyFirstBoss 
        : KnightEnemy
    {
        public override void Awake()
        {
            AbstractObjectSounder =
                GetComponent<KnightSounder>();
            EnemyOpponentChoiser =
                GetComponent<EnemyOpponentChoiser>();
            EnemyAnimationsController =
                GetComponent<EnemyAnimationsController>();
            EnemyAttack =
                GetComponent<EnemyAttack>();
            EnemyConditions =
                GetComponent<EnemyConditions>();
            EnemyMove =
                GetComponent<EnemyMove>();
            DownInterfaceRotater =
                GetComponent<DownInterfaceRotater>();

            IceEffect =
                LibraryStaticFunctions.DeepFind(transform, "IceStack")
                .GetComponent<IIceEffect>();
            ElectricEffect =
                LibraryStaticFunctions.DeepFind(transform, "ElectricStack")
                .GetComponent<IElectricEffect>();
            Physicffect =
                LibraryStaticFunctions.DeepFind(transform, "PhysicStack")
                .GetComponent<IPhysicEffect>();
            ScoreAddingEffect =
                LibraryStaticFunctions.DeepFind(transform, "ScoreStack")
                .GetComponent<IScoreAddingEffect>();
            FireEffect =
                LibraryStaticFunctions.DeepFind(transform, "FireStack")
                .GetComponent<IFireEffect>();

            gameObject.SetActive(false);
        }

        public override void RestartEnemy()
        {
            base.RestartEnemy();
            Debug.Log("Перезагрузка");
        }
    }
}

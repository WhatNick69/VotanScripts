using MovementEffects;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove 
    : MonoBehaviour {

    private NavMeshAgent agent;
    private Transform playerUnitTransform;
    private bool isAlive;
    [SerializeField,Tooltip("Частота обновления позиции врага")]
    private float frequencySearching;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        playerUnitTransform = GameObject.FindWithTag("Player").transform;
        isAlive = true;
        Timing.RunCoroutine(CoroutineForSearchingByPlayerObject());
    }
    
    IEnumerator<float> CoroutineForSearchingByPlayerObject()
    {
        while (isAlive)
        {
            if (agent != null)
            {
                agent.SetDestination(playerUnitTransform.position);
            }
            yield return Timing.WaitForSeconds(frequencySearching);
        }
    }
}

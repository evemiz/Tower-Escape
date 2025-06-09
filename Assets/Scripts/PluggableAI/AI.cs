using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class AI : MonoBehaviour
{
    NavMeshAgent agent;
    Animator anim;
    public Transform player;

    public GameObject projectilePrefab;
    public Transform fireballSpawnPoint;

    State currentState;

    public Transform checkpointsParent;
    public List<Transform> checkpoints = new List<Transform>();


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        foreach (Transform child in checkpointsParent)
        {
            checkpoints.Add(child);
        }

        currentState = new Idle(gameObject, agent, anim, player);
    }

    void Update()
    {
        currentState = currentState.Process();
    }

    public GameObject GetProjectile() => projectilePrefab;
    public Transform GetShootPoint() => fireballSpawnPoint;

    public void TakeDamage()
{
    if (!(currentState is Attack))
    {
        currentState = new Attack(this.gameObject, agent, anim, player, GetProjectile(), GetShootPoint());
    }
}
}

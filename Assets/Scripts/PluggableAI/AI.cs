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

    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        anim = this.GetComponent<Animator>();
        currentState = new Idle(this.gameObject, agent, anim, player);
    }

    void Update()
    {
        currentState = currentState.Process();
    }

    public GameObject GetProjectile() => projectilePrefab;
    public Transform GetShootPoint() => fireballSpawnPoint;
}

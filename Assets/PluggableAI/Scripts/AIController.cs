using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject target;
    Animator anim;
    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        agent.stoppingDistance = 2f;
        anim = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(target.transform.position);
        if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        {
            anim.SetBool("isMoving", false);
        }
        else
        {
            anim.SetBool("isMoving", true);
        }
    }
}

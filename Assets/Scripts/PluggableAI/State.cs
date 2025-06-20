using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// Base State class for NPC AI state machine.
public class State
{
    // Possible states for the NPC
    public enum STATE
    {
        IDLE, PATROL, PURSUE, ATTACK
    };
    // Stages of a state (used for transitions)
    public enum EVENT
    {
        ENTER, UPDATE, EXIT
    };
    public STATE name; // Current state name
    protected EVENT stage; // Current stage in the state
    protected GameObject npc; // Reference to the NPC GameObject
    protected Animator anim; // Reference to the NPC's Animator
    protected Transform player; // Reference to the player Transform
    protected State nextState; // Next state to transition to
    protected NavMeshAgent agent; // Reference to the NPC's NavMeshAgent

    // Vision and attack parameters
    float visDist = 10.0f; // Vision distance
    float visAngle = 90.0f; // Vision angle
    float shootDist = 7.0f; // Shooting distance

    /// Constructor for the State class.
    public State(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player)
    {
        npc = _npc;
        agent = _agent;
        anim = _anim;
        stage = EVENT.ENTER;
        player = _player;
    }

    // Methods to handle state transitions
    public virtual void Enter() { stage = EVENT.UPDATE; }
    public virtual void Update() { stage = EVENT.UPDATE; }
    public virtual void Exit() { stage = EVENT.EXIT; }

    /// Processes the current state and handles transitions.
    public State Process()
    {
        if (stage == EVENT.ENTER) Enter();
        if (stage == EVENT.UPDATE) Update();
        if (stage == EVENT.EXIT)
        {
            Exit();
            return nextState;
        }
        return this;
    }

    /// Checks if the NPC can see the player based on distance and angle.
    public bool CanSeePlayer()
    {
        Vector3 direction = player.position - npc.transform.position;
        float angle = Vector3.Angle(direction, npc.transform.forward);

        if (direction.magnitude < visDist && angle < visAngle)
        {
            return true;
        }
        return false;
    }

    /// Checks if the NPC can attack the player based on distance.
    public bool CanAttackPlayer()
    {
        Vector3 direction = player.position - npc.transform.position;
        if (direction.magnitude < shootDist)
        {
            return true;
        }
        return false;
    }
}

/// Idle state for the NPC.
public class Idle : State
{
    public Idle(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player) : base(_npc, _agent, _anim, _player)
    {
        name = STATE.IDLE;
    }

    /// Called when entering the Idle state.
    public override void Enter()
    {
        anim.SetTrigger("isIdle"); // Set idle animation
        base.Enter();
    }

    /// Called every frame while in the Idle state.
    public override void Update()
    {
        if (CanSeePlayer())
        {
            nextState = new Pursue(npc, agent, anim, player);
            stage = EVENT.EXIT;
        }
        else if (Random.Range(0, 100) < 10)
        {
            AI ai = npc.GetComponent<AI>();
            nextState = new Patrol(npc, agent, anim, player, ai.checkpoints);
            stage = EVENT.EXIT;
        }
    }

    /// Called when exiting the Idle state.
    public override void Exit()
    {
        anim.ResetTrigger("isIdle"); // Reset idle animation trigger
        base.Exit();
    }
}

/// Patrol state for the NPC. NPC moves between checkpoints.
public class Patrol : State
{
    int currentIndex = -1;
    List<Transform> checkpoints;

    public Patrol(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player, List<Transform> _checkpoints)
        : base(_npc, _agent, _anim, _player)
    {
        name = STATE.PATROL;
        checkpoints = _checkpoints;
        agent.speed = 1;
        agent.isStopped = false;
    }

    public override void Enter()
    {
        float lastDist = Mathf.Infinity;
        for (int i = 0; i < checkpoints.Count; i++)
        {
            float distance = Vector3.Distance(npc.transform.position, checkpoints[i].position);
            if (distance < lastDist)
            {
                currentIndex = i - 1;
                lastDist = distance;
            }
        }

        anim.SetTrigger("isWalking");
        base.Enter();
    }

    public override void Update()
    {
        if (agent.remainingDistance < 1)
        {
            currentIndex = (currentIndex + 1) % checkpoints.Count;
            agent.SetDestination(checkpoints[currentIndex].position);
        }

        if (CanSeePlayer())
        {
            nextState = new Pursue(npc, agent, anim, player);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        anim.ResetTrigger("isWalking");
        base.Exit();
    }
}


public class Pursue : State
{
    public Pursue(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player) : base(_npc, _agent, _anim, _player)
    {
        name = STATE.PURSUE;
        agent.speed = 5;
        agent.isStopped = false;
    }

    public override void Enter()
    {
        anim.SetTrigger("isRunning");
        base.Enter();
    }

    public override void Update()
    {
        agent.SetDestination(player.position);
        if (agent.hasPath)
        {
            if (CanAttackPlayer())
            {
                AI ai = npc.GetComponent<AI>();
                nextState = new Attack(npc, agent, anim, player, ai.GetProjectile(), ai.GetShootPoint());
                stage = EVENT.EXIT;
            }
            else if (!CanSeePlayer())
            {
                AI ai = npc.GetComponent<AI>();
                nextState = new Patrol(npc, agent, anim, player, ai.checkpoints);
                stage = EVENT.EXIT;
            }
        }
    }

    public override void Exit()
    {
        anim.ResetTrigger("isRunning");
        base.Exit();
    }
}

public class Attack : State
{
    float rotationSpeed = 2.0f;
    // AudioSource attackSound;

    GameObject projectilePrefab;
    Transform shootPoint;
    float shootForce = 800f;
    float shootCooldown = 1f;
    float shootTimer = 0f;  

    public Attack(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player, GameObject _projectilePrefab, Transform _shootPoint)
        : base(_npc, _agent, _anim, _player)
    {
        name = STATE.ATTACK;
        projectilePrefab = _projectilePrefab;
        shootPoint = _shootPoint;
    }

    public override void Enter()
    {
        anim.SetTrigger("isAttack");
        agent.isStopped = true;
        // attackSound.Play();
        shootTimer = shootCooldown;
        base.Enter();
    }

    public override void Update()
    {
        Vector3 direction = player.position - npc.transform.position;
        npc.transform.rotation = Quaternion.Slerp(npc.transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * rotationSpeed);

        shootTimer += Time.deltaTime;
        if (shootTimer >= shootCooldown)
        {
            ShootAtPlayer();
            shootTimer = 0f;
        }

        if (!CanAttackPlayer())
        {
            nextState = new Idle(npc, agent, anim, player);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        anim.ResetTrigger("isAttack");
        // attackSound.Stop();
        base.Exit();
    }

    void ShootAtPlayer()
    {
        Debug.Log("Shooting fireball!");
        GameObject projectile = GameObject.Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 shootDir = (player.position - shootPoint.position).normalized;
            rb.AddForce(shootDir * shootForce);
        }
    }

}

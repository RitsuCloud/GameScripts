using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public float lookRadius = 10f;
    Animator animator;

    public GeneralHealth generalHealth;
    Transform target;
    NavMeshAgent agent;

    bool spawned = false;

    // Start is called before the first frame update
    void Start()
    {   
        animator = GetComponent<Animator>();
        Debug.Log(animator);
        target = PlayerManager.instance.player.transform;
        agent = GetComponent<NavMeshAgent>();

        agent.enabled = false; // Disable NavMeshAgent to not move
    }

    // Update is called once per frame
    void Update()
    {
        AnimatorStateInfo animationState = animator.GetCurrentAnimatorStateInfo(0);

        
        if (animationState.IsName("Spawn"))
        {
            return;
        }

        // Enable the agent after the spawn animation has finished
        if (!spawned && !animationState.IsName("Spawn"))
        {
            agent.enabled = true;
            spawned = true;
        }

        float distance = Vector3.Distance(target.position, transform.position);

        if (distance <= lookRadius && generalHealth.IsDead == false)
        {
            // make character move
            animator.SetBool("IsWalking", true);

            // make the character move towards player
            agent.SetDestination(target.position);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}

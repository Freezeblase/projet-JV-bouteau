using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    public int health = 2;
    public float detectionRadius = 10f;  // Radius within which the enemy detects the player
    public float attackRange = 2f;       // Range at which the enemy attacks the player
    public float attackCooldown = 1.5f; // Time between attacks

    private NavMeshAgent agent;          // Reference to the NavMeshAgent
    private Animator animator;           // Reference to the Animator
    private Transform player;            // Reference to the player
    private bool isAttacking = false;    // Tracks if the enemy is attacking
    private float attackCooldownTimer = 0f; // Timer for attack cooldown

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player == null) return;

        // Calculate distance to the player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // AI behavior
        if (distanceToPlayer <= detectionRadius && distanceToPlayer > attackRange)
        {
            ChasePlayer();
        }
        else if (distanceToPlayer <= attackRange)
        {
            if (!isAttacking && attackCooldownTimer <= 0f)
            {
                AttackPlayer();
            }
        }
        else
        {
            Idle();
        }

        // Handle attack cooldown
        if (attackCooldownTimer > 0f)
        {
            attackCooldownTimer -= Time.deltaTime;
        }
    }

    private void ChasePlayer()
    {
        if (agent.isStopped) agent.isStopped = false;

        agent.SetDestination(player.position);
        animator.SetBool("IsMoving", true);
    }

    private void Idle()
    {
        agent.isStopped = true;
        animator.SetBool("IsMoving", false);
    }

    private void AttackPlayer()
    {
        isAttacking = true;
        agent.isStopped = true; // Stop moving during the attack
        animator.SetBool("IsMoving", false);
        animator.SetTrigger("IsAttacking");

        // Reset attack cooldown timer
        attackCooldownTimer = attackCooldown;
    }

    // Triggered by animation event at the moment of dealing damage
    public void AnimationEvent_DealDamage()
    {
        // Ensure the enemy is still in range to attack
        if (Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            Debug.Log("Player takes damage");
            player.GetComponent<PlayerHealth>().TakeDamage(1);
        }
    }

    // Triggered by animation event at the end of the attack animation
    public void AnimationEvent_AttackEnd()
    {
        isAttacking = false; // Reset the attacking status
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("Got Hit");
        animator.SetTrigger("IsHit");
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Enemy died");
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize detection radius in the Scene view
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        // Visualize attack range in the Scene view
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("AttackArea"))
        {
            TakeDamage(1);  // Reduce 1 HP on attack
        }
    }
}

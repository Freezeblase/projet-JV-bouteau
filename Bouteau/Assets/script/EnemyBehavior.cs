using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    public int health = 2;
    public float detectionRadius = 10f;  
    public float attackRange = 2f;       
    public float attackCooldown = 1.5f; 
    public int attackStrenght = 1;

    private NavMeshAgent agent;          
    private Animator animator;
    private Transform player;
    private bool isAttacking = false;
    private float attackCooldownTimer = 0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

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
        agent.isStopped = true;
        animator.SetBool("IsMoving", false);
        animator.SetTrigger("IsAttacking");

        attackCooldownTimer = attackCooldown;
    }

    public void AnimationEvent_DealDamage()
    {
        if (Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            Debug.Log("Player takes damage");
            player.GetComponent<PlayerHealth>().TakeDamage(attackStrenght);
        }
    }

    public void AnimationEvent_AttackEnd()
    {
        isAttacking = false; 
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
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("AttackArea"))
        {
            TakeDamage(1); 
        }
    }
}

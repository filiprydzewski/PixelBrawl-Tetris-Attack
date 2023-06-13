using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{

    public Animator animator;
    
    public Transform attackPoint;
    public LayerMask enemyLayer;

    public float attackRange = 0.5f;
    public int attackDamage = 10;

    public float attackRate = 2f;
    float nextAttackTime = 0f;

    [SerializeField] private KeyCode attackKey;

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetKeyDown(attackKey) && !gameObject.GetComponent<ObjectStunning>().isStunned)
            {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
    }

    void Attack()
    {
        // Play an attack animation
        // Maybe check if last attack was < 1 sec ago to enable combos?
        animator.SetTrigger("Attack");

        // Detect enemy in range of attack
        // bedzie tylko jeden ale zrobilem tablice bo tak bylo w poradniku
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

        foreach(Collider2D enemy in hitEnemies)
        {
            if (!enemy.GetComponent<ObjectStunning>().isStunned)
            {
                Debug.Log("We hit " + enemy.name);
                enemy.GetComponent<ObjectHealth>().TakeDamage(attackDamage);
                enemy.GetComponent<ObjectStunning>().stun();
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

}



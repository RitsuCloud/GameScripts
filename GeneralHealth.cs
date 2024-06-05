using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralHealth : MonoBehaviour
{
    public bool IsDead;
    public int maxHealth = 100;

    Animator animator;

    public GameObject curObject;
    public string tagOfTarget;

    public int currentHealth;
    public int damageAmount = 10;

    public HealthBar healthBar;

    public delegate void DeathEvent();
    public event DeathEvent OnDeath;


    void Start()
    {
        IsDead = false;
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag(tagOfTarget))
        {
            animator.SetBool("IsAttacking", true);
            TakeDamage(damageAmount);
        }
    }

    void OnTriggerExit(Collider other)
    {
        animator.SetBool("IsAttacking", false);
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damageAmount;
        healthBar.SetHealth(currentHealth);
        if (currentHealth <= 0)
        {
            deadAnimation();
        }
    }

    void deadAnimation()
    {
        IsDead = true;
        animator.SetBool("IsDead", true);
        Invoke("Die", 5);
    }
    void Die()
    {
        if (OnDeath != null)
        {
            OnDeath();
        }
        curObject.SetActive(false);
    }
}

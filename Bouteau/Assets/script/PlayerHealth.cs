using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 5;          
    private int currentHealth;         
    public float invincibilityDuration = 1f; 
    private bool isInvincible = false; 
    private Animator animator;         

    public TextMeshProUGUI healthText;

    void Start()
    {
        currentHealth = maxHealth;

        animator = transform.Find("ToonRTS_demo_Knight").GetComponent<Animator>();

        UpdateHealthText();
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible) return;

        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;

        UpdateHealthText();

        if (currentHealth > 0)
        {
            StartCoroutine(ActivateInvincibility());
        }
        else
        {
            Die();
        }
    }

    private void UpdateHealthText()
    {
        if (healthText != null)
        {
            healthText.text = "Health: " + currentHealth + "/" + maxHealth;
        }
    }

    private void Die()
    {
        Debug.Log("Player Died");

         if (animator != null)
        {
            animator.SetBool("isDead", true);
        }

        PlayerMovement playerMovement = GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }

        CharacterController characterController = GetComponent<CharacterController>();
        if (characterController != null)
        {
            characterController.enabled = false;
        }

        Collider playerCollider = GetComponent<Collider>();
        if (playerCollider != null)
        {
            playerCollider.enabled = false;
        }

        Transform playerBody = transform.Find("PlayerBody");
        if (playerBody != null)
        {
            playerBody.gameObject.SetActive(false);
        }

        StartCoroutine(WaitForRestart());
    }

    private System.Collections.IEnumerator ActivateInvincibility()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibilityDuration);
        isInvincible = false;
    }

    private System.Collections.IEnumerator WaitForRestart()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                break;
            }
            yield return null;
        }
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}

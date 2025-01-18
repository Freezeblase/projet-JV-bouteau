using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
       public int maxHealth = 5;          // Maximum health of the player
    private int currentHealth;         // Current health of the player
    public float invincibilityDuration = 1f; // Duration of invincibility frames
    private bool isInvincible = false; // Tracks if the player is currently invincible

    public Image healthBar;            // Reference to the health bar UI element
    private Animator animator;         // Reference to the Animator
    public TMP_Text  restartText;           // Reference to the "Press R to Restart" text UI element

    void Start()
    {
        // Initialize health
        currentHealth = maxHealth;

        // Get the Animator component
        animator = transform.Find("PlayerBody").GetComponent<Animator>();

        // Update the health bar UI at the start
        UpdateHealthBar();

        // Hide the restart text initially
        if (restartText != null)
        {
            restartText.gameObject.SetActive(false);
        }
    }

    // This function handles taking damage
    public void TakeDamage(int damage)
    {
        if (isInvincible) return; // Don't take damage if invincible

        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;

        // Trigger the isHit animation
        animator.SetTrigger("isHit");

        // Update the health bar UI
        UpdateHealthBar();

        // Start invincibility frames
        if (currentHealth > 0) // Only trigger invincibility if still alive
        {
            StartCoroutine(ActivateInvincibility());
        }
        else
        {
            Die();
        }
    }

    // This function handles player death
    private void Die()
    {
        Debug.Log("Player Died");

        // Disable the player's movement (disable the character controller or movement script)
        CleanThirdPersonController playerMovement = GetComponent<CleanThirdPersonController>();
        if (playerMovement != null)
        {
            playerMovement.enabled = false;  // Disable movement script or any relevant behavior
        }

        // Disable the player's movement by disabling the CharacterController
        CharacterController characterController = GetComponent<CharacterController>();
        if (characterController != null)
        {
            characterController.enabled = false;  // Disable the CharacterController to stop movement
        }

        // Disable the player's collider (optional, prevents further interactions)
        Collider playerCollider = GetComponent<Collider>();
        if (playerCollider != null)
        {
            playerCollider.enabled = false;
        }

        // Find and disable the PlayerBody child
        Transform playerBody = transform.Find("PlayerBody");  // Ensure "PlayerBody" is the correct name of the child
        if (playerBody != null)
        {
            playerBody.gameObject.SetActive(false);  // Disable the PlayerBody GameObject
        }

        // Show the restart text
        if (restartText != null)
        {
            restartText.gameObject.SetActive(true);
        }

        // Start listening for restart input
        StartCoroutine(WaitForRestart());
    }



    // Activates invincibility for a set duration
    private System.Collections.IEnumerator ActivateInvincibility()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibilityDuration);
        isInvincible = false;
    }

    // Updates the health bar UI
    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = (float)currentHealth / maxHealth;
        }
    }

    // Heal the player (optional)
    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;

        // Update the health bar UI
        UpdateHealthBar();
    }

    // Wait for the player to press "R" to restart the scene
    private System.Collections.IEnumerator WaitForRestart()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.R)) // Check for "R" key press
            {
                // Reload the current scene
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                break;
            }
            yield return null;
        }
    }

    // Gets the player's current health
    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}

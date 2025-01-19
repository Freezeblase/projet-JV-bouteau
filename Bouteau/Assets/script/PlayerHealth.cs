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
    private Animator animator;         // Reference to the Animator

    // Reference to the TextMesh Pro UI element
    public TextMeshProUGUI healthText;

    void Start()
    {
        // Initialize health
        currentHealth = maxHealth;

        // Get the Animator component
        animator = transform.Find("ToonRTS_demo_Knight").GetComponent<Animator>();

        // Update the health text at the start
        UpdateHealthText();
    }

    // This function handles taking damage
    public void TakeDamage(int damage)
    {
        if (isInvincible) return; // Don't take damage if invincible

        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;

        // Update the health text
        UpdateHealthText();

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

    // This function updates the health display text
    private void UpdateHealthText()
    {
        // Update the text to show current health / max health
        if (healthText != null)
        {
            healthText.text = "Health: " + currentHealth + "/" + maxHealth;
        }
    }

    // This function handles player death
    private void Die()
    {
        Debug.Log("Player Died");

         if (animator != null)
        {
            animator.SetBool("isDead", true);
        }

        // Disable the player's movement (disable the character controller or movement script)
        PlayerMovement playerMovement = GetComponent<PlayerMovement>();
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

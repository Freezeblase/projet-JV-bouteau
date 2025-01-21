using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class win : MonoBehaviour
{
    public int requiredChests = 3; // Number of chests needed to win
    public GameObject winTextCanvas; // Reference to the Canvas GameObject

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Get the PlayerControler component from the player
            PlayerControler playerControler = other.GetComponent<PlayerControler>();

            if (playerControler != null && playerControler.count >= requiredChests)
            {
                Debug.Log("You Win!");
                // Enable the WinText Canvas
                winTextCanvas.SetActive(true);
            }
            else
            {
                Debug.Log("You need more chests to win!");
            }
        }
    }

}

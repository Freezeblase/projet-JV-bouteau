using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class win : MonoBehaviour
{
    public int requiredChests = 3;
    public GameObject winTextCanvas;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerControler playerControler = other.GetComponent<PlayerControler>();

            if (playerControler != null && playerControler.count >= requiredChests)
            {
                Debug.Log("You Win!");
                winTextCanvas.SetActive(true);
            }
            else
            {
                Debug.Log("You need more chests to win!");
            }
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class PlayerControler : MonoBehaviour
{
 public int count;
 public TextMeshProUGUI countText;
 public GameObject winTextObject;

    // Start is called before the first frame update
    void Start()
    {
        count = 0;
        SetCountText();
        winTextObject.SetActive(false);    
    }

    void OnTriggerEnter(Collider other) 
    {
     if (other.gameObject.CompareTag("PickUp")) 
        {
            other.gameObject.SetActive(false);
            count = count + 1;
            SetCountText();
        }
    }


    void SetCountText() 
    {
       countText.text = "Count: " + count.ToString();
    /*if (count >= 3)
        {
            winTextObject.SetActive(true);
        }*/
    }

    // Update is called once per frame
    /*void Update()
    {
        
    }*/
}

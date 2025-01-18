using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script : MonoBehaviour
{

    private float compteur;

    // Start is called before the first frame update
    void Start()
    {
        print("début");
    }

    // Update is called once per frame
    void Update()
    {
        compteur = compteur + 1;
        print(compteur);
    }
}

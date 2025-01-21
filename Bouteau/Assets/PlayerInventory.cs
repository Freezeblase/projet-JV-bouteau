using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public int chestCount = 0;
    public int requiredChests = 3;

    public void AddChest()
    {
        chestCount++;
    }

    public bool HasAllChests()
    {
        return chestCount >= requiredChests;
    }
}

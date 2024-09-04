using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private int order = 0;
    
    public int getOrder()
    {
        return order;
    }
}

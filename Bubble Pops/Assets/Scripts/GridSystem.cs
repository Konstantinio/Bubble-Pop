using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem : MonoBehaviour
{
    private static List<Bubble> bubbles;
    private void Start()
    {
        InitializeGridSystem();
    }

    private void InitializeGridSystem()
    {
        
    }

    public static List<Bubble> GetAllBubbles()
    {
        return bubbles;
    }
}

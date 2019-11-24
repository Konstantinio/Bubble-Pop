using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerationManager : MonoBehaviour
{
    public GameObject bubblePrefab;
    public GameObject ghostBubblePrefab;
    public GameObject parent;
    public Vector2 startPosition;
    public float xGap;
    public float yGap;
    private int lineLength = 6;
    private int startHeight = 5;

    private void Start()
    {
        GenerateBasicLevel();
    }

//    private void Update()
//    {
//        GenerateBasicLevel();
//    }

    private void GenerateBasicLevel()
    {
        for (int y = 0; y < lineLength; y++)
        {
            for (int x = 0; x < startHeight; x++)
            {
                
                if (x == startHeight - 1)
                {
                    Instantiate(ghostBubblePrefab, startPosition+new Vector2(y*xGap + x%2*xGap/2f,-x*yGap), Quaternion.identity, parent.transform);
                }else
                {
                    Instantiate(bubblePrefab, startPosition+new Vector2(y*xGap + x%2*xGap/2f,-x*yGap), Quaternion.identity, parent.transform);
                }
            }
            
        }
    }
}

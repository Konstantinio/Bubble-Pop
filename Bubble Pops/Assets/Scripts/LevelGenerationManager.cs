using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerationManager : MonoBehaviour
{
    public GameObject ballPrefab;
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

    private void GenerateBasicLevel()
    {
        for (int y = 0; y < startHeight; y++)
        {
            for (int x = 0; x < lineLength; x++)
            {
                Instantiate(ballPrefab, startPosition+new Vector2(x*xGap + y%2*xGap/2.2f,-y*yGap), Quaternion.identity, parent.transform);
            }
        }
    }
}

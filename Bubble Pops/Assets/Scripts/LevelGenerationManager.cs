using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

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
    [SerializeField] private Camera _camera;
    [SerializeField] private ColorConfiguration colorConfiguration;

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
                GameObject newBubble;
                if (x == startHeight - 1)
                {
                   newBubble =  Instantiate(ghostBubblePrefab, startPosition+new Vector2(y*xGap + x%2*xGap/2f,-x*yGap), Quaternion.identity, parent.transform);
                }else
                {
                    newBubble =   Instantiate(bubblePrefab, startPosition+new Vector2(y*xGap + x%2*xGap/2f,-x*yGap), Quaternion.identity, parent.transform);
                }

                if (newBubble.GetComponent<Bubble>().isGhost)
                {
                    newBubble.GetComponent<Bubble>().indexer = -1;
                }
                else
                {
                    int indexer = (int) Math.Pow(2, Random.Range(1, 4));
                    newBubble.GetComponent<Bubble>().SetIndexer(indexer);
                    newBubble.GetComponent<SpriteRenderer>().color =
                        colorConfiguration.colorPowers.First(l => l.indexer == indexer ).color;
                    newBubble.GetComponentInChildren<Canvas>().worldCamera = _camera;
                }
               
            }
            
        }
    }
}

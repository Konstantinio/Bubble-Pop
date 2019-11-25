using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class LevelGenerationManager : MonoBehaviour
{
    public List<GameObject> allBubbles;
    public GameObject bubblePrefab;
    public GameObject ghostBubblePrefab;
    public GameObject parent;
    public Vector2 startPosition;
    public float xGap;
    public float yGap;
    private int lineLength =  6;
    private int startHeight = 5;
    [SerializeField] private Camera _camera;
    [SerializeField] private ColorConfiguration colorConfiguration;

    private void Start()
    {
        allBubbles = new List<GameObject>();
        GenerateBasicLevel();
    }

    public void RefreshIsVisited()
    {
        allBubbles.Where(x => x != null).ToList().ForEach(x=>x.GetComponent<Bubble>().isVisited=false);
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
                newBubble =   Instantiate(bubblePrefab, startPosition+new Vector2(y*xGap + x%2*xGap/2f,-x*yGap), Quaternion.identity, parent.transform);
                newBubble.GetComponent<Bubble>().startGap = x % 2*xGap/2f;
                    int indexer = (int) Math.Pow(2, Random.Range(1, 6));
                    newBubble.GetComponent<Bubble>().SetIndexer(indexer);
                    newBubble.GetComponent<Fly>().isHitted = true;
                    newBubble.GetComponent<SpriteRenderer>().color =
                        colorConfiguration.colorPowers.First(l => l.indexer == indexer ).color;
                    newBubble.GetComponentInChildren<Canvas>().worldCamera = _camera;

            }
            
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Bubble : MonoBehaviour
{
    public GameObject ghostBubblePrefab;
    public Bubble[] nearBubbles = new Bubble[6];
    public int indexer;
    public float maxDistance;
    public bool isShowingRedLines;
    public Text numberText;

    public bool isVisited;
    public bool isGhost;
    public LevelGenerationManager manager;

    //  private float timer = 0.1f;
    private void Start()
    {
        InitializeBubble();
        manager = GameObject.FindGameObjectWithTag("LevelGenerationManager").GetComponent<LevelGenerationManager>();
        if (!isGhost)
        {
            manager.allBubbles.Add(gameObject);
        }
    }

    public void StartChain()
    {
        Bubble[] nearBubblesWithoutNulls = nearBubbles.Where(x => x != null).ToArray();
        if (!isVisited && !isGhost)
        {
            isVisited = true;

            var collection = nearBubblesWithoutNulls.Where(x => x.indexer == indexer).ToList();
            collection.ForEach(y => y.StartChain());
            if (collection.Count != 0)
            {
                nearBubblesWithoutNulls.Where(c => c.isGhost).ToList().ForEach(x => Destroy(x.gameObject));
                Destroy(gameObject);
                // GetComponent<SpriteRenderer>().color = Color.clear;
                // foreach (Transform child in transform)
                //  {
                //      child.gameObject.SetActive(false);
                //  }
            }
        }
    }

    private void Update()
    {
        InitializeBubble();
//        timer -= Time.deltaTime;
//         if (timer <= 0)
//         {
//             Destroy(gameObject);
//         }
    }

    public void SetIndexer(int _indexer)
    {
        indexer = _indexer;
        numberText.text = _indexer.ToString();
    }

    private void InitializeBubble()
    {

            Vector2 p = transform.position;
            SetNearBubble(0, ShootRay(p, 0));
            SetNearBubble(1, ShootRay(p, 1));
            SetNearBubble(2, ShootRay(p, 2));
            SetNearBubble(3, ShootRay(p, 3));
            SetNearBubble(4, ShootRay(p, 4));
            SetNearBubble(5, ShootRay(p, 5));
        
            for (int i = 0; i < 6; i++)
            {
                if (nearBubbles[i] != null)
                {
                    if (i <= 2)
                    {
                        nearBubbles[i].nearBubbles[i + 3] = this;
                    }
                    else
                    {
                        nearBubbles[i].nearBubbles[i - 3] = this;
                    }
                }
                
        }
        
    }

    private void SetNearBubble(int index, Bubble bubbleToSet)
    {
        // if (nearBubbles[index] == null)
       // {
        nearBubbles[index] = bubbleToSet;
       // }
    }


    [CanBeNull]
    private Bubble ShootRay(Vector2 startPosition, int index)
    {
        var x = startPosition.x;
        var y = startPosition.y;
        var endPosition = Vector2.zero;
        switch (index)
        {
            case 0:
                endPosition = Vector2.left + Vector2.up;
                break;
            case 1:
                endPosition = Vector2.right + Vector2.up;
                break;
            case 2:
                endPosition = Vector2.right;
                break;
            case 3:
                endPosition = Vector2.right + Vector2.down;
                break;
            case 4:
                endPosition = Vector2.left + Vector2.down;
                break;
            case 5:
                endPosition = Vector2.left;
                break;
        }

        var hit = Physics2D.Raycast(startPosition, endPosition.normalized, maxDistance, ~(1 << 10));
        if (hit.collider != null && hit.collider.gameObject.GetComponent<Bubble>() != null)
        {
            var bubble = hit.collider.gameObject.GetComponent<Bubble>();
            Debug.DrawLine(startPosition, hit.point,
                (bubble.indexer == 1 && this.indexer == 1) ? Color.yellow : Color.blue);
            return bubble;
        }
        else
        {
            if (isShowingRedLines)
            {
                Debug.DrawRay(startPosition, endPosition * 1000, Color.red);
            }

            if (!isGhost && nearBubbles[index] == null && GetComponent<Fly>().isHitted)
            {
                switch (index)
                {
                    case 0:
                        endPosition = new Vector2(x - 0.322f, y + 0.64f); //Vector2.left + Vector2.up;
                        break;
                    case 1:
                        endPosition = new Vector2(x + 0.322f, y + 0.64f); // Vector2.right + Vector2.up;
                        break;
                    case 2:
                        endPosition = new Vector2(x + 0.692f + 0.05f, y); //Vector2.right;
                        break;
                    case 3:
                        endPosition = new Vector2(x + 0.322f + 0.07f, y - 0.64f); //Vector2.right + Vector2.down;
                        break;
                    case 4:
                        endPosition = new Vector2(x - 0.322f, y - 0.64f); //Vector2.left + Vector2.down;
                        break;
                    case 5:
                        endPosition = new Vector2(x - 0.692f, y); //Vector2.left;
                        break;
                }

                return Instantiate(ghostBubblePrefab, endPosition, Quaternion.identity).GetComponent<Bubble>();
            }
            else
            {
                return null;
            }
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Bubble : MonoBehaviour
{
    public Bubble[] nearBubbles = new Bubble[6];
    public int indexer;
    public float maxDistance;
    public bool isShowingRedLines;
    public Text numberText;
        

    public bool isGhost;

  //  private float timer = 0.1f;
    private void Start()
    {
        InitializeBubble();
    }

//    private void Update()
//    {
//        InitializeBubble();
//        timer -= Time.deltaTime;
//         if (timer <= 0)
//         {
//             Destroy(gameObject);
//         }
//    }

    public void SetIndexer(int _indexer)
    {
        indexer = _indexer;
        numberText.text = _indexer.ToString();
        
    }
    private void InitializeBubble()
    {
        Vector2 p = transform.position;
        SetNearBubble(0, ShootRay(p, Vector2.left + Vector2.up));
        SetNearBubble(1, ShootRay(p, Vector2.right + Vector2.up));
        SetNearBubble(2, ShootRay(p, Vector2.right));
        SetNearBubble(3, ShootRay(p, Vector2.right + Vector2.down));
        SetNearBubble(4, ShootRay(p, Vector2.left + Vector2.down));
        SetNearBubble(5, ShootRay(p, Vector2.left));

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
        if (nearBubbles[index] == null)
        {
            nearBubbles[index] = bubbleToSet;
        }
    }


    [CanBeNull]
    private Bubble ShootRay(Vector2 startPosition, Vector2 endPosition)
    {
        var hit = Physics2D.Raycast(startPosition, endPosition, maxDistance,~(1<<10));
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

            return null;
        }
    }
}
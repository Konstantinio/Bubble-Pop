using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    public Bubble[] nearBubbles = new Bubble[6];
    public int indexer;
    public float maxDistance;
    public bool isShowingRedLines;
    private void Start()
    {
        InitializeBubble();
    }

    private void Update()
    {
        InitializeBubble();
    }

    private void InitializeBubble()
    {
        Vector2 p = transform.position;
        nearBubbles[0] = ShootRay(p, Vector2.left + Vector2.up);
       nearBubbles[1] = ShootRay(p,Vector2.right+Vector2.up);
       nearBubbles[2] = ShootRay(p,Vector2.right);
       nearBubbles[3] = ShootRay(p,Vector2.right+Vector2.down);
       nearBubbles[4] = ShootRay(p,Vector2.left+Vector2.down);
       nearBubbles[5] = ShootRay(p,Vector2.left);

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

    [CanBeNull]
    private Bubble ShootRay(Vector2 startPosition, Vector2 endPosition)
    {
        var hit = Physics2D.Raycast(startPosition, endPosition, maxDistance);
        if (hit.collider != null && hit.collider.gameObject.GetComponent<Bubble>()!=null)
        {
            var bubble = hit.collider.gameObject.GetComponent<Bubble>();
            Debug.DrawLine(startPosition, hit.point, (bubble.indexer == 1 && this.indexer==1) ? Color.yellow : Color.blue);
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

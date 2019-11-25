using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Crush : MonoBehaviour
{
    public float speed;
    private Vector2 startPosition;
    private Vector2 neededPosition;
    private const float TOLERANCE = 0.01f;
    private float koef;
    public bool isFlying;
    public bool isHighest;

    void Update()
    {
        if (isFlying)
        {
            try
            {
                var collection = GetComponent<Bubble>().nearBubbles.Where(x => x.isGhost);
                collection.ToList().ForEach(y=>Destroy(y.gameObject));
            }
            catch (Exception e)
            {
                // ignored
            }


            koef += Time.deltaTime * speed / 10;
            transform.position = Vector2.Lerp(startPosition, neededPosition, koef);
            if (Vector2.Distance(transform.position, neededPosition) < TOLERANCE)
            {
                isFlying = false;
                if (!isHighest)
                {
                    Destroy(gameObject);
                    isHighest = false;
                }
               
            } 
        }
        
    }

    public void Fly(Vector2 position)
    {
        neededPosition = position;
        startPosition = transform.position;
        isFlying = true;
    }
}

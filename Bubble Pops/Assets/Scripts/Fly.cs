using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly : MonoBehaviour
{
    public float speed;
    private Vector2 startPosition;
    private Vector2 neededPosition;
    private bool isFlying;
    private const float TOLERANCE = 0.1f;
    private Vector2[] positions = new Vector2[0];
    private int index = 0;
    private float koef;

    private void Start()
    {
        var position = transform.position;
        startPosition = position;
        neededPosition = position;
    }

    private void Update()
    {
        if (isFlying)
        {
            koef += Time.deltaTime * speed/10;
            transform.position = Vector2.Lerp(startPosition, neededPosition, koef);
            if (Vector2.Distance(transform.position, neededPosition) < TOLERANCE)
            {
                if (index < positions.Length)
                {
                    StartFlying(positions[index]);
                }
                else
                {
                    isFlying = false;
                    index = 0;
                    koef = 0;
                }
                index++;
            } 
        }
      
    }

    public void Launch(Vector2[] _positions)
    {
        positions = _positions;
        isFlying = true;
    }
    private void StartFlying(Vector2 position)
    {
        koef = 0;
        startPosition = transform.position;
         neededPosition = position;
         
    }
}

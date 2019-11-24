using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Fly : MonoBehaviour
{
    public float speed;
    private Vector2 startPosition;
    private Vector2 neededPosition;
    private bool isFlying;
    private const float TOLERANCE = 0.01f;
    private Vector2[] positions = new Vector2[0];
    private int index = 0;
    private float koef;
    public ShootingManager manager;

    private void Start()
    {
        var position = transform.position;
        startPosition = position;
        neededPosition = position;
    }

    private void Update()
    {
        FlyCheck();

    }

    private void FlyCheck()
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
                    enabled = false;
                    manager.Reload();
                }
                index++;
            } 
        }
    }

    public void Launch(Vector2[] _positions)
    {
        startPosition = transform.position;
        positions = _positions;
        neededPosition = positions[0];
        isFlying = true;
    }
    private void StartFlying(Vector2 position)
    {
        koef = 0;
        startPosition = transform.position;
        neededPosition = position;
         
    }
}
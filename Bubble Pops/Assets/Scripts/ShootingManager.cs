using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class ShootingManager : MonoBehaviour
{
    [SerializeField] private Camera cam;
    public Vector2 origin;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Material transparentMaterial;
    [SerializeField] private Material defaultLineMaterial;
    

    void Update()
    {
        DrawLine();
    }

    private void DrawLine()
    {
        if (Input.GetMouseButton(0))
        {

            var mouseVector = (Vector2) cam.ScreenToWorldPoint(Input.mousePosition) - origin;
            var hit = Physics2D.Raycast(origin,mouseVector , Mathf.Infinity);
            if (hit.collider != null)
            {
                Debug.DrawLine(origin, hit.point,Color.green);
                Debug.DrawRay(origin,hit.point-origin,Color.magenta);
                Debug.DrawRay(hit.point,Vector2.Reflect(hit.point-origin,Vector2.right),Color.magenta);
                lineRenderer.sharedMaterial = defaultLineMaterial;
                
                lineRenderer.SetPosition(0,origin);
                lineRenderer.SetPosition(1,hit.point);
                if (hit.collider.CompareTag("Wall"))
                {
                    RaycastHit2D hit2;
                    if (hit.point.x >= 0)
                    {
                        hit2 = Physics2D.Raycast(hit.point - Vector2.right*0.01f, Vector2.Reflect(hit.point - origin, Vector2.right), Mathf.Infinity);
                    }
                    else
                    {
                        hit2 = Physics2D.Raycast(hit.point + Vector2.right*0.01f, Vector2.Reflect(hit.point - origin, Vector2.right), Mathf.Infinity);
                    }

                    lineRenderer.SetPosition(2, !hit2.collider.CompareTag("Wall") ? hit2.point : hit.point);
                }
                else
                {
                    lineRenderer.SetPosition(2,hit.point);
                }
                
               
            }
            else
            {
                lineRenderer.sharedMaterial = transparentMaterial;
            }
        }
    }
    
    
}

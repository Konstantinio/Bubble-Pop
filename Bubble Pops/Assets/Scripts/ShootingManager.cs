using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class ShootingManager : MonoBehaviour
{
    [SerializeField] private Camera cam;
    public Transform originTransform;
    public Transform nextBubbleOriginTransform;
    private Vector2 origin;
    private Vector2 nextBubbleOrigin;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Material transparentMaterial;
    [SerializeField] private Material defaultLineMaterial;
    [SerializeField] private ColorConfiguration colorConfiguration;
    [SerializeField] private GameObject prefab;
    private Vector2 firstPoint;
    private Vector2 secondPoint;
    private Fly shot;
    private Fly nextShot;
    private bool isActive = false;
    private bool isShooting = false;
    private void Start()
    {
        origin = originTransform.position;
        nextBubbleOrigin = nextBubbleOriginTransform.position;
        shot = Instantiate(prefab, origin, Quaternion.identity).GetComponent<Fly>();
        nextShot = Instantiate(prefab, nextBubbleOrigin, Quaternion.identity).GetComponent<Fly>();
        shot.manager = this;
        nextShot.manager = this;
    }

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
                firstPoint = hit.point;
                Debug.DrawLine(origin, hit.point,Color.green);
                Debug.DrawRay(origin,hit.point-origin,Color.magenta);
                Debug.DrawRay(hit.point,Vector2.Reflect(hit.point-origin,Vector2.right),Color.magenta);
                ActivateLine();
                
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

                    if (hit2.collider.CompareTag("Wall"))
                    {
                        DiactivateLine();
                    }
                    else
                    {
                        lineRenderer.SetPosition(2, hit2.point);
                        secondPoint = hit2.point;
                    }
                }
                else
                {
                    lineRenderer.SetPosition(2,hit.point);
                    secondPoint = Vector2.zero;
                }
                
               
            }
            else
            {
                DiactivateLine();
            }
            
            
        }

        if (Input.GetMouseButtonUp(0) && isActive && !isShooting)
        {
            
            if (secondPoint == Vector2.zero)
            {
                Shoot(firstPoint);
            }
            else
            {
                Shoot(firstPoint,secondPoint);
            }

            isShooting = true;
        }
    }

    private void Shoot(Vector2 firstPosition, Vector2 secondPosition)
    {
        
        shot.Launch(new[]{firstPosition,secondPosition});
    }
    
    private void Shoot( Vector2 position)
    {
        shot.Launch(new[]{position});
    }

    private void DiactivateLine()
    {
        isActive = false;
        lineRenderer.sharedMaterial = transparentMaterial;
    }

    private void ActivateLine()
    {
        isActive = true;
        lineRenderer.sharedMaterial = defaultLineMaterial;
    }

    public void Reload()
    {
        nextShot.transform.position = origin;
        shot = nextShot;
        nextShot = Instantiate(prefab, nextBubbleOrigin, Quaternion.identity).GetComponent<Fly>();
        nextShot.manager = this;
        isShooting = false;
    }
    
    

}

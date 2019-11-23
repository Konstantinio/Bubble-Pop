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
            var hit = Physics2D.Raycast(origin, (Vector2)cam.ScreenToWorldPoint(Input.mousePosition)-origin, Mathf.Infinity);
            if (hit.collider != null)
            {
                Debug.DrawLine(origin, hit.point,Color.yellow);
                lineRenderer.sharedMaterial = defaultLineMaterial;
                lineRenderer.SetPosition(0,origin);
                lineRenderer.SetPosition(1,hit.point);
                if (hit.collider.gameObject.CompareTag("Wall"))
                {
                    Vector2 newVector = Vector2.Reflect((Vector2)cam.ScreenToWorldPoint(Input.mousePosition)-origin, hit.normal);
                    var hit2 = Physics2D.Raycast(hit.point, newVector, Mathf.Infinity);
                    
                    if (hit2.collider != null && !hit2.collider.gameObject.CompareTag("Wall"))
                    {
                        lineRenderer.SetPosition(2,hit2.point);
                      //  Debug.Log(hit2.point);
                    }
                    else
                    {
                       // Debug.Log("Nothing after wall");
                        //  lineRenderer.SetPosition(2,hit.point);
                    }
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

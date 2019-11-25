using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;
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
    [SerializeField] private GameObject ghostBubblePrefab;
    private Vector2 firstPoint;
    private Vector2 secondPoint;
    private Fly shot;
    private Fly nextShot;
    private bool isActive = false;
    private bool isShooting = false;
    private GameObject ghostBubbleEffect;
    private void Start()
    {
        SetOrigins();
        InitializeShootingSystem();
    }

    private void SetOrigins()
    {
        origin = originTransform.position;
        nextBubbleOrigin = nextBubbleOriginTransform.position;
        
    }
    private void InitializeShootingSystem()
    {
        int index1 = GetRandomIndexer();
        shot = Instantiate(prefab, origin, Quaternion.identity).GetComponent<Fly>();
        shot.GetComponent<SpriteRenderer>().color = colorConfiguration.colorPowers.First(x => x.indexer == index1).color;
        shot.GetComponent<Bubble>().SetIndexer(index1);
        
        int index2 = GetRandomIndexer();
        nextShot = Instantiate(prefab, nextBubbleOrigin, Quaternion.identity).GetComponent<Fly>();
        nextShot.GetComponent<SpriteRenderer>().color = colorConfiguration.colorPowers.First(x => x.indexer == index2).color;
        nextShot.GetComponent<Bubble>().SetIndexer(index2);
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
            RaycastHit2D hit2 = new RaycastHit2D();
            var hit = Physics2D.Raycast(origin,mouseVector , Mathf.Infinity,~(1<<9));
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
                    
                    if (hit.point.x >= 0)
                    {
                        hit2 = Physics2D.Raycast(hit.point - Vector2.right*0.01f, Vector2.Reflect(hit.point - origin, Vector2.right), Mathf.Infinity,~(1<<9));
                    }
                    else
                    {
                        hit2 = Physics2D.Raycast(hit.point + Vector2.right*0.01f, Vector2.Reflect(hit.point - origin, Vector2.right), Mathf.Infinity,~(1<<9));
                    }

                    if (hit2.collider.CompareTag("Wall"))
                    {
                        DeactivateLine();
                    }
                    else
                    {
                        lineRenderer.SetPosition(2, hit2.point);
                        secondPoint = hit2.point;
                    }
                }
                else
                {
                    hit2 = hit;
                    secondPoint = Vector2.zero;
                }
                lineRenderer.SetPosition(2,hit2.point);
                if (isActive)
                {
                   
                    Bubble ghostBubble = hit2.collider.gameObject.GetComponent<Bubble>().nearBubbles.ToList().Where(a=>a!=null).OrderBy(x => Vector2.Distance(hit2.point, x.transform.position)).FirstOrDefault(y => y.isGhost);
                    if (ghostBubble != null)
                    {
                        if (secondPoint == Vector2.zero)
                        {
                            firstPoint = ghostBubble.transform.position;
                            secondPoint = firstPoint;
                        }
                        else
                        {
                            secondPoint = ghostBubble.transform.position;
                        }
                    }
                   
                    
                }

                if (ghostBubbleEffect==null||(Vector2)ghostBubbleEffect.transform.position != secondPoint)
                {
                    Destroy(ghostBubbleEffect);
                    ghostBubbleEffect = Instantiate(ghostBubblePrefab, secondPoint, Quaternion.identity);
                }
            }
            else
            {
                Destroy(ghostBubbleEffect);
                DeactivateLine();
            }
            
           
        }

        if (Input.GetMouseButtonUp(0) && isActive && !isShooting)
        {
            Destroy(ghostBubbleEffect);
            DeactivateLine();
            if (secondPoint == Vector2.zero)
            {
                Debug.Log("onepoint");
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

    private void DeactivateLine()
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
        int indexForNewShot = GetRandomIndexer();
        nextShot.transform.position = origin;
        shot = nextShot;
        nextShot = Instantiate(prefab, nextBubbleOrigin, Quaternion.identity).GetComponent<Fly>();
        nextShot.GetComponent<Bubble>().SetIndexer(indexForNewShot);
        nextShot.GetComponent<SpriteRenderer>().color =
            colorConfiguration.colorPowers.First(x => x.indexer == indexForNewShot).color;
        nextShot.manager = this;
        isShooting = false;
    }

    private int GetRandomIndexer()
    {
        return (int)Math.Pow(2, Random.Range(1, 4));
    }
    

}

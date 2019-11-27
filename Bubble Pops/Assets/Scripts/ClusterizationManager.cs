using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class ClusterizationManager : MonoBehaviour
{
    public LevelGenerationManager manager;
    public bool isCrushed;
    private void Start()
    {
    }

    private void Update()
    {
        if (isCrushed)
        {
            CheckForClusters();
            isCrushed = false;
        }
        
    }

    public void CheckForClusters()
    {
        bool isNewCluster = true;
        List<List<Bubble>> clusters = new List<List<Bubble>>();
        var allBubbles = manager.allBubbles.Where(x => x != null).ToArray();
        for (int x = 0; x < allBubbles.Count(); x++)
        {
            var nearbubbles = allBubbles[x].GetComponent<Bubble>().nearBubbles;
            for (int y = 0; y < nearbubbles.Length; y++)
            {
                for (int z = 0; z < clusters.Count; z++)
                {
                    if (clusters[z].Contains(nearbubbles[y]) ||
                        clusters[z].Contains(allBubbles[x].GetComponent<Bubble>()))
                    {
                        clusters[z] = clusters[z].Union(nearbubbles.ToList()).ToList();
                        clusters[z].Add(allBubbles[x].GetComponent<Bubble>());
                        isNewCluster = false;
                      //  break;
                    }
                }

                if (isNewCluster)
                {
                    clusters.Add(nearbubbles.ToList());
                }

                isNewCluster = true;
            }
        }

        Debug.Log(clusters.Count);
        if (clusters.Count >2)
        {
            var cluster = clusters[Random.Range(0, clusters.Count + 1)];
            for (var j = 0; j < cluster.Count; j++)
            {
                foreach (var bubble in cluster)
                {
                    if (bubble != null)
                        bubble.GetComponent<Rigidbody2D>().gravityScale = 1;
                }
            }
        }
       
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CrushingManager : MonoBehaviour
{
    public List<Fly> CrushingBubbles;
    public ColorConfiguration ColorConfiguration;
    private Fly highest;

    public void Crush()
    {
        if (CrushingBubbles.Count >= 2)
        {
           
            highest = CrushingBubbles.OrderByDescending(x => x.transform.position.y).ToArray()[0];
            highest.GetComponent<Crush>().isHighest = true;
            var position = (Vector2)highest.transform.position;
          //  highest.GetComponent<Bubble>().SetIndexer(highest.GetComponent<Bubble>().indexer*(int)Math.Pow(2,CrushingBubbles.Count-1));
           // highest.GetComponent<SpriteRenderer>().color = ColorConfiguration.colorPowers
             //   .FirstOrDefault(x => x.indexer == highest.GetComponent<Bubble>().indexer).color;
         
             
           var newBubble =  Instantiate(GetComponent<LevelGenerationManager>().bubblePrefab);
           newBubble.transform.position = position;
           newBubble.GetComponent<Bubble>().SetIndexer(highest.GetComponent<Bubble>().indexer*(int)Math.Pow(2,CrushingBubbles.Count-1));
           newBubble.GetComponent<SpriteRenderer>().color=ColorConfiguration.colorPowers
               .FirstOrDefault(x => x.indexer == newBubble.GetComponent<Bubble>().indexer).color;
           CrushingBubbles.ForEach(x => x.GetComponent<Crush>().Fly(position));
            CrushingBubbles = new List<Fly>();
            
          
          
        }
       
    }
    
}

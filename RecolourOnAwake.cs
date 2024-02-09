using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecolourOnAwake : MonoBehaviour
{
    
    void OnEnable()
    {
        if(FindObjectOfType<GameData>() == null)
        {
            return;
        }
        GetComponent<SpriteRenderer>().color = FindObjectOfType<GameData>().GetColour();
    }
}

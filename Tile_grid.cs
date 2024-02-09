using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile_grid : MonoBehaviour
{
    public int y_dims;
    public int x_dims;
    public List<string> tile_defs = new List<string>();
    public List<List<Tile>> tiles = new List<List<Tile>>();
    public Controller control;
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

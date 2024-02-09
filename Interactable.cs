using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public bool triggered = false;
    public int triggers = 1;
    public int x;
    public int y;
    private void Start()
    {
        Controller control = FindObjectOfType<Controller>();
        if (control != null)
        {
            control.tiles[x][y].inter = this;
        }
    }
    public void Trigger()
    {
        if(triggers !=0)
        {
            triggers--;
            triggered = !triggered;
        }
    }
}

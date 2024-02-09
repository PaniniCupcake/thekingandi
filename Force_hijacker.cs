using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Force_hijacker : MonoBehaviour
{
    public Piece p;
    public bool white;
    public Controller control;
    public List<int> wkx= new List<int>();
    public List<int> wky = new List<int>();
    public List<int> bkx = new List<int>();
    public List<int> bky = new List<int>();
    public List<int> mx = new List<int>();
    public List<int> my = new List<int>();
    void Start()
    {
        control = FindObjectOfType<Controller>();
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0;i>wkx.Count;i++)
        {
            checkPositions(wkx[i],wky[i],bkx[i],bky[i],mx[i],my[i]);
        }
    }

    private void checkPositions(int a, int b, int c, int d,int x,int y)
    {
        if (control.wking_p.x == a && control.wking_p.y == b && control.bking_p.x == c && control.bking_p.y == d)
        {
            List<int> t1 = new List<int>();
            List<int> t2 = new List<int>();
            if(white)
            {
                t1.Add(a);
                t2.Add(b);
            }
            else
            {
                t1.Add(c);
                t2.Add(d);
            }
            
            List<int> t3 = new List<int>();
            t3.Add(x);
            List<int> t4 = new List<int>();
            t4.Add(y);
            p.forced_moves_trigger_x = t1;
            p.forced_moves_trigger_y = t2;
            p.forced_moves_x = t3;
            p.forced_moves_y = t4;
}
    }
}

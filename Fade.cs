using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
    private int countdown =0;
    void FixedUpdate()
    {
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, (20f - countdown)/20f);
        if(countdown == 20)
        {
            Destroy(this);
        }
        countdown++;
    }
}

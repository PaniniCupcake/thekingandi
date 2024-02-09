using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pedestal : MonoBehaviour
{
    public Interactable trigger;
    private int descending = 0;
    public Sprite empty;
    private void Start()
    {
        if (trigger == null)
        {
            trigger = this.GetComponent<Interactable>();
        }
    }
    void Update()
    {
        if(trigger.triggered)
        {
            trigger.triggered = false;
            Activate();
        }
    }
    private void FixedUpdate()
    {
        if(descending > 20)
        {
            descending--;
        }
        else if (descending > 0)
        {
            //transform.position -= new Vector3(0, 0.05f, 0);
            descending--;
            if(descending == 0)
            {
                Destroy(this);
            }
        }
    }
    public void Activate()
    {
        GetComponent<SpriteRenderer>().sprite = empty;
        descending = 40;
    }

}

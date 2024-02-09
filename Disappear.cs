using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disappear : MonoBehaviour
{
    private int countdown =17;
    void OnEnable()
    {
        countdown = 17;
    }
    private void Update()
    {
        if(countdown == 0)
        {
            gameObject.SetActive(false);
        }
        countdown--;
    }
}

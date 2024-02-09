using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Scene_fade : MonoBehaviour
{
    public string scenename;
    private int countdown = -1;
    private int div;
    private void Start()
    {
        
    }
    void Update()
    {

        if (countdown != -1)
        {
            countdown++;
            if (countdown % (12/div) == 0 && countdown < 100/div)
            {
                print("WA");
                GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.125f * (countdown / (12f/div)));
            }
            if (countdown == 150/div)
            {
                SceneManager.LoadScene(scenename);
            }
        }
        else
        {
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        }
    }

    public void NewScene(string s,int divider)
    {
        scenename = s;
        countdown = 0;
        div = divider;
    }
}

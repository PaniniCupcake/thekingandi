using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneTime : MonoBehaviour
{
    public int x;
    public int y;
    public Sprite S;
    public int trigger_activated = -1;
    public int item_get = -1;
    public bool activated = false;
    public TextInterStore store;
    public bool flipper;
    // Update is called once per frame
    public void Activate()
    {
        if (item_get != -1)
        {
            FindObjectOfType<GameData>().items_obtained[item_get] = true;
            FindObjectOfType<Gui_menu>().Setup();
        }
        if (S != null)
        {
            GetComponent<SpriteRenderer>().sprite = S;
        }
        if (trigger_activated != -1)
        {
            FindObjectOfType<GameData>().game_triggers[trigger_activated] = true;
        }
        activated = true;
    }
}

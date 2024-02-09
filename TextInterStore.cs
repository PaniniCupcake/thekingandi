using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextInterStore : MonoBehaviour
{
    public List<string> lines = new List<string>();
    public List<int> newlines = new List<int>();
    public int loop_start;//Where to start looping dialogue from;
    private int cur_inter = 0;
    public int alt_trigger = -1;
    public int event_trigger = -1;
    public int event_trigger2 = -1;
    public int item_get = -1;
    public List<string> lines2 = new List<string>();
    public List<int> newlines2 = new List<int>();
    public int loop_start2;
    public int item_get2 = -1;
    public int finish_trigger = -1;
    public GameObject changed;
    public AudioSource unlock;
    private void Start()
    {
        if (newlines.Count == 0)
        {
            newlines.Add(0);
        }
        newlines.Add(lines.Count);
        if (newlines2.Count == 0)
        {
            newlines2.Add(0);
        }
        newlines2.Add(lines2.Count);
    }
    public List<string> Activate()
    {
        Controller control = FindObjectOfType<Controller>();

        List<string> s = new List<string>();
        if (finish_trigger != -1 && FindObjectOfType<GameData>().game_triggers[finish_trigger])
        {
            s.Add("#m3There's nothing here.");
            return s;
        }
        if (alt_trigger != - 1 && FindObjectOfType<GameData>().game_triggers[alt_trigger])
        {
            for (int i = newlines2[cur_inter]; i < newlines2[cur_inter + 1]; i++)
            {
                s.Add(lines2[i]);
            }
            cur_inter++;
            if(changed != null)
            {
                unlock.Play(0);
                Destroy(changed);
            }
            if (cur_inter >= newlines2.Count - 1)
            {
                cur_inter = loop_start2;
            }
            if (item_get2 == -2)
            {
                FindObjectOfType<GameData>().artefacts++;
            }
            else if (item_get2 != -1)
            {
                FindObjectOfType<GameData>().items_obtained[item_get2] = true;
            }
            
            if (event_trigger2 != -1)
            {
                FindObjectOfType<GameData>().game_triggers[event_trigger2] = true;
            }
            FindObjectOfType<Gui_menu>().Setup();
            return s;
        }

        for (int i = newlines[cur_inter]; i < newlines[cur_inter + 1]; i++)
        {
            s.Add(lines[i]);
        }
        cur_inter++;
        if (cur_inter >= newlines.Count - 1)
        {
            cur_inter = loop_start;
        }
        if (item_get == -2)
        {
            FindObjectOfType<GameData>().artefacts++;
        }
        else if (item_get != -1)
        {
            FindObjectOfType<GameData>().items_obtained[item_get] = true;
        }
        
        if (event_trigger != -1)
        {
            FindObjectOfType<GameData>().game_triggers[event_trigger] = true;
        }
        FindObjectOfType<Gui_menu>().Setup();
        return s;
    }
}

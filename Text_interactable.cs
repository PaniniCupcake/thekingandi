using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Text_interactable : MonoBehaviour
{
    public Interactable trigger;
    private bool toggle = true;
    public List<string> lines = new List<string>();
    public List<int> newlines = new List<int>();
    public int loop_start;//Where to start looping dialogue from;
    private int cur_inter = 0;
    private void Start()
    {
        if (trigger == null)
        {
            trigger = this.GetComponent<Interactable>();
        }
        if(newlines.Count == 0)
        {
            newlines.Add(0);
        }
        newlines.Add(lines.Count);
    }
    void Update()
    {
        if (trigger.triggered == toggle)
        {
            toggle = !toggle;
            Activate();
        }
    }

    public void Activate()
    {
        Textbox box;
        Controller control = FindObjectOfType<Controller>();
        if (control == null)
        {
            box = FindObjectOfType<Intro>().tbox;
        }
        else
        {
            box = control.tbox;
        }
        print("CUr int num is" + cur_inter);
        box.startStrings.Clear();
        for (int i = newlines[cur_inter]; i < newlines[cur_inter + 1]; i++)
        {
            box.startStrings.Add(lines[i]);
        }
        box.gameObject.SetActive(true);
        cur_inter++;
        if (cur_inter >= newlines.Count - 1)
        {
            cur_inter = loop_start;
        }
    }
}

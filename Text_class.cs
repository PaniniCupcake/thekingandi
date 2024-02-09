using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Text_class
{
    public List<string> lines = new List<string>();
    private Intro control;
    public Text_class(Intro controller)
    {
        control = controller;
    }
    public void Activate()
    {
        Textbox box;
        box = control.tbox;
        box.startStrings.Clear();
        for (int i = 0; i < lines.Count; i++)
        {
            box.startStrings.Add(lines[i]);
        }
        box.gameObject.SetActive(true);
    }
}

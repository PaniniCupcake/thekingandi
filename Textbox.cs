using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Textbox : MonoBehaviour
{
    public Letter_bank bank;
    public List<string> startStrings = new List<string>();
    public int string_index = 0;
    private GameObject letter_container;
    public GameObject container_prefab;
    private int cur_line, i, j = 0;
    private int asc;
    private int prev_type = 0;
    private int linewidth;
    private int last_start;
    private int anchor;
    private Transform cam;
    // public TextAsset text_file;
    public List<GameObject> lines = new List<GameObject>();
    public List<int> breaks = new List<int>();
    private bool waiting = true;
    private int line = 0;
    private List<bool> spaces = new List<bool>();
    private int move_counter;
    private int pointer;
    private Intro intro;
    public Controller control;
    public List<GameObject> checks;
    private List<int> next_check = new List<int>();
    private int check_count;
    private int input_cooldown;
    private string last_string = "";
    private List<int> speaker_type;
    private List<Sprite> speaker_variant;
    private bool no_check = false;
    public SpriteRenderer pfp;
    public List<Sprite> spriteQueue = new List<Sprite>();
    public AudioSource click;
    private GameData dat;
    public GameObject datfab;
    public bool opening = false;
    public Scene_fade fade;
    private void OnEnable()
    {
        dat = FindObjectOfType<GameData>();
        if (dat == null)
        {
            dat = Instantiate(datfab, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<GameData>();
        }
        Time.timeScale = dat.text_speed;
        input_cooldown = dat.cooldowns;
        print("Awake");
        no_check = false;
        cam = FindObjectOfType<Camera>().gameObject.transform;
        intro = FindObjectOfType<Intro>();
        if (intro != null)
        {
            intro.paused = true;
        }
        if (!opening)
        {
            control.text = true;
            control = FindObjectOfType<Controller>();
        }
        for (i = 0; i < 4; i++)
        {
            breaks.Add(-1);
        }
        input_cooldown = -1;
        string_index = 0;
        List<string> temp = new List<string>();
        for (int i = 0; i < startStrings.Count; i++)
        {
            temp.Add(startStrings[i]);
        }
        startStrings = temp;
        bank.items[9] = dat.sprite_list[4];
        assign_Lines();
    }

    private void changeLine(char[] rem, int index)
    {
        cur_line++;
        print(cur_line);
        if (cur_line >= 4)
        {
            while (lines.Count >= last_start)
            {
                lines.RemoveAt(lines.Count - 1);
                spaces.RemoveAt(lines.Count - 1);
            }
            startStrings[string_index] = "";
            startStrings[string_index] += last_string;
            print(last_string + ",");
            print(string_index);
            print(last_string);
            for (int n = index; n < rem.Length; n++)
            {
                startStrings[string_index] += rem[n];
            }
            print(startStrings[string_index]);
            if (rem.Length != 0)
            {
                string_index--;
            }
            linewidth = 0;
            print("Wh no check activated");
            no_check = true;
        }
        else
        {
            print("WHY ARE");
            for (; last_start < lines.Count; last_start++)
            {
                print("a");
                lines[last_start].transform.position -= new Vector3(anchor / 21f, (18f) / 21f, 1);
            }
            linewidth -= anchor;
            anchor = 0;
            last_start = 0;
        }
        prev_type = -1;
        last_string = "";
    }

    bool assign_Lines()
    {
        print("New stuff");
        if (string_index < startStrings.Count)
        {
            bool check_cancelled = no_check;
            print("WAT" + check_cancelled);
            no_check = false;
            lines.Clear();
            breaks.Clear();
            spriteQueue.Clear();
            pointer = 0;
            next_check.Clear();
            check_count = 0;
            for (i = 0; i < 4; i++)
            {
                breaks.Add(-1);
                checks[i].SetActive(false);
                next_check.Add(-1);
                spriteQueue.Add(null);
            }
            letter_container = Instantiate(container_prefab, transform.position + new Vector3(4/21f, 2.404762f, 1), Quaternion.identity);
            letter_container.transform.parent = cam.transform;
            cur_line = 0;
            prev_type = -1;
            GameObject cur = null;
            last_string = "";

            for (; string_index < startStrings.Count && cur_line < 4; string_index++)
            {
                next_check[check_count] = cur_line;
                print("New break added " + lines.Count);
                print(cur_line);
                breaks[check_count] = lines.Count;
                print(string_index);
                for(int b = 0;b<breaks.Count;b++)
                {
                    //print(next_check[b]);
                    print(breaks[b]);
                }
                char[] chars = startStrings[string_index].ToCharArray();
                int j = 0;
                if(chars[j]=='£')//shaky
                {
                    if(control == null)
                    {
                        if (!opening)
                        {
                            if (intro.white_turn)
                            {
                                intro.left_camera.GetComponent<Tutorial_camera_follow>().shaking = 109;

                            }
                            else
                            {
                                intro.right_camera.GetComponent<Tutorial_camera_follow>().shaking = 109;
                            }
                        }
                    }
                    j++;
                }
                if (chars[j] == '#')//# must be followed with appropriate chars for everything to work
                {
                    j++;
                    if (chars[j] == 'e')
                    {
                        spriteQueue[check_count] = bank.empty;
                    }
                    if (chars[j] == 'm')
                    {
                        j++;
                        asc = chars[j] - 48;
                        spriteQueue[check_count] = bank.misc[asc];
                    }
                    else if (chars[j] == 'i')
                    {
                        j++;
                        asc = chars[j] - 48;
                        spriteQueue[check_count] = bank.items[asc];
                    }
                    else if (chars[j] == 'k')
                    {
                        j++;
                        if (chars[j] == 'w')
                        {
                            j++;
                            asc = chars[j] - 48;
                            spriteQueue[check_count] = bank.wking[asc];
                        }
                        else
                        {
                            j++;
                            asc = chars[j] - 48;
                            spriteQueue[check_count] = bank.bking[asc];
                        }
                    }
                    else if (chars[j] == 'q')
                    {
                        j++;
                        if (chars[j] == 'w')
                        {
                            j++;
                            asc = chars[j] - 48;
                            spriteQueue[check_count] = bank.wqueen[asc];
                        }
                        else
                        {
                            j++;
                            asc = chars[j] - 48;
                            spriteQueue[check_count] = bank.bqueen[asc];
                        }
                    }
                    else if (chars[j] == 'n')
                    {
                        j++;
                        if (chars[j] == 'w')
                        {
                            j++;
                            asc = chars[j] - 48;
                            spriteQueue[check_count] = bank.wknight[asc];
                        }
                        else
                        {
                            j++;
                            asc = chars[j] - 48;
                            spriteQueue[check_count] = bank.bknight[asc];
                        }
                    }
                    else if (chars[j] == 'b')
                    {
                        j++;
                        if (chars[j] == 'w')
                        {
                            j++;
                            asc = chars[j] - 48;
                            spriteQueue[check_count] = bank.wbishop[asc];
                        }
                        else
                        {
                            j++;
                            asc = chars[j] - 48;
                            spriteQueue[check_count] = bank.bbishop[asc];
                        }
                    }
                    else if (chars[j] == 'p')
                    {
                        j++;
                        if (chars[j] == 'w')
                        {
                            j++;
                            asc = chars[j] - 48;
                            spriteQueue[check_count] = bank.wpawn[asc];
                        }
                        else
                        {
                            j++;
                            asc = chars[j] - 48;
                            spriteQueue[check_count] = bank.bpawn[asc];
                        }
                    }
                    else if (chars[j] == 'r')
                    {
                        j++;
                        if (chars[j] == 'w')
                        {
                            j++;
                            asc = chars[j] - 48;
                            spriteQueue[check_count] = bank.wrook[asc];
                        }
                        else
                        {
                            j++;
                            asc = chars[j] - 48;
                            spriteQueue[check_count] = bank.brook[asc];
                        }
                    }
                    j++;
                }
                check_count++;
                for (; j < chars.Length && cur_line < 4; j++)
                {
                    ////print("Oh god");
                    //print(cur_line);
                    //print(chars[j]);
                    asc = chars[j];
                    last_string += chars[j];
                    //print("Next");
                    //print(asc);
                    //print(chars[j] + " and " + linewidth);
                    //print(linewidth);
                    
                    if (asc > 96)
                    {
                        switch (chars[j])
                        {
                            case 't':
                            case 'f':
                                if (prev_type != 0)
                                {
                                    linewidth--;
                                }
                                break;
                            case 'j':
                                if (prev_type != 1 && prev_type != 0)
                                {
                                    linewidth--;
                                }
                                prev_type = 1;
                                break;
                            case 'q':
                            case 'y':
                            case 'g':
                                prev_type = 1;
                                break;
                            case 'd':
                                prev_type = 0;
                                break;
                            case 'l':
                                if (prev_type == 2)
                                {
                                    linewidth++;
                                }
                                prev_type = 2;
                                break;
                            default:
                                prev_type = -1;
                                break;
                        }

                        //print(j + "hm" + ((linewidth - 53) / 21f));
                        lines.Add(cur = Instantiate(bank.lower_case[asc - 97], letter_container.transform.position + new Vector3((linewidth - 53) / 21f, (-26f - 18f * cur_line) / 21f, 1), Quaternion.identity));
                        linewidth += bank.lower_space[asc - 97] + 1;
                        spaces.Add(false);

                    }//Please don't rely on these line skips
                    else if (chars[j] == '>')//TEXT AFTER GOES IN NEXT BOX
                    {//untested mostly
                        prev_type = -1; last_string = "";
                        string_index++;
                        break;
                        
                        //changeLine(chars, j);
                    }
                    else if (chars[j] == '<')//TEXT AFTER GOES ON NEXT LINE
                    {
                        prev_type = -1;
                        breaks[check_count] = lines.Count;
                        changeLine(chars, j);
                        next_check[check_count] = cur_line;
                        check_count++;
                    }
                    else
                    {
                        if (asc > 64 && asc <= 90)
                        {
                            lines.Add(cur = Instantiate(bank.upper_case[asc - 65], letter_container.transform.position + new Vector3((linewidth - 53) / 21f, (-26f - 18f * cur_line) / 21f, 1), Quaternion.identity));
                            spaces.Add(false);
                            prev_type = -1;
                            linewidth += 6;
                            prev_type = 0;
                        }
                        else if (asc > 47 && asc < 58)
                        {
                            lines.Add(cur = Instantiate(bank.numbers[asc - 48], letter_container.transform.position + new Vector3((linewidth - 53) / 21f, (-26f - 18f * cur_line) / 21f, 1), Quaternion.identity));
                            spaces.Add(false);
                            prev_type = -1;
                            linewidth += 6;
                            prev_type = 0;
                        }
                        else
                        {
                            if (asc == 32)//Space
                            {
                                if (linewidth > 150)
                                {
                                    last_string = last_string.Remove(last_string.Length - 1, 1);
                                    changeLine(chars, j);
                                }
                                lines.Add(cur = Instantiate(bank.other[9], letter_container.transform.position + new Vector3((linewidth - 53) / 21f, (-26f - 18f * cur_line) / 21f, 1), Quaternion.identity));
                                linewidth += bank.punc_space[9] + 1;
                                last_string = "";
                                spaces.Add(true);
                                anchor = linewidth;
                                last_start = lines.Count;
                            }
                            else if (asc == 33)//!
                            {
                                lines.Add(cur = Instantiate(bank.other[2], letter_container.transform.position + new Vector3((linewidth - 53) / 21f, (-26f - 18f * cur_line) / 21f, 1), Quaternion.identity));
                                linewidth += bank.punc_space[2] + 1;
                                spaces.Add(false);
                            }
                            else if (asc == 40)//(
                            {
                                lines.Add(cur = Instantiate(bank.other[6], letter_container.transform.position + new Vector3((linewidth - 53) / 21f, (-26f - 18f * cur_line) / 21f, 1), Quaternion.identity));
                                linewidth += bank.punc_space[6] + 1;
                                spaces.Add(false);
                            }
                            else if (asc == 41)
                            {
                                lines.Add(cur = Instantiate(bank.other[7], letter_container.transform.position + new Vector3((linewidth - 53) / 21f, (-26f - 18f * cur_line) / 21f, 1), Quaternion.identity));
                                linewidth += bank.punc_space[7] + 1;
                                spaces.Add(false);
                            }
                            else if (asc == 44)
                            {
                                if (prev_type == 1)
                                {
                                    linewidth++;
                                }
                                lines.Add(cur = Instantiate(bank.other[1], letter_container.transform.position + new Vector3((linewidth - 53) / 21f, (-26f - 18f * cur_line) / 21f, 1), Quaternion.identity));
                                linewidth += bank.punc_space[1] + 1;
                                spaces.Add(false);
                            }
                            else if (asc == 46)
                            {
                                if (prev_type == 2)
                                {
                                    linewidth++;
                                }
                                lines.Add(cur = Instantiate(bank.other[0], letter_container.transform.position + new Vector3((linewidth - 53) / 21f, (-26f - 18f * cur_line) / 21f, 1), Quaternion.identity));
                                linewidth += bank.punc_space[0] + 1;
                                spaces.Add(false);
                            }
                            else if (asc == 64)
                            {
                                lines.Add(cur = Instantiate(bank.other[8], letter_container.transform.position + new Vector3((linewidth - 53) / 21f, (-26f - 18f * cur_line) / 21f, 1), Quaternion.identity));
                                linewidth += bank.punc_space[8] + 1;
                                spaces.Add(false);
                            }
                            else if (asc == 58)
                            {
                                lines.Add(cur = Instantiate(bank.other[4], letter_container.transform.position + new Vector3((linewidth - 53) / 21f, (-26f - 18f * cur_line) / 21f, 1), Quaternion.identity));
                                linewidth += bank.punc_space[4] + 1;
                                spaces.Add(false);
                            }
                            else if (asc == 59)
                            {
                                lines.Add(cur = Instantiate(bank.other[5], letter_container.transform.position + new Vector3((linewidth - 53) / 21f, (-26f - 18f * cur_line) / 21f, 1), Quaternion.identity));
                                linewidth += bank.punc_space[5] + 1;
                                spaces.Add(false);
                            }
                            else if (asc == 63)
                            {
                                lines.Add(cur = Instantiate(bank.other[3], letter_container.transform.position + new Vector3((linewidth - 53) / 21f, (-26f - 18f * cur_line) / 21f, 1), Quaternion.identity));
                                linewidth += bank.punc_space[3] + 1;
                                spaces.Add(false);
                            }
                            else if (asc == 39)//Apostrophe
                            {
                                if (prev_type == 0)
                                {
                                    linewidth++;
                                }
                                lines.Add(cur = Instantiate(bank.other[10], letter_container.transform.position + new Vector3((linewidth - 53) / 21f, (-26f - 17f * cur_line) / 21f, 1), Quaternion.identity));
                                linewidth += bank.punc_space[10] + 1;
                                spaces.Add(false);
                            }
                            prev_type = -1;
                        }
                    }
                    cur.transform.parent = letter_container.transform;
                }
                print(linewidth);
                if (linewidth > 150)
                {
                    changeLine(chars, j);
                }
                prev_type = -1;
                anchor = 0;
                last_start = 0;
                linewidth = 0;
                lines.Add(cur = Instantiate(bank.other[9], letter_container.transform.position, Quaternion.identity));
                spaces.Add(true);
                cur_line++;
                if (j < chars.Length && chars[j] == '>')
                {
                    break;
                }
            }
            line = 1;
            //print(string_index);
            waiting = false;
            check_count = 1;
            if (!check_cancelled)
            {
                print("Huh?");
                checks[0].SetActive(true);
                if (spriteQueue[0] != null)
                {
                    print("Hyh?!?!?");
                    pfp.sprite = spriteQueue[0];
                }
            }
            for(i = 0;i<4;i++)
            {
                print(breaks[i]);
            }
            return true;
        }
        return false;

    }

    private void Update()
    {

        if (input_cooldown == 0)
        {
            if (Input.GetAxis("Skip") > 0 || Input.GetAxis("Submit") > 0)
            {
                if (!waiting)
                {
                    Instantiate(bank.click, new Vector3(99, 99, 0), Quaternion.identity).transform.parent = letter_container.transform;
                    while (!waiting)
                    {
                        nextChar(false);
                    }
                }
                else
                {
                    waiting = false;
                    print("Check?");
                    print(check_count);
                    print(next_check[check_count]);
                    line++;
                    if (next_check[check_count] >= 0 && check_count < 4)
                    {
                        checks[next_check[check_count]].SetActive(true);
                        print(next_check[check_count]);
                        
                        if (spriteQueue[check_count] != null)
                        {
                            pfp.sprite = spriteQueue[check_count];
                        }
                        check_count++;
                    }
                    if (pointer >= lines.Count)
                    {
                        Destroy(letter_container);
                        if (!assign_Lines())
                        {
                            Close();
                        }
                    }
                }
                input_cooldown = dat.cooldowns;
            }
        }
        else if (Input.GetAxis("Submit") == 0 && Input.GetAxis("Skip") == 0)
        {
            input_cooldown = 0;
        }
        if (Input.GetAxis("Close") > 0)
        {
            Close();
        }
    }

    public void Close()
    {
        
        if (intro != null)
        {
            intro.paused = false;
            intro.cooldown = -1;
        }
        if (control != null)
        {
            control.text = false;
            control.cooldown = -1;
            control.p_cooldown = true;
        }
        if(opening)
        {
            fade.NewScene("Home screen",1);
            Destroy(this);
            return;
        }
        Destroy(letter_container);
        Time.timeScale = dat.game_speed;
        gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (!waiting)
        {
            if (move_counter == 2)
            {
                nextChar(true);
                move_counter = -1;
            }
            move_counter++;
        }
        if(input_cooldown > 0)
        {
            input_cooldown--;
        }

    }
    public void nextChar(bool sound)
    {

        if (!waiting && pointer < lines.Count)
        {
            lines[pointer].SetActive(true);
            if (sound && (pointer >= spaces.Count || !spaces[pointer]))
            {
                Instantiate(bank.click, new Vector3(99,99,0), Quaternion.identity).transform.parent = letter_container.transform;
            }
        }
        print(pointer);
        print("line?");
        print(line);
        if (pointer == breaks[line] - 1|| pointer >= lines.Count)
        {
            print("BREAK!" + (breaks[line] - 1));
            waiting = true;
            for (int b = 0; b < breaks.Count; b++)
            {
                //print(next_check[b]);
                print(breaks[b]);
            }
        }

        pointer++;
    }

}

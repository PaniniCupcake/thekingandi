using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro : MonoBehaviour
{
    public int y_dims;
    private int x_dims;
    public int wking_x = 0;
    public int wking_y = 0;
    public int bking_x = 0;
    public int bking_y = 0;
    public GameObject wking;
    public GameObject bking;
    public Animator wking_anim;
    public Animator bking_anim;
    public float true_y;
    public Piece rook;
    public Animator rook_anim;
    //public Transform camera;
    public List<string> tile_defs = new List<string>(); //Fill in editor. Start tiles from 0, how many true, how many false, how many true ...
    public List<List<bool>> valid_tiles = new List<List<bool>>();
    public List<List<int>> piece_occupants = new List<List<int>>(); //1 white, 0 none, -1 black
    public List<List<bool>> white_threats = new List<List<bool>>();
    public List<List<bool>> black_threats = new List<List<bool>>();
    public bool alone = false;
    public bool white_turn; //true for white, false for black;
    private int x_dir = 0;
    public int y_dir = 0;
    private int move_length = 100;
    private bool can_move = true;
    private GameObject move_target;
    private int piece_selected; //0 king, 1 queen, 2 rook, 3 bishop, 4 knight, 5 pawn
    private int piece_number_selected;
    public int piece_count = -1;
    public int move_counter = -1;
    public List<int> forced_moves_trigger_x;
    public List<int> forced_moves_trigger_y;
    public List<int> forced_moves_x = new List<int>();
    public List<int> forced_moves_y = new List<int>();
    private int white_waiting = 0;
    public Interactable pedestal;
    public Interactable sign;
    public Interactable pedestal_sign;
    public Interactable sign1;
    public Interactable sign2;
    public Interactable sign3;
    public Interactable sign4;
    public GameObject cross;
    public GameObject nought;
    private int rook_lock = 0;
    public bool paused = false;
    public Textbox tbox;
    public int cooldown = 0;
    public List<Text_class> tile_texts = new List<Text_class>();
    public List<Text_class> rook_texts = new List<Text_class>();
    public List<Text_class> king_texts = new List<Text_class>();
    private int camera_slide_left = 0;//60
    private int camera_slide_right = 0;
    public Camera left_camera;
    public Camera right_camera;
    public Transform left_box;
    public Transform right_box;
    public Transform seperator;
    private Animator cur;
    private int moved = 400;
    private int flashing = -1;
    public GameObject blocker;
    private bool text_triggered = false;
    public AudioSource tap;
    private int rep_count;
    void Awake()
    {
        tbox.transform.position = new Vector3(right_box.transform.position.x - (89f / 21f), tbox.transform.position.y, 0);
        //tbox = FindObjectOfType<Textbox>();
        x_dims = tile_defs.Count;
        for (int i = 0; i < x_dims; i++)
        {
            //print(i + "uwu");
            List<bool> temp_tiles = new List<bool>();
            int k = 0;
            int j = 0;
            int str_val = 0;
            int total = 0;
            bool valid = true;
            while (k < tile_defs[i].Length)
            {
                if (tile_defs[i][k] == ',')
                {
                    for (j = total; j < total + str_val; j++)
                    {
                        temp_tiles.Add(valid);
                    }
                    total += str_val;
                    str_val = 0;
                    valid = !valid;
                }
                else
                {
                    str_val = str_val * 10 + (tile_defs[i][k] - '0');
                    //print(str_val);
                }
                k++;
            }
            for (j = total; j < total + str_val; j++)
            {
                //print(j + "" +valid);
                temp_tiles.Add(valid);
            }
            total += str_val;
            valid = !valid;
            for (j = total; j < y_dims; j++)
            {
                //print(j + "" + valid);
                temp_tiles.Add(valid);
            }

            //string temp = "";
            //print(temp);
            valid_tiles.Add(temp_tiles);
        }
        valid_tiles[wking_x][wking_y] = false;
        valid_tiles[bking_x][bking_y] = false; 
        for (int i = 0; i < x_dims; i++)
        {
            string temp = "";
            for (int j = 0; j < y_dims; j++)
            {
                temp += "," + valid_tiles[i][j];
            }
            //print(i + " " + temp);
        }
        print("Wah");
        for (int i = 0; i < x_dims; i++)
        {
            List<int> temp1 = new List<int>();
            List<bool> temp2 = new List<bool>();
            List<bool> temp3 = new List<bool>();
            List<Cross_tile> temp4 = new List<Cross_tile>();
            for (int j = 0; j < y_dims; j++)
            {
                temp1.Add(0);
                temp2.Add(false);
                temp3.Add(false);
            }
            piece_occupants.Add(temp1);
            white_threats.Add(temp2);
            black_threats.Add(temp3);
        }
        piece_occupants[wking_x][wking_y] = 1;
        piece_occupants[bking_x][bking_y] = -1;
        valid_tiles[6][6] = false;


        Text_class rook1 = new Text_class(this);
        //rook1.lines.Add("#rb0Keep it moving.");
        rook1.lines.Add("#rb6...");
        rook_texts.Add(rook1);
        Text_class rook2 = new Text_class(this);
        //rook2.lines.Add("#rb0Your gift is just over there on that pedestal.");
        //rook2.lines.Add("Take it and we can keep going.");
        rook2.lines.Add("#rb6...>");
        rook2.lines.Add("#kb0They don't seem much for conversation...");
        rook2.lines.Add("I should check out what's on that pedestal there.");
        rook_texts.Add(rook2);
        Text_class rook3 = new Text_class(this);
        //rook3.lines.Add("#rb0Sorry King, this isn't the way we're going.");
        //rook3.lines.Add("When did I say to go up here?");
        rook3.lines.Add("#rb6.....");
        rook3.lines.Add("#rb6..!....");
        rook_texts.Add(rook3);
        Text_class rook4 = new Text_class(this);
        rook3.lines.Add("#rb6...");
        rook3.lines.Add("#rb6....");
        rook3.lines.Add("#rb6.....");

        Text_class king1 = new Text_class(this);
        king1.lines.Add("#kw6..."); king1.lines.Add("#kw6...");
        king_texts.Add(king1);
        Text_class king2= new Text_class(this);
        king2.lines.Add("#kw6..."); king2.lines.Add("#kw6..."); king2.lines.Add("#kw6...psst...");
        king_texts.Add(king2);
        Text_class kinga = new Text_class(this);
        kinga.lines.Add("#kw6...over here...");
        king_texts.Add(kinga);
        Text_class king3 = new Text_class(this);
        king3.lines.Add("£#kw3HEY!!!"); king3.lines.Add("#kw3I'M TALKING TO YOU.");
        king_texts.Add(king3);
        Text_class king4 = new Text_class(this);
        king4.lines.Add("#kw6Come on, at least acknowledge me."); king4.lines.Add("#kw6I haven't had company for a long while.");
        king_texts.Add(king4);
        Text_class king5 = new Text_class(this);
        king5.lines.Add("#kw6Ok, I see how it is.>");
        king5.lines.Add("Listen, I know we aren't exactly on the best terms, but I have been waiting in this tiny 3x3 cell for so many turns I've lost count.");
        king5.lines.Add("And just so you know, being here is NOT an aquired taste.");
        king5.lines.Add("But I have been planning my escape every moment since I came here.");
        king5.lines.Add("I knew my ex wife wouuld want you trapped here with me. She's a big fan of irony.");
        king5.lines.Add("But that will be her undoing if we can work together. Will you help me?");
        king_texts.Add(king5);
        // rook4.lines.Add("#rb0Nice place, isn't it?");
        //rook4.lines.Add("#rb1Even better when you get to share it with good friends.");
        //rook4.lines.Add("But don't worry, I'm not planning on leaving for a while.");
        rook_texts.Add(rook4);
        Text_class text1 = new Text_class(this);
        text1.lines.Add("#rb0Onwards we go.");
        //text1.lines.Add("#m2(You can skip to the end of a segment of dialogue by pressing the submit key).>");
        tile_texts.Add(text1);
        Text_class text2 = new Text_class(this);
        text2.lines.Add("#rb0We've almost reached your new home.");
        //text2.lines.Add("#m2(Dialogue can also be skipped entirely by pressing the menu key).");
        tile_texts.Add(text2);
        Text_class text3 = new Text_class(this);
        //text3.lines.Add("#rb0Just so you know, I never truly respected you.");
        text3.lines.Add("#rb0I'm quite envious of you, you know.");
        tile_texts.Add(text3);
        Text_class text4 = new Text_class(this);
        //text4.lines.Add("#rb0You were always weak, but nobody else would say it.");
        text4.lines.Add("#rb0It's not every day you get to take an extended vacation.");
        tile_texts.Add(text4);
        //#rb1You were always weak, but nobody else would say it.
        Text_class text5 = new Text_class(this);
        text5.lines.Add("#rb1Some time away from it all, you know? A bit of rest and relaxation."); 
        tile_texts.Add(text5);
        Text_class text6 = new Text_class(this);
        text6.lines.Add("#rb0And the Queen was even kind enough to get you a going away present!");
        text6.lines.Add("#rb0Why don't you go and take it from that pedestal?");
        tile_texts.Add(text6);
        Text_class text7 = new Text_class(this);
        text7.lines.Add("#rb0You're going to need to take what's on it then continue left.>");
        text7.lines.Add("#rb1The Queen wants to keep you here for a while.");
        tile_texts.Add(text7);
        Text_class text8 = new Text_class(this);
        text8.lines.Add("#rb1Good boy.");
        text8.lines.Add("#rb1Time for you to reunite with an old friend.");
        tile_texts.Add(text8);
        Text_class text9 = new Text_class(this);
        text9.lines.Add("#kw0......................................>");
        text9.lines.Add("#kb0......................................");
        tile_texts.Add(text9);
        Text_class text10 = new Text_class(this);
        text10.lines.Add("#rb1What a touching reunion.>");
        text10.lines.Add("#rb1Time will really fly by if you keep making scintilating conversation like that.");
        tile_texts.Add(text10);
        Text_class text11 = new Text_class(this);
        text11.lines.Add("#rb0Now move to the bottom left and we can be done here.");
        tile_texts.Add(text11);
        Text_class text12 = new Text_class(this);
        text12.lines.Add("#kw0...This is my half of the 3x3 grid.");
        tile_texts.Add(text12);
        //But now I can finally put you in your place.
    }

    public void recalculateThreats()
    {
        for (int i = 0; i < x_dims; i++)
        {
            for (int j = 0; j < y_dims; j++)
            {
                white_threats[i][j] = false;
                black_threats[i][j] = false;
            }
        }

        print("King pos");
        print(wking_x + "," + wking_y);
        for (int xinc = -1; xinc < 2; xinc++)
        {
            for (int yinc = -1; yinc < 2; yinc++)
            {
                setTileThreat(bking_x + xinc, bking_y + yinc, false);
                setTileThreat(wking_x + xinc, wking_y + yinc, true);
            }
        }
        print("Black threats");

        print("White threats");
        for (int i = 0; i < 6; i++)
        {
            string s = "";
            for (int j = 0; j < y_dims; j++)
            {
                s += white_threats[i][j];
                s += ",";
            }
        }
    }

    private void setTileThreat(int x, int y, bool white)
    {
        if (!outOfBounds(x, y))
        {
            if (white)
            {
                white_threats[x][y] = true;
            }
            else
            {
                black_threats[x][y] = true;
            }
        }
    }

    private bool outOfBounds(int x, int y)
    {
        return (x < 0 || x >= x_dims || y < 0 || y >= y_dims || !valid_tiles[x][y]);
    }

    private void calculateThreat(bool white)
    {

    }

    private bool bkingTile(int x,int y)
    {
        return (bking_x == x && bking_y == y);
    }

    private void FixedUpdate()
    {
        if(camera_slide_left > 0)//left camera coming
        {
            left_camera.rect = new Rect(left_camera.rect.x + 0.0125f / 1.5f, left_camera.rect.y,left_camera.rect.width,left_camera.rect.height) ;
            right_camera.rect = new Rect(right_camera.rect.x + 0.0125f / 1.5f, right_camera.rect.y, right_camera.rect.width - 0.0125f / 1.5f, right_camera.rect.height);
            left_box.position += new Vector3(97f/420f/1.5f,0, 0);
            right_box.position += new Vector3(97f / 840f / 1.5f, 0, 0);
            seperator.position += new Vector3(26 / 420f / 1.5f, 0,0);
            seperator.localScale -= new Vector3(0.014f / 1.5f, 0,0);
            //-381/21
            //90 / 21
            //187/21
            camera_slide_left--;
        }
        else if (camera_slide_left < 0)//left camera going
        {
            left_camera.rect = new Rect(left_camera.rect.x - 0.0125f / 1.5f, left_camera.rect.y, left_camera.rect.width, left_camera.rect.height);
            right_camera.rect = new Rect(right_camera.rect.x - 0.0125f / 1.5f, right_camera.rect.y, right_camera.rect.width + 0.0125f / 1.5f, right_camera.rect.height);
            camera_slide_left++;
            left_box.position -= new Vector3(97f / 420f / 1.5f, 0, 0);
            right_box.position -= new Vector3(97f / 840f / 1.5f, 0, 0);
            seperator.position -= new Vector3(26 / 420f / 1.5f, 0, 0);
            seperator.localScale += new Vector3(0.014f / 1.5f, 0, 0);
        }
        else if (camera_slide_left == 0)
        {

        }
        if (camera_slide_right > 0)//right camera coming
        {
            left_camera.rect = new Rect(left_camera.rect.x - 0.0125f / 1.5f, left_camera.rect.y, left_camera.rect.width - 0.0125f / 1.5f, left_camera.rect.height);
            right_camera.rect = new Rect(right_camera.rect.x - 0.0125f / 1.5f, right_camera.rect.y, right_camera.rect.width, right_camera.rect.height);
            right_box.position -= new Vector3(97f / 420f / 1.5f, 0, 0);
            left_box.position -= new Vector3(97f / 840f / 1.5f, 0, 0);
            seperator.position -= new Vector3(26 / 420f / 1.5f, 0, 0);
            seperator.localScale -= new Vector3(0.014f / 1.5f, 0, 0);
            camera_slide_right--;
        }
        else if (camera_slide_right < 0)//right camera going
        {
            left_camera.rect = new Rect(left_camera.rect.x - 0.0125f / 1.5f, left_camera.rect.y, left_camera.rect.width + 0.0125f / 1.5f, left_camera.rect.height);
            right_camera.rect = new Rect(right_camera.rect.x - 0.0125f / 1.5f, right_camera.rect.y, right_camera.rect.width, right_camera.rect.height);
            right_box.position += new Vector3(97f / 420f / 1.5f, 0, 0);
            left_box.position += new Vector3(97f / 840f / 1.5f, 0, 0);
            seperator.position += new Vector3(26 / 420f / 1.5f, 0, 0);
            seperator.localScale += new Vector3(0.014f / 1.5f, 0, 0);
            camera_slide_right++;
        }
        else if (camera_slide_right == 0)
        {

        }
        if (move_counter != -1)
        {
            //print("Moving");
            //print(move_length);
            //print(move_counter);
            if (move_counter < 8)
            {
                move_target.transform.position += new Vector3(0, 0.05f, 0);
                if(x_dir == 0)
                {
                    cur.SetFloat("x", 0);
                }
                else
                {
                    cur.SetFloat("x", Mathf.Sign(x_dir));
                }
                if(y_dir == 0)
                {
                    cur.SetFloat("y", 0);
                }
                else
                {
                    cur.SetFloat("y", Mathf.Sign(y_dir));
                }
                
            }
            else if (move_counter >= move_length * 20 - 5)
            {
                move_target.transform.position -= new Vector3(0, 0.05f, 0);
            }
            if (move_counter < move_length * 20)
            {
                //print("moving");
                if (piece_count == -1)
                {
                    true_y += 0.05f * y_dir;
                }
                move_target.transform.position += new Vector3(0.05f * x_dir, 0.05f * y_dir, 0);
            }
            move_counter++;
            if (move_counter == move_length * 20 + 3 && alone)
            {
                tap.Play(0);
                move_counter = -1;
                piece_count++;
                print(piece_count);
                

                if (piece_count == 0 && rook_lock == 0)
                {
                    if (bking_x > 12)
                    {
                        x_dir = -1;
                        y_dir = 0;
                        piece_occupants[rook.x][rook.y] = 0;
                        rook.x += x_dir;
                        rook.y -= y_dir;
                        piece_occupants[rook.x][rook.y] = -1;
                        move_target = rook.gameObject;
                        move_counter = 0;
                        cur = rook_anim;
                    }
                    else
                    {
                        for (int j = 0; j < forced_moves_x.Count; j++)
                        {
                            if ((bking_x == forced_moves_trigger_x[j] && bking_y == forced_moves_trigger_y[j] && !(forced_moves_x[j] == rook.x && forced_moves_y[j] == rook.y)))
                            {
                                if (bking_y == 7 && bking_x == 0)
                                {
                                    rook_lock = 1;
                                }
                                x_dir = forced_moves_x[j] - rook.x;
                                y_dir = -(forced_moves_y[j] - rook.y);
                                piece_occupants[rook.x][rook.y] = 0;
                                rook.x += x_dir;
                                rook.y -= y_dir;
                                piece_occupants[rook.x][rook.y] = -1;
                                move_target = rook.gameObject;
                                move_counter = 0;
                                cur = rook_anim;
                                break;
                            }

                        }
                    }
                }
                else if(!text_triggered)
                {
                    text_triggered = true;
                    /*for (int i = 0;i<7;i++)
                    {
                        if(bkingTile(17-i,6))
                        {
                            tile_texts[i].Activate(); 
                        }
                    }
                    */
                    if (bkingTile(11, 6))
                    {
                        //tile_texts[6].Activate();
                    }
                    if (bkingTile(13, 6))
                    {
                        //tile_texts[5].Activate();
                    }
                    if (bkingTile(15, 6))
                    {
                        //tile_texts[4].Activate();
                    }
                    if (bkingTile(17, 6))
                    {
                        //tile_texts[3].Activate();
                    }
                    /*
                    if(bkingTile(6,6))
                    {
                        tile_texts[7].Activate();
                    }
                    else if (bkingTile(4, 6))
                    {
                        tile_texts[9].Activate();
                    }
                    else if (bkingTile(3, 6))
                    {
                        tile_texts[10].Activate();
                    }*/
                }
                if (piece_count < 2 && move_counter == -1)
                {
                    move_target = wking.gameObject;
                    piece_count = 2;
                    x_dir = 0;
                    y_dir = 0;
                    move_counter = 0;
                    if (bking_x == 6)
                    {
                        white_waiting = -1;
                    }
                    else
                    {
                        white_waiting = -white_waiting;
                    }
                    x_dir = white_waiting;
                    if (bking_y == 6 && bking_x == 2)
                    {
                        x_dir = - wking_x;
                        y_dir = -(6 - wking_y);
                    }
                    else if(wking_y == 6)
                    {
                        x_dir = 0;
                        y_dir = 1;
                    }
                    piece_occupants[wking_x][wking_y] = 0;
                    wking_x += x_dir;
                    wking_y -= y_dir;
                    print(wking_y);
                    piece_occupants[wking_x][wking_y] = 1;
                    recalculateThreats();
                    cur = wking_anim;
                    print("King activated");
                }

            }
            //print("W");
            //print(piece_count);
            if (move_counter == -1)
            {
                cooldown = 10;
                if (bkingTile(5, 6))
                {
                    //tile_texts[8].Activate();
                    //text_triggered = true;
                }
                if (bking_x == 0)
                {
                    if (rep_count == 3)
                    {
                        wking_anim.enabled = true;
                        wking_anim.SetFloat("y", -1);
                    }
                    king_texts[rep_count].Activate();
                    rep_count++;
                    
                }
                if (!alone)
                {
                    white_turn = !white_turn;
                    if(white_turn)
                    {
                        tbox.transform.position = new Vector3(left_box.transform.position.x + 89f/21f, tbox.transform.position.y, 0);
                    }
                    else
                    {
                        tbox.transform.position = new Vector3(right_box.transform.position.x - 89f / 21f, tbox.transform.position.y, 0); 
                    }
                    
                }
                piece_count = -1;
                can_move = true;

                //print("moved")
            }
            move_target.GetComponent<SpriteRenderer>().sortingOrder = (int)(-move_target.transform.position.y * 200);
        }
        if(cooldown > 0)
        {
            cooldown--;
        }
        moved--;
        if(moved == 0)
        {
            tbox.gameObject.SetActive(true);
        }
        if(flashing >= 0)
        {
            if(flashing % 20 == 0)
            {
                blocker.SetActive(true);
            }
            else if(flashing % 10 == 0)
            {
                blocker.SetActive(false);
            }
            flashing--;
        }
    }
    void Update()
    {
        if(paused)
        {
            return;
        }
        if (move_counter == -1 && can_move)
        {
            x_dir = 0;
            y_dir = 0;
            if (white_turn)
            {
                true_y = wking.transform.position.y;
            }
            else
            {
                true_y = bking.transform.position.y;
            }
            if (Input.GetAxis("Vertical") >= 0.1)
            {
                y_dir = 1;
            }
            else if (Input.GetAxis("Vertical") <= -0.1)
            {
                y_dir = -1;
            }
            if (Input.GetAxis("Horizontal") >= 0.1)
            {
                x_dir = 1;
            }
            else if (Input.GetAxis("Horizontal") <= -0.1)
            {
                x_dir = -1;
            }
            bking_anim.SetFloat("x_dir", x_dir);
            bking_anim.SetFloat("y_dir", y_dir);

            //print(x_dir + "," + y_dir);
            if (Input.GetAxis("Submit") > 0 && !(x_dir == 0 && y_dir == 0) && cooldown == 0)
            {
                print("Hh");
                moved = -1;
                if (white_turn)
                {
                    if (!(outOfBounds(wking_x + x_dir, wking_y - y_dir) || !valid_tiles[wking_x + x_dir][wking_y - y_dir] || piece_occupants[wking_x + x_dir][wking_y - y_dir] == 1 || black_threats[wking_x + x_dir][wking_y - y_dir]))
                    {
                        valid_tiles[wking_x][wking_y] = true;
                        piece_occupants[wking_x][wking_y] = 0;
                        wking_x += x_dir;
                        wking_y -= y_dir;
                        valid_tiles[wking_x][wking_y] = false;
                        piece_occupants[wking_x][wking_y] = 1;
                        can_move = false;
                        move_length = 1;
                        piece_selected = 0;
                        move_target = wking;
                        piece_number_selected = 0;
                        move_counter = 0;
                        recalculateThreats();
                        cur = wking_anim;
                    }
                }
                else
                {
                    if (bking_x + x_dir == 9 && bking_y - y_dir == 3)
                    {
                        if (valid_tiles[6][6] == false)
                        {
                            pedestal.Trigger();//Change how out of bounds works
                            flashing = 140;
                            print("Hi hi h ih !");
                            valid_tiles[6][6] = true;

                            Sprite temp = nought.GetComponent<SpriteRenderer>().sprite;
                            nought.GetComponent<SpriteRenderer>().sprite = cross.GetComponent<SpriteRenderer>().sprite;
                            cross.GetComponent<SpriteRenderer>().sprite = temp;
                        }
                        pedestal_sign.Trigger();
                    }
                    else if (bking_x + x_dir == sign.x && bking_y - y_dir == sign.y)
                    {
                        sign.Trigger();
                    }
                    else if (bking_x + x_dir == sign1.x && bking_y - y_dir == sign1.y)
                    {
                        sign1.Trigger();
                    }
                    else if (bking_x + x_dir == sign2.x && bking_y - y_dir == sign2.y)
                    {
                        sign2.Trigger();
                    }
                    else if (bking_x + x_dir == sign3.x && bking_y - y_dir == sign3.y)
                    {
                        sign3.Trigger();
                    }
                    else if (bking_x + x_dir == sign4.x && bking_y - y_dir == sign4.y)
                    {
                        sign4.Trigger();
                    }
                    else if (bking_x + x_dir == rook.x && bking_y - y_dir == rook.y)
                    {
                        if (rook.x == 11 && rook.y == 6)
                        {
                            rook_texts[1].Activate();
                        }
                        else if (rook.x == 3 && rook.y == 0)
                        {
                            rook_texts[3].Activate();
                        }
                        else if (rook.x == 11 && rook.y == 0)
                        {
                            rook_texts[2].Activate();
                        }
                        else 
                        {
                            rook_texts[0].Activate();
                        }
                    }
                    else if (!(outOfBounds(bking_x + x_dir, bking_y - y_dir) || !valid_tiles[bking_x + x_dir][bking_y - y_dir] || piece_occupants[bking_x + x_dir][bking_y - y_dir] == -1 || white_threats[bking_x + x_dir][bking_y - y_dir]))
                    {
                        print("E");
                        valid_tiles[bking_x][bking_y] = true;
                        piece_occupants[bking_x][bking_y] = 0;
                        bking_x += x_dir;
                        bking_y -= y_dir;
                        valid_tiles[bking_x][bking_y] = false;
                        piece_occupants[bking_x][bking_y] = -1;
                        can_move = false;
                        move_length = 1;
                        move_target = bking;
                        piece_selected = 0;
                        piece_number_selected = 1;
                        move_counter = 0;
                        recalculateThreats();
                        cur = bking_anim;
                        text_triggered = false;
                    }
                }
                cooldown = 10;
            }
            else if(Input.GetAxis("Submit") == 0)
            {
                cooldown = 0;
            }
        }

    }
}

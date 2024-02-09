using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
public class Controller : MonoBehaviour
{
    //public TextFile storefile;
    private bool wking_has_moved_for_intro;
    public bool loadfromfile;
    public bool savetofile;
    public string filename;
    public bool special_level; //false for puzzle
    public int cur_level;
    public int exit_dir;
    public List<int> wkingx_starts;
    public List<int> wkingy_starts;
    public List<int> bkingx_starts;
    public List<int> bkingy_starts;
    public List<string> exit_levels;//format for level is lxxx, other is any
    private GameData dat;
    public AudioSource scrape;
    public AudioSource tap;
    public AudioSource move;
    public AudioSource back;
    public AudioSource undo;
    public AudioSource com;
    public AudioSource cancel;
    public AudioSource uncom;
    public AudioSource slide;
    public GameObject temp_map;
    Tile_grid tile_grid;
    public GameObject wking;
    public GameObject bking;
    public Piece wking_p;
    public Piece bking_p;
    public float true_y;
    public List<Piece> white_pieces = new List<Piece>();
    public List<Piece> black_pieces = new List<Piece>();
    //public Transform camera;
    //Fill in editor. Start tiles from 0, how many true, how many false, how many true ...
    public int y_dims;
    private int x_dims;
    public List<string> tile_defs = new List<string>();
    public List<List<Tile>> tiles = new List<List<Tile>>();
    public bool alone = false;
    public bool white_turn; //true for white, false for black;
    private int x_dir = 0;
    public int y_dir = 0;
    private int move_length = 100;
    private bool can_move = true;
    private GameObject move_target;
    private int piece_selected; //0 king, 1 queen, 2 rook, 3 bishop, 4 knight, 5 pawn,10 wqueen,11bqueen
    private int piece_number_selected;
    public int piece_count = -1;
    public bool paused = false;
    public bool p_cooldown;
    public Textbox tbox;
    public int cooldown;
    public GameObject white_selector;
    public GameObject black_selector;
    public Sprite valid;
    public Sprite invalid;
    public Piece cur_piece;
    private int cur_x;
    private int cur_y;
    private int dir_lock;//up, right, down, left, 0-7
    private List<int> dir_cooldowns = new List<int>();
    public Transform asc_piece;
    private int ascend_count = 0;
    public int passant_x = 99;
    public int passant_y = 99;
    private Piece passant_piece;
    private bool king_command;
    private bool forced_invalid = false;
    public Letter_bank bank;
    public Gui_menu GUI;
    public Tile_mutators mutations;
    private Tile temp_tile;
    private int startup = 0;
    public bool no_check;
    public bool decrement;
    public bool adjacent_noughts;
    public bool capture_all;
    public bool control;
    public GameObject Tile;
    public GameObject Lower;
    private Stack<Undo_state> undos = new Stack<Undo_state>();
    public GameObject w_promotion;
    public GameObject b_promotion;
    public GameObject promotion_pointer;
    private int promotion = 0;
    private int promotion_choice;
    public bool text = false;
    private int undocooldown = 0;
    public GameObject undo_hide;
    private bool can_undo = false;
    public int move_counter = -1;
    private bool detection_enabled;
    private bool threat_detecting;
    private bool threat_cooldown;
    public GameObject datfab;
    private bool GOBACK = false;
    public Scene_fade fade;
    public GameObject speech;
    public GameObject gfab;
    //public List<Piece> props = new List<Piece>();
    private bool interact = false;
    private bool completed = false;
    private int menu_countdown = 80;
    private bool external;
    public List<OneTime> oneoffs = new List<OneTime>();
    private bool once = false;
    public List<BonusTile> b_tiles;
    private int total_turns;
    public bool entry_text;
    private bool flat_down;
    private int flat;
    private bool loss = false;
    public bool hide_tiles;
    private void UpdateGui()
    {
        if(external)
        {
            return;
        }
        Sprite white_sprite = null;
        Sprite black_sprite = null;
        if (white_turn)
        {
            //turn_indicator.transform.position = GUI.transform.position + new Vector3(-146.5f / 21f, -76f / 21f, 0);
            black_sprite = bank.bking[0];
        }
        else
        {
            //turn_indicator.transform.position = GUI.transform.position + new Vector3(146.5f / 21f, -76f / 21f, 0);
            white_sprite = bank.wking[0];
        }
        int emotion = 0;



        if((cur_x == 0 && cur_y == 0) || invalidTile(cur_x + cur_piece.x,cur_piece.y - cur_y))
        {
            emotion = 0;
            if(cur_piece.type == 5)
            {
                if((white_turn && tiles[cur_piece.x][cur_piece.y].black_threat)|| (!white_turn && tiles[cur_piece.x][cur_piece.y].white_threat))
                {
                    emotion = 9;
                }
            }
        }
        else if (tiles[cur_x + cur_piece.x][cur_piece.y - cur_y].Occupied() && tiles[cur_x + cur_piece.x][cur_piece.y -cur_y].White() == white_turn)
        {
            if(cur_piece.type == 5)
            {
                emotion = 8;
            }
            else
            {
                emotion = 2;
            }
        }
        else if(!king_command && forced_invalid)
        {
            emotion = 2;
        }
        else if (!tiles[cur_x + cur_piece.x][cur_piece.y + -cur_y].Occupied())
        {
            emotion = 1;
        }
        else if(!tiles[cur_x + cur_piece.x][cur_piece.y - cur_y].occupant.uncapturable)
        {
            emotion = 7;
        }
        switch (cur_piece.type)
        {
            case 0:
                if (white_turn)
                {
                    //gui_portrait_left.Sprite();
                    white_sprite = bank.wqueen[emotion];
                }
                else
                {
                    black_sprite = bank.bqueen[emotion];
                }
                break;
            case 1:
                if (white_turn)
                {
                    //gui_portrait_left.Sprite();
                    white_sprite = bank.wrook[emotion];
                }
                else
                {
                    black_sprite = bank.brook[emotion];
                }
                break;
            case 2:
                if (white_turn)
                {
                    //gui_portrait_left.Sprite();
                    white_sprite = bank.wbishop[emotion];
                }
                else
                {
                    black_sprite = bank.bbishop[emotion];
                }
                break;
            case 3:
                if (white_turn)
                {
                    //gui_portrait_left.Sprite();
                    white_sprite = bank.wknight[emotion];
                }
                else
                {
                    black_sprite = bank.bknight[emotion];
                }
                break;
            case 4:
                if (white_turn)
                {
                    //gui_portrait_left.Sprite();
                    white_sprite = bank.wpawn[emotion];
                }
                else
                {
                    black_sprite = bank.bpawn[emotion];
                }
                break;
            case 5:
                if (white_turn)
                {
                    //gui_portrait_left.Sprite();
                    white_sprite = bank.wking[emotion];
                }
                else
                {
                    black_sprite = bank.bking[emotion];
                }
                break;
        }
        if (!GUI.black_only)
        {
            GUI.white_king.sprite = white_sprite;
        }
        if (!GUI.white_only)
        {
            GUI.black_king.sprite = black_sprite;
        }
    }
    void Awake()
    {
        dat = FindObjectOfType<GameData>();
        if (dat == null)
        {
            dat = Instantiate(datfab, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<GameData>();
        }
        if (!special_level && !savetofile)
        {
            print("Fuck");
            filename = dat.next_level;
            //cur_level = dat.cur_level;
        }
        if (loadfromfile)
        {
            Destroy(wking);
            Destroy(bking);
            for (int i = 0; i < black_pieces.Count; i++)
            {
                Destroy(black_pieces[i].gameObject);
            }
            for (int i = 0; i < white_pieces.Count; i++)
            {
                Destroy(white_pieces[i].gameObject);
            }
            white_pieces.Clear();
            black_pieces.Clear();
            LoadFromFile();
        }
        else
        {
            int temporary = 0;
            if (dat.x_entry == 1)
            {
                temporary = 1;
            }
            else if (dat.x_entry == -1)
            {
                temporary = 3;
            }
            else if (dat.y_entry == 1)
            {
                temporary = 0;
            }
            else if (dat.y_entry == -1)
            {
                temporary = 2;
            }
            if (bkingx_starts.Count != 0)
            {
                bking_p.x = bkingx_starts[temporary];
                bking_p.y = bkingy_starts[temporary];
                bking_p.x_dir = -dat.x_entry;
                bking_p.y_dir = -dat.y_entry;
            }
            if(wkingx_starts.Count != 0)
            {
                wking_p.x = wkingx_starts[temporary];
                wking_p.y = wkingy_starts[temporary];
                wking_p.x_dir = -dat.x_entry;
                wking_p.y_dir = -dat.y_entry;
            }
        }
        print("WUck");
        print(cur_level);
        if (cur_level !=85)
        {
            dat.cur_level = cur_level;
        }

        if (GUI == null)
        {
            GUI = Instantiate(gfab, new Vector3(80, -0.5f, 0), Quaternion.identity).GetComponent<Gui_menu>();
        }
        if (alone && white_turn)
        {
            GUI.white_only = true;
        }
        else if (alone && !white_turn)
        {
            GUI.black_only = true;
        }
        if (dat.y_entry == 1)
        {
            dat.north_exits[cur_level] = true;
        }
        else if (dat.x_entry == 1)
        {
            dat.east_exits[cur_level] = true;
        }
        else if (dat.y_entry == -1)
        {
            dat.south_exits[cur_level] = true;
        }
        else if (dat.x_entry == -1)
        {
            dat.west_exits[cur_level] = true;
        }

        Time.timeScale = dat.game_speed;
        control = false;
        scrape = dat.sfx[3];
        tap = dat.sfx[5];
        move = dat.sfx[2];
        slide = dat.sfx[10];
        back = dat.sfx[7];
        undo = dat.sfx[4];
        com = dat.sfx[6];
        cancel = dat.sfx[1];
        uncom = dat.sfx[8];
        detection_enabled = false;
        threat_detecting = false;
        if (dat.items_obtained[18])
        {
            control = true;
        }
        if(cur_level > 9)
        {
            dat.items_obtained[3] = true;
        }
        if (dat.items_obtained[3])
        {
            can_undo = true;
        }
        if (dat.items_obtained[4])
        {
            detection_enabled = true;
            threat_detecting = dat.threats_toggled;
        }
        if (temp_map != null)
        {
            Destroy(temp_map);
        }
        GUI.Setup();

        x_dims = tile_defs.Count;
        for (int i = 0; i < x_dims; i++)
        {
            //print(i + "uwu");
            List<Tile> temp_tiles = new List<Tile>();
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
                        Tile t = Instantiate(Tile, new Vector3(0, 30, 0), Quaternion.identity).GetComponent<Tile>();
                        Transform lower = Instantiate(Lower, new Vector3(0, 29, 0), Quaternion.identity).transform;
                        t.transform.parent = transform;
                        lower.parent = t.transform;
                        t.lower = lower.GetComponent<SpriteRenderer>();
                        t.valid = valid;
                        temp_tiles.Add(t);
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
                Tile t = Instantiate(Tile, new Vector3(0, 30, 0), Quaternion.identity).GetComponent<Tile>();
                Transform lower = Instantiate(Lower, new Vector3(0, 29, 0), Quaternion.identity).transform;
                t.transform.parent = transform;
                lower.parent = t.transform;
                t.lower = lower.GetComponent<SpriteRenderer>();
                t.valid = valid;
                temp_tiles.Add(t);
            }
            total += str_val;
            valid = !valid;
            for (j = total; j < y_dims; j++)
            {
                //print(j + "" + valid);
                Tile t = Instantiate(Tile, new Vector3(0, 30, 0), Quaternion.identity).GetComponent<Tile>();
                Transform lower = Instantiate(Lower, new Vector3(0, 29, 0), Quaternion.identity).transform;
                t.transform.parent = transform;
                lower.parent = t.transform;
                t.lower = lower.GetComponent<SpriteRenderer>();
                t.valid = valid;
                temp_tiles.Add(t);
            }

            //string temp = "";
            //print(temp);
            tiles.Add(temp_tiles);
        }
        if(cur_level == 85)
        {
            control = true;
        }
        //BAH
        /*(for (int i = 0; i < y_dims; i++)
        {
            string s = "";
            for (int j = 0; j < x_dims; j++)
            {
                if (tiles[j][i].valid || tiles[j][i].Occupied())
                {
                    s += "Ye";
                }
                else
                {
                    s += "No";
                }
                s += ",";
            }
            //print(s);
        }*/


        if (!(alone && !white_turn) && wking!= null)
        {
            tiles[wking_p.x][wking_p.y].occupant = wking_p;
        }
        if (!(alone && white_turn) && bking != null)
        {
            tiles[bking_p.x][bking_p.y].occupant = bking_p;
        }


        for (int i = 0; i < x_dims; i++)
        {
            for (int j = 0; j < y_dims; j++)
            {
                tiles[i][j].x = i;
                tiles[i][j].y = j;
            }
        }

        
        cur_x = 0;
        cur_y = 0;
        //white_selector.transform.position = wking.transform.position;
        white_selector.GetComponent<SpriteRenderer>().sprite = invalid;
        dir_cooldowns.Add(0);//up
        dir_cooldowns.Add(0);//down
        dir_cooldowns.Add(0);//left
        dir_cooldowns.Add(0);//right
        mutations.Startup(x_dims, y_dims);

        //wking.transform.position += new Vector3(0, 12f, 0);
        //bking.transform.position += new Vector3(0, 12f, 0);
        if (wking != null)
        {
            wking.transform.parent = this.transform;
        }
        if (bking != null)
        {
            bking.transform.parent = this.transform;
        }
        for (int i = 0; i < black_pieces.Count; i++)
        {
            tiles[black_pieces[i].x][black_pieces[i].y].occupant = black_pieces[i];
            black_pieces[i].transform.parent = this.transform;
            //black_pieces[i].transform.position += new Vector3(0, 12f, 0);
        }
        for (int i = 0; i < white_pieces.Count; i++)
        {
            tiles[white_pieces[i].x][white_pieces[i].y].occupant = white_pieces[i];
            white_pieces[i].transform.parent = this.transform;
            //white_pieces[i].transform.position += new Vector3(0, 12f, 0);
        }
        /*for(int i = 0;i< props.Count;i++)
        { 
            tiles[props[i].x][props[i].y].occupant = props[i];
            props[i].transform.parent = this.transform;
            tiles[props[i].x][props[i].y].valid = false;
        }*/
        for (int i = 0; i < x_dims; i++)
        {
            for (int j = 0; j < y_dims; j++)
            {
                if (mutations.tile_sprites[i][j] != null)
                {
                    //mutations.tile_sprites[i][j].transform.position -= new Vector3(0, 12f, 0);
                }
            }
        }
        /*for (int i = 0; i < y_dims; i++)
        {
            string s = "";
            for (int j = 0; j < x_dims; j++)
            {
                if (tiles[j][i].valid || tiles[j][i].Occupied())
                {
                    s += "Ye";
                }
                else
                {
                    s += "No";
                }
                s += ",";
            }
            print(s);
        }*/
        recalculateThreats();
        transform.position -= new Vector3(12f * dat.x_entry, 12f * dat.y_entry, 0);
        dat.UpdateColour();
        cur_piece = wking_p;
        if (alone && !white_turn)
        {
            cur_piece = bking_p;
            
        }
        if (savetofile)
        {
            SaveToFile();
        }


        if(cur_level == 64 && dat.game_triggers[2])
        {
            tbox.gameObject.SetActive(false);
            entry_text = false;
        }
        else if(cur_level == 64)
        {
            dat.game_triggers[2] = true;
        }
    }

    void SaveToFile()
    {
        string s = "";
        s += (cur_level / 100);
        s += (cur_level / 10) % 10;
        s += (cur_level % 10);
        s += x_dims;
        s += y_dims;
        s += exit_dir + 1;
        //Tile layout
        print("Saving");
        print(s.Length);
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                if (invalidTile(i, j))
                {
                    s += 0;
                }
                else
                {
                    s += 1;
                }
            }
        }
        print(1);
        print(s.Length);
        //public List<int> x_dirs; //100 tiles
        for (int i = 0; i < 100; i++)
        {
            if (invalidTile(i % 10, i / 10))
            {
                s += 1;
            }
            else
            {
                s += (tiles[i % 10][i / 10].x_push + 1);
            }
        }
        print(2);
        print(s.Length);
        // public List<int> y_dirs; //100 tiles
        for (int i = 0; i < 100; i++)
        {
            if (invalidTile(i % 10, i / 10))
            {
                s += 1;
            }
            else
            {
                s += (tiles[i % 10][i / 10].y_push + 1);
            }
        }
        print(3);
        print(s.Length);
        //public List<int> noughts; // -1 for cross 1 for nought
        for (int i = 0; i < 100; i++)
        {
            if (invalidTile(i % 10, i / 10))
            {
                s += 1;
            }
            else
            {
                s += (tiles[i % 10][i / 10].nought + 1);
            }
        }
        print(4);
        print(s.Length);
        ///public List<int> numbering; //-1 for none, else a number
        for (int i = 0; i < 100; i++)
        {
            if (invalidTile(i % 10, i / 10))
            {
                s += 0;
            }
            else
            {
                s += (tiles[i % 10][i / 10].number + 1);
            }
        }
        //public List<int> occupants;
        mutations.occupants.Clear();
        for (int i = 0; i < 100; i++)
        {
            if (invalidTile(i % 10, i / 10) || !tiles[i % 10][i / 10].Occupied())
            {
                s += "00";
            }
            else if (tiles[i % 10][i / 10].White())
            {
                if (tiles[i % 10][i / 10].occupant.type == 5)
                {
                    s += "01";
                }
                else
                {
                    s += "0";
                    s += tiles[i % 10][i / 10].occupant.type + 1;
                }

            }
            else if (tiles[i % 10][i / 10].Black())
            {
                if (tiles[i % 10][i / 10].occupant.type == 5)
                {
                    s += "06";
                }
                else
                {
                    s += (tiles[i % 10][i / 10].occupant.type + 6) / 10;
                    s += (tiles[i % 10][i / 10].occupant.type + 6) % 10;
                }
            }
        }
        print(6);
        print(s.Length);
        //public List<int> occ_x_dirs;
        for (int i = 0; i < 100; i++)
        {
            if (invalidTile(i % 10, i / 10) || !tiles[i % 10][i / 10].Occupied())
            {
                s += 1;
            }
            else
            {
                s += (tiles[i % 10][i / 10].occupant.x_dir + 1);
            }
        }
        print(7);
        print(s.Length);
        //public List<int> occ_y_dirs;
        for (int i = 0; i < 100; i++)
        {
            if (invalidTile(i % 10, i / 10) || !tiles[i % 10][i / 10].Occupied())
            {
                s += 1;
            }
            else
            {
                s += (tiles[i % 10][i / 10].occupant.y_dir + 1);
            }
        }
        print(8);
        print(s.Length);
        // public List<bool> exits;
        for (int i = 0; i < 100; i++)
        {
            if (invalidTile(i % 10, i / 10))
            {
                s += 0;
            }
            else if (tiles[i % 10][i / 10].king_exit || tiles[i % 10][i / 10].arrow_exit != -1)
            {
                s += 1;
            }
            else
            {
                s += 0;
            }
        }
        print(9);
        print(s.Length);
        // public List<int> specialtiles = new List<int>();
        for (int i = 0; i < 100; i++)
        {
            if (outOfBounds(i % 10, i / 10))
            {
                s += "0";
            }
            else
            {
                s += (mutations.specialtiles[i]) + 1;
            }
        }
        print(10);
        print(s.Length);
        StreamWriter sw = new StreamWriter(Application.dataPath + "/Files/" + filename + ".txt");
        sw.Write(s);
        sw.Close();
        print("Saving dobe");
    }
    private int filePointer;
    void LoadFromFile()
    {
        //SET STUFF TO GUI
        /*
        public List<GameObject> pieces;//0 king 1 rook 2 bishop 3 knight 4 pawn, +5 for black,10 wqueen,11 bqueen*/
        //x dims
        //y dims
        //valid tiles :)
        StreamReader read = new StreamReader(Application.dataPath + "/Files/" + filename + ".txt");
        string line = read.ReadLine();
        read.Close();
        char[] s = line.ToCharArray();
        cur_level = getNext(s)*100+ getNext(s)*10+ getNext(s);
        print("Loading");
        print(filePointer);
        x_dims = getNext(s);
        if(x_dims == 1)
        {
            x_dims = 10 + getNext(s);
        }
        y_dims = getNext(s);
        if(y_dims == 1)
        {
            y_dims = 10 + getNext(s);
        }
        exit_dir = getNext(s) - 1;
        List<List<int>> t = new List<List<int>>();
        tile_defs = new List<string>();
        //Tile layout
        print(1);
        print(filePointer);
        for (int i = 0;i<10;i++)
        {
            List<int> t2 = new List<int>();
            for (int j = 0; j < 10; j++)
            {
                t2.Add(getNext(s));
            }
            t.Add(t2);
        }
        print(2);
        print(filePointer);
        int streak = 0;
        bool cur = true;
        for (int i = 0; i < x_dims; i++)
        {
            string defs = "";
            streak = 0;
            cur = true;
            for (int j = 0; j < y_dims; j++)
            {
                if((cur && t[i][j] == 1)|| (!cur && t[i][j] == 0))
                {
                    streak++;
                }
                else 
                {
                    cur = !cur;
                    defs += streak + ",";
                    streak = 1;
                }
            }
            defs += streak;
            tile_defs.Add(defs);
        }
        //public List<int> x_dirs; //100 tiles
        mutations.x_dirs.Clear();
        print(3);
        print(filePointer);
        for (int i = 0; i < 100; i++)
        {
            mutations.x_dirs.Add(getNext(s) - 1);
        }
        // public List<int> y_dirs; //100 tiles
        mutations.y_dirs.Clear();
        print(4);
        print(filePointer);
        for (int i = 0; i < 100; i++)
        {
            mutations.y_dirs.Add(getNext(s) - 1);
        }
        print(6);
        print(filePointer);
        //public List<int> noughts; // -1 for cross 1 for nought
        mutations.noughts.Clear();
        for (int i = 0; i < 100; i++)
        {
            mutations.noughts.Add(getNext(s) - 1);
        }
        print(6);
        print(filePointer);
        ///public List<int> numbering; //-1 for none, else a number
        mutations.numbering.Clear();
        for (int i = 0; i < 100; i++)
        {
            mutations.numbering.Add(getNext(s) - 1);
        }
        print(7);
        print(filePointer);
        //public List<int> occupants;
        mutations.occupants.Clear();
        for (int i = 0; i < 100; i++)
        {
            mutations.occupants.Add(getNext(s) * 10 + getNext(s) - 1);
        }
        print(8);
        print(filePointer);
        //public List<int> occ_x_dirs;
        mutations.occ_x_dirs.Clear();
        for (int i = 0; i < 100; i++)
        {
            mutations.occ_x_dirs.Add(getNext(s) - 1);
        }
        print(9);
        print(filePointer);
        //public List<int> occ_y_dirs;
        mutations.occ_y_dirs.Clear();
        for (int i = 0; i < 100; i++)
        {
            mutations.occ_y_dirs.Add(getNext(s) - 1);
        }
        print(10);
        print(filePointer);
        // public List<bool> exits;
        mutations.exits.Clear();
        for (int i = 0; i < 100; i++)
        {
            mutations.exits.Add(getNext(s)==1);
        }
        print(11);
        print(filePointer);
        // public List<int> specialtiles = new List<int>();
        mutations.specialtiles.Clear();
        for (int i = 0; i < 100; i++)
        {
            mutations.specialtiles.Add(getNext(s)-1);
        }
        print(12);
        print(filePointer);

        print("Loading done");

    }
    int getNext(char[] s)
    {
        filePointer++;
        return s[filePointer - 1] - '0';
    }





    private void FixedUpdate()
    {
        if(completed)
        {
            if(exit_dir == 0)
            {
                transform.position -= new Vector3(0, 6 / 21f, 0);
                dat.y_entry = -1;
                dat.x_entry = 0;
            }
            else if (exit_dir == 1)
            {
                transform.position -= new Vector3(6 / 21f,0, 0);
                dat.y_entry = 0;
                dat.x_entry = -1;
            }
            else if (exit_dir == 2)
            {
                transform.position -= new Vector3(0, -6 / 21f, 0);
                dat.y_entry = 1;
                dat.x_entry = 0;
            }
            else if (exit_dir == 3)
            {
                transform.position -= new Vector3(-6 / 21f, 0, 0);
                dat.y_entry = 0;
                dat.x_entry = 1;
            }
            menu_countdown--;
            if (menu_countdown == 0)
            {
                print("E?");
                if(special_level)
                {
                    if (exit_dir == 0)
                    {
                        dat.north_exits[cur_level] = true;
                    }
                    else if (exit_dir == 1)
                    {
                        dat.east_exits[cur_level] = true;
                    }
                    else if (exit_dir == 2)
                    {
                        dat.south_exits[cur_level] = true;
                    }
                    else if (exit_dir == 3)
                    {
                        dat.west_exits[cur_level] = true;
                    }
                }
                else
                {
                    dat.north_exits[cur_level] = true;
                    dat.south_exits[cur_level] = true;
                    dat.east_exits[cur_level] = true;
                    dat.west_exits[cur_level] = true;
                }
                if (cur_level < 6)
                {
                    if(cur_level == 5 && exit_dir == 3)
                    {
                        SceneManager.LoadScene("Intro extra");
                    }
                    else
                    {
                        SceneManager.LoadScene("Intro" + (cur_level + 2));
                    }
                    
                }
                else if (cur_level == 6 && !dat.items_obtained[1])
                {
                    SceneManager.LoadScene("Intro7");
                }
                else if(cur_level == 7)
                {
                    SceneManager.LoadScene("Intro6");
                }
                else
                {
                    print("Gyuh?");
                    
                    if (special_level)
                    {
                        dat.next_level = exit_levels[exit_dir];
                        char[] chars = exit_levels[exit_dir].ToCharArray();
                        
                        if (chars[0] == 'l')
                        {
                            print(chars.Length);
                            if (chars.Length == 2)
                            {
                                dat.cur_level = (chars[1] - '0');
                                SceneManager.LoadScene("Puzzle default");
                            }
                            else if (chars.Length == 3)
                            {
                                dat.cur_level = (chars[1] - '0') * 10 + (chars[2] - '0');
                                SceneManager.LoadScene("Puzzle default");
                            }
                            else
                            {
                                dat.cur_level = (chars[1] - '0') * 100 + (chars[2] - '0') * 10 + (chars[3] - '0');
                                SceneManager.LoadScene("Puzzle default");
                            }
                            
                        }
                        else
                        {
                            SceneManager.LoadScene(exit_levels[exit_dir]);
                        }
                    }
                    else
                    {
                        if(cur_level == 13)
                        {
                            SceneManager.LoadScene("Reg 2");
                        }
                        else if(cur_level % 5 == 0 && cur_level > 10)
                        {
                            List<string> victory_levels = new List<string>();
                            victory_levels.Add("Reg victory 1");
                            victory_levels.Add("Arrow victory 1");
                            victory_levels.Add("Nc victory 1");
                            victory_levels.Add("Number victory 1");
                            victory_levels.Add("Variety victory 1");
                            victory_levels.Add("Reg victory 2");
                            victory_levels.Add("Arrow victory 2");
                            victory_levels.Add("Nc victory 2");
                            victory_levels.Add("Number victory 2");
                            victory_levels.Add("Var victory 2");
                            SceneManager.LoadScene(victory_levels[(cur_level - 11)/5]);
                        }
                        else
                        {
                            dat.next_level = "l"+ (cur_level - 9);
                            dat.cur_level = cur_level + 1;
                            SceneManager.LoadScene("Puzzle default");
                        }
                    }
                    
                }
                
            }
        }
        if(paused || text)
        {
            return;
        }
        UpdateGui();
        if (startup != -1 || startup > 42)
        {
            {/*bool remaining = false;
            int counter = 0;
            for (int j = 0; j < y_dims; j += 2)
            {
                for (int i = 0; i < y_dims; i += 2)
                {
                    //mutations.tile_sprites[i][j].transform.position += new Vector3(0, 8f / 21f, 0);
                    print(j);
                    if (counter > startup / 2)
                    {
                        break;
                    }
                    bool any = false;
                    if (tiles[i][j].valid || tiles[i][j].Occupied())
                    {
                        if (tiles[i][j].arise < 21)
                        {
                            tiles[i][j].arise++;
                            mutations.tile_sprites[i][j].transform.position += new Vector3(0, 12f / 21f, 0);
                            remaining = true;
                        }
                        any = true;
                    }
                    if (j + 1 < y_dims && (tiles[i][j + 1].valid || tiles[i][j + 1].Occupied()))
                    {
                        if (tiles[i][j + 1].arise < 21)
                        {
                            tiles[i][j + 1].arise++;
                            mutations.tile_sprites[i][j + 1].transform.position += new Vector3(0, 12f / 21f, 0);
                            remaining = true;
                        }
                        any = true;
                    }
                    if (i + 1 < x_dims && (tiles[i + 1][j].valid || tiles[i + 1][j].Occupied()))
                    {
                        if (tiles[i + 1][j].arise < 21)
                        {
                            tiles[i + 1][j].arise++;
                            mutations.tile_sprites[i + 1][j].transform.position += new Vector3(0, 12f / 21f, 0);
                            remaining = true;
                        }
                        any = true;
                    }
                    if (i + 1 < y_dims && j + 1 < y_dims && (tiles[i][j + 1].valid || tiles[i][j + 1].Occupied()))
                    {
                        if (tiles[i + 1][j + 1].arise < 21)
                        {
                            tiles[i + 1][j + 1].arise++;
                            mutations.tile_sprites[i + 1][j + 1].transform.position += new Vector3(0, 12f / 21f, 0);
                            remaining = true;
                        }
                        any = true;
                    }
                    if (any)
                    {
                        counter += 1;
                    }
                }
                if (counter > startup / 2)
                {
                    break;
                }
            } 
            counter = 0;
            if (!remaining)
            {
                counter = 0;
            }*/}
            transform.position -= new Vector3(6 / 21f * -dat.x_entry, 6 / 21f * -dat.y_entry, 0);
            if(startup == 42)
            {
                transform.position = new Vector3(0, 0, 0);
                startup = -2;
                if(entry_text)
                {
                    tbox.gameObject.SetActive(true);
                }
                if (wking != null && white_turn)
                {
                    white_selector.transform.position = wking.transform.position;
                }
                else
                {
                    black_selector.transform.position = bking.transform.position;
                }
                for (int i = 0; i < black_pieces.Count; i++)
                {
                    black_pieces[i].GetComponent<SpriteRenderer>().sortingOrder = (int)(-black_pieces[i].transform.position.y * 200);
                }
                for (int i = 0; i < white_pieces.Count; i++)
                {
                    white_pieces[i].GetComponent<SpriteRenderer>().sortingOrder = (int)(-white_pieces[i].transform.position.y * 200);
                }
                /*for (int i = 0; i < props.Count; i++)
                {
                    props[i].GetComponent<SpriteRenderer>().sortingOrder = (int)(-props[i].transform.position.y * 200);
                }*/
                if (bking != null)
                {
                    bking.GetComponent<SpriteRenderer>().sortingOrder = (int)(-bking.transform.position.y * 200);
                }
                if (wking != null)
                {
                    wking.GetComponent<SpriteRenderer>().sortingOrder = (int)(-wking.transform.position.y * 200);
                }
            }
            if(startup == 84)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            //print(startup);
            startup++;
        }

        if(ascend_count > 0)
        {
            if (asc_piece != null)
            {
                asc_piece.transform.position += new Vector3(0, 5 / 21f, 0);
            }
            ascend_count--;
            if (ascend_count == 0)
            {
                scrape.Play(0);
            }
        }
        else if (move_counter != -1)
        {
           // print(move_counter);
            if (asc_piece != null && move_counter < 24)
            {
                asc_piece.transform.position += new Vector3(0, 5 / 21f, 0);
            }
            //print("E"); 
            //print(x_dir);
            //print(y_dir);
            if (move_counter < 8)
            {
                move_target.transform.position += new Vector3(0, 1f/21f, 0);
            }
            else if (move_counter >= move_length * 21 - 5)
            {
                move_target.transform.position -= new Vector3(0, 1f / 21f, 0);
            }
            if (move_counter < move_length * 21)
            {
                //print("moving");
                if (piece_count == -1)
                {
                    true_y += 1f/21f * y_dir;
                }
                   
                move_target.transform.position += new Vector3(1f / 21f * x_dir, 1f / 21f * y_dir, 0);
            }
            move_counter++;
            if (move_counter == move_length * 21 + 3)
            {
                tap.Play(0);
                tiles[cur_piece.x][cur_piece.y].Landed();
                modifySpecials(true);
                if(tiles[cur_piece.x][cur_piece.y].special == 4)
                {
                    for (int i = 0; i < x_dims; i++)
                    {
                        for (int j = 0; j < x_dims; j++)
                        {
                            if (tiles[i][j].special != 5)
                            {
                                //tiles[i][j].nought = -tiles[i][j].nought;
                            }
                            tiles[i][j].x_push = -tiles[i][j].x_push;
                            tiles[i][j].y_push = -tiles[i][j].y_push;
                        }
                    }
                }
                if (asc_piece != null)
                {
                    asc_piece = null;
                    //Destroy(asc_piece.gameObject);
                }
                if (adjacent_noughts)
                {
                    if (!outOfBounds(cur_piece.x + 1, cur_piece.y))
                    {
                        tiles[cur_piece.x + 1][cur_piece.y].nought = -tiles[cur_piece.x + 1][cur_piece.y].nought;
                    }
                    if (!outOfBounds(cur_piece.x - 1, cur_piece.y))
                    {
                        tiles[cur_piece.x - 1][cur_piece.y].nought = -tiles[cur_piece.x - 1][cur_piece.y].nought;
                    }
                    if (!outOfBounds(cur_piece.x, cur_piece.y + 1))
                    {
                        tiles[cur_piece.x][cur_piece.y + 1].nought = -tiles[cur_piece.x][cur_piece.y + 1].nought;
                    }
                    if (!outOfBounds(cur_piece.x, cur_piece.y - 1))
                    {
                        tiles[cur_piece.x][cur_piece.y - 1].nought = -tiles[cur_piece.x][cur_piece.y - 1].nought;
                    }
                    mutations.Refresh(x_dims, y_dims);
                }
                if (decrement)
                {
                    for (int i = 0; i < x_dims; i++)
                    {
                        for (int j = 0; j < y_dims; j++)
                        {
                            if (!tiles[i][j].Occupied())
                            {
                                tiles[i][j].Decrement();
                            }
                        }
                    }
                    mutations.Refresh(x_dims, y_dims);
                }
                int total_noughts = 0;
                int total_crosses = 0;
                for (int i = 0; i < x_dims; i++)
                {
                    for (int j = 0; j < y_dims; j++)
                    {
                        if (!tiles[i][j].Occupied())
                        {
                            if (tiles[i][j].nought == -1 && tiles[i][j].special != 5)
                            {
                                total_noughts++;
                            }
                            else if (tiles[i][j].nought == 1)
                            {
                                total_crosses++;
                            }
                        }
                    }
                }
                if (total_noughts == 0 || total_crosses == 0)
                {
                    for (int i = 0; i < x_dims; i++)
                    {
                        for (int j = 0; j < y_dims; j++)
                        {
                            tiles[i][j].nought = 0;
                        }
                    }
                    mutations.Refresh(x_dims, y_dims);
                }
                List<int> total_nums = new List<int>();
                total_nums.Add(0); total_nums.Add(0); total_nums.Add(0); total_nums.Add(0);
                for (int i = 0; i < x_dims; i++)
                {
                    for (int j = 0; j < y_dims; j++)
                    {
                        if (tiles[i][j].number != -1)
                        {
                            total_nums[tiles[i][j].number] = 1;
                        }
                    }
                }
                int sum = 0;
                for (int i = 0; i < 4; i++)
                {
                    sum += total_nums[i];
                }
                if (sum <= 1)
                {
                    for (int i = 0; i < x_dims; i++)
                    {
                        for (int j = 0; j < y_dims; j++)
                        {
                            tiles[i][j].number = -1;
                        }
                    }
                }
                if ((total_noughts == 0 || total_crosses == 0) && sum == 0)
                {
                    for (int i = 0; i < x_dims; i++)
                    {
                        for (int j = 0; j < y_dims; j++)
                        {
                            if(tiles[i][j].special == 5)
                            {
                                tiles[i][j].special = -1;
                            }
                        }
                    }
                }


                white_selector.GetComponent<SpriteRenderer>().sprite = invalid;
                black_selector.GetComponent<SpriteRenderer>().sprite = invalid;
                if (!cur_piece.faceless)
                {
                    cur_piece.animator.SetFloat("x", cur_piece.x_dir);
                    cur_piece.animator.SetFloat("y", cur_piece.y_dir);
                }
                cur_piece.transform.position = new Vector3(Mathf.Round(2 * cur_piece.transform.position.x)/2, Mathf.Round(2 * cur_piece.transform.position.y)/2, 0);


                cur_x = tiles[cur_piece.x][cur_piece.y].x_push;
                cur_y = tiles[cur_piece.x][cur_piece.y].y_push;
                if ((cur_x != 0 || cur_y != 0) && !invalidTile(cur_piece.x + cur_x,cur_piece.y - cur_y) &&  tiles[cur_piece.x + cur_x][cur_piece.y - cur_y].nought != -1 && tiles[cur_piece.x + cur_x][cur_piece.y - cur_y].number != 0 && (!tiles[cur_piece.x + cur_x][cur_piece.y - cur_y].Occupied() || tiles[cur_piece.x + cur_x][cur_piece.y - cur_y].Black() == white_turn))
                {
                    print("Gu");
                    print(!tiles[cur_piece.x + cur_x][cur_piece.y - cur_y].Occupied());
                    print(tiles[cur_piece.x + cur_x][cur_piece.y - cur_y].Black() != white_turn);
                    if (tiles[cur_piece.x + cur_x][cur_piece.y - cur_y].occupant != null && tiles[cur_piece.x + cur_x][cur_piece.y - cur_y].Black() == white_turn)
                    {
                        asc_piece = tiles[cur_piece.x + cur_x][cur_piece.y - cur_y].occupant.transform;
                        tiles[cur_piece.x + cur_x][cur_piece.y - cur_y].occupant.captured = true;
                        ascend_count = 18;
                        //Destroy(tiles[cur_piece.x + cur_x][cur_piece.y - cur_y].occupant);
                    }
                    print("EEEEEE");
                    x_dir = cur_x;
                    y_dir = cur_y;
                    cur_piece.animator.SetFloat("x", cur_x);
                    cur_piece.animator.SetFloat("y", cur_y);
                    tiles[cur_piece.x][cur_piece.y].valid = true;
                    Piece temp = tiles[cur_piece.x][cur_piece.y].occupant;
                    tiles[cur_piece.x][cur_piece.y].occupant = null;
                    mutations.Refresh(x_dims, y_dims);
                    cur_piece.x += cur_x;
                    cur_piece.y -= cur_y;
                    tiles[cur_piece.x][cur_piece.y].valid = false;
                    tiles[cur_piece.x][cur_piece.y].occupant = temp;
                    //scrape.Play(0);
                    can_move = false;
                    move_length = 1;
                    move_target = cur_piece.gameObject;
                    //piece_selected = 0;
                    piece_number_selected = 1;
                    move_counter = 0;
                    recalculateThreats();
                    white_selector.SetActive(false);
                    black_selector.SetActive(false);
                    print(move_counter);
                }
                else if (cur_piece.type == 4 && invalidTile(cur_piece.x + cur_piece.x_dir, cur_piece.y - cur_piece.y_dir) && !loss)
                {
                    print(cur_piece.x + cur_piece.x_dir);
                    print(cur_piece.y - cur_piece.y_dir);
                    if (white_turn)
                    {
                        w_promotion.SetActive(true);
                    }
                    else
                    {
                        b_promotion.SetActive(true);
                    }
                    promotion_pointer.SetActive(true);
                    promotion_pointer.transform.position = GUI.transform.position + new Vector3(0, 22f / 21f, 0);
                    promotion = 1;
                    promotion_choice = 1;
                    scrape.Play(0);
                    move_counter = -1;
                }
                else
                {
                    external = false;
                    if(white_turn)
                    {
                        for(int i = 0;i<white_pieces.Count;i++)
                        {
                            if(white_pieces[i].automove && !white_pieces[i].captured)
                            {
                                for (int j = 0; j < white_pieces[i].forced_moves_x.Count; j++)
                                {
                                    Piece wp = white_pieces[i];
                                    if (wking_p.x == wp.forced_moves_trigger_x[j]&& wking_p.y == wp.forced_moves_trigger_y[j] && (wp.forced_moves_x[j] != wp.x || wp.forced_moves_y[j] != wp.y))
                                    {
                                        print(wp.forced_moves_x[j]);
                                        print(wp.forced_moves_y[j]);
                                        /*x_dir = forced_moves_x[j] - rook.x;
                                        y_dir = -(forced_moves_y[j] - rook.y);
                                        piece_occupants[rook.x][rook.y] = 0;
                                        rook.x += x_dir;
                                        rook.y -= y_dir;
                                        piece_occupants[rook.x][rook.y] = -1;
                                        move_target = rook.gameObject;
                                        cur = rook_anim;*/
                                        move_counter = 0;
                                        print("Balls");
                                        x_dir = wp.forced_moves_x[j] - wp.x;
                                        y_dir = -(wp.forced_moves_y[j] - wp.y);
                                        cur_piece = wp;
                                        //cur_piece.animator.SetFloat("x", cur_x);
                                        //cur_piece.animator.SetFloat("y", cur_y);
                                        tiles[cur_piece.x][cur_piece.y].valid = true;
                                        Piece temp = tiles[cur_piece.x][cur_piece.y].occupant;
                                        tiles[cur_piece.x][cur_piece.y].occupant = null;
                                        mutations.Refresh(x_dims, y_dims);
                                        cur_piece.x = wp.forced_moves_x[j];
                                        cur_piece.y = wp.forced_moves_y[j];
                                        tiles[cur_piece.x][cur_piece.y].valid = false;
                                        tiles[cur_piece.x][cur_piece.y].occupant = temp;
                                        scrape.Play(0);
                                        can_move = false;
                                        move_length = 1;
                                        move_target = cur_piece.gameObject;
                                        piece_number_selected = 1;
                                        move_counter = 0;
                                        recalculateThreats();
                                        white_selector.SetActive(false);
                                        black_selector.SetActive(false);
                                        external = true;

                                        break;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < black_pieces.Count; i++)
                        {
                            if (black_pieces[i].automove && !black_pieces[i].captured)
                            {
                                for (int j = 0; j < black_pieces[i].forced_moves_x.Count; j++)
                                {
                                    Piece bp = black_pieces[i];
                                    //print(j);
                                   // print("E");
                                    if (bking_p.x == bp.forced_moves_trigger_x[j] && bking_p.y == bp.forced_moves_trigger_y[j] && (bp.forced_moves_x[j] != bp.x || bp.forced_moves_y[j] != bp.y))
                                    {
                                        print(bp.forced_moves_x[j]);
                                        print(bp.forced_moves_y[j]);
                                        //print("bug");
                                        /*x_dir = forced_moves_x[j] - rook.x;
                                        y_dir = -(forced_moves_y[j] - rook.y);
                                        piece_occupants[rook.x][rook.y] = 0;
                                        rook.x += x_dir;
                                        rook.y -= y_dir;
                                        piece_occupants[rook.x][rook.y] = -1;
                                        move_target = rook.gameObject;
                                        cur = rook_anim;*/
                                        move_counter = 0;
                                        print("Buh");
                                        x_dir = bp.forced_moves_x[j] - bp.x;
                                        y_dir = -(bp.forced_moves_y[j]-bp.y);
                                        cur_piece = bp;
                                        //cur_piece.animator.SetFloat("x", cur_x);
                                        //cur_piece.animator.SetFloat("y", cur_y);
                                        tiles[cur_piece.x][cur_piece.y].valid = true;
                                        Piece temp = tiles[cur_piece.x][cur_piece.y].occupant;
                                        tiles[cur_piece.x][cur_piece.y].occupant = null;
                                        mutations.Refresh(x_dims, y_dims);
                                        cur_piece.x = bp.forced_moves_x[j];
                                        cur_piece.y = bp.forced_moves_y[j];
                                        tiles[cur_piece.x][cur_piece.y].valid = false;
                                        tiles[cur_piece.x][cur_piece.y].occupant = temp;
                                        scrape.Play(0);
                                        can_move = false;
                                        move_length = 1;
                                        move_target = cur_piece.gameObject;
                                        piece_number_selected = 1;
                                        move_counter = 0;
                                        recalculateThreats();
                                        white_selector.SetActive(false);
                                        black_selector.SetActive(false);
                                        external = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    if(!external)
                    {
                        if (wking_p != null && wking_p.automove && !wking_has_moved_for_intro)
                        {
                            for (int j = 0; j < wking_p.forced_moves_x.Count; j++)
                            {

                                Piece bp = wking_p;
                                //print(j);
                                // print("E");
                                if (bking_p.x == bp.forced_moves_trigger_x[j] && bking_p.y == bp.forced_moves_trigger_y[j])
                                {
                                    if (tiles[1][1].black_threat || (bp.x == 1 && bp.y == 1))
                                    {
                                        if (tiles[0][1].black_threat || (bp.x == 0 && bp.y == 1))
                                        {
                                            if (tiles[0][2].black_threat || (bp.x == 0 && bp.y == 2))
                                            {
                                                List<string> strs = new List<string>();
                                                strs.Add("#kw3Well now I can't move.");
                                                strs.Add("Let's go back a second and try not to do this again.");
                                                tbox.startStrings = strs;
                                                tbox.gameObject.SetActive(true);
                                                startup = 43;
                                                break;
                                            }
                                            else
                                            {
                                                bp.forced_moves_x[j] = 0;
                                                bp.forced_moves_y[j] = 2;
                                            }
                                        }
                                        else
                                        {
                                            bp.forced_moves_x[j] = 0;
                                            bp.forced_moves_y[j] = 1;
                                        }
                                    }
                                    else
                                    {
                                        bp.forced_moves_x[j] = 1;
                                        bp.forced_moves_y[j] = 1;
                                    }
                                    wking_has_moved_for_intro = true;
                                    //print("bug");
                                    /*x_dir = forced_moves_x[j] - rook.x;
                                    y_dir = -(forced_moves_y[j] - rook.y);
                                    piece_occupants[rook.x][rook.y] = 0;
                                    rook.x += x_dir;
                                    rook.y -= y_dir;
                                    piece_occupants[rook.x][rook.y] = -1;
                                    move_target = rook.gameObject;
                                    cur = rook_anim;*/
                                    move_counter = 0;
                                    print("MUh");
                                    x_dir = bp.forced_moves_x[j] - bp.x;
                                    y_dir = -(bp.forced_moves_y[j] - bp.y);
                                    cur_piece = bp;
                                    bp.animator.SetFloat("x", x_dir);
                                    bp.animator.SetFloat("y", y_dir);
                                    tiles[cur_piece.x][cur_piece.y].valid = true;
                                    Piece temp = tiles[cur_piece.x][cur_piece.y].occupant;
                                    tiles[cur_piece.x][cur_piece.y].occupant = null;
                                    mutations.Refresh(x_dims, y_dims);
                                    cur_piece.x = bp.forced_moves_x[j];
                                    cur_piece.y = bp.forced_moves_y[j];
                                    tiles[cur_piece.x][cur_piece.y].valid = false;
                                    tiles[cur_piece.x][cur_piece.y].occupant = temp;
                                    scrape.Play(0);
                                    can_move = false;
                                    move_length = 1;
                                    move_target = cur_piece.gameObject;
                                    piece_number_selected = 1;
                                    move_counter = 0;
                                    recalculateThreats();
                                    white_selector.SetActive(false);
                                    black_selector.SetActive(false);
                                    external = true;
                                    break;
                                }
                            }
                        }
                    }
                    if (!external)
                    {
                        if (!alone)
                        {
                            white_turn = !white_turn;
                        }
                        if (white_turn)
                        {
                            cur_piece = wking_p;
                            white_selector.SetActive(true);
                            white_selector.transform.position = wking.transform.position;
                        }
                        else
                        {
                            cur_piece = bking_p;
                            black_selector.SetActive(true);
                            black_selector.transform.position = bking.transform.position;
                        }
                        
                        can_move = true;
                        move_counter = -1;
                        checkFinish();
                    }
                    modifySpecials(true);
                }
                cur_x = 0;
                cur_y = 0;
                //print("moved")
            }
            move_target.GetComponent<SpriteRenderer>().sortingOrder = (int)(-move_target.transform.position.y * 200);
        }

        if(promotion > 0 && promotion < 21)
        {
            cur_piece.transform.position += new Vector3(0, 10 / 21f, 0);
            promotion++;
        }
        else if(promotion < 0 && promotion > -21)
        {
            cur_piece.transform.position -= new Vector3(0, 10 / 21f, 0);
            promotion--;
        }
        else if(promotion == -21)
        {
            promotion = 0;
            tap.Play(0);
            if (!alone)
            {
                white_turn = !white_turn;
                if (white_turn)
                {
                    cur_piece = wking_p;
                    white_selector.SetActive(true);
                    white_selector.transform.position = wking.transform.position;
                }
                else
                {
                    cur_piece = bking_p;
                    black_selector.SetActive(true);
                    black_selector.transform.position = bking.transform.position;
                }

            }
            w_promotion.SetActive(false);
            b_promotion.SetActive(false);
            promotion_pointer.SetActive(false);
            can_move = true;
            move_counter = -1;
            modifySpecials(true);
            checkFinish();
        }
        
    }


    void Update()
    {
        for (int i = 0; i < 4; i++)
        {
            if (dir_cooldowns[i] > 0)
            {
                dir_cooldowns[i]--;
            }
        }
        if (cooldown > 0)
        {
            cooldown--;
        }
        if(undocooldown > 0)
        {
            undocooldown--;
        }
        if (text || completed)
        {
            return;
        }
        if (!paused)
        {
            if (Input.GetAxis("Close") > 0 && !p_cooldown && cur_piece.type == 5)
            {
                back.Play(0);
                paused = true;
                GUI.Pause(white_turn);
                p_cooldown = true;
            }
            else if (Input.GetAxis("Close") == 0)
            {
                p_cooldown = false;
            }
            if (flat == 0)
            {
                if (Input.GetAxis("2d") > 0 && !flat_down)
                {
                    flat = 1;
                    flat_down = true;
                    for (int i = 0; i < black_pieces.Count; i++)
                    {
                        black_pieces[i].animator.SetBool("2d", true);
                        black_pieces[i].animator.SetFloat("Type2d",black_pieces[i].type);
                    }
                    for (int i = 0; i < white_pieces.Count; i++)
                    {
                        white_pieces[i].animator.SetBool("2d", true);
                        white_pieces[i].animator.SetFloat("Type2d", white_pieces[i].type);
                    }
                    wking_p.animator.SetBool("2d", true);
                    wking_p.animator.SetFloat("Type2d", 5);
                    bking_p.animator.SetBool("2d", true);
                    bking_p.animator.SetFloat("Type2d", 5);
                }
                else if (Input.GetAxis("2d") == 0)
                {
                    flat_down = false;
                }
            }
            else if (flat == 1)
            {
                if (Input.GetAxis("2d") > 0 && !flat_down)
                {
                    flat = 2;
                    flat_down = true;
                    for (int i = 0; i < black_pieces.Count; i++)
                    {
                        black_pieces[i].GetComponent<SpriteRenderer>().enabled = false;
                    }
                    for (int i = 0; i < white_pieces.Count; i++)
                    {
                        white_pieces[i].GetComponent<SpriteRenderer>().enabled = false;
                    }
                    wking_p.GetComponent<SpriteRenderer>().enabled = false;
                    bking_p.GetComponent<SpriteRenderer>().enabled = false;
                }
                else if (Input.GetAxis("2d") == 0)
                {
                    flat_down = false;
                }
            }
            else if (flat == 2)
            {
                if (Input.GetAxis("2d") > 0 && !flat_down)
                {
                    flat = 0;
                    flat_down = true;
                    for (int i = 0; i < black_pieces.Count; i++)
                    {
                        black_pieces[i].animator.SetBool("2d", false);
                        black_pieces[i].GetComponent<SpriteRenderer>().enabled = true;
                    }
                    for (int i = 0; i < white_pieces.Count; i++)
                    {
                        white_pieces[i].animator.SetBool("2d", false);
                        white_pieces[i].GetComponent<SpriteRenderer>().enabled = true;
                    }
                    wking_p.animator.SetBool("2d", false);
                    bking_p.animator.SetBool("2d", false);
                    wking_p.GetComponent<SpriteRenderer>().enabled = true;
                    bking_p.GetComponent<SpriteRenderer>().enabled = true;
                }
                else if (Input.GetAxis("2d") == 0)
                {
                    flat_down = false;
                }
            }
        }
        if (paused)
        {
            GUI.PauseMenu(white_turn);
            if (Input.GetAxis("Close") > 0 && !p_cooldown)
            {
                back.Play(0);
                print("Uh");
                paused = false;
                GUI.Unpause();
                p_cooldown = true;
            }
            else if(Input.GetAxis("Close") == 0)
            {
                p_cooldown = false;
            }
        }
        else if (promotion > 0)
        {
            if (promotion_choice >= 0)
            {
                if (Input.GetAxis("Horizontal") >= 0.1)
                {
                    dir_cooldowns[3] = 0;
                    if (dir_cooldowns[2] == 0)
                    {
                        dir_cooldowns[2] = dat.cooldowns;
                        promotion_choice++;
                        promotion_choice %= 3;
                        promotion_pointer.transform.position = GUI.transform.position + new Vector3(-2 + 2 * promotion_choice, 22f / 21f, 0);
                    }
                }
                else if (Input.GetAxis("Horizontal") <= -0.1)
                {
                    dir_cooldowns[2] = 0;
                    if (dir_cooldowns[3] == 0)
                    {
                        dir_cooldowns[3] = dat.cooldowns;
                        promotion_choice += 2;
                        promotion_choice %= 3;
                        promotion_pointer.transform.position = GUI.transform.position + new Vector3(-2 + 2 * promotion_choice, 22f / 21f, 0);
                    }
                }
                else
                {
                    dir_cooldowns[2] = 0; dir_cooldowns[3] = 0;
                }
                if (Input.GetAxis("Submit") > 0)
                {
                    cur_piece.type = 3 - promotion_choice;
                    cur_piece.SetAnims();
                    promotion_choice = -1;
                }
            }
            if(promotion == 21 && promotion_choice ==-1)
            {
                promotion = -1;
                recalculateThreats();
                mutations.Refresh(x_dims,y_dims);
            }
        }
        else if (move_counter == -1 && can_move && startup == -1 && promotion == 0)
        {
            if(Input.GetAxis("Map") >= 0.1f && dat.items_obtained[1])
            {
                back.Play(0);
                dat.cur_level = cur_level;
                SceneManager.LoadScene("Map");
            }
            if(Input.GetAxis("Threat")>=0.1f && !threat_cooldown && detection_enabled)
            {
                threat_cooldown = true;
                threat_detecting = !threat_detecting;
                dat.threats_toggled = threat_detecting;
                mutations.ShowThreats(threat_detecting, x_dims, y_dims);
            }
            else if(Input.GetAxis("Threat") == 0)
            {
                threat_cooldown = false;
            }
            if (Input.GetAxis("Reset") >= 0.1 && cur_level >= 4)
            {
                startup = 43;
                for (int k = 0; k < white_pieces.Count; k++)
                {
                    if (white_pieces[k].captured)
                    {
                        Destroy(white_pieces[k].gameObject);
                    }
                }
                for (int k = 0; k < black_pieces.Count; k++)
                {
                    if (black_pieces[k].captured)
                    {
                        Destroy(black_pieces[k].gameObject);
                    }
                }
                if (bking_p.captured)
                {
                    Destroy(bking);
                }
                if (wking_p.captured)
                {
                    Destroy(wking);
                }
                white_selector.SetActive(false);
                black_selector.SetActive(false);
            }
            bool inputted = false;
            x_dir = 0;
            y_dir = 0;
            if(loss)
            {
                if (Input.GetAxis("Undo") >= 0.1f && undocooldown == 0)
                {
                    LoadBoardState();
                    undocooldown = -1;
                }
                else if (Input.GetAxis("Undo") == 0)
                {
                    undocooldown = 0;
                }
                if (Input.GetAxis("Submit") >= 0.1f && cooldown == 0)
                {
                    startup = 43;
                    for (int k = 0; k < white_pieces.Count; k++)
                    {
                        if (white_pieces[k].captured)
                        {
                            Destroy(white_pieces[k].gameObject);
                        }
                    }
                    for (int k = 0; k < black_pieces.Count; k++)
                    {
                        if (black_pieces[k].captured)
                        {
                            Destroy(black_pieces[k].gameObject);
                        }
                    }
                    if (bking_p.captured)
                    {
                        Destroy(bking);
                    }
                    if (wking_p.captured)
                    {
                        Destroy(wking);
                    }
                    white_selector.SetActive(false);
                    black_selector.SetActive(false);
                }
                else if (Input.GetAxis("Submit") == 0)
                {
                    cooldown = 0;
                }
                return;
            }
            int x_store = cur_x;
            int y_store = cur_y;
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
                dir_cooldowns[1] = 0;
                if (dir_cooldowns[0] == 0)
                {
                    //move.Play(0);
                    dir_cooldowns[0] = dat.cooldowns;
                    cur_y++;
                    y_dir = 1;
                    //print(cur_y);
                    inputted = true;
                }
            }
            else if (Input.GetAxis("Vertical") <= -0.1)
            {
                dir_cooldowns[0] = 0;
                if (dir_cooldowns[1] == 0)
                {
                    //move.Play(0);
                    cur_y--;
                    dir_cooldowns[1] = dat.cooldowns;
                    y_dir = -1;
                    //print(cur_y);
                    inputted = true;
                }
            }
            else
            {
                dir_cooldowns[0] = 0; dir_cooldowns[1] = 0;
            }
            if (Input.GetAxis("Horizontal") >= 0.1)
            {
                dir_cooldowns[3] = 0;
                if (dir_cooldowns[2] == 0)
                {
                    //move.Play(0);
                    cur_x++;
                    dir_cooldowns[2] = dat.cooldowns;
                    print("Hell");
                    x_dir = 1;
                    inputted = true;
                    //print(cur_x);
                }
            }
            else if (Input.GetAxis("Horizontal") <= -0.1)
            {
                dir_cooldowns[2] = 0;
                if (dir_cooldowns[3] == 0)
                {
                    //move.Play(0);
                    cur_x--;
                    dir_cooldowns[3] = dat.cooldowns;
                    print("Be");
                    x_dir = -1;
                    inputted = true;
                    //print(cur_x);
                }
            }
            else
            {
                dir_cooldowns[2] = 0; dir_cooldowns[3] = 0;
            }
            if (outOfBounds(cur_piece.x + cur_x, cur_piece.y + cur_y) || (tiles[cur_piece.x + cur_x][cur_piece.y + cur_y].Occupied() && !(cur_x == 0 && cur_y == 0)))
            {
                //cur_x = x_store;
            }
            if (Input.GetAxis("Close") > 0 && cur_piece.type != 5)
            {
                uncom.Play(0);
                p_cooldown = true;
                if (white_turn)
                {
                    cur_piece = wking_p;
                }
                else
                {
                    cur_piece = bking_p;
                }
                cur_x = 0;
                cur_y = 0;
                white_selector.transform.position = cur_piece.transform.position + new Vector3(cur_x, cur_y, 0);
                black_selector.transform.position = cur_piece.transform.position + new Vector3(cur_x, cur_y, 0);
            }
            if (inputted)
            {//HEREEE
                forced_invalid = false;
                bool exceeded = false;
                if (cur_piece.type != 3)
                {
                    if (cur_piece.x + cur_x < 0 || cur_piece.x + cur_x >= x_dims)
                    {
                        cancel.Play(0);
                        cur_x = x_store;
                        exceeded = true;
                    }
                    if (cur_piece.y - cur_y < 0 || cur_piece.y - cur_y >= y_dims)
                    {
                        cancel.Play(0);
                        exceeded = true;
                        cur_y = y_store;
                        print("Ex");
                    }
                }
                checkValidPieceMove(x_store, y_store, exceeded);
                cur_piece.animator.SetFloat("x", cur_x);
                cur_piece.animator.SetFloat("y", cur_y);
                if ((cur_x == 0 && cur_y == 0))
                {
                    print("Invalidated due to  no change");
                    forced_invalid = true;
                }
                else if (invalidTile(cur_piece.x + cur_x, cur_piece.y - cur_y) && !(!outOfBounds(cur_piece.x + cur_x, cur_piece.y - cur_y) && !tiles[cur_piece.x + cur_x][cur_piece.y - cur_y].Occupied()))
                {
                    print("Invalidated due to invalid tile");
                    forced_invalid = true;
                }
                else if ((tiles[cur_x + cur_piece.x][cur_piece.y + -cur_y].Black() && !white_turn) || (tiles[cur_x + cur_piece.x][cur_piece.y + -cur_y].White() && white_turn))
                {
                    print("Invalidated due to friendly tile");
                    forced_invalid = true;
                }
                else
                {
                    //Calculate tile that will be moved to after arrows
                    int t_y = cur_y;
                    int t_x = cur_x;
                    t_x += tiles[cur_piece.x + cur_x][cur_piece.y - cur_y].x_push;
                    t_y += tiles[cur_piece.x + cur_x][cur_piece.y - cur_y].y_push;
                    int txstore = cur_x;
                    int tystore = cur_y;
                    if (tiles[cur_piece.x + cur_x][cur_piece.y - cur_y].Occupied() && tiles[cur_piece.x + cur_x][cur_piece.y - cur_y].White() != white_turn)
                    {
                        tiles[cur_piece.x + cur_x][cur_piece.y - cur_y].occupant.ghosted = true;
                    }
                    print("Huh");
                    while ((!invalidTile(cur_piece.x + t_x, cur_piece.y - t_y) || (!outOfBounds(cur_piece.x + t_x, cur_piece.y - t_y) && tiles[cur_piece.x + t_x][cur_piece.y - t_y].Occupied())) && (tiles[cur_piece.x + t_x][cur_piece.y - t_y].x_push != 0 || tiles[cur_piece.x + t_x][cur_piece.y - t_y].y_push != 0))
                    {
                        if (tiles[cur_piece.x + t_x][cur_piece.y - t_y].Occupied())
                        {
                            print("u");
                            if (tiles[cur_piece.x + t_x][cur_piece.y - t_y].White() == white_turn && tiles[cur_piece.x + t_x][cur_piece.y-t_y].occupant != cur_piece)
                            {
                                break;
                            }
                            else
                            {
                                tiles[cur_piece.x + t_x][cur_piece.y - t_y].occupant.ghosted = true;
                            }
                        }
                        txstore = t_x;
                        tystore = t_y;
                        print("Mu");
                        print(t_x);
                        print(t_y);
                        int tmp = t_x;
                        t_x += tiles[cur_piece.x + t_x][cur_piece.y - t_y].x_push;
                        t_y += tiles[cur_piece.x + tmp][cur_piece.y - t_y].y_push;
                        print("hu");
                        print(t_x);
                        print(t_y);
                    }
                    if (invalidTile(cur_piece.x + t_x, cur_piece.y - t_y) || (!outOfBounds(cur_piece.x + t_x, cur_piece.y - t_y) && tiles[cur_piece.x + t_x][cur_piece.y - t_y].Occupied() && tiles[cur_piece.x + t_x][cur_piece.y - t_y].White() == white_turn && tiles[cur_piece.x + t_x][cur_piece.y - t_y].occupant != cur_piece))
                    {
                        t_x = txstore;
                        t_y = tystore;
                    }
                    print("E");
                    print(t_x);
                    print(t_y);

                    recalculateThreats();

                    //checking if was blocking check
                    //Doesnt deal with captures
                    if (cur_piece.type == 5) //No you aren't blocking the tile you move to from check
                    {
                        cur_piece.ghosted = true;
                    }
                    if (!invalidTile(cur_piece.x + t_x, cur_piece.y - t_y))
                    {
                        print("All that jazz");
                        Piece tem2 = tiles[cur_piece.x + t_x][cur_piece.y - t_y].occupant;
                        if (tem2 != null)
                        {
                            tem2.ghosted = true;
                        }
                        Piece tem = tiles[cur_piece.x][cur_piece.y].occupant;
                        tiles[cur_piece.x][cur_piece.y].occupant = null;
                        tiles[cur_piece.x][cur_piece.y].valid = true;
                        tiles[cur_piece.x + t_x][cur_piece.y - t_y].valid = false;
                        tiles[cur_piece.x + t_x][cur_piece.y - t_y].occupant = tem;
                        int x_st = cur_piece.x;
                        int y_st = cur_piece.y;
                        cur_piece.x = cur_piece.x + t_x;
                        cur_piece.y = cur_piece.y - t_y;
                        int num_store = tiles[cur_piece.x][cur_piece.y].number;
                        //Modify special tiles
                        modifySpecials(false);
                        int total_noughts = 0;
                        int total_crosses = 0;
                        if (tiles[cur_piece.x][cur_piece.y].special == 4)
                        {
                            for (int i = 0; i < x_dims; i++)
                            {
                                for (int j = 0; j < y_dims; j++)
                                {
                                    if (tiles[i][j].special != 5)
                                    {
                                        //tiles[i][j].nought = -tiles[i][j].nought;
                                    }
                                    tiles[i][j].x_push = -tiles[i][j].x_push;
                                    tiles[i][j].y_push = -tiles[i][j].y_push;
                                }
                            }
                        }
                        List<List<int>> numstore = new List<List<int>>();
                        for (int i = 0; i < x_dims; i++)
                        {
                            List<int> subnumstore = new List<int>();
                            for (int j = 0; j < y_dims; j++)
                            {
                                subnumstore.Add(tiles[i][j].number);
                            }
                            numstore.Add(subnumstore);
                        }
                        tiles[cur_piece.x][cur_piece.y].Landed();
                        List<List<int>> ncstore = new List<List<int>>();
                        for (int i = 0; i < x_dims; i++)
                        {
                            List<int> subncstore = new List<int>();
                            for (int j = 0; j < y_dims; j++)
                            {
                                subncstore.Add(tiles[i][j].nought);
                                if (decrement && !tiles[i][j].Occupied())
                                {
                                    tiles[i][j].Decrement();
                                }
                                if (!tiles[i][j].Occupied() || tiles[i][j].occupant == cur_piece)
                                {
                                    if (tiles[i][j].nought == -1 && tiles[i][j].special != 5)
                                    {
                                        total_noughts++;
                                    }
                                    else if (tiles[i][j].nought == 1)
                                    {
                                        total_crosses++;
                                    }
                                }
                            }
                            ncstore.Add(subncstore);
                        }
                        if (total_noughts == 0 || total_crosses == 0)
                        {
                            for (int i = 0; i < x_dims; i++)
                            {
                                for (int j = 0; j < y_dims; j++)
                                {
                                    tiles[i][j].nought = 0;
                                }
                            }
                        }

                        List<int> total_nums = new List<int>();
                        total_nums.Add(0); total_nums.Add(0); total_nums.Add(0); total_nums.Add(0);
                        for (int i = 0; i < x_dims; i++)
                        {
                            for (int j = 0; j < y_dims; j++)
                            {
                                if (tiles[i][j].number != -1)
                                {
                                    total_nums[tiles[i][j].number] = 1;
                                    print(tiles[i][j].number);
                                }
                            }
                        }
                        int sum = 0;
                        for (int i = 0; i < 4; i++)
                        {
                            sum += total_nums[i];
                        }
                        if (sum <= 1)
                        {
                            for (int i = 0; i < x_dims; i++)
                            {
                                for (int j = 0; j < y_dims; j++)
                                {
                                    tiles[i][j].number = -1;
                                }
                            }
                        }

                        recalculateThreats();


                        //Check illegal
                        if ((cur_piece.type != 5) && ((white_turn && tiles[wking_p.x][wking_p.y].black_threat) || (!white_turn && tiles[bking_p.x][bking_p.y].white_threat)))
                        {
                            print("Invalidated due to revealed check");
                            forced_invalid = true;
                        }
                        if (no_check)
                        {
                            cur_piece.ghosted = false;
                            recalculateThreats();
                            if (((!white_turn && tiles[wking_p.x][wking_p.y].black_threat) || (white_turn && tiles[bking_p.x][bking_p.y].white_threat)))
                            {
                                print("WHAT");
                                forced_invalid = true;
                            }
                            recalculateThreats();
                            cur_piece.ghosted = true;
                        }

                        if (cur_piece.type == 5)
                        {
                            if (((tiles[cur_piece.x][cur_piece.y].black_threat && white_turn) || (tiles[cur_piece.x][cur_piece.y].white_threat && !white_turn))&&special_level)
                            {
                                print("Invalidated due to tile threat");
                                print(cur_piece.x);
                                print(cur_piece.y);
                                forced_invalid = true;
                            }
                            else
                            {
                                print("Move is safe");
                                white_selector.GetComponent<SpriteRenderer>().sprite = valid;
                                black_selector.GetComponent<SpriteRenderer>().sprite = valid;
                            }
                        }
                        print(tiles[cur_piece.x][cur_piece.y].nought == -1);
                        if ((tiles[cur_piece.x][cur_piece.y].number == 0 && num_store == 0) || tiles[cur_piece.x][cur_piece.y].nought == -1)
                        {
                            print("Invalidated cus cross or 0 tile");
                            print(cur_piece.x);
                            print(cur_piece.y);
                            print(tiles[cur_piece.x][cur_piece.y].number == 0);
                            print(tiles[cur_piece.x][cur_piece.y].nought == -1);
                            forced_invalid = true;
                        }
                        //revert
                        if (tiles[cur_piece.x][cur_piece.y].special == 4)
                        {
                            for (int i = 0; i < x_dims; i++)
                            {
                                for (int j = 0; j < y_dims; j++)
                                {
                                    //tiles[i][j].nought = -tiles[i][j].nought;
                                    tiles[i][j].x_push = -tiles[i][j].x_push;
                                    tiles[i][j].y_push = -tiles[i][j].y_push;
                                }
                            }
                        }
                        cur_piece.x = x_st;
                        cur_piece.y = y_st;

                        tiles[cur_piece.x + t_x][cur_piece.y - t_y].occupant = tem2;
                        if (tem2 == null)
                        {
                            tiles[cur_piece.x + t_x][cur_piece.y - t_y].valid = true;
                        }
                        else
                        {
                            tiles[cur_piece.x + t_x][cur_piece.y - t_y].valid = false;
                        }
                        tiles[cur_piece.x][cur_piece.y].valid = false;
                        tiles[cur_piece.x][cur_piece.y].occupant = tem;
                        for (int i = 0; i < x_dims; i++)
                        {
                            for (int j = 0; j < y_dims; j++)
                            {
                                tiles[i][j].number = numstore[i][j];
                                tiles[i][j].nought = ncstore[i][j];
                            }
                        }
                    }



                    for (int i = 0; i < black_pieces.Count; i++)
                    {
                        black_pieces[i].ghosted = false;
                    }
                    for (int i = 0; i < white_pieces.Count; i++)
                    {
                        white_pieces[i].ghosted = false;
                    }
                    recalculateThreats();

                    //Finished checking for revealed pos

                    modifySpecials(false);
                    //print(invalidTile(cur_x + cur_piece.x, -cur_y + cur_piece.y));
                }

                if (forced_invalid)
                {
                    white_selector.GetComponent<SpriteRenderer>().sprite = invalid;
                    black_selector.GetComponent<SpriteRenderer>().sprite = invalid;
                }
                else
                {
                    white_selector.GetComponent<SpriteRenderer>().sprite = valid;
                    black_selector.GetComponent<SpriteRenderer>().sprite = valid;
                }
                white_selector.transform.position = cur_piece.transform.position + new Vector3(cur_x, cur_y, 0);
                black_selector.transform.position = cur_piece.transform.position + new Vector3(cur_x, cur_y, 0);

                king_command = false;
                interact = false;
                //print("hug");
                //print(cur_piece.type == 5);
                //print(!(cur_x == 0 && cur_y == 0));
                //print((tiles[cur_x + cur_piece.x][-cur_y + cur_piece.y].Black() && !white_turn));
                if ((cur_x == 0 && cur_y == 0))
                {
                    print("Invalidated due to  no change");
                    forced_invalid = true;
                    white_selector.GetComponent<SpriteRenderer>().sprite = invalid;
                    black_selector.GetComponent<SpriteRenderer>().sprite = invalid;
                }
                else if (invalidTile(cur_piece.x + cur_x, cur_piece.y - cur_y) && (outOfBounds(cur_piece.x + cur_x, cur_piece.y - cur_y) || !tiles[cur_piece.x + cur_x][cur_piece.y - cur_y].Occupied()))
                {
                    print("Invalidated due to invalid tile");
                    forced_invalid = true;
                    white_selector.GetComponent<SpriteRenderer>().sprite = invalid;
                    black_selector.GetComponent<SpriteRenderer>().sprite = invalid;
                }
                else
                {
                    if (control)
                    {
                        if (cur_piece.type == 5 && !invalidTile(cur_x + cur_piece.x, -cur_y + cur_piece.y) && !(cur_x == 0 && cur_y == 0) && ((tiles[cur_x + cur_piece.x][-cur_y + cur_piece.y].Black() && !white_turn) || (tiles[cur_x + cur_piece.x][-cur_y + cur_piece.y].White() && white_turn)))
                        {
                            print("Commanding");
                            king_command = true;
                            white_selector.GetComponent<SpriteRenderer>().sprite = valid;
                            black_selector.GetComponent<SpriteRenderer>().sprite = valid;
                        }
                    }
                    if (cur_piece.type == 5 && tiles[cur_x + cur_piece.x][-cur_y + cur_piece.y].Occupied() && tiles[cur_x + cur_piece.x][-cur_y + cur_piece.y].occupant.interactable && (tiles[cur_x + cur_piece.x][-cur_y + cur_piece.y].occupant.uncapturable || (tiles[cur_x + cur_piece.x][-cur_y + cur_piece.y].Black() && !white_turn) || (tiles[cur_x + cur_piece.x][-cur_y + cur_piece.y].White() && white_turn)))
                    {
                        print("Int");
                        interact = true;
                        white_selector.GetComponent<SpriteRenderer>().sprite = valid;
                        black_selector.GetComponent<SpriteRenderer>().sprite = valid;
                    }
                }
                once = false;
                for (int i = 0; i < oneoffs.Count; i++)
                {
                    if (cur_x + cur_piece.x == oneoffs[i].x && -cur_y + cur_piece.y == oneoffs[i].y)
                    {
                        once = true;
                    }
                }
                bking_p.ghosted = false;
                if (wking_p != null)
                {
                    wking_p.ghosted = false;
                }
                ///bking_anim.SetFloat("x_dir", x_dir);
                //bking_anim.SetFloat("y_dir", y_dir);
            }

            if ((cur_x != 0 || cur_y != 0) && cur_piece.type == 3)
            {
                x_dir = 2 * cur_x / Mathf.Max(Mathf.Abs(cur_x), Mathf.Abs(cur_y));
                y_dir = 2 * cur_y / Mathf.Max(Mathf.Abs(cur_x), Mathf.Abs(cur_y));
            }
            else
            {
                x_dir = cur_x;
                y_dir = cur_y;
            }
            if (cur_piece.type != 4)
            {
                cur_piece.x_dir = cur_x;
                cur_piece.y_dir = cur_y;
            }
            else if (cur_y == 0 && cur_x == 0)
            {
                cur_piece.animator.SetFloat("x", cur_piece.x_dir);
                cur_piece.animator.SetFloat("y", cur_piece.y_dir);
            }



            if (Input.GetAxis("Submit") > 0 && cooldown == 0)
            {
                if(once)
                {
                    for (int i = 0; i < oneoffs.Count; i++)
                    {
                        if (cur_x + cur_piece.x == oneoffs[i].x && -cur_y + cur_piece.y == oneoffs[i].y)
                        {
                            if (oneoffs[i].flipper)
                            {
                                for (int j = 0; j < x_dims; j++)
                                {
                                    for (int k = 0; k < y_dims; k++)
                                    {
                                        tiles[j][k].nought = -tiles[j][k].nought;
                                    }
                                }
                                mutations.Refresh(x_dims, y_dims);
                                oneoffs[i].flipper = false;
                            }
                            if (oneoffs[i].store != null)
                            {
                                tbox.startStrings = oneoffs[i].store.Activate();
                                tbox.gameObject.SetActive(true);
                                text = true;
                            }
                            oneoffs[i].Activate();
                        }
                        
                    }
                }
                else if(interact)
                {
                    tbox.startStrings = tiles[cur_piece.x + cur_x][cur_piece.y - cur_y].occupant.text.Activate();
                    tbox.gameObject.SetActive(true);
                    cooldown = -1;
                    p_cooldown = true;
                    text = true;
                }
                else if (king_command)
                {
                    com.Play(0);
                    if (white_turn)
                    {
                        for (int i = 0; i < white_pieces.Count; i++)
                        {
                            if (white_pieces[i].x == cur_x + cur_piece.x && white_pieces[i].y == -cur_y + cur_piece.y && !white_pieces[i].captured)
                            {
                                cur_piece = white_pieces[i];
                                break;
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < black_pieces.Count; i++)
                        {
                            if (black_pieces[i].x == cur_x + cur_piece.x && black_pieces[i].y == -cur_y + cur_piece.y && !black_pieces[i].captured)
                            {
                                cur_piece = black_pieces[i];
                                break;
                            }
                        }
                    }
                    cur_x = 0;
                    cur_y = 0;
                    white_selector.transform.position = cur_piece.transform.position + new Vector3(cur_x, cur_y, 0);
                    black_selector.transform.position = cur_piece.transform.position + new Vector3(cur_x, cur_y, 0);
                    white_selector.GetComponent<SpriteRenderer>().sprite = invalid;
                    black_selector.GetComponent<SpriteRenderer>().sprite = invalid;
                    cooldown = dat.cooldowns;
                    king_command = false;
                }
                else if (forced_invalid)
                {
                    cancel.Play(0);
                }
                else if (cur_x == 0 && cur_y == 0)
                {

                }
                else
                {
                    SaveBoardState();
                    wking_has_moved_for_intro = false;
                    total_turns++;
                    if (tiles[cur_piece.x + cur_x][cur_piece.y - cur_y].occupant != null)
                    {
                        asc_piece = tiles[cur_piece.x + cur_x][cur_piece.y - cur_y].occupant.transform;
                        ascend_count = 18;
                        tiles[cur_piece.x + cur_x][cur_piece.y - cur_y].occupant.captured = true;
                        //white_pieces.Remove(tiles[cur_piece.x + cur_x][cur_piece.y - cur_y].occupant);
                        //black_pieces.Remove(tiles[cur_piece.x + cur_x][cur_piece.y - cur_y].occupant);

                        //Destroy(tiles[cur_piece.x + cur_x][cur_piece.y - cur_y].occupant);
                    }
                    if (cur_piece.type == 4 && cur_x != 0 && cur_y != 0)
                    {
                        if (cur_piece.x + cur_x == passant_x && cur_piece.y - cur_y == passant_y)
                        {
                            asc_piece = passant_piece.transform;
                            ascend_count = 18;
                            passant_piece.captured = true;
                            print("WHAT");
                            print(passant_x);
                            print(passant_y);
                            tiles[passant_piece.x][passant_piece.y].occupant = null;
                            tiles[passant_piece.x][passant_piece.y].valid = true;
                        }
                    }
                    cur_piece.has_moved = true;
                    if (cur_piece.type == 4 && (Mathf.Abs(x_dir) == 2 || Mathf.Abs(y_dir) == 2))
                    {
                        print("Passant me");
                        passant_piece = cur_piece;
                        passant_x = cur_piece.x + cur_x / 2;
                        passant_y = cur_piece.y - cur_y / 2;
                    }
                    else
                    {
                        passant_piece = null;
                        passant_x = 99;
                        passant_y = 99;
                    }
                    tiles[cur_piece.x][cur_piece.y].valid = true;
                    Piece temp = tiles[cur_piece.x][cur_piece.y].occupant;
                    tiles[cur_piece.x][cur_piece.y].occupant = null;
                    mutations.Refresh(x_dims, y_dims);
                    cur_piece.x += cur_x;
                    cur_piece.y -= cur_y;
                    tiles[cur_piece.x][cur_piece.y].valid = false;
                    tiles[cur_piece.x][cur_piece.y].occupant = temp;
                    scrape.Play(0);
                    can_move = false;
                    move_length = Mathf.Max(Mathf.Abs(cur_x), Mathf.Abs(cur_y));
                    move_length = 1;
                    move_target = cur_piece.gameObject;
                    //piece_selected = 0;
                    piece_number_selected = 1;
                    move_counter = 0;
                    recalculateThreats();
                    cur_x = 0;
                    cur_y = 0;
                    white_selector.SetActive(false);
                    black_selector.SetActive(false);
                }
                if (!outOfBounds(bking_p.x + x_dir, bking_p.y - y_dir) && tiles[bking_p.x + x_dir][bking_p.y - y_dir].inter != null)
                {
                    /*interactables[bking_p.x + x_dir][bking_p.y - y_dir].Trigger();//Change how out of bounds works
                    print("Hi hi h ih !");
                    if (!alone)
                    {
                        white_turn = !white_turn;
                    }
                    */
                }
            }
            else if (Input.GetAxis("Submit") == 0)
            {
                cooldown = 0;

                if(Input.GetAxis("Undo") >= 0.1f && undocooldown == 0)
                {
                    LoadBoardState();
                    undocooldown = -1;
                }
                else if (Input.GetAxis("Undo") == 0)
                {
                    undocooldown = 0;
                }
            }
        }
    }

    public void recalculateThreats()
    {

        for (int i = 0; i < x_dims; i++)
        {
            for (int j = 0; j < y_dims; j++)
            {
                tiles[i][j].white_threat = false;
                tiles[i][j].black_threat = false;
                tiles[i][j].white_threatener = null;
                tiles[i][j].black_threatener = null;
                tiles[i][j].white_threat_source = null;
                tiles[i][j].black_threat_source = null;
            }
        }
        if (passant_x != 99)
        {

            tiles[passant_x][passant_y].passing = true;
        }
        //print("King pos");
        //print(wking_p.x + "," + wking_p.y);
        for (int xinc = -1; xinc < 2; xinc++)
        {
            for (int yinc = -1; yinc < 2; yinc++)
            {
                if (xinc != 0 || yinc != 0)
                {
                    if (bking != null)
                    {


                        setTileThreat(bking_p.x + xinc, bking_p.y + yinc, false,bking_p);
                    }
                    if (wking != null)
                    {
                        setTileThreat(wking_p.x + xinc, wking_p.y + yinc, true,wking_p);
                    }
                }
            }
        }
        Queue<Piece> wpqueue = new Queue<Piece>();
        for (int i = 0; i < white_pieces.Count; i++)
        {
            if (white_pieces[i].ghosted || white_pieces[i].captured)
            {
                continue;
            }
            int x = white_pieces[i].x;
            int y = white_pieces[i].y;
            switch (white_pieces[i].type)
            {
                case 0:
                    for (int xinc = -1; xinc < 2; xinc++)
                    {
                        for (int yinc = -1; yinc < 2; yinc++)
                        {
                            int step = 1;
                            if (xinc != 0 || yinc != 0)
                            {
                                while (true)
                                {

                                    if (invalidTile(x + xinc * step, y + yinc * step))
                                    {
                                        break;
                                    }
                                    setTileThreat(x + xinc * step, y + yinc * step, true,white_pieces[i]);
                                    if (tiles[x + xinc * step][y + yinc * step].occupant != null && !tiles[x + xinc * step][y + yinc * step].occupant.ghosted)
                                    {
                                        break;
                                    }
                                    step++;
                                }
                            }
                        }
                    }
                    break;
                case 1:
                    for (int inc = -1; inc < 2; inc += 2)
                    {
                        int step = 1;
                        while (true)
                        {
                            //print("x = " + x + inc * step);
                            if (invalidTile(x + inc * step, y))
                            {
                                break;
                            }
                            setTileThreat(x + inc * step, y, true, white_pieces[i]);
                            if (tiles[x + inc * step][y].occupant != null && !tiles[x + inc * step][y].occupant.ghosted)
                            {
                                break;
                            }
                            step++;
                        }
                    }
                    for (int inc = -1; inc < 2; inc += 2)
                    {
                        int step = 1;
                        while (true)
                        {
                            if (invalidTile(x, y + inc * step))
                            {
                                break;
                            }
                            setTileThreat(x, y + inc * step, true, white_pieces[i]);
                            if (tiles[x][y + inc * step].occupant != null && !tiles[x][y + inc * step].occupant.ghosted)
                            {
                                break;
                            }
                            step++;
                        }
                    }
                    break;
                case 2:
                    for (int xinc = -1; xinc < 2; xinc += 2)
                    {
                        for (int yinc = -1; yinc < 2; yinc += 2)
                        {
                            int step = 1;
                            while (true)
                            {
                                if (invalidTile(x + xinc * step, y + yinc * step))
                                {
                                    break;
                                }
                                setTileThreat(x + xinc * step, y + yinc * step, true, white_pieces[i]);
                                if (tiles[x + xinc * step][y + yinc * step].occupant != null)
                                {
                                    break;
                                }
                                step++;
                            }
                        }
                    }
                    break;
                case 3:
                    setTileThreat(x + 2, y + 1, true, white_pieces[i]);
                    setTileThreat(x + 2, y - 1, true, white_pieces[i]);
                    setTileThreat(x - 2, y + 1, true, white_pieces[i]);
                    setTileThreat(x - 2, y - 1, true, white_pieces[i]);
                    setTileThreat(x + 1, y + 2, true, white_pieces[i]);
                    setTileThreat(x - 1, y + 2, true, white_pieces[i]);
                    setTileThreat(x + 1, y - 2, true, white_pieces[i]);
                    setTileThreat(x - 1, y - 2, true, white_pieces[i]);
                    break;
                case 4:
                    wpqueue.Enqueue(white_pieces[i]);
                    if (white_pieces[i].x_dir > 0)
                    {
                        if (!invalidTile(x + 1, y) && (tiles[x + 1][y].x_push != 0 || tiles[x + 1][y].y_push != 0))
                        {
                            setTileThreat(x + 1 + tiles[x + 1][y].x_push, y - tiles[x + 1][y].y_push, true, white_pieces[i], x+1, y);
                        }
                        if (!invalidTile(x + 1, y + 1) && tiles[x + 1][y + 1].Occupied(true) && tiles[x + 1][y + 1].Black())
                        {
                            setTileThreat(x + 1, y + 1, true, white_pieces[i]);
                        }
                        if (!invalidTile(x + 1, y - 1) && tiles[x + 1][y - 1].Occupied(true) && tiles[x + 1][y - 1].Black())
                        {
                            setTileThreat(x + 1, y - 1, true, white_pieces[i]);
                        }
                    }
                    else if (white_pieces[i].x_dir < 0)
                    {
                        if (!invalidTile(x - 1, y) && (tiles[x - 1][y].x_push != 0 || tiles[x - 1][y].y_push != 0))
                        {
                            setTileThreat(x - 1 + tiles[x - 1][y].x_push, y - tiles[x - 1][y].y_push, true, white_pieces[i], x -1, y);
                        }
                        if (!invalidTile(x - 1, y + 1) && tiles[x - 1][y + 1].Occupied(true) && tiles[x - 1][y + 1].Black())
                        {
                            setTileThreat(x - 1, y + 1, true, white_pieces[i]);
                        }
                        if (!invalidTile(x - 1, y - 1) && tiles[x - 1][y - 1].Occupied(true) && tiles[x - 1][y - 1].Black())
                        {
                            setTileThreat(x - 1, y - 1, true, white_pieces[i]);
                        }
                    }
                    else if (white_pieces[i].y_dir < 0)
                    {

                        if (!invalidTile(x, y + 1) && (tiles[x][y + 1].x_push != 0 || tiles[x][y + 1].y_push != 0))
                        {
                            setTileThreat(x + tiles[x][y + 1].x_push, y + 1 - tiles[x][y + 1].y_push, true, white_pieces[i], x, y + 1);
                        }
                        if (!invalidTile(x + 1, y + 1) && tiles[x + 1][y + 1].Occupied(true) && tiles[x + 1][y + 1].Black())
                        {
                            setTileThreat(x + 1, y + 1, true, white_pieces[i]);
                        }
                        if (!invalidTile(x - 1, y + 1) && tiles[x - 1][y + 1].Occupied(true) && tiles[x - 1][y + 1].Black())
                        {
                            setTileThreat(x - 1, y + 1, true, white_pieces[i]);
                        }
                    }
                    else
                    {
                        print("HELLo");
                        print(y -1);
                        if (!invalidTile(x, y - 1) && (tiles[x][y - 1].x_push != 0 || tiles[x][y - 1].y_push != 0))
                        {
                            setTileThreat(x + tiles[x][y - 1].x_push, y - 1 - tiles[x][y - 1].y_push, true, white_pieces[i], x, y - 1);
                        }
                        if (!invalidTile(x - 1, y - 1) && tiles[x - 1][y - 1].Occupied(true) && tiles[x - 1][y - 1].Black())
                        {
                            setTileThreat(x - 1, y - 1, true, white_pieces[i]);
                        }
                        if (!invalidTile(x + 1, y - 1) && tiles[x + 1][y - 1].Occupied(true) && tiles[x + 1][y - 1].Black())
                        {
                            setTileThreat(x + 1, y - 1, true, white_pieces[i]);
                        }
                    }
                    break;
            }
        }
        Queue<Piece> bpqueue = new Queue<Piece>();
        for (int i = 0; i < black_pieces.Count; i++)
        {
            if (black_pieces[i].ghosted || black_pieces[i].captured)
            {
                continue;
            }
            int x = black_pieces[i].x;
            int y = black_pieces[i].y;

            switch (black_pieces[i].type)
            {
                case 0:
                    for (int xinc = -1; xinc < 2; xinc++)
                    {
                        for (int yinc = -1; yinc < 2; yinc++)
                        {
                            int step = 1;
                            if (xinc != 0 || yinc != 0)
                            {
                                while (true)
                                {
                                    if (invalidTile(x + xinc * step, y + yinc * step))
                                    {
                                        break;
                                    }
                                    setTileThreat(x + xinc * step, y + yinc * step, false,black_pieces[i]);
                                    if (tiles[x + xinc * step][y + yinc * step].occupant != null && !tiles[x + xinc * step][y + yinc * step].occupant.ghosted)
                                    {
                                        break;
                                    }
                                    step++;
                                }
                            }
                        }
                    }
                    break;
                case 1:
                    for (int inc = -1; inc < 2; inc += 2)
                    {
                        int step = 1;
                        while (true)
                        {
                            if (invalidTile(x + inc * step, y))
                            {
                                break;
                            }
                            setTileThreat(x + inc * step, y, false, black_pieces[i]);
                            
                            if (tiles[x + inc * step][y].occupant != null && !tiles[x + inc * step][y].occupant.ghosted)
                            {
                                break;
                            }
                            step++;
                        }
                    }
                    for (int inc = -1; inc < 2; inc += 2)
                    {
                        int step = 1;
                        while (true)
                        {
                            if (invalidTile(x, y + inc * step))
                            {
                                break;
                            }
                            setTileThreat(x, y + inc * step, false, black_pieces[i]);
                            if (tiles[x][y + inc * step].occupant != null && !tiles[x][y + inc * step].occupant.ghosted)
                            {
                                break;
                            }
                            step++;
                        }
                    }
                    break;
                case 2:

                    for (int xinc = -1; xinc < 2; xinc += 2)
                    {
                        for (int yinc = -1; yinc < 2; yinc += 2)
                        {
                            int step = 1;
                            while (true)
                            {
                                if (invalidTile(x + xinc * step, y + yinc * step))
                                {
                                    break;
                                }
                                setTileThreat(x + xinc * step, y + yinc * step, false, black_pieces[i]);
                                if (tiles[x + xinc * step][y + yinc * step].occupant != null)
                                {
                                    break;
                                }
                                step++;
                            }
                        }
                    }
                    break;
                case 3:
                    setTileThreat(x + 2, y + 1, false, black_pieces[i]);
                    setTileThreat(x + 2, y - 1, false, black_pieces[i]);
                    setTileThreat(x - 2, y + 1, false, black_pieces[i]);
                    setTileThreat(x - 2, y - 1, false, black_pieces[i]);
                    setTileThreat(x + 1, y + 2, false, black_pieces[i]);
                    setTileThreat(x - 1, y + 2, false, black_pieces[i]);
                    setTileThreat(x + 1, y - 2, false, black_pieces[i]);
                    setTileThreat(x - 1, y - 2, false, black_pieces[i]);
                    break;
                case 4:
                    bpqueue.Enqueue(black_pieces[i]);
                    if (black_pieces[i].x_dir > 0)
                    {
                        if(!invalidTile(x+1,y) && (tiles[x+1][y].x_push != 0 || tiles[x + 1][y].y_push != 0))
                        {
                            setTileThreat(x + 1 + tiles[x + 1][y].x_push,  y - tiles[x + 1][y].y_push, false, black_pieces[i], x+1, y);
                        }
                        if (!invalidTile(x + 1, y + 1) && tiles[x + 1][y + 1].Occupied(true) && tiles[x + 1][y + 1].White())
                        {
                            setTileThreat(x + 1, y + 1, false, black_pieces[i]);
                        }
                        if (!invalidTile(x + 1, y - 1) && tiles[x + 1][y - 1].Occupied(true) && tiles[x + 1][y - 1].White())
                        {
                            setTileThreat(x + 1, y - 1, false, black_pieces[i]);
                        }
                    }
                    else if (black_pieces[i].x_dir < 0)
                    {
                        if (!invalidTile(x - 1, y) && (tiles[x - 1][y].x_push != 0 || tiles[x - 1][y].y_push != 0))
                        {
                            setTileThreat(x - 1 + tiles[x - 1][y].x_push, y - tiles[x - 1][y].y_push, false, black_pieces[i], x-1, y);
                        }
                        if (!invalidTile(x - 1, y + 1) && tiles[x - 1][y + 1].Occupied(true) && tiles[x - 1][y + 1].White())
                        {
                            setTileThreat(x - 1, y + 1, false, black_pieces[i]);
                        }
                        if (!invalidTile(x - 1, y - 1) && tiles[x - 1][y - 1].Occupied(true) && tiles[x - 1][y - 1].White())
                        {
                            setTileThreat(x - 1, y - 1, false, black_pieces[i]);
                        }
                    }
                    else if (black_pieces[i].y_dir < 0)
                    {
                        if (!invalidTile(x, y+1) && (tiles[x][y+1].x_push != 0 || tiles[x][y + 1].y_push != 0))
                        {
                            setTileThreat(x + tiles[x][y+1].x_push, y+1 - tiles[x][y + 1].y_push, false, black_pieces[i], x, y + 1);
                        }
                        if (!invalidTile(x + 1, y + 1) && tiles[x + 1][y + 1].Occupied(true) && tiles[x + 1][y + 1].White())
                        {
                            setTileThreat(x + 1, y + 1, false, black_pieces[i]);
                        }
                        if (!invalidTile(x - 1, y + 1) && tiles[x - 1][y + 1].Occupied(true) && tiles[x - 1][y + 1].White())
                        {
                            setTileThreat(x - 1, y + 1, false, black_pieces[i]);
                        }
                    }
                    else
                    {
                        if (!invalidTile(x, y - 1) && (tiles[x][y - 1].x_push != 0 || tiles[x][y - 1].y_push != 0))
                        {
                            setTileThreat(x + tiles[x][y - 1].x_push, y - 1 - tiles[x][y - 1].y_push, false, black_pieces[i], x,y-1);
                        }
                        if (!invalidTile(x - 1, y - 1) && tiles[x - 1][y - 1].Occupied(true) && tiles[x - 1][y - 1].White())
                        {
                            setTileThreat(x - 1, y - 1, false, black_pieces[i]);
                        }
                        if (!invalidTile(x + 1, y - 1) && tiles[x + 1][y - 1].Occupied(true) && tiles[x + 1][y - 1].White())
                        {
                            setTileThreat(x + 1, y - 1, false, black_pieces[i]);
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        Queue<Tile> t_queue = new Queue<Tile>();
        for (int i = 0; i < x_dims; i++)
        {
            for (int j = 0; j < y_dims; j++)
            {
                t_queue.Enqueue(tiles[i][j]);
            }
        }
        while (t_queue.Count > 0)
        {
            Tile t = t_queue.Dequeue();
            if (t.x_push != 0 || t.y_push != 0)
            {
                bool changed = false;
                if(invalidTile(t.x + t.x_push,t.y - t.y_push) && !(!outOfBounds(t.x + t.x_push, t.y - t.y_push) && tiles[t.x + t.x_push][t.y - t.y_push].Occupied()))
                {
                    continue;
                }
                if (t.black_threat && !tiles[t.x + t.x_push][t.y - t.y_push].black_threat)
                {
                    print(t.x + t.x_push);
                    print(t.y - t.y_push);

                    if (!(t.Occupied() && t.Black()))
                    {
                        tiles[t.x + t.x_push][t.y - t.y_push].black_threat = true;
                        setTileThreat(t.x + t.x_push, t.y - t.y_push, false, t.black_threat_source,t.x,t.y);
                        changed = true;
                    }
                }
                if (t.white_threat && !tiles[t.x + t.x_push][t.y - t.y_push].white_threat)
                {
                    
                    if (!(t.Occupied() && t.White()))
                    {
                        setTileThreat(t.x + t.x_push, t.y - t.y_push, true, t.white_threat_source, t.x, t.y);
                        changed = true;
                    }
                }
                if (changed)
                {
                    t_queue.Enqueue(tiles[t.x + t.x_push][t.y - t.y_push]);
                }
            }
        }
        while (bpqueue.Count != 0)
        {
            Piece pawn = bpqueue.Dequeue();
            int x = pawn.x;
            int y = pawn.y;
            if (pawn.x_dir > 0)
            {
                setTileThreat(x + 1, y - 1, false,pawn);
                setTileThreat(x + 1, y + 1, false, pawn);
            }
            else if (pawn.x_dir < 0)
            {
                setTileThreat(x - 1, y + 1, false, pawn);
                setTileThreat(x - 1, y - 1, false, pawn);
            }
            else if (pawn.y_dir < 0)
            {
                setTileThreat(x + 1, y + 1, false, pawn);
                setTileThreat(x - 1, y + 1, false, pawn);
            }
            else
            {
                setTileThreat(x + 1, y - 1, false, pawn);
                setTileThreat(x - 1, y - 1, false, pawn);
            }
        }
        while (wpqueue.Count != 0)
        {
            Piece pawn = wpqueue.Dequeue();
            int x = pawn.x;
            int y = pawn.y;
            if (pawn.x_dir > 0)
            {
                setTileThreat(x + 1, y - 1, true, pawn);
                setTileThreat(x + 1, y + 1, true, pawn);
            }
            else if (pawn.x_dir < 0)
            {
                setTileThreat(x - 1, y + 1, true, pawn);
                setTileThreat(x - 1, y - 1, true, pawn);
            }
            else if (pawn.y_dir < 0)
            {
                setTileThreat(x + 1, y + 1, true, pawn);
                setTileThreat(x - 1, y + 1, true, pawn);
            }
            else
            {
                setTileThreat(x + 1, y - 1, true, pawn);
                setTileThreat(x - 1, y - 1, true, pawn);
            }
        }
        mutations.ShowThreats(threat_detecting, x_dims, y_dims);
        for(int i = 0;i < x_dims;i++)
        {
            for (int j = 0; j < y_dims; j++)
            {
                if(tiles[i][j].nought == -1 || tiles[i][j].number == 0)
                {
                    tiles[i][j].black_threat = false;
                    tiles[i][j].white_threat = false;
                }
            }
        }
        if (passant_x != 99)
        {

            tiles[passant_x][passant_y].passing = false;
        }
    }

    private void setTileThreat(int x, int y, bool white, Piece source)
    {
        if (!invalidTile(x, y))
        {
            if (white)
            {
                tiles[x][y].white_threat = true;
                tiles[x][y].white_threatener = source;
                tiles[x][y].white_threat_source = tiles[x][y];
            }
            else
            {
                tiles[x][y].black_threat = true;
                tiles[x][y].black_threatener = source;
                tiles[x][y].black_threat_source = tiles[x][y];
            }
        }
    }

    private void setTileThreat(int x, int y, bool white, Tile source, int source_x, int source_y)
    {
        if (!invalidTile(x, y))
        {
            if (white)
            {
                tiles[x][y].white_threat = true;
                tiles[x][y].white_threatener = source.white_threatener;
                tiles[x][y].white_threat_source = tiles[source_x][source_y].white_threat_source;
            }
            else
            {
                tiles[x][y].black_threat = true;
                tiles[x][y].black_threatener = source.black_threatener;
                tiles[x][y].black_threat_source = tiles[source_x][source_y].black_threat_source;
            }
        }
    }
    private void setTileThreat(int x, int y, bool white, Piece source, int source_x, int source_y)
    {
        if (!invalidTile(x, y))
        {
            if (white)
            {
                tiles[x][y].white_threat = true;
                tiles[x][y].white_threatener = source;
                tiles[x][y].white_threat_source = tiles[source_x][source_y];
            }
            else
            {
                tiles[x][y].black_threat = true;
                tiles[x][y].black_threatener = source;
                tiles[x][y].black_threat_source = tiles[source_x][source_y];
            }
        }
    }

    private bool invalidTile(int x, int y)
    {
        return (x < 0 || x >= x_dims || y < 0 || y >= y_dims || (!tiles[x][y].valid && !tiles[x][y].Occupied()));
    }

    private bool outOfBounds(int x, int y)
    {
        return (x < 0 || x >= x_dims || y < 0 || y >= y_dims);
    }

    private void calculateThreat(bool white)
    {

    }

    private void modifySpecials(bool real)
    {
        decrement = false;
        adjacent_noughts = false;
        no_check = false;
        for (int i = 0; i < x_dims; i++)
        {
            for (int j = 0; j < y_dims; j++)
            {
                if (tiles[i][j].special == 1 && !tiles[i][j].Occupied())
                {
                    decrement = true;
                }
                else if (tiles[i][j].special == 3 && !tiles[i][j].Occupied())
                {
                    adjacent_noughts = true;
                }
                else if (tiles[i][j].special == 0 && !tiles[i][j].Occupied())
                {
                    no_check = true;
                }
            }
        }

        mutations.Refresh(x_dims, y_dims);
    }

    private void checkFinish()
    {
        bool escape = true;
        bool can_escape = false;
        recalculateThreats();
        if(((bking_p != null && white_turn && tiles[bking_p.x][bking_p.y].white_threat) ||(wking_p != null && !white_turn &&tiles[wking_p.x][wking_p.y].black_threat)) && !loss)
        {
            if(white_turn)
            {
                cur_piece = tiles[bking_p.x][bking_p.y].white_threatener;
                cur_x = tiles[bking_p.x][bking_p.y].white_threat_source.x - cur_piece.x;
                cur_y = -tiles[bking_p.x][bking_p.y].white_threat_source.y + cur_piece.y;
            }
            else
            {
                cur_piece = tiles[wking_p.x][wking_p.y].black_threatener;
                cur_x = tiles[wking_p.x][wking_p.y].black_threat_source.x - cur_piece.x;
                cur_y = -tiles[wking_p.x][wking_p.y].black_threat_source.y + cur_piece.y;
            }
            print("Hm");
            print(cur_x);
            print(cur_y);
            if (tiles[cur_piece.x + cur_x][cur_piece.y - cur_y].occupant != null)
            {
                asc_piece = tiles[cur_piece.x + cur_x][cur_piece.y - cur_y].occupant.transform;
                ascend_count = 18;
                tiles[cur_piece.x + cur_x][cur_piece.y - cur_y].occupant.captured = true;
                //white_pieces.Remove(tiles[cur_piece.x + cur_x][cur_piece.y - cur_y].occupant);
                //black_pieces.Remove(tiles[cur_piece.x + cur_x][cur_piece.y - cur_y].occupant);

                //Destroy(tiles[cur_piece.x + cur_x][cur_piece.y - cur_y].occupant);
            }
            if (cur_piece.type == 4 && cur_x != 0 && cur_y != 0)
            {
                if (cur_piece.x + cur_x == passant_x && cur_piece.y - cur_y == passant_y)
                {
                    asc_piece = passant_piece.transform;
                    ascend_count = 18;
                    passant_piece.captured = true;
                    print("WHAT");
                    print(passant_x);
                    print(passant_y);
                    tiles[passant_piece.x][passant_piece.y].occupant = null;
                    tiles[passant_piece.x][passant_piece.y].valid = true;
                }
            }
            tiles[cur_piece.x][cur_piece.y].valid = true;
            Piece temp = tiles[cur_piece.x][cur_piece.y].occupant;
            tiles[cur_piece.x][cur_piece.y].occupant = null;
            mutations.Refresh(x_dims, y_dims);
            cur_piece.x += cur_x;
            cur_piece.y -= cur_y;
            tiles[cur_piece.x][cur_piece.y].valid = false;
            tiles[cur_piece.x][cur_piece.y].occupant = temp;
            scrape.Play(0);
            can_move = false;
            move_length = Mathf.Max(Mathf.Abs(cur_x), Mathf.Abs(cur_y));
            move_length = 1;
            move_target = cur_piece.gameObject;
            //piece_selected = 0;
            piece_number_selected = 1;
            move_counter = 0;
            cur_piece.x_dir = cur_x;
            cur_piece.y_dir = cur_y;
            x_dir = cur_x;
            y_dir = cur_y;
            print("e");
            print(cur_x);
            print(cur_y);
            loss = true;
        }

        if(loss)
        {

            return;
        }

        for (int i = 0; i < x_dims; i++)
        {
            for (int j = 0; j < y_dims; j++)
            {
                if (tiles[i][j].king_exit)
                {
                    can_escape = true;
                }
                if ((tiles[i][j].king_exit) && (!tiles[i][j].Occupied() || tiles[i][j].occupant.type != 5))
                {
                    escape = false;
                }
                if ((tiles[i][j].arrow_exit >= 0) && (tiles[i][j].Occupied() && tiles[i][j].occupant.type == 5))
                {
                    escape = true;
                    can_escape = true;
                    print("BASTARD");
                    exit_dir = tiles[i][j].arrow_exit;
                    i = 20;
                    break;
                }
            }
        }
        for (int i = 0; i < x_dims; i++)
        {
            for (int j = 0; j < y_dims; j++) 
            {
                if (tiles[i][j].special == 2 && !tiles[i][j].Occupied())
                {
                    for (int k = 0; k < white_pieces.Count; k++)
                    {
                        if (!white_pieces[k].captured)
                        {
                            can_escape = false;
                        }
                    }
                    for (int k = 0; k < black_pieces.Count; k++)
                    {
                        if (!black_pieces[k].captured)
                        {
                            can_escape = false;
                        }
                    }
                }
            }
        }
        if (can_escape && escape)
        {
            scrape.Play(0);
            print("BYUH"); completed = true;
            if (exit_dir == 0)
            {
                dat.north_exits[cur_level] = true;
                if (exit_levels.Count > 0 && exit_levels[0] == "Shop")
                {
                    slide.Pause();
                    fade.NewScene("Shop", 2);
                    completed = false;
                }
            }
            else if (exit_dir == 1)
            {
                dat.west_exits[cur_level] = true;
            }
            else if (exit_dir == 2)
            {
                dat.south_exits[cur_level] = true;
            }
            else if (exit_dir == 3)
            {
                dat.east_exits[cur_level] = true;
            }
            
           
            for (int k = 0; k < white_pieces.Count; k++)
            {
                if (white_pieces[k].captured)
                {
                    Destroy(white_pieces[k].gameObject);
                }
            }
            for (int k = 0; k < black_pieces.Count; k++)
            {
                if (black_pieces[k].captured)
                {
                    Destroy(black_pieces[k].gameObject);
                }
            }
        }
        if (white_turn || !white_turn && alone)
        {
            for (int i = 0; i < b_tiles.Count; i++)
            {
                if (bking_p.x == b_tiles[i].x && bking_p.y == b_tiles[i].y && !b_tiles[i].triggered)
                {
                    if (b_tiles[i].load_scene)
                    {
                        dat.x_entry = 0;
                        dat.y_entry = 0;
                        SceneManager.LoadScene(b_tiles[i].scene);
                    }
                    if (b_tiles[i].has_text && !b_tiles[i].triggered)
                    {
                        b_tiles[i].triggered = true;
                        text = true;
                        tbox.startStrings = b_tiles[i].text;
                        tbox.gameObject.SetActive(true);
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < b_tiles.Count; i++)
            {
                if (wking_p.x == b_tiles[i].x && wking_p.y == b_tiles[i].y && !b_tiles[i].triggered)
                {
                    if (b_tiles[i].load_scene)
                    {
                        dat.x_entry = 0;
                        dat.y_entry = 0;
                        SceneManager.LoadScene(b_tiles[i].scene);
                    }
                    if (b_tiles[i].has_text && !b_tiles[i].triggered)
                    {
                        b_tiles[i].triggered = true;
                        text = true;
                        tbox.startStrings = b_tiles[i].text;
                        tbox.gameObject.SetActive(true);
                    }
                }
            }
        }
        mutations.Refresh(x_dims, y_dims);
    }

    private void checkValidPieceMove(int x_store,int y_store,bool exceeded)
    {
        switch (cur_piece.type)
        {
            case 5:
                if (Mathf.Abs(cur_x) > 1)
                {
                    cur_x /= Mathf.Abs(cur_x);
                }
                if (Mathf.Abs(cur_y) > 1)
                {
                    cur_y /= Mathf.Abs(cur_y);
                }
                break;
            case 4:
                if (cur_piece.x_dir == 1)
                {
                    if (cur_x > 2)
                    {
                        cur_x = 2;
                    }
                    if (cur_x == 2 && (cur_piece.has_moved || tiles[cur_piece.x + 1][cur_piece.y].Occupied()))
                    {
                        print("WHY");
                        cur_x = 1;
                    }
                    if (cur_x < 0)
                    {
                        cur_x = 0;
                    }
                    if (cur_x == 2 || cur_x == 0)
                    {
                        cur_y = 0;
                    }
                    if (Mathf.Abs(cur_y) > 1)
                    {
                        cur_y = y_store;
                    }
                }
                else if (cur_piece.y_dir == 1)
                {
                    if (cur_y > 2)
                    {
                        cur_y = 2;
                    }
                    if (cur_y == 2 && (cur_piece.has_moved || tiles[cur_piece.x][cur_piece.y-1].Occupied()))
                    {
                        cur_y = 1;
                    }
                    if (cur_y < 0)
                    {
                        cur_y = 0;
                    }
                    if (cur_y == 2 || cur_y == 0)
                    {
                        cur_x = 0;
                    }
                    if (Mathf.Abs(cur_x) > 1)
                    {
                        cur_x = x_store;
                    }
                }
                else if (cur_piece.x_dir == -1)
                {
                    if (cur_x < -2)
                    {
                        cur_x = -2;
                    }
                    if (cur_x == -2 && (cur_piece.has_moved || tiles[cur_piece.x - 1][cur_piece.y].Occupied()))
                    {
                        cur_x = -1;
                    }
                    if (cur_x > 0)
                    {
                        cur_x = 0;
                    }
                    if (cur_x == -2 || cur_x == 0)
                    {
                        cur_y = 0;
                    }
                    if (Mathf.Abs(cur_y) > 1)
                    {
                        cur_y = y_store;
                    }
                }
                else if (cur_piece.y_dir == -1)
                {
                    if (cur_y < -2)
                    {
                        cur_y = -2;
                    }
                    if (cur_y == -2 && (cur_piece.has_moved || tiles[cur_piece.x][cur_piece.y+1].Occupied()))
                    {
                        cur_y = -1;
                    }
                    if (cur_y > 0)
                    {
                        cur_y = 0;
                    }
                    if (cur_y == -2 || cur_y == 0)
                    {
                        cur_x = 0;
                    }
                    if (Mathf.Abs(cur_x) > 1)
                    {
                        cur_x = x_store;
                    }
                }
                if (cur_x != 0 && cur_y != 0)
                {
                    if (!((cur_piece.x + cur_x == passant_x && cur_piece.y - cur_y == passant_y) || (tiles[cur_piece.x + cur_x][cur_piece.y + -cur_y].Occupied() && tiles[cur_piece.x + cur_x][cur_piece.y + -cur_y].White() != white_turn)))
                    {
                        print("Invalid cus no capture");
                        forced_invalid = true;
                    }
                }
                else if ((cur_x != 0 || cur_y != 0) && tiles[cur_piece.x + cur_x][cur_piece.y + -cur_y].Occupied())
                {
                    cur_x = cur_x / Mathf.Max(Mathf.Abs(cur_x), Mathf.Abs(cur_y));
                    cur_y = cur_y / Mathf.Max(Mathf.Abs(cur_x), Mathf.Abs(cur_y));
                    print("Invalid oawn");
                    forced_invalid = true;
                }
                break;
            case 3:
                if (cur_x > 2)
                {
                    cur_x = 2;
                    cur_y = 0;
                }
                if (cur_y > 2)
                {
                    cur_x = 0;
                    cur_y = 2;
                }
                if (cur_x < -2)
                {
                    cur_x = -2;
                    cur_y = 0;
                }
                if (cur_y < -2)
                {
                    cur_x = 0;
                    cur_y = -2;
                }
                if (Mathf.Abs(cur_x) == 1 && Mathf.Abs(cur_y) == 1)
                {
                    if (x_store == 0 && y_store == 0)
                    {
                        cur_x = 0;
                        cur_y = 0;
                    }
                    if (y_store == 2 || y_store == -2)
                    {
                        cur_x *= 2;
                    }
                    if (x_store == 2 || x_store == -2)
                    {
                        cur_y *= 2;
                    }
                }
                else
                {
                    if (Mathf.Abs(cur_x) == 1 && Mathf.Abs(cur_y) != 2)
                    {
                        print("Invalid cus knight");
                        forced_invalid = true;
                        if (Mathf.Sign(cur_x) == x_dir && !exceeded)
                        {
                            cur_x = 2 * x_dir;
                        }
                        else
                        {
                            cur_x = 0;
                        }
                    }
                    if (Mathf.Abs(cur_y) == 1 && Mathf.Abs(cur_x) != 2)
                    {
                        print("Invalid cus knight");
                        forced_invalid = true;
                        if (Mathf.Sign(cur_y) == y_dir && !exceeded)
                        {
                            cur_y = 2 * y_dir;
                        }
                        else
                        {
                            cur_y = 0;
                        }
                    }
                }

                if (cur_x == 2 && cur_y == 2)
                {
                    if (x_dir == 1)
                    {
                        cur_y = 1;
                    }
                    else if (y_dir == 1)
                    {
                        cur_x = 1;
                    }
                }
                else if (cur_x == -2 && cur_y == 2)
                {
                    if (x_dir == -1)
                    {
                        cur_y = 1;
                    }
                    else if (y_dir == 1)
                    {
                        cur_x = -1;
                    }
                }
                else if (cur_x == 2 && cur_y == -2)
                {
                    if (x_dir == 1)
                    {
                        cur_y = -1;
                    }
                    else if (y_dir == -1)
                    {
                        cur_x = 1;
                    }
                }
                else if (cur_x == -2 && cur_y == -2)
                {
                    if (x_dir == -1)
                    {
                        cur_y = -1;
                    }
                    else if (y_dir == -1)
                    {
                        cur_x = -1;
                    }
                }
                if (!((Mathf.Abs(cur_x) == 2 && Mathf.Abs(cur_y) == 1) || (Mathf.Abs(cur_y) == 2 && Mathf.Abs(cur_x) == 1)))
                {
                    print("Invalid cus knight");
                    forced_invalid = true;
                }
                break;
            case 2:
                if (cur_x != 0 && cur_y == 0)
                {
                    cur_x = Mathf.Max(cur_x, -1);
                    cur_x = Mathf.Min(cur_x, 1);
                }
                else if (cur_x == 0 && cur_y != 0)
                {
                    cur_y = Mathf.Max(cur_y, -1);
                    cur_y = Mathf.Min(cur_y, 1);
                }
                if (cur_x < -1 && Mathf.Abs(cur_x) > Mathf.Abs(cur_y))
                {
                    if (x_dir == -1)
                    {
                        if (cur_y < 0)
                        {
                            cur_y--;
                        }
                        else
                        {
                            cur_y++;
                        }
                    }
                    else
                    {
                        cur_x++;
                    }
                }
                else if (cur_x > 1 && Mathf.Abs(cur_x) > Mathf.Abs(cur_y))
                {
                    if (x_dir == 1)
                    {
                        if (cur_y < 0)
                        {
                            cur_y--;
                        }
                        else
                        {
                            cur_y++;
                        }
                    }
                    else
                    {
                        cur_x--;
                    }
                }
                else if (cur_y < -1 && Mathf.Abs(cur_x) < Mathf.Abs(cur_y))
                {
                    if (y_dir == -1)
                    {
                        if (cur_x < 0)
                        {
                            cur_x--;
                        }
                        else
                        {
                            cur_x++;
                        }
                    }
                    else
                    {
                        cur_y++;
                    }
                }
                else if (cur_y > 1 && Mathf.Abs(cur_x) < Mathf.Abs(cur_y))
                {
                    if (y_dir == 1)
                    {
                        if (cur_x < 0)
                        {
                            cur_x--;
                        }
                        else
                        {
                            cur_x++;
                        }
                    }
                    else
                    {
                        cur_y--;
                    }
                }
                if (cur_piece.x + cur_x < 0 || cur_piece.x + cur_x >= x_dims)
                {
                    cur_x = x_store;
                    cur_y = y_store;
                }
                if (cur_piece.y - cur_y < 0 || cur_piece.y - cur_y >= y_dims)
                {
                    cur_x = x_store;
                    cur_y = y_store;
                }
                if (invalidTile(cur_piece.x + cur_x, cur_piece.y - cur_y) && !tiles[cur_piece.x + cur_x][cur_piece.y - cur_y].Occupied() && Mathf.Abs(cur_x) == Mathf.Abs(cur_y))
                {
                    cur_x = x_store;
                    cur_y = y_store;
                }
                if (cur_x == 0 || cur_y == 0)
                {
                    print("Invalid cus b");
                    forced_invalid = true;
                }
                else
                {
                    if ((invalidTile(cur_piece.x + cur_x, cur_piece.y - cur_y) && cur_x == cur_y) || (tiles[cur_piece.x + cur_x][cur_piece.y + -cur_y].Black() && !white_turn) || (tiles[cur_piece.x + cur_x][cur_piece.y + -cur_y].White() && white_turn))//1 for white
                    {
                        print("Problem 3");
                        cur_x = x_store;
                        cur_y = y_store;
                    }
                    else if ((Mathf.Abs(cur_x) > 1 || Mathf.Abs(cur_y) > 1) && tiles[cur_piece.x + x_store][cur_piece.y + -y_store].Occupied() && !(y_store == 0 && x_store == 0) && (Mathf.Abs(cur_y) > Mathf.Abs(y_store) || (Mathf.Abs(cur_x) > Mathf.Abs(x_store))))
                    {
                        print("Problem 4");
                        cur_x = x_store;
                        cur_y = y_store;
                    }
                    if (cur_x == 0 || cur_y == 0)
                    {
                        print("Invalid cus b");
                        forced_invalid = true;
                    }
                }

                break;
            case 1:
                if (Mathf.Abs(cur_x) > Mathf.Abs(cur_y))
                {
                    cur_y = 0;
                }
                if (Mathf.Abs(cur_x) < Mathf.Abs(cur_y))
                {
                    cur_x = 0;
                }
                if (Mathf.Abs(cur_x) == 1 && Mathf.Abs(cur_y) == 1)
                {
                    if (x_store == 0)
                    {
                        cur_x = 0;
                    }
                    if (y_store == 0)
                    {
                        cur_y = 0;
                    }
                    print("Prob 1");
                }
                if(invalidTile(cur_piece.x + cur_x,cur_piece.y -cur_y) && !tiles[cur_piece.x+cur_x][cur_piece.y - cur_y].Occupied())
                {
                    cur_x = x_store;
                    cur_y = y_store;
                }
                if ((cur_y != 0 || cur_x != 0) && (invalidTile(cur_piece.x + x_store, cur_piece.y + -y_store) || (tiles[cur_piece.x + cur_x][cur_piece.y + -cur_y].Black() && !white_turn) || (tiles[cur_piece.x + cur_x][cur_piece.y + -cur_y].White() && white_turn)))//1 for white
                {
                    cur_x = x_store;
                    cur_y = y_store;
                    print("Prob 2");
                }
                if ((tiles[cur_piece.x + x_store][cur_piece.y + -y_store].Occupied() || invalidTile(cur_piece.x + x_store, cur_piece.y + -y_store)) && !(y_store == 0 && x_store == 0) && (Mathf.Abs(cur_y) > Mathf.Abs(y_store) || (Mathf.Abs(cur_x) > Mathf.Abs(x_store))))
                {
                    cur_x = x_store;
                    cur_y = y_store;
                    print("prob 3");
                }
                break;
            case 0:
                if((GOBACK && Mathf.Abs(cur_x) > 1) || GOBACK && Mathf.Abs(cur_y) > 1)
                {
                    cur_x = x_store;
                    cur_y = y_store;
                }
                else if(cur_x == x_store && cur_y == y_store)
                {

                }
                else
                {
                    GOBACK = false;
                }
                if(Input.GetAxis("Horizontal") != 0 && Input.GetAxis("Vertical") != 0)
                {
                    cur_x = x_store;
                    cur_y = y_store;
                }
                if (cur_x == 0 || cur_y == 0 || (x_store == 0 && Mathf.Abs(y_store) > 1) || (y_store == 0 && Mathf.Abs(x_store) > 1))
                {
                    if (Mathf.Abs(cur_x) > Mathf.Abs(cur_y))
                    {
                        cur_y = 0;
                    }
                    if (Mathf.Abs(cur_x) < Mathf.Abs(cur_y))
                    {
                        cur_x = 0;
                    }
                    if (Mathf.Abs(cur_x) == 1 && Mathf.Abs(cur_y) == 1)
                    {
                        if (x_store == 0)
                        {
                            cur_x = 0;
                        }
                        if (y_store == 0)
                        {
                            cur_y = 0;
                        }
                        print("Prob 1");
                    }
                    if ((cur_y != 0 || cur_x != 0) && (invalidTile(cur_piece.x + x_store, cur_piece.y + -y_store) || (tiles[cur_piece.x + cur_x][cur_piece.y + -cur_y].Black() && !white_turn) || (tiles[cur_piece.x + cur_x][cur_piece.y + -cur_y].White() && white_turn)))//1 for white
                    {
                        if (cur_x == 0 || cur_y == 0)
                        {
                            GOBACK = true;
                        }
                        else
                        {
                            cur_x = x_store;
                            cur_y = y_store;
                        }
                    }
                    if ((tiles[cur_piece.x + x_store][cur_piece.y + -y_store].Occupied() || invalidTile(cur_piece.x + x_store, cur_piece.y + -y_store)) && !(y_store == 0 && x_store == 0) && (Mathf.Abs(cur_y) > Mathf.Abs(y_store) || (Mathf.Abs(cur_x) > Mathf.Abs(x_store))))
                    {
                        if (cur_x == 0 || cur_y == 0)
                        {
                            GOBACK = true;
                        }
                        else
                        {
                            cur_x = x_store;
                            cur_y = y_store;
                        }
                    }
                }
                else
                {
                    if (cur_x != 0 && cur_y == 0)
                    {
                        cur_x = Mathf.Max(cur_x, -1);
                        cur_x = Mathf.Min(cur_x, 1);
                    }
                    else if (cur_x == 0 && cur_y != 0)
                    {
                        cur_y = Mathf.Max(cur_y, -1);
                        cur_y = Mathf.Min(cur_y, 1);
                    }
                    if (cur_x < -1 && Mathf.Abs(cur_x) > Mathf.Abs(cur_y))
                    {
                        if (x_dir == -1)
                        {
                            if (cur_y < 0)
                            {
                                cur_y--;
                            }
                            else
                            {
                                cur_y++;
                            }
                        }
                        else
                        {
                            cur_x++;
                        }
                    }
                    else if (cur_x > 1 && Mathf.Abs(cur_x) > Mathf.Abs(cur_y))
                    {
                        if (x_dir == 1)
                        {
                            if (cur_y < 0)
                            {
                                cur_y--;
                            }
                            else
                            {
                                cur_y++;
                            }
                        }
                        else
                        {
                            cur_x--;
                        }
                    }
                    else if (cur_y < -1 && Mathf.Abs(cur_x) < Mathf.Abs(cur_y))
                    {
                        if (y_dir == -1)
                        {
                            if (cur_x < 0)
                            {
                                cur_x--;
                            }
                            else
                            {
                                cur_x++;
                            }
                        }
                        else
                        {
                            cur_y++;
                        }
                    }
                    else if (cur_y > 1 && Mathf.Abs(cur_x) < Mathf.Abs(cur_y))
                    {
                        if (y_dir == 1)
                        {
                            if (cur_x < 0)
                            {
                                cur_x--;
                            }
                            else
                            {
                                cur_x++;
                            }
                        }
                        else
                        {
                            cur_y--;
                        }
                    }
                    if (cur_piece.x + cur_x < 0 || cur_piece.x + cur_x >= x_dims)
                    {
                        cur_x = x_store;
                        cur_y = y_store;
                    }
                    if (cur_piece.y - cur_y < 0 || cur_piece.y - cur_y >= y_dims)
                    {
                        cur_x = x_store;
                        cur_y = y_store;
                    }
                    if (cur_x == 0 || cur_y == 0)
                    {
                        print("Invalid cus b");
                    }
                    else
                    {
                        if ((invalidTile(cur_piece.x + cur_x, cur_piece.y - cur_y) && cur_x == cur_y) || (tiles[cur_piece.x + cur_x][cur_piece.y + -cur_y].Black() && !white_turn) || (tiles[cur_piece.x + cur_x][cur_piece.y + -cur_y].White() && white_turn))//1 for white
                        {
                            print("Problem 3");
                            cur_x = x_store;
                            cur_y = y_store;
                        }
                        else if ((Mathf.Abs(cur_x) > 1 || Mathf.Abs(cur_y) > 1) && tiles[cur_piece.x + x_store][cur_piece.y + -y_store].Occupied() && !(y_store == 0 && x_store == 0) && (Mathf.Abs(cur_y) > Mathf.Abs(y_store) || (Mathf.Abs(cur_x) > Mathf.Abs(x_store))))
                        {
                            print("Problem 4");
                            cur_x = x_store;
                            cur_y = y_store;
                        }
                        if (cur_x == 0 || cur_y == 0)
                        {
                            print("Invalid cus b");
                        }
                    }
                }
                break;
        }
        if (cur_piece.x + cur_x < 0 || cur_piece.x + cur_x >= x_dims)
        {
            cur_x = x_store;
            cur_y = y_store;
        }
        if (cur_piece.y - cur_y < 0 || cur_piece.y - cur_y >= y_dims)
        {
            cur_x = x_store;
            cur_y = y_store;
        }
        if(cur_x == x_store && cur_y == y_store)
        {
            cancel.Play(0);
        }
    }

    private void SaveBoardState()
    {
        List<List<Undo_tile>> undotiles = new List<List<Undo_tile>>();
        for(int i = 0;i<x_dims;i++)
        {
            List<Undo_tile> untemp = new List<Undo_tile>();
            for (int j = 0; j < y_dims; j++)
            {
                untemp.Add(new Undo_tile(tiles[i][j]));
            }
            undotiles.Add(untemp);
        }
        List<Undo_piece> undoblack = new List<Undo_piece>();
        for(int i = 0;i<black_pieces.Count;i++)
        {
            undoblack.Add(new Undo_piece(black_pieces[i]));
        }
        List<Undo_piece> undowhite = new List<Undo_piece>();
        for (int i = 0; i < white_pieces.Count; i++)
        {
            undowhite.Add(new Undo_piece(white_pieces[i]));
        }
        undos.Push(new Undo_state(undoblack, undowhite,undotiles,new Undo_piece(wking_p),new Undo_piece(bking_p),passant_x,passant_y,passant_piece,white_turn));
    }

    private void LoadBoardState()
    {
        loss = false;
        if(undos.Count == 0 || !can_undo)
        {
            return;
        }
        undo.Play(0);
        //undo_hide.SetActive(true);
        Undo_state undstate = undos.Pop();
        for (int i = 0; i < x_dims; i++)
        {
            for (int j = 0; j < y_dims; j++)
            {
                tiles[i][j].UndoInherit(undstate.tiles[i][j]);
            }
        }
        for (int i = 0; i < black_pieces.Count; i++)
        {
            black_pieces[i].UndoInherit(undstate.black_pieces[i]);
            black_pieces[i].transform.position = mutations.start_position + new Vector3(black_pieces[i].x,-black_pieces[i].y,0);
            black_pieces[i].GetComponent<SpriteRenderer>().sortingOrder = (int)(-black_pieces[i].transform.position.y * 200);
            if (black_pieces[i].captured)
            {
                black_pieces[i].transform.position += new Vector3(0,10,0);
            }
            else
            {
                tiles[black_pieces[i].x][black_pieces[i].y].occupant = black_pieces[i];
            }

        }
        for (int i = 0; i < white_pieces.Count; i++)
        {
            white_pieces[i].UndoInherit(undstate.white_pieces[i]);
            white_pieces[i].transform.position = mutations.start_position + new Vector3(white_pieces[i].x, -white_pieces[i].y, 0);
            white_pieces[i].GetComponent<SpriteRenderer>().sortingOrder = (int)(-white_pieces[i].transform.position.y * 200);
            if (white_pieces[i].captured)
            {
                white_pieces[i].transform.position += new Vector3(0, 10, 0);
            }
            else
            {
                tiles[white_pieces[i].x][white_pieces[i].y].occupant = white_pieces[i];
            }
        }
        if (wking != null)
        {
            wking_p.UndoInherit(undstate.wking_p);
            wking_p.transform.position = mutations.start_position + new Vector3(wking_p.x, -wking_p.y, 0);
            tiles[wking_p.x][wking_p.y].occupant = wking_p;
            wking.GetComponent<SpriteRenderer>().sortingOrder = (int)(-wking.transform.position.y * 200);
        }
        if (bking != null)
        {
            bking_p.UndoInherit(undstate.bking_p);
            bking_p.transform.position = mutations.start_position + new Vector3(bking_p.x, -bking_p.y, 0);
            bking.GetComponent<SpriteRenderer>().sortingOrder = (int)(-bking.transform.position.y * 200);
            tiles[bking_p.x][bking_p.y].occupant = bking_p;
        }
        passant_x = undstate.passant_x;
        passant_y = undstate.passant_y;
        passant_piece = undstate.passant_piece;
        white_turn = undstate.white_turn;
        if(white_turn)
        {
            cur_piece = wking_p;
            black_selector.transform.position = wking.transform.position;
            white_selector.transform.position = wking.transform.position;
        }
        else
        {
            cur_piece = bking_p;
            black_selector.transform.position = bking.transform.position;
            white_selector.transform.position = bking.transform.position;
        }
        recalculateThreats();
        mutations.Refresh(x_dims,y_dims);
        cur_x = 0;
        cur_y = 0;
    }
}

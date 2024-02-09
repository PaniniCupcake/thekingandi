using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile_mutators : MonoBehaviour
{
    //max grid size is 10x10 but I don't think that'll ever show up if I can help it
    public Controller control;
    public List<int> x_dirs; //100 tiles
    public List<int> y_dirs; //100 tiles
    public List<int> noughts; // -1 for cross 1 for nought
    public List<int> numbering; //-1 for none, else a number
    public List<int> occupants;
    public List<int> occ_x_dirs;
    public List<int> occ_y_dirs;
    public List<bool> exits;
    public int k1x = -1; 
    public int k1y = -1;
    public int k2x = -1;
    public int k2y = -1;
    public Sprite black_tile;
    public Sprite white_tile;
    public Sprite white_lower;
    public List<Sprite> black_arrows = new List<Sprite>();
    public List<Sprite> black_exit_arrows = new List<Sprite>();
    public List<Sprite> white_arrows = new List<Sprite>();
    public List<Sprite> white_exit_arrows = new List<Sprite>();
    public List<Sprite> black_numbers = new List<Sprite>();
    public List<Sprite> white_numbers = new List<Sprite>();
    public List<int> specialtiles = new List<int>();//and buttons
    public List<GameObject> pieces;//0 king 1 rook 2 bishop 3 knight 4 pawn, +5 for black,10 wqueen,11 bqueen
    public Sprite white_nought;
    public Sprite white_cross;
    public Sprite black_nought;
    public Sprite black_cross;
    public Sprite white_exit;
    public Sprite black_exit;
    public Vector3 start_position;
    public List<List<SpriteRenderer>> tile_sprites = new List<List<SpriteRenderer>>();
    public List<List<bool>> black_tiles = new List<List<bool>>();
    public List<List<Transform>> black_threats = new List<List<Transform>>();
    public List<List<Transform>> white_threats = new List<List<Transform>>();
    public GameObject w_marker;
    public GameObject b_marker;
    public List<Sprite> special_tilesprites = new List<Sprite>();

    public void Startup(int x_dims, int y_dims)
    {
        Vector3 position = new Vector3(-0.5f * x_dims,0.5f*y_dims - 0.5f,0);
        for(int i = 0;i<x_dims;i++)
        {
            List<SpriteRenderer> t1 = new List<SpriteRenderer>();
            List<bool> t2 = new List<bool>();
            List<Transform> t3 = new List<Transform>();
            List<Transform> t4 = new List<Transform>();
            for (int j = 0;j<y_dims;j++)
            {
                t1.Add(null);
                t2.Add(false);
                t3.Add(null);
                t4.Add(null);
            }
            tile_sprites.Add(t1);
            black_tiles.Add(t2);
            black_threats.Add(t3);
            white_threats.Add(t4);
        }
        position += new Vector3(0.5f, -0.5f, 0);
        start_position = new Vector3(position.x, position.y, 0);
        /*if(y_dims == 8)
        {
            position -= new Vector3(0, 1f, 0);
        }
        else if(y_dims == 7)
        {
            position -= new Vector3(0, 0.5f, 0);
        }*/

        bool black = false;
        for(int j = 0;j<y_dims;j++)
        {
            for (int i = 0; i < x_dims; i++)
            {
                Tile t = control.tiles[i][j];
                if (t == null || (!t.valid && !t.Occupied()))
                {
                    position += new Vector3(1, 0, 0);
                    print(i);
                    print(j);
                    black = !black;
                    continue;
                   
                }
                
                black_threats[i][j] = Instantiate(b_marker, position, Quaternion.identity).transform;
                black_threats[i][j].parent = control.transform;
                white_threats[i][j] = Instantiate(w_marker, position, Quaternion.identity).transform;
                white_threats[i][j].parent = control.transform;
                //Settings
                t.special = specialtiles[i + j * 10];

                if (exits[i+j*10])
                {
                    if(x_dirs[i + j * 10] != 0 || y_dirs[i + j * 10] != 0)
                    {
                        if (y_dirs[i + j * 10] > 0)
                        {
                            t.arrow_exit = 0;
                        }
                        else if (y_dirs[i + j * 10] < 0)
                        {
                            t.arrow_exit = 2;
                        }
                        else if (x_dirs[i + j * 10] > 0)
                        {
                            t.arrow_exit = 1;
                        }
                        else if (x_dirs[i + j * 10] < 0)
                        {
                            t.arrow_exit = 3;
                        }

                    }
                    else
                    {
                        t.king_exit = true;
                    }
                }
                else
                {
                    t.x_push = x_dirs[i + j * 10];
                    t.y_push = y_dirs[i + j * 10];
                    t.nought = noughts[i + j * 10];
                    t.number = numbering[i + j * 10];
                    if (t.special == 5)
                    {
                        t.nought = -1;
                        t.number = 0;
                    }
                }
                if (occupants[i + j * 10] != -1)
                {
                    GameObject piece = Instantiate(pieces[occupants[i + j * 10]], position, Quaternion.identity);
                    Piece p = piece.GetComponent<Piece>();

                    p.x = i;
                    p.y = j;
                    p.x_dir = occ_x_dirs[i + j * 10];
                    p.y_dir = occ_y_dirs[i + j * 10];
                    piece.GetComponent<Animator>().SetFloat("x", p.x_dir);
                    piece.GetComponent<Animator>().SetFloat("y", p.y_dir);
                    if (occupants[i + j * 10] % 5 == 0)
                    {
                        if (occupants[i + j * 10] >= 5)
                        {
                            p.black = true;
                            control.bking = piece;
                            control.bking_p = p;

                        }
                        else
                        {
                            p.black = false;
                            control.wking = piece;
                            control.wking_p = p;
                        }
                    }
                    else
                    {
                        if (occupants[i + j * 10] >= 5)
                        {
                            p.black = true;
                            control.black_pieces.Add(p);
                        }
                        else
                        {
                            p.black = false;
                            control.white_pieces.Add(p);
                        }
                    }
                    control.tiles[i][j].occupant = p;
                }
                //Appearance

                if (t.valid || t.Occupied())
                {
                    SpriteRenderer tile = t.GetComponent<SpriteRenderer>();
                   
                    tile.transform.position = position;
                    SpriteRenderer lower = t.lower;
                    if (control.hide_tiles)
                    {
                        tile.enabled = false;
                        lower.GetComponent<SpriteRenderer>().enabled = false;
                    }
                    t.black_colour = black;
                    tile_sprites[i][j] = tile;
                    black_tiles[i][j] = black;
                    if (!black)
                    {
                        lower.sprite = white_lower;
                    }
                    if(t.x_push !=0 || t.y_push != 0)
                    {
                        int num = 4 + t.x_push - 3*t.y_push;
                        if(num >= 4)
                        {
                            num--;
                        }
                        if (black)
                        {
                            tile.sprite = black_arrows[num];
                        }
                        else
                        {
                            tile.sprite = white_arrows[num];
                        }
                    }
                    else if (t.special != -1)
                    {
                        if (!t.black_colour)
                        {
                            tile.sprite = special_tilesprites[t.special * 2 + 1];
                        }
                        else
                        {
                            tile.sprite = special_tilesprites[t.special * 2];
                        }
                    }
                    else if(t.number != -1)
                    {
                        if (!black)
                        {
                            tile.sprite = white_numbers[t.number];
                        }
                        else
                        {
                            tile.sprite = black_numbers[t.number];
                        }
                    }
                    else if(t.king_exit)
                    {
                        if (!black)
                        {
                            tile.sprite = white_exit;
                        }
                        else
                        {
                            tile.sprite = black_exit;
                        }
                    }
                    else if(t.arrow_exit != -1)
                    {
                        if (!black)
                        {
                            tile.sprite = white_exit_arrows[t.arrow_exit];
                        }
                        else
                        {
                            tile.sprite = black_exit_arrows[t.arrow_exit];
                        }
                    }
                    else if(t.nought != 0)
                    {
                        if (!black)
                        {
                            if(t.nought == 1)
                            {
                                tile.sprite = white_nought;
                            }
                            else
                            {
                                tile.sprite = white_cross;
                            }
                        }
                        else
                        {
                            if (t.nought == 1)
                            {
                                tile.sprite = black_nought;
                            }
                            else
                            {
                                tile.sprite = black_cross;
                            }
                        }
                    }

                    else if (!black)
                    {
                        tile.sprite = white_tile;
                    }
                }
                else
                {
                        t.GetComponent<SpriteRenderer>().enabled = false;
                        t.lower.GetComponent<SpriteRenderer>().enabled = false;
                }
                black = !black;
                position += new Vector3(1, 0, 0);
            }
            position -= new Vector3(x_dims, 1,0);
            if(x_dims % 2 == 0)
            {
                black = !black;
            }
        }
        control.bking.transform.position = start_position + new Vector3(control.bking_p.x, -control.bking_p.y, 0);
        if (control.wking != null)
        {
            control.wking.transform.position = start_position + new Vector3(control.wking_p.x, -control.wking_p.y, 0);
        }
    }

    public void ShowThreats(bool enabled,int x_dims,int y_dims)
    {
        if (enabled)
        {
            for (int i = 0; i < x_dims; i++)
            {
                for (int j = 0; j < y_dims; j++)
                {
                    if (tile_sprites[i][j] != null)
                    {
                        if (control.tiles[i][j].white_threat)
                        {
                            white_threats[i][j].transform.position = start_position + new Vector3(i, -j, 0);
                        }
                        else
                        {
                            white_threats[i][j].transform.position = new Vector3(99, 99, 0);
                        }
                        if (control.tiles[i][j].black_threat)
                        {
                            black_threats[i][j].transform.position = start_position + new Vector3(i, -j, 0);
                        }
                        else
                        {
                            black_threats[i][j].transform.position = new Vector3(99, 99, 0);
                        }
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < x_dims; i++)
            {
                for (int j = 0; j < y_dims; j++)
                {
                    if (tile_sprites[i][j] != null)
                    {
                        white_threats[i][j].transform.position = new Vector3(99, 99, 0);
                        black_threats[i][j].transform.position = new Vector3(99, 99, 0);
                    }
                }
            }
        }
    }

    public void Refresh(int x_dims, int y_dims)
    {
        for (int j = 0; j < y_dims; j++)
        {
            for (int i = 0; i < x_dims; i++)
            {
                Tile t = control.tiles[i][j];
                //Appearance
                if(t == null ||(!t.valid && !t.Occupied()))
                {
                    //print("Gus");
                    continue;
                }
                if (t.valid || t.Occupied())
                {
                    SpriteRenderer tile = tile_sprites[i][j];
                    if(tile == null)
                    {
                        continue;
                    }
                    if (t.x_push != 0 || t.y_push != 0)
                    {
                        int num = 4 + t.x_push - 3 * t.y_push;
                        if (num >= 4)
                        {
                            num--;
                        }
                        if (t.black_colour)
                        {
                            tile.sprite = black_arrows[num];
                        }
                        else
                        {
                            tile.sprite = white_arrows[num];
                        }
                    }
                    else if (t.special != -1)
                    {
                        if (!t.black_colour)
                        {
                            tile.sprite = special_tilesprites[t.special * 2 + 1];
                        }
                        else
                        {
                            tile.sprite = special_tilesprites[t.special * 2];
                        }
                    }
                    else if (t.number != -1)
                    {
                        if (!t.black_colour)
                        {
                            tile.sprite = white_numbers[t.number];
                        }
                        else
                        {
                            tile.sprite = black_numbers[t.number];
                        }
                    }
                    else if (t.king_exit)
                    {
                        if (!t.black_colour)
                        {
                            tile.sprite = white_exit;
                        }
                        else
                        {
                            tile.sprite = black_exit;
                        }
                    }
                    else if (t.arrow_exit != -1)
                    {
                        if (!t.black_colour)
                        {
                            tile.sprite = white_exit_arrows[t.arrow_exit];
                        }
                        else
                        {
                            tile.sprite = black_exit_arrows[t.arrow_exit];
                        }
                    }
                    else if (t.nought != 0)
                    {
                        if (!t.black_colour)
                        {
                            if (t.nought == 1)
                            {
                                tile.sprite = white_nought;
                            }
                            else
                            {
                                tile.sprite = white_cross;
                            }
                        }
                        else
                        {
                            if (t.nought == 1)
                            {
                                tile.sprite = black_nought;
                            }
                            else
                            {
                                tile.sprite = black_cross;
                            }
                        }
                    }
                    else if (!t.black_colour)
                    {
                        tile.sprite = white_tile;
                    }
                    else
                    {
                        tile.sprite = black_tile;
                    }
                }
            }
        }
    }
}

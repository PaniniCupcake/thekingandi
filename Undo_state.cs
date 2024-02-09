using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Undo_state 
{
    public List<Undo_piece> black_pieces;
    public List<Undo_piece> white_pieces;
    public List<List<Undo_tile>> tiles;
    public Undo_piece wking_p;
    public Undo_piece bking_p;
    public int passant_x;
    public int passant_y;
    public Piece passant_piece;
    public bool white_turn;
    public Undo_state(int x_dims, int y_dims)
    {
        black_pieces = new List<Undo_piece>();
        white_pieces = new List<Undo_piece>();
        for(int i = 0;i<x_dims;i++)
        {
            List<Undo_tile> t = new List<Undo_tile>();
            for (int j = 0; j < x_dims; j++)
            {
                t.Add(null);
            }
            tiles.Add(t);
        }
    }
    public Undo_state(List<Undo_piece> b, List<Undo_piece> w,List<List<Undo_tile>> t,Undo_piece wk,Undo_piece bk,int px,int py, Piece pp,bool turn)
    {
        black_pieces = b;
        white_pieces = w;
        tiles = t;
        wking_p = wk;
        bking_p = bk;
        passant_x = px;
        passant_y = py;
        passant_piece = pp;
        white_turn = turn;
    }
}

public class Undo_tile
{
    public bool valid = false;
    public int x = 0;
    public int y = 0;
    public int nought = 0; //0 if neither, 1 for nought, -1 for cross
    public int number = -1;
    public int x_push = 0;
    public int y_push = 0;
    public bool king_exit = false;
    public int arrow_exit = -1; //-1 for none, 0 up 1 right 2 down 3 left
    public bool button = false;
    public int special;

    public Undo_tile(Tile og)
    {
        valid = og.valid;
        x = og.x;
        y = og.y;
        nought = og.nought;
        number = og.number;
        x_push = og.x_push;
        y_push = og.y_push;
        king_exit = og.king_exit;
        arrow_exit = og.arrow_exit;
        special = og.special;
    }
}

public class Undo_piece
{
    public int type = 0; //0 for queen, 1 for rook, 2 for bishop, 3 for knight, 4 for pawn
    public int x = 0;
    public int y = 0;
    public int x_dir;
    public int y_dir;
    public bool black;
    public bool has_moved = false;
    public bool captured;
    public Undo_piece(Piece og)
    {
        if(og == null)
        {
            return;
        }
        type = og.type;
        x = og.x;
        y = og.y;
        x_dir = og.x_dir;
        y_dir = og.y_dir;
        black = og.black;
        has_moved = og.has_moved;
        captured = og.captured;
    }
}

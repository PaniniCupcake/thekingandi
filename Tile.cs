using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool valid = false;
    public Piece occupant = null;
    public int x = 0;
    public int y = 0;
    public int nought = 0; //0 if neither, 1 for nought, -1 for cross
    public int number = -1;
    public int x_push = 0;
    public int y_push = 0;
    public bool black_threat = false;
    public bool white_threat = false;
    public bool king_exit = false;
    public int arrow_exit = -1; //-1 for none, 0 up 1 right 2 down 3 left
    public Interactable inter = null;
    public bool black_colour = false;
    public int arise = 0;
    public int special = -1;// 0 for check,1 for dec, 2 for kill, 3 for adj, 4 for flip, 5 for lock
    public SpriteRenderer lower;
    public Piece white_threatener;
    public Piece black_threatener;
    public Tile white_threat_source;
    public Tile black_threat_source;
    public bool passing;
    public void UndoInherit(Undo_tile tile)
    {
        valid = tile.valid;
        x = tile.x;
        y = tile.y;
        nought = tile.nought;
        number = tile.number;
        x_push = tile.x_push;
        y_push = tile.y_push;
        king_exit = tile.king_exit;
        arrow_exit = tile.arrow_exit;
        occupant = null;
        special = tile.special;
    }

    public bool Occupied()
    {
        return occupant != null;
    }
    public bool Occupied(bool x)
    {
        return passing || occupant != null;
    }
    public bool Black()
    {
        return (occupant != null && occupant.black);
    }

    public bool White()
    {
        return (occupant != null && !occupant.black);
    }

    public void Landed()
    {
        if (number > 0)
        {
            number--;
        }
    }
    public void Left()
    {
        if (number > 0)
        {
            number--;
        }
    }

    public void Returned(int num)
    {
        if(number >= 0 && number < 3 && num != 0)
        {
            number++;
        }
    }

    public void Decrement()
    {
        if(number >=0 && !Occupied() && special == -1)
        {
            number += 3;
            number %= 4;
        }
    }
    public void Increment()
    {
        if (number >= 0 && !Occupied() && special == -1)
        {
            number += 1;
            number %= 4;
        }
    }
}

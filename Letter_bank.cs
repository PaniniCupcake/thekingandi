using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Letter_bank : MonoBehaviour
{
    public List<GameObject> lower_case;
    public List<GameObject> upper_case;
    public List<GameObject> numbers;
    public List<GameObject> other;//other[9] is empty
    public List<int> lower_space;
    public List<int> punc_space;
    public GameObject click;
    public Sprite empty;
    public List<Sprite> items;//0 = 1/2, 1 = fish
    public List<Sprite> misc;//0: sign 1:fish
    public List<Sprite> wking;
    public List<Sprite> bking;
    public List<Sprite> wqueen;
    public List<Sprite> bqueen;
    public List<Sprite> wbishop;
    public List<Sprite> bbishop;
    public List<Sprite> wknight;
    public List<Sprite> bknight;
    public List<Sprite> wrook;
    public List<Sprite> brook;
    public List<Sprite> wpawn;
    public List<Sprite> bpawn;
    public List<Animator> piece_anims; //0 queen, 1 rook, 2 bishop, 3 knight, 4 pawn, 5 king, + 5 for black
}

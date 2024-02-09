using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public int type = 0; //0 for queen, 1 for rook, 2 for bishop, 3 for knight, 4 for pawn
    public int x = 0;
    public int y = 0;
    public int x_dir;
    public int y_dir;
    public bool black;
    public Transform my_pos;
    private Controller control;
    public Animator animator;
    public List<int> limit;
    public bool has_moved = false;
    public bool ghosted = false;
    public int arise = 0;
    public bool captured;
    public bool interactable = false;
    public TextInterStore text;
    public bool uncapturable = false;//for stuff
    public bool automove = false;
    public List<int> forced_moves_trigger_x = new List<int>();
    public List<int> forced_moves_trigger_y = new List<int>();
    public List<int> forced_moves_x = new List<int>();
    public List<int> forced_moves_y = new List<int>();
    public bool faceless;
    void Start()
    {

        control = FindObjectOfType<Controller>();
        my_pos = gameObject.transform;
        if (type != 6 && !faceless)
        {
            animator = this.GetComponent<Animator>();
            animator.SetFloat("x", x_dir);
            animator.SetFloat("y", y_dir);
            animator.SetInteger("Type", type);
            animator.SetBool("black", black);
        }
        GetComponent<SpriteRenderer>().sortingOrder = (int)(-transform.position.y * 200);
        if(interactable)
        {
            int modif = 43;
            if (type == 1)
            {
                modif = 30;
            }
            else if (type == 2)
            {
                modif = 39;
            }
            else if (type == 3)
            {
                modif = 29;
            }
            else if (type == 4)
            {
                modif = 27;
            }
            if (type != 6)
            {
                GameObject t = Instantiate(control.speech, transform.position + new Vector3(0, modif / 21f, 0), Quaternion.identity);
                t.transform.parent = this.transform;
            }
        }
    }

    public void SetAnims()
    {
        if(type == 6 || faceless)
        {
            return;
        }
        animator.SetFloat("x", x_dir);
        animator.SetFloat("y", y_dir);
        animator.SetInteger("Type", type);
        animator.SetBool("black", black);
        animator.SetFloat("Type2d", type);
    }

    public void UndoInherit(Undo_piece piece)
    {
        type = piece.type;
        x = piece.x;
        y = piece.y;
        x_dir = piece.x_dir;
        y_dir = piece.y_dir;
        black = piece.black;
        has_moved = piece.has_moved;
        animator.SetFloat("x", x_dir);
        animator.SetFloat("y", y_dir);
        animator.SetInteger("Type", type);
        animator.SetBool("black", black);
        captured = piece.captured;
    }
}

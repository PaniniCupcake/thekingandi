using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
public class Gui_menu : MonoBehaviour
{
    public bool black_only;
    public bool white_only;
    private AudioSource move;
    public List<SpriteRenderer> white_boxes = new List<SpriteRenderer>();
    public List<SpriteRenderer> black_boxes = new List<SpriteRenderer>();
    public List<Sprite> sprite_list = new List<Sprite>();
    //public List<int> sprite_positions = new List<int>();
    public Sprite empty;
    public Sprite box;
    public SpriteRenderer white_king;
    public SpriteRenderer black_king;
    public Controller control;
    public List<GameObject> left_arrows;
    public List<GameObject> right_arrows;
    public GameObject right_menu;
    public GameObject left_menu;
    public int option_selected;
    public GameObject item_select;
    public List<int> dir_cooldowns = new List<int>();
    public int cooldown;
    public List<List<string>> item_descriptors = new List<List<string>>();
    public GameObject pause_bar;
    public GameObject tbox;
    public Sprite reg_pause;
    public Sprite exp_pause;
    public List<GameObject> pause_menus = new List<GameObject>();
    private int cur_menu;
    private int menu_option;
    public GameObject menu_arrow;
    private GameData dat;
    public List<Sprite> numbers;
    public List<SpriteRenderer> sfx_nums;
    public List<SpriteRenderer> music_nums;
    private int left_momentum = 0;
    private int right_momentum = 0;
    private int cooldown_length;
    private int game_speed;
    private int text_speed;
    private int red_level;
    private int blue_level;
    private int green_level;
    public List<SpriteRenderer> col_nums;
    public List<SpriteRenderer> speed_stuff;
    public List<Sprite> cooldownsprites;
    public List<Sprite> speed_nums;
    public AudioSource select;
    public GameObject map;
    public GameObject jbhider;
    public GameObject svhider;
    public Scene_fade fade;
    private int white_items = 0;
    private int black_items = 0;
    public void Setup()
    {
        fade = FindObjectOfType<Scene_fade>();
        if (control == null)
        {
            control = FindObjectOfType<Controller>();
        }
        if (tbox == null)
        {
            tbox = control.tbox.gameObject;
        }
        dat = FindObjectOfType<GameData>();
        dir_cooldowns.Add(0);
        dir_cooldowns.Add(0);
        dir_cooldowns.Add(0);
        dir_cooldowns.Add(0);
        select = dat.sfx[0];
        move = dat.sfx[7];
        cooldown = 0;
        if (dat.items_obtained[6])
        {
            jbhider.SetActive(false);
        }
        if (dat.items_obtained[7])
        {
            svhider.SetActive(false);
        }
        ChangeSprites();
        {
            List<string> s1 = new List<string>();
            s1.Add("#i0STALEMATE BLOCKER:");
            s1.Add("A mysterious power within this item prevents stalemate from occuring just by being near it.>");
            List<string> s2 = new List<string>();
            s2.Add("#i2MAP:");
            s2.Add("An odd map that seems to expand the more you explore. It also allows for quick traversal between areas.>");
            s2.Add("You can press the map key (M by default) to open it.>");
            List<string> s3 = new List<string>();
            s3.Add("#i3STOCK KEY:");
            s3.Add("A small, generic looking key. Maybe it can be used to unlock something?");
            List<string> s4 = new List<string>();
            s4.Add("#i4UNDO BUTTON:");
            s4.Add("An unassuming piece of plastic with a z written on it.>");
            s4.Add("If you can control its power, you may be able to rewind time...>");
            s4.Add("You can press the undo key (Z by default) to take back any number of turns.>");
            List<string> sx = new List<string>();
            if(FindObjectOfType<Controller>() != null && FindObjectOfType<Controller>().special_level == false)
            {
                switch (FindObjectOfType<Controller>().cur_level)
                {
                    case 11:
                        sx.Add("#i9Why. You already beat this level.");
                        break;
                    case 12:
                        sx.Add("#i9You've beaten this level too. I'm assuming you tried this for level 1 as well.");
                        break;
                    case 13:
                        sx.Add("#i9Stop asking for hints on levels you had to complete before getting hints.");
                        break;
                    case 14:
                        sx.Add("#i9Just shield one king with the other. Keep space in the centre in case you need to block for an extra turn.>");
                        break;
                    case 15:
                        sx.Add("#i9The rooks seem to be defending each other. If only there was something that could get in the way...>");
                        break;
                    case 16:
                        sx.Add("#i9Arrows can place you back on the tile you started!>");
                        sx.Add("Keep one king idle whilst the other gets in position.>");
                        break;
                    case 17:
                        sx.Add("#i9Pay attention to how pawns interact with arrows.>");
                        sx.Add("The pawns at the very top and bottom can only access the diagonal arrow tiles if there is an enemy on them.>");
                        sx.Add("Get past the centre and you should be fine.>");
                        break;
                    case 18:
                        sx.Add("#i9The centre tile is dangerous!>");
                        sx.Add("You'll have to move onto it at some point, just make sure neither king is next to an arrow pointing away from it first!>");
                        break;
                    case 19:
                        sx.Add("#i9Watch out!>");
                        sx.Add("Without a pawn, this puzzle would be impossible.>");
                        break;
                    case 20:
                        sx.Add("#i9It's as simple as swapping the kings, right?>");
                        sx.Add("Try experimenting with the leftmost flipper.>");
                        break;
                    case 21:
                        sx.Add("#i9Come on, just move both kings onto a nought tile and bring them back.");
                        sx.Add("The black king will have to go in there first.>");
                        break;
                    case 22:
                        sx.Add("#i9Capture the left white rook by blocking with the white king.");
                        sx.Add("Come on, these are intro levels.>");
                        break;
                    case 23:
                        sx.Add("#i9You can capture the upper bishops by blocking off the lower bishops with a king.");
                        break;
                    case 24:
                        sx.Add("#i9Just because there are noughts doesn't mean they need to be stood on!>");
                        sx.Add("Start by going anticlockwise.>");
                        break;
                    case 25:
                        sx.Add("#i9Hm, those exit tiles seem to be on the wrong side of the crosses...");
                        break;
                    case 26:
                        sx.Add("#i9It looks like you have to cross all the bridges, but that doesn't have to be the case if you're struggling.>");
                        break;
                    case 27:
                        sx.Add("#i9This is just a basic rook blocking puzzle.");
                        sx.Add("Step on the 2s enough and you'll be able to capture them easily.");
                        break;
                    case 28:
                        sx.Add("#i9The white rooks aren't doing anything, but the black ones are.");
                        break;
                    case 29:
                        sx.Add("#i9Count the tiles, each king will have to land on half of them.");
                        sx.Add("Make sure they aren't meeting at the finish.>");
                        break;
                    case 30:
                        sx.Add("#i9Take out the 2 tiles as soon as possible and this should be easy.");
                        break;
                    case 31:
                        sx.Add("#i9You'll have to kill everything here.");
                        sx.Add("Start with the bishop.");
                        break;
                    case 32:
                        sx.Add("#i9Make sure the black king always has space to move to the middle after blocking with it.");
                        break;
                    case 33:
                        sx.Add("#i9It's almost like there's an arrow pointing where to go first.>");
                        sx.Add("Both kings need to take one of the 1s in the middle.>");
                        break;
                    case 34:
                        sx.Add("#i9Remember, the tiles only count down if the minus 1 is uncovered!");
                        break;
                    case 35:
                        sx.Add("#i9The right puzzle is solvable by just moving between one corner and the centre>.");
                        sx.Add("For the left one, you'll have to stand on two opposite side tiles once each.>");
                        break;
                    case 36:
                        sx.Add("#i9Black rook goes in the middle.");
                        sx.Add("You can do the rest.>");
                        break;
                    case 37:
                        sx.Add("#i9Good old fashioned rook. Can't go wrong with that.");
                        break;
                    case 38:
                        sx.Add("The central black tiles might as well be gaps.");
                        sx.Add("Cross one king at a time using your bishops, and keep the other out of the way.>");
                        break;
                    case 39:
                        sx.Add("#i9What a knightmare, huh?>");
                        sx.Add("Try moving a black one into the centre.>");
                        break;
                    case 40:
                        sx.Add("#i9This one's all about the first move. There are a few moves that don't deliver check immediately.>");
                        sx.Add("You're not gonna be taking one of them.>");
                        break;
                    case 41:
                        sx.Add("#i9You only need one of those bishops at the top.");
                        sx.Add("You can use it to capture a bishop at the bottom.");
                        break;
                    case 42:
                        sx.Add("#i9Remember that arrow tiles only activate when landed on, not after.");
                        sx.Add("A king can't threaten the middle if there's a friendly piece in the way...>");
                        break;
                    case 43:
                        sx.Add("#i9For one night only, you need one knight only!");
                        sx.Add("You might want to flip the arrows fairly early.>");
                        break;
                    case 44:
                        sx.Add("#i9The knight and king can work together to climb the hourglass!>");
                        sx.Add("It looks like the kings swapping sides is necessary first.>");
                        sx.Add("But how can you stop the bottom tile from being threatened during the swap?>");
                        break;
                    case 45:
                        sx.Add("#i9You get the idea, only one rook required.>");
                        sx.Add("Get one king into a safe position where the other king and his rook can work their magic to reach the middle.>");
                        break;
                    case 46:
                        sx.Add("#i9You gotta cover all 4 noughts.");
                        sx.Add("Make sure the rooks are both on the same column.>");
                        break;
                    case 47:
                        sx.Add("#i9Looks like both sides can only make one capture.>");
                        sx.Add("You'll have to take one edge pawn and one centre pawn.>");
                        break;
                    case 48:
                        sx.Add("#i9I hate when levels have to be assymetric like this.");
                        sx.Add("Just find a way to invert who goes first.>");
                        break;
                    case 49:
                        sx.Add("#i9When you move a bishop into a corner, you can't get it back.");
                        sx.Add("You'll need to keep one around to reach the exits.>");
                        break;
                    case 50:
                        sx.Add("#i9Looks like only a knight can reach that nought tile.");
                        sx.Add("You'll need to capture one of the pawns to let the other through.>");
                        sx.Add("The moves needed afterwards with the knight might surprise you!>");
                        break;
                    case 51:
                        sx.Add("#i9The layout might look like both kings need to get through, but truth is it's only one.");
                        sx.Add("Don't block both of them off with bishops!.>");
                        sx.Add("Use it in the menu during a puzzle for a hint.>");
                        break;
                    case 52:
                        sx.Add("#i9Just move the pawn forwards two spaces. That's it.");
                        break;
                    case 53:
                        sx.Add("#i9Take advantage of check immunity again!");
                        sx.Add("Get those rooks moving!>");
                        break;
                    case 54:
                        sx.Add("#i9Inch those rooks forwards.");
                        sx.Add("Only one will need to move more than one tile at a time.>");
                        break;
                    case 55:
                        sx.Add("#i9Ya gotta move two knights onto each corner.");
                        sx.Add("Start with the knights in the middle, then move the edge knights around.>");
                        break;
                    case 56:
                        sx.Add("#i9This puzzle would still be doable if the two tiles at each side were cropped off.>");
                        sx.Add("#i9Everybody loves a bit of rotational symmetry.>");
                        break;
                    case 57:
                        sx.Add("#i9You may not be able to move onto cross tiles but you can still move pieces off them!>");
                        break;
                    case 58:
                        sx.Add("Position the bishops to block the rooks, then send the king with the further away bishop to the other side.");
                        break;
                    case 59:
                        sx.Add("#i9You may realise there's no way for every knight to cross the gap.");
                        sx.Add("Maybe you can bypass the skull tiles another way.>");
                        break;
                    default:
                        sx.Add("#i9Google en passant.");
                        break;
                }
            }
            else
            {
                sx.Add("#i9HINT BOOK:");
                sx.Add("A book containing various logical assistance.>");
                sx.Add("Use it in the menu during a puzzle for a hint.>");
            }
            
            List<string> s5 = new List<string>();
            s5.Add("#i5THREAT DETECTOR:");
            s5.Add("A small package containing various objects that seem to gravitate towards tiles that are under threat.>");
            s5.Add("You can press the threats key (T by default) to toggle displaying threats on and off.>");
            s5.Add("Be warned that tiles are only shown as threatened if they are threatened on the current turn.>");
            List<string> s6 = new List<string>();
            s6.Add("#i6JUKEBOX:");
            s6.Add("A pocket sized jukebox that seems capable of playing a variety of songs.>");
            s6.Add("Its functionality can be accessed in the audio settings.");
            List<string> s7 = new List<string>();
            s7.Add("#i7RGBEASEL:");
            s7.Add("A small easel capable of modifying red, green and blue colour values.>");
            s7.Add("Its functionality can be accessed in the visual settings.");
            List<string> s8 = new List<string>();
            s8.Add("#i8CHESS GUIDEBOOK:");
            s8.Add("A thick book that presumably contains rules for playing chess.>");
            s8.Add("It contains a lot of words, so remember the default key for skipping dialogue is x or esc.>");
            s8.Add("Chess is a 2 player game played on a board with 64 spaces.>");
            s8.Add("Each player controls a king, a queen, two rooks, bishops and knights, and eight pawns.>");
            s8.Add("These pieces all move in a specific way, and can capture enemy pieces by landing on the same space as them, removing them from the game.>");
            s8.Add("Rooks can move any number of spaces horizontally or vertically, while bishops can move any number of spaces diagonally.>");
            s8.Add("Queens can move any number of spaces horizontally, vertically, or diagonally.>");
            s8.Add("Knights can move in an L shape. This is the same as moving 2 spaces horizontally or vertically, turning 90 degrees, then moving forwards one more space.>");
            s8.Add("They are the only piece that can move over enemy pieces and gaps.>");
            s8.Add("Pawns can exclusively move forwards in the direction they are facing.>");
            s8.Add("If they have not moved, they can choose to move two spaces, otherwise only one.>");
            s8.Add("Pawns can not capture by moving forwards, but by moving one tile diagonally forwards.>");
            s8.Add("They can also capture a pawn that has moved two spaces by moving diagonally onto the tile behind them.>");
            s8.Add("If the tile in front of a pawn can not be moved to and is not occupied, the pawn is promoted to a non king piece.>");
            s8.Add("The king can move one tile in any direction.>");
            s8.Add("If a player's king is captured, they lose the game.>");
            s8.Add("Therefore, it is illegal for a king to move onto a tile where it could be captured.>");
            s8.Add("If you have no legal moves and your king is in check, the game ends in checkmate and you lose.>");
            s8.Add("Otherwise, if you have no legal moves, the game ends in stalemate as a draw.>");
            s8.Add("The same happens if the same position occurs 3 times, or 50 moves elapse without a capture or pawn move.>");
            s8.Add("The king can also make a special move called castling, but your king has already moved a bunch so it doesn't matter.>");
            List<string> s9 = new List<string>();
            s9.Add("You're not supposed to be here.");
            List<string> s10 = new List<string>();
            s10.Add("You're not supposed to be here.");
            List<string> s11 = new List<string>();
            s11.Add("You're not supposed to be here.");
            List<string> s12 = new List<string>();
            s12.Add("You're not supposed to be here.");
            List<string> s13 = new List<string>();
            s13.Add("You're not supposed to be here.");
            List<string> s14 = new List<string>();
            s14.Add("You're not supposed to be here.");
            List<string> s15 = new List<string>();
            s15.Add("You're not supposed to be here.");
            List<string> s17 = new List<string>();
            s17.Add("#i1FISH:");
            s17.Add("1t's m3, y0ur fr13ndly f1shy 4ssistant!.>");
            s17.Add("W1th my ch3ss 3xpetertise, y0u c4n c0mmand p13ces n3xt t0 y0u t0 m0ve!.>");
            s17.Add("L3t's w0rk t0gether t0 d3feat th4t d4stardly qu33n!>");
            s17.Add("#m3You can control other pieces by attempting to move onto their tile.>");
            s17.Add("Moving another piece will use your turn.>");
            s17.Add("You can cancel a move by pressing the menu key (x by default).");
            item_descriptors.Add(s1);
            item_descriptors.Add(s2);
            item_descriptors.Add(s3);
            item_descriptors.Add(s4);
            item_descriptors.Add(sx);
            item_descriptors.Add(s5);
            item_descriptors.Add(s6);
            item_descriptors.Add(s7);
            item_descriptors.Add(s9);
            item_descriptors.Add(s8);
            item_descriptors.Add(s10);
            item_descriptors.Add(s11);
            item_descriptors.Add(s12);
            item_descriptors.Add(s13);
            item_descriptors.Add(s14);
            item_descriptors.Add(s15);
            item_descriptors.Add(s17);
        }
    }

    public void ChangeSprites()
    {
        int i = 0;
        if (!black_only)
        {
            for (i = 0; i < 8; i += 1)
            {
                white_boxes[i].sprite = box;
            }
            white_boxes[0].sprite = dat.sprite_list[0];
            white_items = 1;


            for (i = 1;i<8;i+=2)
            {
                if (dat.items_obtained[i])
                {
                    white_items++;
                    white_boxes[(i+1)/2].sprite = dat.sprite_list[i];
                }
                else
                {
                    white_boxes[(i+1) / 2].sprite = box;
                }
            }
            if (dat.items_obtained[9])
            {
                white_boxes[4].sprite = dat.sprite_list[9];
            }
        }
        else
        {
            white_king.sprite = empty;
            for (i = 0; i < 8; i++)
            {
                white_boxes[i].sprite = empty;
            }
        }
        if (!white_only)
        {
            for (i = 0; i < 9; i+= 2)
            {
                if (dat.items_obtained[i])
                {
                    black_boxes[i/2].sprite = dat.sprite_list[i];
                    black_items++;
                }
                else
                {
                    black_boxes[(i + 1) / 2].sprite = box;
                }
            }
            if (dat.items_obtained[10])
            {
                black_boxes[4].sprite = dat.sprite_list[9];
            }
            if (dat.items_obtained[18])
            {
                black_boxes[1].sprite = dat.sprite_list[18];
            }
        }
        else
        {
            black_king.sprite = empty;
            for (i = 0; i < 8; i++)
            {
                black_boxes[i].sprite = empty;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {

    }

    public void Pause(bool white_turn)
    {
        if(white_turn)
        {
            white_king.gameObject.SetActive(false);
            left_arrows[0].SetActive(true);
            left_menu.SetActive(true);
        }
        else
        {
            right_arrows[0].SetActive(true);
            black_king.gameObject.SetActive(false);
            right_menu.SetActive(true);
        }
        option_selected = 8;
        pause_bar.SetActive(true);
        cur_menu = 0;
        pause_bar.GetComponent<SpriteRenderer>().sprite = reg_pause;
    }

    public void Unpause()
    {
        white_king.gameObject.SetActive(true);
        black_king.gameObject.SetActive(true);
        right_menu.SetActive(false);
        left_menu.SetActive(false);
        item_select.SetActive(false);
        pause_bar.SetActive(false);
        for(int i = 0;i<4;i++)
        {
            left_arrows[i].SetActive(false);
            right_arrows[i].SetActive(false);
            pause_menus[i].SetActive(false);
        }
        
        menu_arrow.SetActive(false);
        menu_arrow.transform.rotation = Quaternion.identity;
        Time.timeScale = dat.game_speed;
        if (control != null)
        {
            control.cooldown = -1;
        }
        else
        {
            FindObjectOfType<MapManager>().cooldown = -1;
        }
    }

    public void PauseMenu(bool white_turn)
    {
        for (int i = 0; i < 4; i++)
        {
            if (dir_cooldowns[i] > 0)
            {
                dir_cooldowns[i] -= 1;
            }
        }
        if (cooldown > 0)
        {
            cooldown--;
        }
        if (cur_menu == 0)
        {
            int prev_action = option_selected;
            if (option_selected >= 8)
            {
                right_arrows[option_selected - 8].SetActive(false);
                left_arrows[option_selected - 8].SetActive(false);
            }

            if (Input.GetAxis("Vertical") >= 0.1)
            {
                dir_cooldowns[1] = 0;
                if (dir_cooldowns[0] == 0)
                {
                    move.Play(0);
                    dir_cooldowns[0] = dat.cooldowns;

                    if (option_selected >= 9)
                    {
                        option_selected--;
                    }
                    else if (option_selected == 8)
                    {
                        if (white_turn)
                        {
                            option_selected = 7;
                        }
                        else
                        {
                            option_selected = 7;
                        }
                        if (option_selected < 0)
                        {
                            option_selected = 11;
                        }
                        else if (option_selected != 0)
                        {
                            option_selected -= option_selected % 2;
                        }
                    }
                    else if (option_selected == 0 || option_selected == 1)
                    {
                        option_selected = 11;
                    }
                    else
                    {
                        option_selected -= 2;
                    }
                }
            }
            else if (Input.GetAxis("Vertical") <= -0.1)
            {
                dir_cooldowns[0] = 0;
                if (dir_cooldowns[1] == 0)
                {
                    move.Play(0);
                    dir_cooldowns[1] = dat.cooldowns;
                    if (option_selected == 11)
                    {
                        option_selected = 0;
                    }
                    else if (option_selected >= 7)
                    {
                        option_selected++;
                    }
                    else
                    {
                        option_selected += 2;
                    }
                    if (option_selected < 8 && ((option_selected >= white_items && white_turn) || (option_selected >= black_items && !white_turn)))
                    {
                        //option_selected = 8;
                    }
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
                    move.Play(0);
                    if (option_selected < 8)
                    {
                        if (option_selected % 2 == 0)
                        {
                            option_selected++;
                        }
                        else if (option_selected % 2 == 1)
                        {
                            option_selected--;
                        }
                        if ((option_selected >= white_items && white_turn) || (option_selected >= black_items && !white_turn))
                        {
                            //option_selected = prev_action;
                        }
                    }
                    dir_cooldowns[2] = dat.cooldowns;
                }
            }
            else if (Input.GetAxis("Horizontal") <= -0.1)
            {
                dir_cooldowns[2] = 0;
                if (dir_cooldowns[3] == 0)
                {
                    move.Play(0);
                    dir_cooldowns[3] = 10;
                    if (option_selected < 8)
                    {
                        if (option_selected % 2 == 0)
                        {
                            option_selected++;
                        }
                        else if (option_selected % 2 == 1)
                        {
                            option_selected--;
                        }
                        if ((option_selected >= white_items && white_turn) || (option_selected >= black_items && !white_turn))
                        {
                            //option_selected = prev_action;
                        }
                    }
                    dir_cooldowns[2] = dat.cooldowns;
                }
            }
            else
            {
                dir_cooldowns[2] = 0; dir_cooldowns[3] = 0;
            }
            if (option_selected >= 8)
            {
                item_select.SetActive(false);
                if (white_turn)
                {
                    left_arrows[option_selected - 8].SetActive(true);
                }
                else
                {
                    right_arrows[option_selected - 8].SetActive(true);
                }

            }
            else
            {
                item_select.SetActive(true);
                if (white_turn)
                {
                    item_select.transform.position = transform.position + new Vector3(-165.5f / 21f, 83f / 21f, 0);
                }
                else
                {
                    item_select.transform.position = transform.position + new Vector3(127.5f / 21f, 83f / 21f, 0);
                }
                if (option_selected % 2 == 0)
                {
                    item_select.transform.position += new Vector3(0, -38f / 21f * option_selected / 2, 0);
                }
                else
                {
                    item_select.transform.position += new Vector3(0, -38f / 21f * (option_selected - 1) / 2, 0);
                    item_select.transform.position += new Vector3(38f / 21f, 0, 0);
                }
            }
            if (Input.GetAxis("Submit") > 0 && cooldown == 0)
            {
                select.Play(0);
                if (option_selected == 8)
                {
                    Unpause();
                    if (control != null)
                    {
                        control.paused = false;
                        control.p_cooldown = true;
                    }
                    else
                    {
                        FindObjectOfType<MapManager>().paused = false;
                        FindObjectOfType<MapManager>().p_cooldown = true;
                    }
                    
                }
                else if (option_selected == 9)
                {
                    cur_menu = 1;
                    left_arrows[1].SetActive(false);
                    right_arrows[1].SetActive(false);
                    pause_menus[0].SetActive(true);
                    menu_arrow.SetActive(true);
                    menu_arrow.transform.position = transform.position + new Vector3(-55f / 21f, 0, 0);
                    pause_bar.GetComponent<SpriteRenderer>().sprite = exp_pause;
                }
                else if (option_selected == 10)
                {
                    //SceneManager.LoadScene("Home screen");
                    fade.NewScene("Home screen",2);
                }
                else if (option_selected == 11)
                {
                    Application.Quit();
                }
                else
                {
                    if (white_turn)
                    {
                        if (option_selected == 0)
                        {
                            tbox.GetComponent<Textbox>().startStrings = item_descriptors[0]; tbox.SetActive(true);
                        }
                        else if (option_selected == 1 && dat.items_obtained[1])
                        {
                            tbox.GetComponent<Textbox>().startStrings = item_descriptors[1]; tbox.SetActive(true);
                        }
                        else if (option_selected == 2 && dat.items_obtained[3])
                        {
                            tbox.GetComponent<Textbox>().startStrings = item_descriptors[3]; tbox.SetActive(true);
                        }
                        else if (option_selected == 3 && dat.items_obtained[5])
                        {
                            tbox.GetComponent<Textbox>().startStrings = item_descriptors[5]; tbox.SetActive(true);
                        }
                        else if (option_selected == 4 && dat.items_obtained[7])
                        {
                            tbox.GetComponent<Textbox>().startStrings = item_descriptors[7]; tbox.SetActive(true);
                        }
                    }
                    else
                    {
                        if (option_selected == 0 && dat.items_obtained[0])
                        {
                            tbox.GetComponent<Textbox>().startStrings = item_descriptors[0];
                            tbox.SetActive(true);
                        }
                        else if (option_selected == 1 && dat.items_obtained[2])
                        {
                            if (dat.items_obtained[18])
                            {
                                tbox.GetComponent<Textbox>().startStrings = item_descriptors[16];
                                tbox.SetActive(true);
                            }
                            else
                            {
                                tbox.GetComponent<Textbox>().startStrings = item_descriptors[2];
                                tbox.SetActive(true);
                            }
                        }
                        else if (option_selected == 2 && dat.items_obtained[4])
                        {
                            tbox.GetComponent<Textbox>().startStrings = item_descriptors[4];
                            tbox.SetActive(true);
                        }
                        else if (option_selected == 3 && dat.items_obtained[6])
                        {
                            tbox.GetComponent<Textbox>().startStrings = item_descriptors[6];
                            tbox.SetActive(true);
                        }
                        else if (option_selected == 4 && dat.items_obtained[10])
                        {
                            tbox.GetComponent<Textbox>().startStrings = item_descriptors[9];
                            tbox.SetActive(true);
                        }
                    }

                }
                cooldown = -1;
            }
            else if (Input.GetAxis("Submit") == 0)
            {
                cooldown = 0;
            }
        }
        else
        {
            if (Input.GetAxis("Vertical") >= 0.1)
            {
                dir_cooldowns[1] = 0;
                if (dir_cooldowns[0] == 0)
                {
                    move.Play(0);
                    dir_cooldowns[0] = dat.cooldowns;
                    if (cur_menu == 5)
                    {
                        if (menu_option < 5)
                        {
                            menu_option += 4;
                            menu_option %= 5;
                        }
                        else
                        {
                            menu_option+=4;
                            menu_option = ((menu_option - 5) % 5) + 5;
                        }
                    }
                    else
                    {
                        menu_option += 3;
                        menu_option %= 4;
                        if (menu_option == 2 && ((!dat.items_obtained[7] && cur_menu == 1) || (!dat.items_obtained[6] && cur_menu == 2)))
                        {
                            menu_option = 1;
                        }
                    }
                    print(menu_option);
                }
            }
            else if (Input.GetAxis("Vertical") <= -0.1)
            {
                dir_cooldowns[0] = 0;
                if (dir_cooldowns[1] == 0)
                {
                    move.Play(0);
                    dir_cooldowns[1] = dat.cooldowns;
                    if (cur_menu == 5)
                    {
                        if (menu_option < 5)
                        {
                            menu_option++;
                            menu_option %= 5;
                        }
                        else
                        {
                            menu_option++;
                            menu_option = ((menu_option - 5) % 5) + 5;
                        }
                    }
                    else
                    {
                        menu_option += 1;
                        menu_option %= 4;
                        if (menu_option == 2 && ((!dat.items_obtained[7] && cur_menu == 1) || (!dat.items_obtained[6] && cur_menu == 2)))
                        {
                            menu_option = 3;
                        }
                    }
                    print(menu_option);
                }

            }
            else
            {
                dir_cooldowns[0] = 0; dir_cooldowns[1] = 0;
            }
            if (Input.GetAxis("Horizontal") >= 0.1)
            {

                left_momentum = 0;
                dir_cooldowns[3] = 0;

                if (dir_cooldowns[2] == 0)
                {
                    dir_cooldowns[2] = dat.cooldowns;
                    move.Play(0);

                    if (cur_menu == 2)
                    {
                        dir_cooldowns[2] = 10 - right_momentum / 2;
                        right_momentum += 1;
                        if (right_momentum > 18)
                        {
                            right_momentum = 18;
                        }
                        if (menu_option == 0)
                        {
                            dat.music_volume += 0.01f;
                            if (dat.music_volume > 1)
                            {
                                dat.music_volume = 1f;
                            }
                        }
                        else if (menu_option == 1)
                        {
                            dat.sfx_volume += 0.01f;
                            if (dat.sfx_volume > 1)
                            {
                                dat.sfx_volume = 1f;
                            }
                        }
                        dat.UpdateAudio();
                    }
                    else if (cur_menu == 3)
                    {
                        if (menu_option == 0)
                        {
                            cooldown_length++;
                            cooldown_length = Mathf.Min(cooldown_length, 4);
                        }
                        else if (menu_option == 1)
                        {
                            game_speed++;
                            game_speed = Mathf.Min(game_speed, 5);
                        }
                        else if (menu_option == 2)
                        {
                            text_speed++;
                            text_speed = Mathf.Min(text_speed, 5);
                        }
                    }
                    else if (cur_menu == 4)
                    {
                        if (menu_option == 0)
                        {
                            red_level++;
                            red_level = Mathf.Min(red_level, 16);
                        }
                        else if (menu_option == 1)
                        {
                            green_level++;
                            green_level = Mathf.Min(green_level, 16);
                        }
                        else if (menu_option == 2)
                        {
                            blue_level++;
                            blue_level = Mathf.Min(blue_level, 16);
                        }
                        dat.red = red_level / 16f;
                        dat.green = green_level / 16f;
                        dat.blue = blue_level / 16f;
                        dat.UpdateColour();
                    }
                    else if (cur_menu == 5)
                    {
                        menu_option += 5;
                        menu_option %= 10;
                    }
                }
            }
            else if (Input.GetAxis("Horizontal") <= -0.1)
            {
                right_momentum = 0;
                dir_cooldowns[2] = 0;

                if (dir_cooldowns[3] == 0)
                {
                    dir_cooldowns[3] = dat.cooldowns;
                    move.Play(0);

                    if (cur_menu == 2)
                    {
                        dir_cooldowns[3] = 10 - left_momentum / 2;
                        left_momentum += 1;
                        if (left_momentum > 18)
                        {
                            left_momentum = 18;
                        }
                        if (menu_option == 0)
                        {
                            dat.music_volume -= 0.01f;
                            if (dat.music_volume < 0)
                            {
                                dat.music_volume = 0f;
                            }
                        }
                        else if (menu_option == 1)
                        {
                            dat.sfx_volume -= 0.01f;
                            if (dat.sfx_volume < 0)
                            {
                                dat.sfx_volume = 0f;
                            }
                        }
                        dat.UpdateAudio();
                    }
                    else if (cur_menu == 3)
                    {
                        if (menu_option == 0)
                        {
                            cooldown_length--;
                            cooldown_length = Mathf.Max(cooldown_length, 0);
                        }
                        else if (menu_option == 1)
                        {
                            game_speed--;
                            game_speed = Mathf.Max(game_speed, 0);
                        }
                        else if (menu_option == 2)
                        {
                            text_speed--;
                            text_speed = Mathf.Max(text_speed, 0);
                        }
                    }
                    else if (cur_menu == 4)
                    {
                        if (red_level + green_level + blue_level >= 10)
                        {
                            if (menu_option == 0)
                            {
                                red_level--;
                                red_level = Mathf.Max(red_level, 0);
                            }
                            else if (menu_option == 1)
                            {
                                green_level--;
                                green_level = Mathf.Max(green_level, 0);
                            }
                            else if (menu_option == 2)
                            {

                                blue_level--;
                                blue_level = Mathf.Max(blue_level, 0);
                            }
                        }
                        dat.red = red_level / 16f;
                        dat.green = green_level / 16f;
                        dat.blue = blue_level / 16f;
                        dat.UpdateColour();
                    }
                    else if (cur_menu == 5)
                    {
                        menu_option += 5;
                        menu_option %= 10;
                    }
                }
            }
            else
            {
                dir_cooldowns[2] = 0; dir_cooldowns[3] = 0; left_momentum = 0; right_momentum = 0;
            }

            menu_arrow.transform.position = transform.position + new Vector3(-55f / 21f, -menu_option * 15f / 21f, 0);
            if(cur_menu == 5)
            {
                if(menu_option < 5)
                {
                    menu_arrow.transform.position = transform.position + new Vector3(-55f / 21f, 5f / 21f - menu_option * 13f / 21f, 0);
                    menu_arrow.transform.rotation = Quaternion.identity;
                }
                else
                {
                    menu_arrow.transform.position = transform.position + new Vector3(55f / 21f, 5f / 21f - (menu_option-5) * 13f / 21f, 0);
                    menu_arrow.transform.rotation = new Quaternion(0,Mathf.PI,0,0);
                }
            }
            if (Input.GetAxis("Submit") > 0 && cooldown == 0)
            {
                select.Play(0);
                if (cur_menu == 1)
                {
                    if (menu_option == 3)
                    {
                        if (white_turn)
                        {
                            left_arrows[0].SetActive(true);
                        }
                        else
                        {
                            right_arrows[0].SetActive(true);
                        }
                        menu_arrow.SetActive(false);
                        pause_menus[0].SetActive(false);
                        menu_option = 0;
                        option_selected = 8;
                        cur_menu = 0;
                        pause_bar.GetComponent<SpriteRenderer>().sprite = reg_pause;
                    }
                    else
                    {
                        cur_menu = menu_option + 2;
                        menu_option = 0;
                        pause_menus[0].SetActive(false);
                        pause_menus[cur_menu - 1].SetActive(true);
                        menu_arrow.transform.position = transform.position + new Vector3(-55f / 21f, 0, 0);
                        if (cur_menu == 2)
                        {
                            music_nums[0].gameObject.SetActive(true);
                            sfx_nums[0].gameObject.SetActive(true);
                        }
                        else if (cur_menu == 3)
                        {
                            if (dat.cooldowns == -1)
                            {
                                cooldown_length = 0;
                            }
                            else
                            {
                                cooldown_length = 20 / dat.cooldowns;
                            }
                            game_speed = speedSet(dat.game_speed);
                            text_speed = speedSet(dat.text_speed);
                        }
                        else if (cur_menu == 4)
                        {
                            red_level = (int)(dat.red * 16);
                            green_level = (int)(dat.green * 16);
                            blue_level = (int)(dat.blue * 16);
                        }
                    }
                }
                else if (cur_menu == 5 && menu_option == 9)
                {
                    pause_menus[cur_menu - 1].SetActive(false);
                    cur_menu = 2;
                    menu_option = 0;
                    pause_menus[1].SetActive(true);
                    menu_arrow.transform.position = transform.position + new Vector3(-55f / 21f, 0, 0);
                    menu_arrow.transform.rotation = new Quaternion(0, 0, 0, 0);
                }
                else if (cur_menu == 2 && menu_option == 2)
                {
                    pause_menus[cur_menu - 1].SetActive(false);
                    cur_menu = 5;
                    menu_option = 0;
                    pause_menus[4].SetActive(true);
                    menu_arrow.transform.position = transform.position + new Vector3(-55f / 21f, 5f/21f, 0);
                }
                else if (cur_menu == 5)
                {
                    dat.songs[dat.cur_track].Stop();
                    dat.songs[menu_option].Play();
                    dat.cur_track = menu_option;
                }
                else if (menu_option == 3)
                {
                    pause_menus[cur_menu - 1].SetActive(false);
                    cur_menu = 1;
                    menu_option = 0;
                    pause_menus[0].SetActive(true);
                    menu_arrow.transform.position = transform.position + new Vector3(-55f / 21f, 0, 0);
                }
                cooldown = -1;

            }
            else if (Input.GetAxis("Submit") == 0)
            {
                cooldown = 0;
            }
            if (cur_menu == 2)
            {
                music_nums[0].sprite = numbers[Mathf.FloorToInt(dat.music_volume)];
                music_nums[1].sprite = numbers[Mathf.FloorToInt((dat.music_volume * 10) % 10)];
                music_nums[2].sprite = numbers[Mathf.FloorToInt((dat.music_volume * 100) % 10)];
                sfx_nums[0].sprite = numbers[Mathf.FloorToInt(dat.sfx_volume)];
                sfx_nums[1].sprite = numbers[Mathf.FloorToInt((dat.sfx_volume * 10) % 10)];
                sfx_nums[2].sprite = numbers[Mathf.FloorToInt((dat.sfx_volume * 100) % 10)];
            }
            else if (cur_menu == 3)
            {
                if (cooldown_length == 0)
                {
                    dat.cooldowns = -1;
                }
                else
                {
                    dat.cooldowns = 20 / cooldown_length;
                }
                print(game_speed);
                print(text_speed);
                speed_stuff[0].sprite = cooldownsprites[cooldown_length];
                speed_stuff[1].sprite = speed_nums[game_speed];
                speed_stuff[2].sprite = speed_nums[text_speed];
                dat.game_speed = speedSelect(game_speed);
                dat.text_speed = speedSelect(text_speed);
            }//0.75,1,1.5,2.25,3,4.5
            else if (cur_menu == 4)
            {
                col_nums[0].sprite = numbers[red_level / 10];
                col_nums[1].sprite = numbers[red_level % 10];
                col_nums[2].sprite = numbers[green_level / 10];
                col_nums[3].sprite = numbers[green_level % 10];
                col_nums[4].sprite = numbers[blue_level / 10];
                col_nums[5].sprite = numbers[blue_level % 10];
            }

        }
    }
    private float speedSelect(int inp)
    {
        switch (inp)
        {
            case 0:
                return 0.5f;
            case 1:
                return 1f;
            case 2:
                return 1.5f;
            case 3:
                return 2.25f;
            case 4:
                return 3f;
            default:
                return 4.5f;
        }
    }
    private int speedSet(float inp)
    {
        switch (inp)
        {
            case 0.5f:
                return 0;
            case 1f:
                return 1;
            case 1.5f:
                return 2;
            case 2.25f:
                return 3;
            case 3f:
                return 4;
            default:
                return 5;
        }
    }
}

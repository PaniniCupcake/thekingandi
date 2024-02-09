using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Start_menu : MonoBehaviour
{
    //public List<int> sprite_positions = new List<int>();
    public int option_selected;
    public List<GameObject> main_options;
    public GameObject bk;
    public GameObject wk;
    public int c1 = 0;
    public int c2 = 50;
    public GameObject b;
    public List<int> dir_cooldowns = new List<int>();
    public int cooldown;
    public List<GameObject> pause_menus = new List<GameObject>();
    private int cur_menu;
    private int menu_option;
    public GameObject menu_arrow;
    private GameData dat;
    private AudioSource move;
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
    public GameObject jbhider;
    public GameObject svhider;
    private int sub_menu;
    public List<GameObject> con_ops;
    public GameObject confirm;
    public int warmup;
    public GameObject text;
    public GameObject men;
    public GameObject datfab;
    public Scene_fade fade;
    void Start()
    {
        dat = FindObjectOfType<GameData>();
        if(dat == null)
        {
            dat = Instantiate(datfab,new Vector3(0,0,0),Quaternion.identity).GetComponent<GameData>();
        }
        dir_cooldowns.Add(0);
        dir_cooldowns.Add(0);
        dir_cooldowns.Add(0);
        dir_cooldowns.Add(0);
        select = dat.sfx[0];
        move = dat.sfx[7];
        cooldown = 0;
        if (dat.items_obtained[5])
        {
            jbhider.SetActive(false);
        }
        if (dat.items_obtained[6])
        {
            svhider.SetActive(false);
        }
        if(dat.new_game)
        {
            b.SetActive(true);
            option_selected = 1;
            main_options[0].SetActive(false);
        }
        men.transform.position += new Vector3(0, -8f, 0);
        text.transform.position += new Vector3(0, 8f, 0);
    }

    private void FixedUpdate()
    {
        if (c1 == 200)
        {
            bk.transform.position += new Vector3(0, 3f / 21f, 0);
            c1 = 0;
        }
        if(c1 == 100)
        {
            bk.transform.position += new Vector3(0, -3f / 21f, 0);
        }
        if(c1 == 150)
        {
            wk.transform.position += new Vector3(0, 3f / 21f, 0);
        }
        if(c1 == 50)
        {
            wk.transform.position += new Vector3(0, -3f / 21f, 0);
        }
        c1++;
        if(warmup != -1)
        {
            men.transform.position += new Vector3(0, 8f/84f, 0);
            text.transform.position += new Vector3(0, -8f/84f, 0);
            if (warmup == 84)
            {
                warmup = -2;
            }
            warmup++;
        }
    }

    void Update()
    {
        if(warmup != -1)
        {
            return;
        }
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

        if(sub_menu > 0)
        {
            int prev_action = option_selected;
            if (Input.GetAxis("Vertical") >= 0.1)
            {
                dir_cooldowns[1] = 0;
                if (dir_cooldowns[0] == 0)
                {
                    move.Play(0);
                    dir_cooldowns[0] = dat.cooldowns;
                    option_selected = 1 - option_selected;
                }
            }
            else if (Input.GetAxis("Vertical") <= -0.1)
            {
                dir_cooldowns[0] = 0;
                if (dir_cooldowns[1] == 0)
                {
                    move.Play(0);
                    dir_cooldowns[1] = dat.cooldowns;
                    option_selected = 1 - option_selected;
                }
            }
            else
            {
                dir_cooldowns[0] = 0; dir_cooldowns[1] = 0;
            }
            if (Input.GetAxis("Submit") > 0 && cooldown == 0)
            {
                select.Play(0);
                cooldown = -1;
                if (option_selected == 0)
                {
                    dat.newFile();
                    fade.NewScene("Intro1", 2);
                    Destroy(this);
                }
                else if (option_selected == 1)
                {

                    confirm.SetActive(false);
                    sub_menu = 0;
                    option_selected = 1;
                    if (dat.new_game)
                    {
                        main_options[0].SetActive(false);
                        option_selected = 1;
                    }
                }

            }
            else if (Input.GetAxis("Submit") == 0)
            {
                cooldown = 0;
            }
            con_ops[prev_action].SetActive(false);
            con_ops[option_selected].SetActive(true);
        }
        else if (cur_menu == 0)
        {
            int prev_action = option_selected;
            if (Input.GetAxis("Vertical") >= 0.1)
            {
                dir_cooldowns[1] = 0;
                if (dir_cooldowns[0] == 0)
                {
                    move.Play(0);
                    dir_cooldowns[0] = dat.cooldowns;
                    option_selected += 4;
                    option_selected %= 5;
                    if(dat.new_game && option_selected == 0)
                    {
                        option_selected = 4;
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
                    option_selected += 1;
                    option_selected %= 5;
                    if (dat.new_game && option_selected == 0)
                    {
                        option_selected = 1;
                    }
                }
            }
            else
            {
                dir_cooldowns[0] = 0; dir_cooldowns[1] = 0;
            }
            if (Input.GetAxis("Submit") > 0 && cooldown == 0)
            {
                select.Play(0);
                cooldown = -1;
                if(option_selected == 0)
                {
                    if(dat.cur_level < 7)
                    {
                        fade.NewScene("Intro" + (dat.cur_level + 1),2);
                        Destroy(this);
                    }
                    else if(dat.cur_level == 7)
                    {
                        fade.NewScene("Intro extra",2);
                        Destroy(this);
                    }
                    else
                    {
                        fade.NewScene("Map", 2);
                        Destroy(this);
                    }
                }
                else if (option_selected == 1)
                {
                    confirm.SetActive(true);
                    sub_menu = 1;
                }
                else if (option_selected == 2)
                {
                    cur_menu = 1;
                    option_selected = 0;
                    pause_menus[0].SetActive(true);
                    menu_arrow.SetActive(true);
                    menu_arrow.transform.position = transform.position + new Vector3(-19.5f / 21f, 0, 0);
                }
                else if (option_selected == 3)
                {
                    //SceneManager.LoadScene("Versus");
                    fade.NewScene("Versus",2);
                }
                else
                {
                    Application.Quit();
                }

            }
            else if (Input.GetAxis("Submit") == 0)
            {
                cooldown = 0;
            }
            main_options[prev_action].SetActive(false);
            main_options[option_selected].SetActive(true);
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
                            menu_option += 4;
                            menu_option = ((menu_option - 5) % 5) + 5;
                        }
                    }
                    else
                    {
                        menu_option += 3;
                        menu_option %= 4;
                        if (menu_option == 2 && ((!dat.items_obtained[5] && cur_menu == 1) || (!dat.items_obtained[6] && cur_menu == 2)))
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
                        if (menu_option == 2 && ((!dat.items_obtained[5] && cur_menu == 1) || (!dat.items_obtained[6] && cur_menu == 2)))
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

            menu_arrow.transform.position = transform.position + new Vector3(-55f / 21f, -19.5f/21f - menu_option * 15f / 21f, 0);
            if (cur_menu == 5)
            {
                if (menu_option < 5)
                {
                    menu_arrow.transform.position = transform.position + new Vector3(-55f / 21f, -14.5f / 21f - menu_option * 13f / 21f, 0);
                    menu_arrow.transform.rotation = Quaternion.identity;
                }
                else
                {
                    menu_arrow.transform.position = transform.position + new Vector3(55f / 21f, -14.5f / 21f - (menu_option - 5) * 13f / 21f, 0);
                    menu_arrow.transform.rotation = new Quaternion(0, Mathf.PI, 0, 0);
                }
            }
            if (Input.GetAxis("Submit") > 0 && cooldown == 0)
            {
                select.Play(0);
                if (cur_menu == 1)
                {
                    if (menu_option == 3)
                    {
                        menu_arrow.SetActive(false);
                        pause_menus[0].SetActive(false);
                        menu_option = 2;
                        option_selected = 2;
                        cur_menu = 0;
                    }
                    else
                    {
                        cur_menu = menu_option + 2;
                        menu_option = 0;
                        pause_menus[0].SetActive(false);
                        pause_menus[cur_menu - 1].SetActive(true);
                        menu_arrow.transform.position = transform.position + new Vector3(-19.5f / 21f, 0, 0);
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
                    menu_arrow.transform.position = transform.position + new Vector3(-19.5f / 21f, 0, 0);
                    menu_arrow.transform.rotation = new Quaternion(0, 0, 0, 0);
                }
                else if (cur_menu == 2 && menu_option == 2)
                {
                    pause_menus[cur_menu - 1].SetActive(false);
                    cur_menu = 5;
                    menu_option = 0;
                    pause_menus[4].SetActive(true);
                    menu_arrow.transform.position = transform.position + new Vector3(-19.5f / 21f, 5f / 21f, 0);
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
                    menu_arrow.transform.position = transform.position + new Vector3(-19.5f / 21f, 0, 0);
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
        if (dat.new_game)
        {
            main_options[0].SetActive(false);
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

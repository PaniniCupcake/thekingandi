using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MapManager : MonoBehaviour
{
    public Transform cam;
    public int cur_x = 0;
    public int cur_y = 4;
    private List<List<Map_tile>> tile_grid = new List<List<Map_tile>>();
    public Map_tile orig;
    public GameObject datfab;
    private GameData dat;
    public int cooldown = 0;
    public bool paused = false;
    List<int> dir_cooldowns = new List<int>();
    public bool p_cooldown;
    public Gui_menu GUI;
    public AudioSource select;
    public AudioSource move;
    private int x_dir = 0;
    private int y_dir = 0;
    public Sprite exclaim;
    private void Start()
    {

        dir_cooldowns.Add(0); dir_cooldowns.Add(0); dir_cooldowns.Add(0); dir_cooldowns.Add(0);
        dat = FindObjectOfType<GameData>();
        if (dat == null)
        {
            dat = Instantiate(datfab, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<GameData>();
        }
        GUI.Setup(); cooldown = dat.cooldowns;
        x_dir = dat.x_entry;
        y_dir = dat.y_entry;
        select = dat.sfx[0];
        move = dat.sfx[7];
        Map_tile[] tiles = FindObjectsOfType<Map_tile>();
        for (int i = 0; i < 14; i++)
        {
            List<Map_tile> t = new List<Map_tile>();

            for (int j = 0; j < 9; j++)
            {
                t.Add(null);
            }
            tile_grid.Add(t);
        }
        for (int k = 0; k < tiles.Length; k++)
        {
            tiles[k].gameObject.SetActive(false);
            tile_grid[(int)(tiles[k].transform.position.x - 9.5) / 2][(int)-(tiles[k].transform.position.y - 2) / 2] = tiles[k];
            tiles[k].x = (int)(tiles[k].transform.position.x - 9.5) / 2;
            tiles[k].y = (int)-(tiles[k].transform.position.y - 2) / 2;

            if (tiles[k].number == dat.cur_level)
            {
                continue;
                print("WHAT TWHAT");
                print(tiles[k].number);
                print(tiles[k].level);
                cur_x = tiles[k].x;
                cur_y = tiles[k].y;
                print(cur_x);
                print(cur_y);
                print(tiles[k].transform.position.x);
                print(tiles[k].transform.position.y);
            }
        }
        Queue<Map_tile> checks = new Queue<Map_tile>();
        checks.Enqueue(orig);
        Map_tile n;
        while (checks.Count > 0)
        {
            n = checks.Dequeue();
            n.valid = true;
            n.gameObject.SetActive(true);
            // print(n.level);
            //print(n.number);
            //print(n.number);
            if (n.up && dat.north_exits[n.number])
            {
                if (!tile_grid[n.x][n.y - 1].valid)
                {
                    checks.Enqueue(tile_grid[n.x][n.y - 1]);
                }
            }
            if (n.down && dat.south_exits[n.number])
            {
                if (!tile_grid[n.x][n.y + 1].valid)
                {
                    checks.Enqueue(tile_grid[n.x][n.y + 1]);
                }
            }
            if (n.left && dat.west_exits[n.number])
            {
                if (!tile_grid[n.x - 1][n.y].valid)
                {
                    checks.Enqueue(tile_grid[n.x - 1][n.y]);
                }
            }
            if (n.right && dat.east_exits[n.number])
            {
                if (!tile_grid[n.x + 1][n.y].valid)
                {
                    checks.Enqueue(tile_grid[n.x + 1][n.y]);
                }
            }
        }
        for (int i = 0; i < tile_grid.Count; i++)
        {
            for (int j = 0; j < tile_grid[i].Count; j++)
            {
                if (tile_grid[i][j] != null && tile_grid[i][j].number == dat.cur_level)
                {
                    print(tile_grid[i][j].level);
                    cur_x = tile_grid[i][j].x;
                    cur_y = tile_grid[i][j].y;
                }
            }
        }
        if (!dat.game_triggers[5])
        {
            tile_grid[3][3].GetComponent<SpriteRenderer>().sprite = exclaim;
        }
        if (!dat.game_triggers[6])
        {
            tile_grid[11][8].GetComponent<SpriteRenderer>().sprite = exclaim;
        }
        if (!dat.game_triggers[7])
        {
            tile_grid[8][5].GetComponent<SpriteRenderer>().sprite = exclaim;
        }
        if (!dat.game_triggers[8])
        {
            tile_grid[13][7].GetComponent<SpriteRenderer>().sprite = exclaim;
        }
        if (!dat.game_triggers[9])
        {
            tile_grid[5][7].GetComponent<SpriteRenderer>().sprite = exclaim;
        }
        if (!dat.game_triggers[10])
        {
            tile_grid[9][7].GetComponent<SpriteRenderer>().sprite = exclaim;
        }
        if (!dat.game_triggers[11])
        {
            tile_grid[10][2].GetComponent<SpriteRenderer>().sprite = exclaim;
        }
        if (!dat.game_triggers[12])
        {
            tile_grid[13][1].GetComponent<SpriteRenderer>().sprite = exclaim;
        }
        if (!dat.game_triggers[13])
        {
            tile_grid[8][1].GetComponent<SpriteRenderer>().sprite = exclaim;
        }
        if (!dat.game_triggers[14])
        {
            tile_grid[9][0].GetComponent<SpriteRenderer>().sprite = exclaim;
        }
        if (!dat.game_triggers[4])
        {
            tile_grid[7][4].GetComponent<SpriteRenderer>().sprite = exclaim;
        }
    }
    // Update is called once per frame
    void Update()
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
        if (paused)
        {
            GUI.PauseMenu(true);
            if (Input.GetAxis("Close") > 0 && !p_cooldown)
            {
                dat.sfx[7].Play(0);
                paused = false;
                GUI.Unpause();
                p_cooldown = true;
            }
            else if (Input.GetAxis("Close") == 0)
            {
                p_cooldown = false;
            }
            return;
        }
        if (Input.GetAxis("Vertical") >= 0.1)
        {
            dir_cooldowns[1] = 0;
            if (dir_cooldowns[0] == 0)
            {
                dir_cooldowns[0] = dat.cooldowns;
                if (tile_grid[cur_x][cur_y].up && dat.north_exits[tile_grid[cur_x][cur_y].number] && cur_y < tile_grid[0].Count)
                {
                    move.Play(0);
                    x_dir = 0;
                    y_dir = -1;
                    cur_y--;
                }
            }
        }
        else if (Input.GetAxis("Vertical") <= -0.1)
        {
            dir_cooldowns[0] = 0;
            if (dir_cooldowns[1] == 0)
            {
                dir_cooldowns[1] = dat.cooldowns;
                if (tile_grid[cur_x][cur_y].down && dat.south_exits[tile_grid[cur_x][cur_y].number])
                {
                    move.Play(0);
                    x_dir = 0;
                    y_dir = 1;
                    cur_y++;
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
                dir_cooldowns[2] = dat.cooldowns;
                if (tile_grid[cur_x][cur_y].right && dat.east_exits[tile_grid[cur_x][cur_y].number] && cur_x < tile_grid.Count)
                {
                    move.Play(0);
                    x_dir = -1;
                    y_dir = 0;
                    cur_x++;
                }
            }

        }
        else if (Input.GetAxis("Horizontal") <= -0.1)
        {
            dir_cooldowns[2] = 0;
            if (dir_cooldowns[3] == 0)
            {
                dir_cooldowns[3] = dat.cooldowns;
                if (tile_grid[cur_x][cur_y].left && dat.west_exits[tile_grid[cur_x][cur_y].number] && cur_x > 0)
                {
                    move.Play(0);
                    x_dir = 1;
                    y_dir = 0;
                    cur_x--;
                }
            }

        }
        else
        {
            dir_cooldowns[2] = 0; dir_cooldowns[3] = 0;
        }
        if ((Input.GetAxis("Submit") > 0 || Input.GetAxis("Map") > 0) && cooldown == 0)
        {
            select.Play(0);
            dat.cur_level = tile_grid[cur_x][cur_y].number;
            dat.x_entry = x_dir;
            dat.y_entry = y_dir;
            dat.next_level = tile_grid[cur_x][cur_y].level;
            char[] chars = tile_grid[cur_x][cur_y].level.ToCharArray();

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
                SceneManager.LoadScene(tile_grid[cur_x][cur_y].level);
            }
            
            cooldown = dat.cooldowns;
           
        }
        else if ((Input.GetAxis("Submit") == 0 || Input.GetAxis("Map") == 0))
        {
            
            cooldown = 0;
        }
        if (Input.GetAxis("Close") > 0 && !p_cooldown)
        {
            dat.sfx[7].Play(0);
            paused = true;
            GUI.Pause(true);
            p_cooldown = true;
        }
        else if (Input.GetAxis("Close") == 0)
        {
            p_cooldown = false;
        }
        cam.transform.position = new Vector3(9.5f + cur_x * 2, 2-cur_y * 2, -10);

    }
}

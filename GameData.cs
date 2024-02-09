using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System.IO;
public class GameData : MonoBehaviour
{
    public float music_volume = 1;
    public float sfx_volume = 0.75f;
    public float red = 1;
    public float green = 1;
    public float blue = 1;
    public float game_speed = 1.5f;
    public float text_speed = 1.5f;
    public int cooldowns = 15;
    public AudioMixer mixer;
    public List<bool> items_obtained;//blocker,map, next 8 are shop, then right shop, then fish
    public List<bool> shop_triggers;
    public int artefacts;
    public List<Sprite> sprite_list = new List<Sprite>();//Same order as obtained
    public bool threats_toggled;
    public List<AudioSource> songs;
    public List<AudioSource> sfx;
    public int cur_level;
    public string next_level;
    public bool new_game = true;
    public List<bool> north_exits;
    public List<bool> south_exits;
    public List<bool> east_exits;
    public List<bool> west_exits;
    public List<bool> game_triggers;
    public int x_entry;
    public int y_entry;
    public bool puzzle;
    public bool shop = true;
    public int cur_track = 0;
    private void OnEnable()
    {
        DontDestroyOnLoad(gameObject);
        loadFromFile();
        shop = true;
        UpdateColour();
        UpdateAudio();
    }
    private void OnLevelWasLoaded()
    {
        UpdateColour();
        Controller c;
        if ((c = FindObjectOfType<Controller>()) != null)
        {
            if(c.special_level)
            {
                print("WHAT");
                if(puzzle || shop)
                {
                    print("THe fuck");
                    songs[cur_track].Stop();
                    cur_track = Random.Range(2, 4);
                    songs[cur_track].Play();
                }
                puzzle = false;
                shop = false;
            }
            else
            {
                if (!puzzle || shop)
                {
                    songs[cur_track].Stop();
                    cur_track = Random.Range(5,9);
                    songs[cur_track].Play();
                }
                puzzle = true;
                shop = false;
            }
        }
        else if(FindObjectOfType<Shop>() != null)
        {
            songs[cur_track].Stop();
            cur_track = 4;
            songs[cur_track].Play();
            shop = true;
        }
        else if(FindObjectOfType<MapManager>() != null)
        {
            if (shop)
            {
                if (!puzzle)
                {
                    songs[cur_track].Stop();
                    cur_track = Random.Range(2, 4);
                    songs[cur_track].Play();
                    shop = false;
                }
                else
                {
                    songs[cur_track].Stop();
                    cur_track = Random.Range(5, 9);
                    songs[cur_track].Play();
                    shop = false;
                }
            }
            shop = false;
        }
        else if(FindObjectOfType<Start_menu>() != null)
        {
            songs[cur_track].Stop();
            cur_track = 1;
            songs[cur_track].Play();
            puzzle = false;
            shop = true;
        }
        saveToFile();
    }
    public void newFile()
    {
        new_game = false;
        artefacts = 0;
        cur_level = 0;
        x_entry = 1;
        y_entry = 0;
        red = 1;
        green = 1;
        blue = 1;
        for (int i = 0; i < items_obtained.Count; i++)
        {
            items_obtained[i] = false;
        }
        for (int i = 0; i < shop_triggers.Count; i++)
        {
            shop_triggers[i] = false;
        }
        for (int i = 0; i < north_exits.Count; i++)
        {
            north_exits[i] = false;
        }
        for (int i = 0; i < south_exits.Count; i++)
        {
            south_exits[i] = false;
        }
        for (int i = 0; i < east_exits.Count; i++)
        {
            east_exits[i] = false;

        }
        for (int i = 0; i < west_exits.Count; i++)
        {
            west_exits[i] = false;
        }
        for (int i = 0; i < game_triggers.Count; i++)
        {
            game_triggers[i] = false;
        }
        saveToFile();
    }

    private string convertFloat(float f)
    {
        int i = (int) (f * 100);
        print(f);
        print("Float!");
        print(i);
        string s = "";
        if(i<100)
        {
            s += "0";
            if(i < 10)
            {
                s += "0";
            }
        }
        s += (i / 1);
        return s;
    }

    private void saveToFile()
    {
        string s = "";
        if(new_game)
        {
            s += "1";
        }
        else
        {
            s += "0";
        }
        s += convertFloat(music_volume);
        s += convertFloat(sfx_volume);
        s += convertFloat(red);
        s += convertFloat(green);
        s += convertFloat(blue);
        s += convertFloat(game_speed);
        s += convertFloat(text_speed);
        print(s);
        int t = cooldowns + 1;
        if(t < 10)
        {
            s += "0";
        }
        s += t;
        if(artefacts < 10)
        {
            s += "0" + artefacts;
        }
        else
        {
            s += artefacts;
        }
        
        
        if(cur_level < 100)
        {
            s += "0";
            if (cur_level < 10)
            {
                s += "0";
            }
        }
        s += cur_level;
        s += (x_entry + 1);
        s += (y_entry + 1); print("Geh"); print(s.Length);
        for (int i = 0; i < items_obtained.Count; i++)
        {
            if (items_obtained[i])
            {
                s += "1";
            }
            else
            {
                s += "0";
            }
        }
        for (int i = 0; i < shop_triggers.Count; i++)
        {
            if (shop_triggers[i])
            {
                s += "1";
            }
            else
            {
                s += "0";
            }
        }
        for (int i = 0; i < north_exits.Count; i++)
        {
            if (north_exits[i])
            {
                s += "1";
            }
            else
            {
                s += "0";
            }
        }
        for (int i = 0; i < south_exits.Count; i++)
        {
            if (south_exits[i])
            {
                s += "1";
            }
            else
            {
                s += "0";
            }
        }
        for (int i = 0; i < east_exits.Count; i++)
        {
            if (east_exits[i])
            {
                s += "1";
            }
            else
            {
                s += "0";
            }
        }
        for (int i = 0; i < west_exits.Count; i++)
        {
            if (west_exits[i])
            {
                s += "1";
            }
            else
            {
                s += "0";
            }
        }

        for (int i = 0; i < game_triggers.Count; i++)
        {
            if (game_triggers[i])
            {
                s += "1";
            }
            else
            {
                s += "0";
            }
        }
        /*if(!File.Exists(Application.dataPath + "/save.txt"))
        {
            File.Create(Application.dataPath + "/save.txt"); 
        }*/
        StreamWriter sw = new StreamWriter(Application.dataPath + "/save.txt");
        sw.Write(s);
        sw.Close();
        print(s.Length);
    }
    private int filePointer;
    private void loadFromFile()
    {
        StreamReader read = new StreamReader(Application.dataPath +"/save.txt");
        string line = read.ReadLine();
        if(line.Length < 10)
        {
            saveToFile();
            return;
        }
        read.Close();
        char[] s = line.ToCharArray();
        new_game = getNext(s) == 1;
        music_volume = getNextf(s);
        sfx_volume = getNextf(s);
        red = getNextf(s);
        green = getNextf(s);
        blue = getNextf(s);
        game_speed = getNextf(s);
        text_speed = getNextf(s);
        cooldowns = getNext(s) * 10 + getNext(s) - 1;
        artefacts = getNext(s) * 10 + getNext(s);
        cur_level = getNext(s) * 100 + getNext(s) * 10 + getNext(s);
        x_entry = getNext(s) - 1;
        y_entry = getNext(s) - 1;
        print("Bingus");
        print(filePointer);
        for (int i = 0; i < items_obtained.Count; i++)
        {
            items_obtained[i] = getNext(s) == 1;
        }
        for (int i = 0; i < shop_triggers.Count; i++)
        {
            shop_triggers[i] = getNext(s) == 1;
        }
        for (int i = 0; i < north_exits.Count; i++)
        {
            north_exits[i] = getNext(s) == 1;
        }
        for (int i = 0; i < south_exits.Count; i++)
        {
            south_exits[i] = getNext(s) == 1;
        }
        for (int i = 0; i < east_exits.Count; i++)
        {
            east_exits[i] = getNext(s) == 1;
        }
        for (int i = 0; i < west_exits.Count; i++)
        {
            west_exits[i] = getNext(s) == 1;
        }
        print(filePointer);
        for (int i = 0; i < game_triggers.Count; i++)
        {
            game_triggers[i] = getNext(s) == 1;
        }
        print("Loading done");
    }
    int getNext(char[] s)
    {
        filePointer++;
        return s[filePointer - 1] - '0';
    }
    float getNextf(char[] s)
    {
        print(s[filePointer]);
        print(s[filePointer + 1]);
        print(s[filePointer + 2]);
        float f = getNext(s) * 100f+ getNext(s) * 10f + getNext(s);
        print("Float?");
        print(f);
        return f / 100f;
    }
    public void UpdateAudio()
    {
        if (music_volume == 0)
        {
            mixer.SetFloat("MusicVolume", -80f);
        }
        else
        {
            mixer.SetFloat("MusicVolume", Mathf.Log10(music_volume) * 20);
        }
        if (sfx_volume == 0)
        {
            mixer.SetFloat("SfxVolume", -80f);
        }
        else
        {
            mixer.SetFloat("SfxVolume", Mathf.Log10(sfx_volume) * 20);
        }
    }
    public void UpdateColour()
    {
        SpriteRenderer[] allSprites = FindObjectsOfType<SpriteRenderer>();
        for (int i = 0; i < allSprites.Length; i++)
        {
            allSprites[i].color = new Color(red, green, blue, 1);
        }
    }
    public Color GetColour()
    {
        return new Color(red, green, blue,1);
    }
}

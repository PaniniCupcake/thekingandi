using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Shop : MonoBehaviour
{
    public ShopTextbox text;
    public SpriteRenderer dialogue_window;
    private Sprite empty;
    public GameData dat;
    public List<GameObject> menus;
    public List<SpriteRenderer> item;
    public List<SpriteRenderer> back;
    public List<SpriteRenderer> confirm;
    public List<SpriteRenderer> info;
    public List<SpriteRenderer> description;
    public List<SpriteRenderer> main;
    public List<Sprite> itemsprites;
    public List<Sprite> confirmsprites;
    public List<Sprite> infosprites;
    public List<Sprite> descriptionsprites;
    public List<Sprite> mainsprites;
    public List<Sprite> numbersprites;
    public List<SpriteRenderer> numbers;
    public List<int> dir_cooldowns = new List<int>();
    public Transform menu_selector;
    public int cooldown;
    public int back_cooldown;
    private int cur_menu;//0 is default, 1 is shop, 2 is dialogue, 3 is buy, 4 is confirm
    public int option_selected;
    public AudioSource move;
    public AudioSource select;
    public AudioSource pay;
    public bool dialogue;
    private int item_select_store;
    public List<bool> left_items_available;
    public List<bool> right_items_available;
    private List<string> intro = new List<string>();
    private List<List<string>> dialogues = new List<List<string>>();
    private List<List<string>> descriptions = new List<List<string>>();
    public GameObject datfab;
    private void Start()
    {
        dir_cooldowns.Add(0);
        dir_cooldowns.Add(0);
        dir_cooldowns.Add(0);
        dir_cooldowns.Add(0);
        dat = FindObjectOfType<GameData>();
        if (dat == null)
        {
            dat = Instantiate(datfab, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<GameData>();
        }
        move = dat.sfx[2];
        select = dat.sfx[0];
        pay = dat.sfx[9];
        for (int i = 0; i < numbers.Count; i++)
        {
            numbers[i].sprite = numbersprites[dat.artefacts];
        }
        for (int i = 0; i < left_items_available.Count; i++)
        {
            if(dat.items_obtained[i+2])
            {
                item[i].sprite = itemsprites[0];
            }
            else if(!left_items_available[i])
            {
                item[i].sprite = itemsprites[1];
            }
            else
            {
                item[i].sprite = itemsprites[i + 2];
            }
        }
        for (int i = 0; i < right_items_available.Count; i++)
        {
            if (dat.items_obtained[i+10])
            {
                item[i+8].sprite = itemsprites[0];
            }
            else if (!right_items_available[i])
            {
                item[i+8].sprite = itemsprites[1];
            }
            else
            {
                item[i+8].sprite = itemsprites[i + 10];
            }
        }

        /*intro.Add("Welcome!>");
        intro.Add("Welcome!!!>");
        intro.Add("...What is happening?>");
        intro.Add("What's happening is you're about to embark on a wondrous commercial adventure!>");
        intro.Add("(Adventure may not be wondrous or adventurous)>");
        intro.Add("Since when can chess pieces set up a shop?>");
        intro.Add("Don't be closed minded Mr. King!>");
        intro.Add("We've got good stuff for sale!>");
        intro.Add("Guaranteed to assist a lonely monarch in his travels!>");
        intro.Add("For a low, low price!>");
        intro.Add("We don't have any money.>");
        intro.Add("Not a problem!>");
        intro.Add("Not a problem at all!>");
        intro.Add("We buy valuable artefacts!>");
        intro.Add("Come take a look!>");*/
        {
            intro.Add("#sw0Well hello there dear customer!>");
            intro.Add("#sb0Welcome to the pawn shop!>");
            intro.Add("#kw0.......");
            intro.Add("What is happening?>");
            intro.Add("#sw2What's happening is you're about to embark on a wondrous commercial adventure!>");
            intro.Add("#sb1#w0(Commercial adventure may not be wondrous or adventurous)>");
            intro.Add("#kw0#b0How did you set up a shop?>");
            intro.Add("#sw2#b0Times are changing, my royal friends!>");
            intro.Add("#sb6#w0You should know that better than most!>");
            intro.Add("#sw2#b0And we've got good stuff for sale!>");
            intro.Add("#sb2#w0Guaranteed to assist lonely monarchs in their travels!>");
            intro.Add("#sw2#b0For a low, low price!>");
            intro.Add("#kw0We don't have money.>");
            intro.Add("#sb5#w0Yeah, nobody has actually come up with anything you could call currency yet.>");
            intro.Add("#sw2#b0So we just take the bartering approach!>");
            intro.Add("#sb2#w0If you find any valuable artefacts, you can exchange them for high quality products!>");
            intro.Add("#sw2#b0Come take a look!>");
        }
        {
            List<string> s1 = new List<string>();
            s1.Add("#sb2Great place, isn't it?>");
            s1.Add("#sw0We found this little nook and settled down here a while back.>");
            s1.Add("#sb0Stocked it right up with a bunch of stuff we definitely obtained legally.>");
            s1.Add("#sw2And now we just sell the junk for better stuff!>");
            s1.Add("#sb6Not that we would sell junk to our royal guests.>");
            s1.Add("#sw3We're only offering you guys exclusive top of the line bargains!");
            List<string> s2 = new List<string>();
            s2.Add("#sb4We're pretty much calling anything that is relatively valuable an artefact.>");
            s2.Add("#sw5Should probably be less lenient with the definition but that would require research I can't be bothered to do.>");
            s2.Add("#sb2Bottom line, if it looks expensive, we'll trade for it!");
            List<string> s3 = new List<string>();
            s3.Add("#sb2We're offering you a selection of our finest wares from the back!>");
            s3.Add("#sw2Whenever we put them on display, they fly right of the shelves!>");
            s3.Add("#sb1(We put them back in storage because people stop coming in)>");
            s3.Add("#sw0If you want to know more about an item than its icon, just ask us for a description!>");
            List<string> s4 = new List<string>();
            s4.Add("#sb2Oh King, surely you know that items like those are out of your current price range.");
            s4.Add("#sb4(Or not for sale)>");
            s4.Add("#sw3Why care about it when we have a wonderful selection already hand picked out for you?>");
            s4.Add("#sb0But if you insist on discussing the clutter behind us, we'd be happy to oblige.>");
            s4.Add("#sw1(Just buy something afterwards.)");
            List<string> s5 = new List<string>();
            s5.Add("#sb2That's some very rare, antique rice!>");
            s5.Add("#sw2It comes with a neat story attached!>");
            s5.Add("#sb5Once, there was a servant who performed a great service for an old king.>");
            s5.Add("#sw5When the king offered repayment, they requested only one thing: a grain of rice on the first square of a chessboard, doubling the amount on each subsequent square.>");
            s5.Add("#sb5Surprised at how minor the request seemed, the king granted it, ordering his servants to start placing rice on the chessboard.>");
            s5.Add("#sw5He soon realised his mistake as the pile of rice grew higher and higher.>");
            s5.Add("#sb2So he had the servant executed for their insolence.>");
            s5.Add("#sw2Pretty good story, wouldn't you say?>");
            s5.Add("#kw0How did you end up with the rice from the story?>");
            s5.Add("#sb3By not asking that question to the guy we bought it from!>");
            List<string> s6 = new List<string>();
            s6.Add("#sb1I'll be honest, that's part of a somewhat sketchy situation.>");
            s6.Add("#sw1Some guy came by with his kids and told us to put this picture on display or else.>");
            s6.Add("#sb4We still sometimes see him looking through the door to check if it's still hanging...>");
            s6.Add("#sw2Still, sales have been up since so there's no real reason to take it down.");
            List<string> s7 = new List<string>();
            s7.Add("#sb2The painting is a souvenier we brought back from the vatican!>");
            s7.Add("#sw1It was a nice trip, although there were a few too many bishops there.>");
            s7.Add("#sb5Not really the best place for two pawns to be hanging around together.");
            List<string> s8 = new List<string>();
            s8.Add("#sb5Sometimes we ask our customers for feedback to help improve the way we run our business.>");
            s8.Add("#sw1I'll be honest, most of the messages we receive contain random unrelated sequences of words.>");
            s8.Add("#sb1Any new ones we get just go in straight in the box now.>");
            s8.Add("#sw1But they just keep coming...");
            List<string> s9 = new List<string>();
            s9.Add("#sb0You'd be surprised how many guns go through here.>");
            s9.Add("#sw0We keep stocking the sniper rifles because bishops keep buying them for some reason.>");
            s9.Add("#sb2As for the shotgun, the guy who sold it to us actually looked a lot like you two!>");
            s9.Add("#sw5Looked mad about giving it up though...");
            List<string> s10 = new List<string>();
            s10.Add("#sb5That mayyy have been an impulse buy...>");
            s10.Add("#sw0We were watching a tutorial online when we realised that we might want to invade Scandinavia one day.>");
            s10.Add("#sb2We couldn't get the recommended brand, but it's better than nothing, right?>");
            s10.Add("#sw6Mark my words, when we need it, we'll be ready.");
            List<string> s11 = new List<string>();
            s11.Add("#sb1Do we really have to do this?>");
            s11.Add("#sw1Like, do you actually care about two random pawns you barely know?>");
            s11.Add("#sb5But fine, if you really want to ask about that kind of stuff, ask away.>");
            List<string> s12 = new List<string>();
            s12.Add("#sb0Our past?>");
            s12.Add("#sw0I was a a8 pawn.>");
            s12.Add("#sb0And I was an g1 pawn.>");
            s12.Add("#sw5Then the White Queen took over and everything changed.>");
            s12.Add("#sb0And now we own a shop.");
            List<string> s13 = new List<string>();
            s13.Add("#sb4Getting awfully personal now aren't you?>");
            s13.Add("#sw4We're business partners and that's all you need to know.");
            List<string> s14 = new List<string>();
            s14.Add("#sb0Pah, that's old talk!>");
            s14.Add("#sw2A pawn can achieve more than just being promoted these days!>");
            s14.Add("#sb4Plus, we aren't allowed to promote to queens any more so what would even be the point?>");
            s14.Add("#sw4You know, some people say being able to promote to a queen makes us girls, but I think we're just inanimate objects.");
            List<string> s15 = new List<string>();
            s15.Add("#sb4Hmm, I guess eventually we'll probably have more stuff than we'll ever need.>");
            s15.Add("#sw4I'm pretty sure that happened a while ago.>");
            s15.Add("#sb0Eh, true.");
            s15.Add("#sb1I guess for the foreseeable future we'll just be upgrading our supply.>");
            s15.Add("#sw2Hopefully with your help!");
            List<string> s16 = new List<string>();
            s16.Add("#sb4Eh, you guys are alright.>");
            s16.Add("#sw4Yeah.>");
            s16.Add("#sb4......>");
            s16.Add("#sw4......>");
            s16.Add("#sb4Keep buying our stuff.>");
            List<string> s17 = new List<string>();
            s17.Add("#sb5She's ambitious to say the least.>");
            s17.Add("#sw5Gives us a lot of freedom, but that's probably just because she doesn't really care about what the little guys get up to.>");
            s17.Add("#sb5You guys may have been boring, but at least you were stable.>");
            s17.Add("#sw2You can consider us anti queen as long as you're shopping here and she isn't!>");
            List<string> s18 = new List<string>();
            s18.Add("#sb0Hm, as a general note I'd say that rooks take their role as the second most valuable piece way too seriously.>");
            s18.Add("#sw0They're always trying to get in the good books with monarchs. I hear some of them are even castling with queen nowadays.>");
            s18.Add("#sb2Bishops are pretty lame. They're always arguing with each other about which tiles are heretical to land on.>");
            s18.Add("#sw0Knights can be classified as either way too mad that they have to be a horsey, or way too into being a horse.>");
            s18.Add("#sb5I'm honestly not sure which is worse.>");
            s18.Add("#sw2Regardless, pawns are naturally cool as always.>");
            s18.Add("#sb2Not as cool as us though.");
            dialogues.Add(s1);
            dialogues.Add(s2);
            dialogues.Add(s3);
            dialogues.Add(s4);
            dialogues.Add(s5);
            dialogues.Add(s6);
            dialogues.Add(s7);
            dialogues.Add(s8);
            dialogues.Add(s9);
            dialogues.Add(s10);
            dialogues.Add(s11);
            dialogues.Add(s12);
            dialogues.Add(s13);
            dialogues.Add(s14);
            dialogues.Add(s15);
            dialogues.Add(s16);
            dialogues.Add(s17);
            dialogues.Add(s18);
        }
        {
            List<string> s1 = new List<string>();
            s1.Add("#sb5I'd act like this is something valuable, but honestly we just found it on the ground somewhere.");
            s1.Add("#sw4It's just some key.>");
            s1.Add("#sb4No clue what it unlocks.>");
            s1.Add("#sw0Probably nothing important.>");
            s1.Add("#sb0Hell, if you buy enough items we might even give it to you for free.");
            List<string> s2 = new List<string>();
            s2.Add("#sb2Ever feel like planning ahead is too much work?>");
            s2.Add("#sw2With the undo key, you can just go back a move!.>");
            s2.Add("#sb2Just keep on pressing it until you finally realise you should think before acting!");
            List<string> s3 = new List<string>();
            s3.Add("#sb2If you're finding a puzzle too hard, this is the thing for you!>");
            s3.Add("#sw2Just read through this handy hints book to receive some helpful info!>");
            s3.Add("#kw0It doesn't look like a hints book...>");
            s3.Add("#sw2Come on, surely you've heard of not judging a book by its cover!");
            List<string> s4 = new List<string>();
            s4.Add("#sb0We empathise that it must be difficult to not move into check.>");
            s4.Add("#sw2With the threat detector, you can simply toggle to see which tiles are threatened by which colour!>");
            s4.Add("#sb3Of course, you could just check which pieces are where like a half decent chess player.>");
            s4.Add("#sw2But who has time for that?>");
            s4.Add("#sb3Warning: Tiles marked by threat detector do not necessarily dictate where each king may or not move when certain special tiles are on the board.>");
            List<string> s5 = new List<string>();
            s5.Add("#sb2What fun is a journey without a little music, eh?>");
            s5.Add("#sw2Sure, you've already got that, but you're royals!>");
            s5.Add("#sb2You should be able to harness it, bend it to your will!>");
            s5.Add("#sw2And with this handy jukebox, you can do just that!");
            List<string> s6 = new List<string>();
            s6.Add("#sb2Tired of this sleek, black and white aesthetic?>");
            s6.Add("#sw2Just buy this easel and you can change the colour scheme to whatever garish hue you want!.>");
            s6.Add("#sb4(Seriously though, you'll probably just mess around with it for a few minutes and then set it back to default.)>");
            List<string> s7 = new List<string>();
            s7.Add("You're not supposed to be here.");
            List<string> s8 = new List<string>();
            s8.Add("You're not supposed to be here.");
            List<string> s9 = new List<string>();
            s9.Add("#sb4Do you not know how to play chess?>");
            s9.Add("#sw3Probably not, otherwise you wouldn't have lost to the queen.>");
            s9.Add("#sb0Maybe giving this a read will help you out?>");
            s9.Add("#sw1Buy it first though, we aren't a library.");
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
            List<string> s16 = new List<string>();
            s16.Add("You're not supposed to be here.");
            descriptions.Add(s1);
            descriptions.Add(s2);
            descriptions.Add(s3);
            descriptions.Add(s4);
            descriptions.Add(s5);
            descriptions.Add(s6);
            descriptions.Add(s7);
            descriptions.Add(s8);
            descriptions.Add(s9);
            descriptions.Add(s10);
            descriptions.Add(s11);
            descriptions.Add(s12);
            descriptions.Add(s13);
            descriptions.Add(s14);
            descriptions.Add(s15);
            descriptions.Add(s16);
        }

        if (!dat.shop_triggers[0])
        {
            dat.shop_triggers[0] = true;
            dialogue = true;
            text.startStrings = intro;
            text.gameObject.SetActive(true);
            
        }
        if(!dat.shop_triggers[1])
        {
            for(int i = 2;i<10;i++)
            {
                info[i].sprite = empty;
            }
        }
        if(!dat.shop_triggers[2])
        {
            for (int i = 12; i < 19; i++)
            {
                info[i].sprite = empty;
            }
        }
        if (!dat.shop_triggers[3])
        {
            for (int i = 4; i < 10; i++)
            {
                info[i].sprite = empty;
            }
        }
    }
    void Update()
    {
        if (dialogue)
        {
            return;
        }
        if (dat.shop_triggers[1])
        {
            info[2].sprite = infosprites[5];
            info[3].sprite = infosprites[7];
        }
        if (dat.shop_triggers[2])
        {
            for (int i = 12; i < 19; i++)
            {
                info[i].sprite = infosprites[i*2-1];
            }
        }
        if (dat.shop_triggers[3])
        {
            for (int i = 4; i < 10; i++)
            {
                info[i].sprite = infosprites[i*2+1];
            }
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
        if (back_cooldown > 0)
        {
            back_cooldown--;
        }
        int prev_action = option_selected;
        if (Input.GetAxis("Vertical") >= 0.1)
        {
            dir_cooldowns[1] = 0;
            if (dir_cooldowns[0] == 0)
            {
                move.Play(0);
                dir_cooldowns[0] = dat.cooldowns;

                if (cur_menu == 0)
                {
                    option_selected += 2;
                    option_selected %= 3;
                }
                else if (cur_menu == 1)
                {
                    if (option_selected < 9)
                    {
                        option_selected -= 2;
                        if (option_selected < 0)
                        {
                            option_selected = 8;
                        }
                    }
                    else
                    {
                        option_selected -= 2;
                        if (option_selected < 9)
                        {
                            option_selected = 17;
                        }
                    }
                }
                else if (cur_menu == 2) //Guh
                {

                    if (option_selected < 11)
                    {
                        if (dat.shop_triggers[1])
                        {
                            if (dat.shop_triggers[3])
                            {
                                option_selected += 10;
                                option_selected %= 11;
                            }
                            else
                            {
                                if (option_selected == 0)
                                {
                                    option_selected = 10;
                                }
                                else if (option_selected == 10)
                                {
                                    option_selected = 3;
                                }
                                else
                                {
                                    option_selected--;
                                }
                            }
                        }
                        else
                        {
                            if (option_selected == 0)
                            {
                                option_selected = 10;
                            }
                            else if (option_selected == 10)
                            {
                                option_selected = 1;
                            }
                            else
                            {
                                option_selected = 0;
                            }
                        }

                    }
                    else
                    {
                        if (dat.shop_triggers[2])
                        {
                            option_selected += 8;
                            option_selected = (option_selected - 11) % 9 + 11;
                        }
                        else
                        {
                            option_selected = 30 - option_selected;
                        }

                    }
                }
                else if (cur_menu == 3)
                {
                    option_selected += 2;
                    option_selected %= 3;
                }
                else
                {
                    option_selected++;
                    option_selected %= 2;
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
                if (cur_menu == 0)
                {
                    option_selected += 1;
                    option_selected %= 3;
                }
                else if (cur_menu == 1)
                {
                    if (option_selected < 9)
                    {
                        option_selected += 2;
                        if (option_selected == 9)
                        {
                            option_selected = 8;
                        }
                        else if (option_selected == 10)
                        {
                            option_selected = 0;
                        }
                    }
                    else
                    {
                        option_selected += 2;
                        if (option_selected == 18)
                        {
                            option_selected = 17;
                        }
                        else if (option_selected == 19)
                        {
                            option_selected = 9;
                        }
                    }
                }
                else if (cur_menu == 2)
                {
                    if (option_selected < 11)
                    {
                        if (dat.shop_triggers[1])
                        {
                            if (dat.shop_triggers[3])
                            {
                                option_selected += 1;
                                option_selected %= 11;
                            }
                            else
                            {
                                if (option_selected == 10)
                                {
                                    option_selected = 0;
                                }
                                else if (option_selected == 3)
                                {
                                    option_selected = 10;
                                }
                                else
                                {
                                    option_selected++;
                                }
                            }
                        }
                        else
                        {
                            if (option_selected == 10)
                            {
                                option_selected = 0;
                            }
                            else if (option_selected == 1)
                            {
                                option_selected = 10;
                            }
                            else
                            {
                                option_selected = 1;
                            }
                        }
                    }
                    else
                    {
                        if (dat.shop_triggers[2])
                        {
                            option_selected += 1;
                            option_selected = (option_selected - 11) % 9 + 11;
                        }
                        else
                        {
                            option_selected = 30 - option_selected;
                        }
                    }
                }
                else if (cur_menu == 3)
                {
                    option_selected += 1;
                    option_selected %= 3;
                }
                else
                {
                    option_selected++;
                    option_selected %= 2;
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
                if (cur_menu == 1)
                {
                    move.Play(0);
                    if (option_selected == 8)
                    {
                        option_selected = 17;
                    }
                    else if (option_selected == 17)
                    {
                        option_selected = 8;
                    }
                    else if (option_selected < 9)
                    {
                        if (option_selected % 2 == 0)
                        {
                            option_selected += 1;
                        }
                        else
                        {
                            option_selected += 8;
                        }
                    }
                    else
                    {
                        if (option_selected % 2 == 1)
                        {
                            option_selected += 1;
                        }
                        else
                        {
                            option_selected -= 10;
                        }
                    }
                }
                else if (cur_menu == 2)
                {
                    move.Play(0);
                    if (option_selected < 11)
                    {
                        if (dat.shop_triggers[2])
                        {
                            option_selected += 11;
                            option_selected = Mathf.Min(option_selected, 19);
                        }
                        else
                        {
                            if (option_selected == 10)
                            {
                                option_selected = 19;
                            }
                            else
                            {
                                option_selected = 11;
                            }
                        }
                    }
                    else
                    {
                        if (option_selected == 19)
                        {
                            option_selected = 10;
                        }
                        else if (dat.shop_triggers[1])
                        {
                            if (dat.shop_triggers[3])
                            {
                                option_selected -= 11;
                            }
                            else
                            {
                                option_selected = Mathf.Min(option_selected - 11, 3);
                            }
                        }
                        else
                        {
                            option_selected = Mathf.Min(option_selected - 11, 1);
                        }
                    }
                }
            }
        }
        else if (Input.GetAxis("Horizontal") <= -0.1)
        {
            dir_cooldowns[2] = 0;
            if (dir_cooldowns[3] == 0)
            {

                dir_cooldowns[3] = dat.cooldowns;
                if (cur_menu == 1)
                {
                    move.Play(0);
                    if (option_selected == 8)
                    {
                        option_selected = 17;
                    }
                    else if (option_selected == 17)
                    {
                        option_selected = 8;
                    }
                    else if (option_selected < 9)
                    {
                        if (option_selected % 2 == 0)
                        {
                            option_selected += 10;
                        }
                        else
                        {
                            option_selected -= 1;
                        }
                    }
                    else
                    {
                        if (option_selected % 2 == 1)
                        {
                            option_selected -= 8;
                        }
                        else
                        {
                            option_selected -= 1;
                        }
                    }
                }
                else if (cur_menu == 2)
                {
                    move.Play(0);
                    if (option_selected < 11)
                    {
                        if (dat.shop_triggers[2])
                        {
                            option_selected += 11;
                            option_selected = Mathf.Min(option_selected, 19);
                        }
                        else
                        {
                            if (option_selected == 10)
                            {
                                option_selected = 19;
                            }
                            else
                            {
                                option_selected = 11;
                            }
                        }
                    }
                    else
                    {
                        if (option_selected == 19)
                        {
                            option_selected = 10;
                        }
                        else if (dat.shop_triggers[1])
                        {
                            if(dat.shop_triggers[3])
                            {
                                option_selected -= 11;
                            }
                            else
                            {
                                option_selected = Mathf.Min(option_selected - 11,3);
                            }
                        }
                        else
                        {
                            option_selected = Mathf.Min(option_selected - 11, 1);
                        } 
                    }
                }
            }
        }
        else
        {
            dir_cooldowns[2] = 0; dir_cooldowns[3] = 0;
        }
        if (Input.GetAxis("Submit") > 0 && cooldown == 0)
        {
            select.Play(0);
            if (cur_menu == 0)
            {
                if (option_selected == 0)
                {
                    menus[5].SetActive(false);
                    menus[1].SetActive(true);
                    menus[0].SetActive(true);
                    cur_menu = 1;
                    main[0].sprite = mainsprites[1];
                    main[3].sprite = mainsprites[1];
                    prev_action = 0;
                }
                else if (option_selected == 1)
                {
                    menus[5].SetActive(false);
                    menus[4].SetActive(true);
                    option_selected = 0;
                    cur_menu = 2;
                    main[1].sprite = mainsprites[3];
                    main[4].sprite = mainsprites[3];
                    prev_action = 0;
                }
                else
                {
                    SceneManager.LoadScene("Shop_outside");
                    dat.x_entry = 0;
                    dat.y_entry = 1;
                }
            }
            else if (cur_menu == 1)
            {
                if (option_selected == 8 || option_selected == 17)
                {
                    back[0].sprite = infosprites[37];
                    back[1].sprite = infosprites[37];
                    cur_menu = 0;
                    option_selected = 0;
                    menus[1].SetActive(false);
                    menus[5].SetActive(true);
                    menus[0].SetActive(false);
                    prev_action = 0;
                }
                else
                {
                    if(option_selected < 8)
                    {
                        if (left_items_available[option_selected] && !dat.items_obtained[option_selected + 2])
                        {
                            item_select_store = option_selected;
                            back[0].sprite = infosprites[37];
                            back[1].sprite = infosprites[37];
                            cur_menu = 3;
                            option_selected = 0;
                            menus[1].SetActive(false);
                            menus[2].SetActive(true);
                            prev_action = 0;
                        }
                    }
                    else
                    {
                        if (right_items_available[option_selected - 9] && !dat.items_obtained[option_selected + 1])
                        {
                            item_select_store = option_selected;
                            back[0].sprite = infosprites[37];
                            back[1].sprite = infosprites[37];
                            cur_menu = 3;
                            option_selected = 0;
                            menus[1].SetActive(false);
                            menus[2].SetActive(true);
                            prev_action = 0;
                        }
                    }
                    

                }
            }
            else if (cur_menu == 2)
            {
                if (option_selected == 10 || option_selected == 19)
                {

                    info[10].sprite = infosprites[37];
                    info[19].sprite = infosprites[37];
                    cur_menu = 0;
                    option_selected = 0;
                    menus[4].SetActive(false);
                    menus[5].SetActive(true);
                    prev_action = 0;
                }
                else
                {
                    dialogue = true;
                    int t = option_selected;
                    if(t > 10)
                    {
                        t--;
                    }
                    text.startStrings = dialogues[t];
                    text.gameObject.SetActive(true);
                    if (option_selected == 0)
                    {
                        dat.shop_triggers[1] = true;

                    }
                    else if (option_selected == 11)
                    {
                        dat.shop_triggers[2] = true;
                    }
                    else if (option_selected == 3)
                    {
                        dat.shop_triggers[3] = true;
                    }
                }
            }
            else if (cur_menu == 3)
            {
                if (option_selected == 2)
                {
                    description[prev_action].sprite = descriptionsprites[prev_action * 2 + 1];
                    description[prev_action + 3].sprite = descriptionsprites[prev_action * 2 + 1];
                    cur_menu = 1;
                    option_selected = item_select_store;
                    menus[2].SetActive(false);
                    menus[1].SetActive(true);
                    prev_action = 0;
                }
                else if(option_selected == 1)
                {
                    
                    if(item_select_store > 8)
                    {
                        text.startStrings = descriptions[item_select_store-1];
                    }
                    else
                    {
                        text.startStrings = descriptions[item_select_store];
                    }
                    dialogue = true;
                    text.gameObject.SetActive(true);
                }
                else
                {
                    int tally = 0;
                    for(int i =0;i<dat.items_obtained.Count;i++)
                    {
                        if(dat.items_obtained[i])
                        {
                            tally++;
                        }
                    }
                    if (dat.artefacts > 0 || (item_select_store == 0 && tally >= 5))
                    {
                        description[prev_action].sprite = descriptionsprites[prev_action * 2 + 1];
                        description[prev_action + 3].sprite = descriptionsprites[prev_action * 2 + 1];
                        cur_menu = 4;
                        menus[2].SetActive(false);
                        menus[3].SetActive(true);
                        prev_action = 0;
                    }
                }
            }
            else
            {
                if (option_selected == 0)
                {
                    pay.Play(0);
                    select.Pause();
                    if (dat.artefacts > 0)
                    {
                        dat.artefacts--;
                    }
                    for (int i = 0; i < numbers.Count; i++)
                    {
                        numbers[i].sprite = numbersprites[dat.artefacts];
                    }
                        if (item_select_store < 8)
                        {
                            dat.items_obtained[item_select_store + 2] = true;
                            item[item_select_store].sprite = itemsprites[0];
                        }
                        else
                        {
                            print("Buh?");
                            dat.items_obtained[item_select_store + 1] = true;
                            item[item_select_store - 1].sprite = itemsprites[0];
                        }
                    dat.game_triggers[4] = true;
                    confirm[prev_action].sprite = confirmsprites[prev_action * 2 + 1];
                    confirm[prev_action + 2].sprite = confirmsprites[prev_action * 2 + 1];
                    cur_menu = 1;
                    option_selected = item_select_store;
                    menus[3].SetActive(false);
                    menus[1].SetActive(true);
                    prev_action = 0;
                }
                else
                {
                    confirm[prev_action].sprite = confirmsprites[prev_action * 2 + 1];
                    confirm[prev_action + 2].sprite = confirmsprites[prev_action * 2 + 1];
                    cur_menu = 3;
                    option_selected = 0;
                    menus[3].SetActive(false);
                    menus[2].SetActive(true);
                    prev_action = 0;
                }
                
            }
            cooldown = -1;
        }
        else if (Input.GetAxis("Submit") == 0)
        {
            cooldown = 0;
        }
        if (Input.GetAxis("Close") > 0 && back_cooldown == 0)
        {
            back_cooldown = dat.cooldowns;
            select.Play(0);
            print(prev_action);
            if(cur_menu == 0)
            {
                SceneManager.LoadScene("Shop_outside");
            }
            else if (cur_menu == 1)
            {
                back[prev_action].sprite = infosprites[prev_action * 2 + 1];
                cur_menu = 0;
                option_selected = 0;
                menus[1].SetActive(false);
                menus[5].SetActive(true);
                menus[0].SetActive(false);
            }
            else if (cur_menu == 2)
            {
                info[prev_action].sprite = infosprites[prev_action * 2 + 1];
                cur_menu = 0;
                option_selected = 0;
                menus[4].SetActive(false);
                menus[5].SetActive(true);
            }
            else if (cur_menu == 3)
            {
                description[prev_action].sprite = descriptionsprites[prev_action * 2 + 1];
                description[prev_action + 3].sprite = descriptionsprites[prev_action * 2 + 1];
                cur_menu = 1;
                option_selected = item_select_store;
                menus[2].SetActive(false);
                menus[1].SetActive(true);

            }
            else
            {
                confirm[prev_action].sprite = confirmsprites[prev_action * 2 + 1];
                confirm[prev_action + 3].sprite = confirmsprites[prev_action * 2 + 1];
                cur_menu = 2;
                    option_selected = 0;
                    menus[3].SetActive(false);
                    menus[2].SetActive(true);
            }
            prev_action = 0;
        }
        else if(Input.GetAxis("Close") == 0)
        {
            back_cooldown = 0;
        }

        if (cur_menu == 0)
        {
            main[prev_action].sprite = mainsprites[prev_action * 2 + 1];
            main[option_selected].sprite = mainsprites[option_selected * 2];
            main[prev_action + 3].sprite = mainsprites[prev_action * 2 + 1];
            main[option_selected + 3].sprite = mainsprites[option_selected * 2];
        }
        else if (cur_menu == 1)
        {
            if (prev_action == 8)
            {
                back[0].sprite = infosprites[37];
            }
            else if (prev_action == 17)
            {
                back[1].sprite = infosprites[37];
            }
            if (option_selected == 8)
            {
                menu_selector.transform.position = new Vector3(0, 25, 0);
                back[0].sprite = infosprites[36];
            }
            else if (option_selected == 17)
            {
                menu_selector.transform.position = new Vector3(0, 25, 0);
                back[1].sprite = infosprites[36];
            }
            else
            {
                
                if (option_selected <= 8)
                {
                    menu_selector.transform.position = new Vector3(-165.5f / 21f, 83f / 21f, 0);
                    if (option_selected % 2 == 0)
                    {
                        menu_selector.transform.position += new Vector3(0, -(38f / 21f) * option_selected / 2, 0);
                    }
                    else
                    {
                        menu_selector.transform.position += new Vector3(38f / 21f, -(38f / 21f) * (option_selected - 1) / 2, 0);
                    }
                }
                else
                {
                    menu_selector.transform.position = new Vector3(127.5f / 21f, 83f / 21f, 0);
                    int t = option_selected - 9;
                    if (t % 2 == 0)
                    {
                        menu_selector.transform.position += new Vector3(0, -(38f / 21f) * t / 2, 0);
                    }
                    else
                    {
                        menu_selector.transform.position += new Vector3(38f / 21f, -(38f / 21f) * (t - 1) / 2, 0);
                    }
                }
            }
        }
        else if (cur_menu == 2)
        {
            if (prev_action == 10 || prev_action == 19)
            {
                info[prev_action].sprite = infosprites[37];
            }
            else if (prev_action < 10)
            {
                info[prev_action].sprite = infosprites[prev_action * 2 + 1];
            }
            else if (prev_action > 10)
            {
                info[prev_action].sprite = infosprites[prev_action * 2 - 1];
            }
            if (option_selected == 10 || option_selected == 19)
            {
                info[option_selected].sprite = infosprites[36];
            }
            else if (option_selected < 10)
            {
                info[option_selected].sprite = infosprites[option_selected * 2];
            }
            else if (option_selected > 10)
            {
                info[option_selected].sprite = infosprites[option_selected * 2 - 2];
            }
        }
        else if (cur_menu == 3)
        {
            description[prev_action].sprite = descriptionsprites[prev_action * 2 + 1];
            description[prev_action+3].sprite = descriptionsprites[prev_action * 2 + 1];
            if (item_select_store <= 7)
            {
                description[option_selected+3].sprite = descriptionsprites[option_selected * 2];
            }
            else
            {
                description[option_selected].sprite = descriptionsprites[option_selected * 2];
            }
            
        }
        else
        {
            confirm[prev_action].sprite = confirmsprites[prev_action * 2 + 1];
            confirm[prev_action+2].sprite = confirmsprites[prev_action * 2 + 1];
            if (item_select_store > 7)
            {
                confirm[option_selected].sprite = confirmsprites[option_selected * 2];
            }
            else
            {
                confirm[option_selected+2].sprite = confirmsprites[option_selected * 2];
            }
            
        }
        
    }

    private void reset_sprites()
    {
        /*public List<SpriteRenderer> item;
        public List<SpriteRenderer> back;
        public List<SpriteRenderer> confirm;
        public List<SpriteRenderer> info;
        public List<SpriteRenderer> description;
        public List<SpriteRenderer> main;*/
        for(int i = 0;i < item.Count;i++)
        {
            
        }
        for (int i = 0; i < back.Count; i++)
        {

        }
        for (int i = 0; i < confirm.Count; i++)
        {

        }
        for (int i = 0; i < info.Count; i++)
        {

        }
        for (int i = 0; i < description.Count; i++)
        {

        }
        for (int i = 0; i < main.Count; i++)
        {

        }
    }
}

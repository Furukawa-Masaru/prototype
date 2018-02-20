using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public GameObject Start_Menu;
    public GameObject Finish_Menu;

    public GameObject Retire;

    public GameObject Timer;

    public GameObject game;
    public GameObject game_youkai;

    public Animator back_shikigami;
    public Animator back_youkai;
    public Animator attack_youkai;

    public GameObject[] room = new GameObject[5];

    public float counter;

    public Evaluation evaluation;
    public FurnitureManagement furnituremanagement;
    public GameObject GalleryManager;
    private GameObject DataManager;

    private int advaice_mode;

    public bool Game_Play = false;

    private List<bool> count = new List<bool>();

    public void Button_Clicked()
    {
        DataManager.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sound/SE/Button_Click"));
    }

    float dist_counter;
    int comment_value;

    public Text Room_Compass_;

    void Start()
	{
        DataManager = GameObject.Find("DataManager");
        advaice_mode = DataManager.GetComponent<DataManager>().read_advaice_mode();
        Debug.Log("LevelManager " + advaice_mode);
        game.SetActive(true);

        if (DataManager.GetComponent<DataManager>().read_room() == Evaluation.Room.Bedroom)
        {
            Room_Compass_.text = "寝室/";
        }
        else if (DataManager.GetComponent<DataManager>().read_room() == Evaluation.Room.Workroom)
        {
            Room_Compass_.text = "仕事部屋/";
        }
        else if (DataManager.GetComponent<DataManager>().read_room() == Evaluation.Room.Living)
        {
            Room_Compass_.text = "リビング/";
        }

        if (DataManager.GetComponent<DataManager>().read_direction() == Evaluation.Direction.North)
        {
            Room_Compass_.text += "北";
        }
        else if (DataManager.GetComponent<DataManager>().read_direction() == Evaluation.Direction.NorthEast)
        {
            Room_Compass_.text += "北東";
        }
        else if (DataManager.GetComponent<DataManager>().read_direction() == Evaluation.Direction.East)
        {
            Room_Compass_.text += "東";
        }
        else if (DataManager.GetComponent<DataManager>().read_direction() == Evaluation.Direction.SouthEast)
        {
            Room_Compass_.text += "南東";
        }
        else if (DataManager.GetComponent<DataManager>().read_direction() == Evaluation.Direction.South)
        {
            Room_Compass_.text += "南";
        }
        else if (DataManager.GetComponent<DataManager>().read_direction() == Evaluation.Direction.SouthWest)
        {
            Room_Compass_.text += "南西";
        }
        else if (DataManager.GetComponent<DataManager>().read_direction() == Evaluation.Direction.West)
        {
            Room_Compass_.text += "西";
        }
        else if (DataManager.GetComponent<DataManager>().read_direction() == Evaluation.Direction.NorthWest)
        {
            Room_Compass_.text += "北西";
        }

        furnituremanagement.Room(DataManager.GetComponent<DataManager>().read_room(), DataManager.GetComponent<DataManager>().read_direction(), DataManager.GetComponent<DataManager>().read_norma_luck(), DataManager.GetComponent<DataManager>().read_advaice_mode());
        
        if (advaice_mode == 0)
        {
            game_youkai.GetComponent<Image>().sprite = Resources.Load<Sprite>("youkai/work/game");          
        }
        else if (advaice_mode == 1)
        {
            game_youkai.GetComponent<Image>().sprite = Resources.Load<Sprite>("youkai/popular/game");
        }
        else if (advaice_mode == 2)
        {
            game_youkai.GetComponent<Image>().sprite = Resources.Load<Sprite>("youkai/health/game");
        }
        else if (advaice_mode == 3)
        {
            game_youkai.GetComponent<Image>().sprite = Resources.Load<Sprite>("youkai/economic/game");
        }
        else if (advaice_mode == 4)
        {
            game_youkai.GetComponent<Image>().sprite = Resources.Load<Sprite>("youkai/love/game");           
        }

        attack_youkai.SetInteger("Element", advaice_mode);

        back_shikigami.Play("式神背景");
        back_youkai.Play("妖怪背景");

        room[advaice_mode].SetActive(true);

        Timer.GetComponent<Text>().text = counter.ToString("f2");
        dist_counter = counter;
        comment_value = 1;

        for (int i = 0; i < 10; i++)
        {
            count.Add(true);
        }
    }

    private void Update()
    {
        if (Game_Play == true)
        {            
            counter -= Time.deltaTime;

            if (dist_counter - counter > 5)
            {                
                dist_counter = counter;

                if (comment_value % 2 == 0)
                {
                    evaluation.Comment_Text(0);
                }
                else
                {
                    evaluation.Comment_Text(1);
                }

                comment_value++;
            }

            if (counter < count.Count)
            {
                int i = (int)counter;
                
                if (count[i] == true)
                {
                    DataManager.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sound/SE/countdown_" + (i + 1)));
                    count[i] = false;
                }               
            }

            //敗北
            if (counter < 0)
            {
                counter = 0;
                FinishGame(false);
            }

            Timer.GetComponent<Text>().text = counter.ToString("f2");
        }        
    }

    public void StartGame()
    {
        DataManager.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sound/SE/start"));
        DataManager.GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Sound/BGM/Game_BGM");
        DataManager.GetComponent<AudioSource>().Play();

        Start_Menu.SetActive(false);
        
        furnituremanagement.Add_.GetComponent<Button>().interactable = true;
        furnituremanagement.Mode_.GetComponent<Button>().interactable = true;
        Retire.GetComponent<Button>().interactable = true; ;

        Game_Play = true;
    }

    public void FinishGame(bool win)
    {       
        if (win)
        {
            evaluation.attack_shikigami.SetInteger("Element", 5);
            attack_youkai.gameObject.SetActive(false);

            back_youkai.gameObject.SetActive(false);
        }
        else
        {
            evaluation.attack_shikigami.gameObject.SetActive(false);
            attack_youkai.SetInteger("Element", 5);

            back_shikigami.gameObject.SetActive(false);
        }

        DataManager.GetComponent<AudioSource>().Stop();
        DataManager.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sound/SE/finish"));
        DataManager.GetComponent<DataManager>().set_win(win);

        GalleryManager.GetComponent<bl_GalleryManager>().FullWindow.SetActive(false);
        GalleryManager.GetComponent<bl_GalleryManager>().Gallery_furniture.SetActive(false);
        GalleryManager.GetComponent<bl_GalleryManager>().Gallery_type.SetActive(false);

        furnituremanagement.Menu.SetActive(false); ;
        furnituremanagement.Add_.GetComponent<Button>().interactable = false;
        furnituremanagement.Mode_.GetComponent<Button>().interactable = false;
        furnituremanagement.move_furniture = false;

        evaluation.set_is_finishedGame(true);
        evaluation.EvaluationTotal();
        evaluation.Comment_Text(0);

        Retire.GetComponent<Button>().interactable = false;
        Finish_Menu.SetActive(true);

        Game_Play = false;

        StartCoroutine(update(win));
    }

    private IEnumerator update(bool win)
    {
        while (true)
        {
            Finish_Menu.GetComponent<RectTransform>().localScale = Vector3.Lerp(Finish_Menu.GetComponent<RectTransform>().localScale, new Vector3(1,1,1), 0.05f);
            
            if (Finish_Menu.GetComponent<RectTransform>().localScale.x - 1 < 0.05f)
            {
                Finish_Menu.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
               
                StartCoroutine(Sample("Result", win));

                yield break;
            }
            yield return null;
        }
    }

    // コルーチン  
    private IEnumerator Sample(string name, bool win)
    {
        DataManager.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sound/SE/death"));

        while (true)
        {
            if (win)
            {
                game_youkai.GetComponent<Image>().color = Color.Lerp(game_youkai.GetComponent<Image>().color, new Color(255, 255, 255, 0), 0.05f);

                if (game_youkai.GetComponent<Image>().color.a < 0.01f)
                {
                    break;
                }
            }
            else
            {
                evaluation.game_shikigami.GetComponent<Image>().color = Color.Lerp(evaluation.game_shikigami.GetComponent<Image>().color, new Color(255, 255, 255, 0), 0.05f);

                if (evaluation.game_shikigami.GetComponent<Image>().color.a < 0.01f)
                {
                    break;
                }
            }

            yield return null;
        }

        yield return new WaitForSeconds(2.0f);

        SceneManager.LoadScene(name);
    }
}

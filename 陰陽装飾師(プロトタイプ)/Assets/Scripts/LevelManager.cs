using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public GameObject Start_Menu;
    public GameObject Finish_Menu;
    public GameObject Timer;

    public GameObject icon;
    public GameObject icon_shikigami;
    public GameObject icon_youkai;
    public GameObject game;
    public GameObject game_shikigami;
    public GameObject game_youkai;
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

    void Start()
	{
        DataManager = GameObject.Find("DataManager");
        advaice_mode = DataManager.GetComponent<DataManager>().read_advaice_mode();

        Timer.GetComponent<Text>().text = counter.ToString("f2");

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

        furnituremanagement.Room(DataManager.GetComponent<DataManager>().read_room(), DataManager.GetComponent<DataManager>().read_direction(), DataManager.GetComponent<DataManager>().read_norma_luck(), DataManager.GetComponent<DataManager>().read_advaice_mode());

        if (advaice_mode == 0)
        {
            icon_shikigami.GetComponent<Image>().sprite = Resources.Load<Sprite>("shikigami/water/icon");
            game_shikigami.GetComponent<Image>().sprite = Resources.Load<Sprite>("shikigami/water/game");
            icon_youkai.GetComponent<Image>().sprite = Resources.Load<Sprite>("youkai/work/icon");            
            game_youkai.GetComponent<Image>().sprite = Resources.Load<Sprite>("youkai/work/game");
        }
        else if (advaice_mode == 1)
        {
            icon_shikigami.GetComponent<Image>().sprite = Resources.Load<Sprite>("shikigami/fire/icon");
            game_shikigami.GetComponent<Image>().sprite = Resources.Load<Sprite>("shikigami/fire/game");
            icon_youkai.GetComponent<Image>().sprite = Resources.Load<Sprite>("youkai/popular/icon");           
            game_youkai.GetComponent<Image>().sprite = Resources.Load<Sprite>("youkai/popular/game");
        }
        else if (advaice_mode == 2)
        {
            icon_shikigami.GetComponent<Image>().sprite = Resources.Load<Sprite>("shikigami/wood/icon");
            game_shikigami.GetComponent<Image>().sprite = Resources.Load<Sprite>("shikigami/wood/game");
            icon_youkai.GetComponent<Image>().sprite = Resources.Load<Sprite>("youkai/health/icon");
            game_youkai.GetComponent<Image>().sprite = Resources.Load<Sprite>("youkai/health/game");
        }
        else if (advaice_mode == 3)
        {
            icon_shikigami.GetComponent<Image>().sprite = Resources.Load<Sprite>("shikigami/metal/icon");
            game_shikigami.GetComponent<Image>().sprite = Resources.Load<Sprite>("shikigami/metal/game");
            icon_youkai.GetComponent<Image>().sprite = Resources.Load<Sprite>("youkai/economic/icon");
            game_youkai.GetComponent<Image>().sprite = Resources.Load<Sprite>("youkai/economic/game");
        }
        else if (advaice_mode == 4)
        {
            icon_shikigami.GetComponent<Image>().sprite = Resources.Load<Sprite>("shikigami/earth/icon");
            game_shikigami.GetComponent<Image>().sprite = Resources.Load<Sprite>("shikigami/earth/game");
            icon_youkai.GetComponent<Image>().sprite = Resources.Load<Sprite>("youkai/love/icon");
            game_youkai.GetComponent<Image>().sprite = Resources.Load<Sprite>("youkai/love/game");
        }

        icon.SetActive(true);
        game.SetActive(true);
        room[advaice_mode].SetActive(true);
        Start_Menu.SetActive(false);
        
        furnituremanagement.Add_.GetComponent<Button>().interactable = true;
        furnituremanagement.Change_Mode_.GetComponent<Button>().interactable = true;

        Game_Play = true;
    }

    public void FinishGame(bool win)
    {
        DataManager.GetComponent<AudioSource>().Stop();
        DataManager.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sound/SE/finish"));
        DataManager.GetComponent<DataManager>().set_win(win);

        GalleryManager.GetComponent<bl_GalleryManager>().FullWindow.SetActive(false);
        GalleryManager.GetComponent<bl_GalleryManager>().Gallery_furniture.SetActive(false);
        GalleryManager.GetComponent<bl_GalleryManager>().Gallery_type.SetActive(false);

        furnituremanagement.Menu.SetActive(false); ;
        furnituremanagement.Add_.GetComponent<Button>().interactable = false;
        furnituremanagement.Change_Mode_.GetComponent<Button>().interactable = false;
        furnituremanagement.move_furniture = false;

        evaluation.set_is_finishedGame(true);
        evaluation.EvaluationTotal();
        evaluation.Comment_Text();

        Finish_Menu.SetActive(true);

        Game_Play = false;

        StartCoroutine(update());
    }

    // コルーチン  
    private IEnumerator Sample(string name)
    {
        yield return new WaitForSeconds(3.0f);

        SceneManager.LoadScene(name);
    }

    private IEnumerator update()
    {
        while (true)
        {
            Finish_Menu.GetComponent<RectTransform>().localScale = Vector3.Lerp(Finish_Menu.GetComponent<RectTransform>().localScale, new Vector3(1,1,1), 0.2f);

            if (Finish_Menu.GetComponent<RectTransform>().localScale.x - 1 < 0.1f)
            {
                Finish_Menu.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);

                StartCoroutine(Sample("Result"));

                yield break;
            }
            yield return null;
        }
    }
}

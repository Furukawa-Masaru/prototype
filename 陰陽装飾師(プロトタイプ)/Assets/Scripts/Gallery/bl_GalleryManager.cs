using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;

public class bl_GalleryManager : MonoBehaviour
{
    public int sum_level;
    private List<bl_UGFInfo> Item_level = new List<bl_UGFInfo>();
    public int sum_type;
    private List<bl_UGFInfo> Item_type = new List<bl_UGFInfo>();
    public List<int> sum_furniture;
    private List<bl_UGFInfo> Item_furniture = new List<bl_UGFInfo>();

    [Header("Gallery")]
    public GameObject Gallery_level = null;
    public GameObject Gallery_type = null;
    public GameObject Gallery_furniture = null;

    [Header("FullWindow")]
    public GameObject FullWindow = null;
    public Image m_FullIcon = null;
    public Text m_TextName = null;
    public Text m_FullInfoText = null;
    public Button FullButtonOption = null;

    [Header("References")]
    public GameObject ItemPrefab = null;
    public Transform Gallery_level_Panel = null;
    public Transform Gallery_type_Panel = null;
    public Transform Gallery_furniture_Panel = null;
    [SerializeField] private ScrollRect ScrollList_level;
    [SerializeField] private ScrollRect ScrollList_type;
    [SerializeField] private ScrollRect ScrollList_furniture;

    public const string UGFName = "UGalleryManager";
    private List<bl_GalleryItem> level = new List<bl_GalleryItem>();
    private List<bl_GalleryItem> type = new List<bl_GalleryItem>();
    private List<bl_GalleryItem> furniture = new List<bl_GalleryItem>();

    public bool start_level;
    private int ID_level;
    //(運気の)ノルマ変数
    //[0] = 仕事運，[1] = 人気運, [2] = 健康運, [3] = 金運, [4] = 恋愛運
    private int[] norma_luck_ = new int[5];
    //アドバイスモード(0 = 仕事運重視，1 = 人気運重視，2 = 健康運重視，3 = 金運重視，4 = 恋愛運重視, 5 = デフォルト(ノルマ重視))
    private int advaice_mode_;
    private Evaluation.Direction temp_compass = new Evaluation.Direction();
    private Evaluation.Room temp_room = new Evaluation.Room();
    public List<GameObject> room = new List<GameObject>();
    public GameObject result;
    private GameObject DataManager;

    private int category_ID_temp;
    private int furniture_ID_temp;
    public enum Mode {Add, Change, Property };
    public Mode mode_;

    public FurnitureManagement FurnitureManagement;
    public GameObject Bottom;

    public Text wood;
    public Text fire;
    public Text earth;
    public Text metal;
    public Text water;
    public Text yin_yang;

    public Text Color_content;
    public Text Material_content;
    public Text Pattern_content;
    public Text Form_content;
    public Text Characteristic_content;

    private bool once_true = true;
    private bool once_false = true;

    public void Button_Clicked()
    {
        GameObject.Find("DataManager").GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sound/SE/Button_Click"));
    }

    private void Start()
    {
        gameObject.name = UGFName;

        DataManager = GameObject.Find("DataManager");

        if (start_level == true)
        {
            Init_level();
        }

    }

    public void Init_level()
    {
        for (int i = 0; i < sum_level; i++)
        {
            Item_level.Add(new bl_UGFInfo());
            Item_level[i].set_item(Instantiate(ItemPrefab) as GameObject);
            Item_level[i].read_item().name = i.ToString();
            Item_level[i].read_item().transform.SetParent(Gallery_level_Panel, false);
            bl_GalleryItem gi = Item_level[i].read_item().GetComponent<bl_GalleryItem>();
            level.Add(gi);

            if (i == 0)
            {
                level[i].GetInfo(i, Resources.Load<Sprite>("youkai/work/body"), bl_GalleryItem.Mode.Level);
            }
            if (i == 1)
            {
                level[i].GetInfo(i, Resources.Load<Sprite>("youkai/popular/body"), bl_GalleryItem.Mode.Level);
            }
            if (i == 2)
            {
                level[i].GetInfo(i, Resources.Load<Sprite>("youkai/health/body"), bl_GalleryItem.Mode.Level);
            }
            if (i == 3)
            {
                level[i].GetInfo(i, Resources.Load<Sprite>("youkai/economic/body"), bl_GalleryItem.Mode.Level);
            }
            if (i == 4)
            {
                level[i].GetInfo(i, Resources.Load<Sprite>("youkai/love/body"), bl_GalleryItem.Mode.Level);
            }

        }
    }

    public void Init_type()
    {
        for (int i = 0; i < sum_type; i++)
        {
            Item_type.Add(new bl_UGFInfo());
            Item_type[i].set_item(Instantiate(ItemPrefab) as GameObject);
            Item_type[i].read_item().name = i.ToString();
            Item_type[i].read_item().transform.SetParent(Gallery_type_Panel, false);
            bl_GalleryItem gi = Item_type[i].read_item().GetComponent<bl_GalleryItem>();
            type.Add(gi);

            type[i].GetInfo(i, Resources.Load<Sprite>("main/main_" + i), bl_GalleryItem.Mode.Type);
        }
    }

    public Text furniture_text;

    public void Init_furniture(int id)
    {
        category_ID_temp = id;

        string furniture_temp = "";

        if (category_ID_temp == 0)
        {
            //ベッド
            furniture_text.text = "ベッド";
            furniture_temp = "bed";
        }
        else if (category_ID_temp == 1)
        {
            //テーブル
            furniture_text.text = "テーブル";
            furniture_temp = "table";
        }
        else if (category_ID_temp == 2)
        {
            //ソファ
            furniture_text.text = "ソファ";
            furniture_temp = "sofa";
        }
        else if (category_ID_temp == 3)
        {
            //カーペット
            furniture_text.text = "カーペット";
            furniture_temp = "carpet";
        }
        else if (category_ID_temp == 4)
        {
            //タンス
            furniture_text.text = "タンス";
            furniture_temp = "cabinet";
        }
        else if (category_ID_temp == 5)
        {
            //机
            furniture_text.text = "机";
            furniture_temp = "desk";
        }
        else if (category_ID_temp == 6)
        {
            //観葉植物
            furniture_text.text = "観葉植物";
            furniture_temp = "foliage";
        }
        else if (category_ID_temp == 7)
        {
            //天井ランプ
            furniture_text.text = "天井ランプ";
            furniture_temp = "ceillamp";
        }
        else if (category_ID_temp == 8)
        {
            //机ランプ
            furniture_text.text = "机ランプ";
            furniture_temp = "desklamp";
        }
        else if (category_ID_temp == 9)
        {
            //家電
            furniture_text.text = "家電";
            furniture_temp = "electronics";
        }
        else if (category_ID_temp == 10)
        {
            //カーテン
            furniture_text.text = "カーテン";
            furniture_temp = "curtain";
        }

        for (int i = 0; i < sum_furniture[category_ID_temp]; i++)
        {
            Item_furniture.Add(new bl_UGFInfo());
            Item_furniture[i].set_item(Instantiate(ItemPrefab) as GameObject);
            Item_furniture[i].read_item().name = i.ToString();
            Item_furniture[i].read_item().transform.SetParent(Gallery_furniture_Panel, false);
            bl_GalleryItem gi = Item_furniture[i].read_item().GetComponent<bl_GalleryItem>();
            furniture.Add(gi);

            furniture[i].GetInfo(i, Resources.Load<Sprite>(furniture_temp + "/" + furniture_temp + "_" + (i + 1)), bl_GalleryItem.Mode.Furniture);
        }
    }

    private void Update()
    {
        for (int i = 0; i < level.Count; i++)
        {
            if (Gallery_level.activeSelf == true) level[i].OnUpdate();
        }

        if (start_level == true)
        {            
            if (gameObject.GetComponent<AudioSource>().isPlaying == true && once_true == true)
            {
                GameObject.Find("DataManager").GetComponent<AudioSource>().volume = 0.3f;
                once_true = false;
                once_false = true;
            }
            else if(gameObject.GetComponent<AudioSource>().isPlaying == false && once_false == true)
            {
                GameObject.Find("DataManager").GetComponent<AudioSource>().volume = 1.0f;
                once_false = false;
                once_true = true;                
            }
        }

        for (int i = 0; i < type.Count; i++)
        {
            if (Gallery_type.activeSelf == true) type[i].OnUpdate();
        }

        for (int i = 0; i < furniture.Count; i++)
        {
            if (Gallery_furniture.activeSelf == true) furniture[i].OnUpdate();
        }
    }

    public void GoToSideList_level(bool left)
    {
        StopAllCoroutines();
        StartCoroutine(IEMoveSide_level(left));
    }

    public void GoToSideList_type(bool left)
    {
        StopAllCoroutines();
        StartCoroutine(IEMoveSide_type(left));
    }

    public void GoToSideList_furniture(bool left)
    {
        StopAllCoroutines();
        StartCoroutine(IEMoveSide_furniture(left));
    }

    public void OpenGallery_level()
    {
        ScrollList_level.horizontalNormalizedPosition = 0;
        Gallery_level.SetActive(true);
    }

    public void NextGallery_level()
    {
        Button_Clicked();

        Gallery_level.SetActive(false);
    }

    public void CloseResult()
    {
        gameObject.GetComponent<AudioSource>().Stop();
        Button_Clicked();
        room[ID_level].SetActive(false);
        result.SetActive(false);
        OpenGallery_level();
    }

    public void Result(bool ok)
    {
        Button_Clicked();

        if (ok == true)
        {
            DataManager.GetComponent<DataManager>().set_direction(temp_compass);
            DataManager.GetComponent<DataManager>().set_room(temp_room);
            DataManager.GetComponent<DataManager>().set_norma_luck(norma_luck_);
            DataManager.GetComponent<DataManager>().set_advaice_mode(advaice_mode_);

            StartCoroutine(Sample("Game"));
        }
        else if (ok == false)
        {
            result.SetActive(false);
            Gallery_level.SetActive(true);
        }

    }

    public void Level(int id)
    {
        ID_level = id;

        if (ID_level == 0)
        {
            gameObject.GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("youkai/work/voice");
        }
        else if (ID_level == 1)
        {
            gameObject.GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("youkai/popular/voice");
        }
        else if (ID_level == 2)
        {
            gameObject.GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("youkai/health/voice");
        }
        else if (ID_level == 3)
        {
            gameObject.GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("youkai/economic/voice");
        }
        else if (ID_level == 4)
        {
            gameObject.GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("youkai/love/voice");
        }

        gameObject.GetComponent<AudioSource>().Play();

        StartCoroutine(Wait(ID_level));
    }

    // コルーチン  
    private IEnumerator Wait(int id)
    {
        yield return new WaitForSeconds(1.0f);

        NextGallery_level();
        //ScrollList_level.horizontalNormalizedPosition = 0;
        result.SetActive(true);

        room[ID_level].SetActive(true);

        for (int i = 0; i < norma_luck_.Length; i++)
        {
            if (i == ID_level)
            {
                norma_luck_[i] = 0;
            }
            else
            {
                norma_luck_[i] = 0;
            }
        }

        advaice_mode_ = ID_level;

        string compass_string = "";

        int ID_compass = Random.Range(0, 8);

        if (ID_compass == 0)
        {
            temp_compass = Evaluation.Direction.North;
            compass_string = "北";
        }
        else if (ID_compass == 1)
        {
            temp_compass = Evaluation.Direction.NorthEast;
            compass_string = "北東";
        }
        else if (ID_compass == 2)
        {
            temp_compass = Evaluation.Direction.East;
            compass_string = "東";
        }
        else if (ID_compass == 3)
        {
            temp_compass = Evaluation.Direction.SouthEast;
            compass_string = "南東";
        }
        else if (ID_compass == 4)
        {
            temp_compass = Evaluation.Direction.South;
            compass_string = "南";
        }
        else if (ID_compass == 5)
        {
            temp_compass = Evaluation.Direction.SouthWest;
            compass_string = "南西";
        }
        else if (ID_compass == 6)
        {
            temp_compass = Evaluation.Direction.West;
            compass_string = "西";
        }
        else if (ID_compass == 7)
        {
            temp_compass = Evaluation.Direction.NorthWest;
            compass_string = "北西";
        }

        int ID_room = Random.Range(0, 3);
        string room_string = "";

        if (ID_room == 0)
        {
            temp_room = Evaluation.Room.Bedroom;
            room_string = "寝室";
        }
        else if (ID_room == 1)
        {
            temp_room = Evaluation.Room.Workroom;
            room_string = "仕事部屋";
        }
        else if (ID_room == 2)
        {
            temp_room = Evaluation.Room.Living;
            room_string = "リビング";
        }
        //else if (ID_room == 3)
        //{
        //    temp_room = Evaluation.Room.Kitchen;
        //    room_string = "台所";
        //}
        //else if (ID_room == 4)
        //{
        //    temp_room = Evaluation.Room.Bathroom;
        //    room_string = "浴室";
        //}

        //result.GetComponent<Text>().text = "方角 " + compass_string + ", 部屋" + room_string;
        result.transform.Find("compass&room").gameObject.GetComponent<Text>().text = "「方角」" + compass_string + ", 「部屋」" + room_string;
        //result.transform.Find("compass&room").gameObject.transform.position = room[ID_level].transform.position + new Vector3(0,10,0);
    }

    public void OpenGallery_type()
    {
        ScrollList_type.horizontalNormalizedPosition = 0;
        Gallery_type.SetActive(true);
    }

    public void CloseGallery_type()
    {
        Button_Clicked();

        Destroy_type();

        Gallery_type.SetActive(false);
        FurnitureManagement.move_furniture = true;
        FurnitureManagement.Add_ChangeMode(true);
    }

    public void Destroy_type()
    {
        for (int i = 0; i < sum_type; i++)
        {
            Destroy(Item_type[0].read_item());
            Item_type.RemoveAt(0);
            type.RemoveAt(0);
        }
    }

    public void NextGallery_type()
    {
        Gallery_type.SetActive(false);
    }

    public void OpenGallery_furniture()
    {
        ScrollList_furniture.horizontalNormalizedPosition = 0;
        Gallery_furniture.SetActive(true);
    }

    public void CloseGallery_furniture()
    {
        Button_Clicked();

        Destroy_furniture();

        Gallery_furniture.SetActive(false);

        if (mode_ == Mode.Add)
        {
            Gallery_type.SetActive(true);
        }
        else
        {
            FurnitureManagement.Menu.SetActive(true);
        }
    }

    public void Destroy_furniture()
    {
        for (int i = 0; i < sum_furniture[category_ID_temp]; i++)
        {
            Destroy(Item_furniture[0].read_item());
            Item_furniture.RemoveAt(0);
            furniture.RemoveAt(0);
        }
    }

    public void NextGallery_furniture()
    {
        Gallery_furniture.SetActive(false);
    }

    public void OpenFullWindow()
    {
        FullWindow.SetActive(true);

        if (mode_ == Mode.Property)
        {
            FullButtonOption.gameObject.SetActive(false);
        }
        else
        {
            FullButtonOption.gameObject.SetActive(true);
        }
    }

    public void CloseFullWindow()
    {
        Button_Clicked();

        FullWindow.SetActive(false);

        if (mode_ != Mode.Property)
        {
            Gallery_furniture.SetActive(true);
        }
              
    }

    //追加から
    public void FullWindow_furniture(int id, Sprite sprite)
    {
        furniture_ID_temp = id;

        FurnitureGrid furnituregrid = new FurnitureGrid();

        furnituregrid.set_category_ID(category_ID_temp);
        furnituregrid.set_furniture_ID(id);
        furnituregrid.Status(furnituregrid.category_ID(), furnituregrid.furniture_ID());

        infomation(furnituregrid);

        //m_FullIcon.gameObject.GetComponent<RectTransform>().localScale = new Vector3(2.0f, 2.0f, 1.0f);
        //m_FullIcon.gameObject.GetComponent<RectTransform>().localPosition += new Vector3(-200.0f, 0, 0); 
        m_FullIcon.sprite = sprite;

        Destroy(furnituregrid);

    }

    //詳細から
    public void FullWindow_property(FurnitureGrid furnituregrid)
    {
        infomation(furnituregrid);

        //m_FullIcon.gameObject.GetComponent<RectTransform>().localScale = new Vector3(2.0f, 2.0f, 1.0f);
        //m_FullIcon.gameObject.GetComponent<RectTransform>().localPosition += new Vector3(-200.0f, 0, 0);
        m_FullIcon.sprite = furnituregrid.read_sprite();
    }

    public void infomation(FurnitureGrid furnituregrid)
    {
        wood.text = furnituregrid.elements_wood().ToString();
        fire.text = furnituregrid.elements_fire().ToString();
        earth.text = furnituregrid.elements_earth().ToString();
        metal.text = furnituregrid.elements_metal().ToString();
        water.text = furnituregrid.elements_water().ToString();
        yin_yang.text = furnituregrid.yin_yang().ToString();

        Color_content.text = "";
        Material_content.text = "";
        Pattern_content.text = "";
        Form_content.text = "";
        Characteristic_content.text = "";

        for (int i = 0; i < furnituregrid.color_name().Count; i++)
        {
            if (furnituregrid.color_name()[i] == FurnitureGrid.ColorName.White)
            {
                Color_content.text += "白";
            }
            else if (furnituregrid.color_name()[i] == FurnitureGrid.ColorName.Black)
            {
                Color_content.text += "黒";
            }
            else if (furnituregrid.color_name()[i] == FurnitureGrid.ColorName.Gray)
            {
                Color_content.text += "灰色";
            }
            else if (furnituregrid.color_name()[i] == FurnitureGrid.ColorName.DarkGray)
            {
                Color_content.text += "濃い灰色";
            }
            else if (furnituregrid.color_name()[i] == FurnitureGrid.ColorName.Red)
            {
                Color_content.text += "赤";
            }
            else if (furnituregrid.color_name()[i] == FurnitureGrid.ColorName.Pink)
            {
                Color_content.text += "ピンク";
            }
            else if (furnituregrid.color_name()[i] == FurnitureGrid.ColorName.Blue)
            {
                Color_content.text += "青";
            }
            else if (furnituregrid.color_name()[i] == FurnitureGrid.ColorName.LightBlue)
            {
                Color_content.text += "水色";
            }
            else if (furnituregrid.color_name()[i] == FurnitureGrid.ColorName.Orange)
            {
                Color_content.text += "オレンジ";
            }
            else if (furnituregrid.color_name()[i] == FurnitureGrid.ColorName.Yellow)
            {
                Color_content.text += "黄色";
            }
            else if (furnituregrid.color_name()[i] == FurnitureGrid.ColorName.Green)
            {
                Color_content.text += "緑";
            }
            else if (furnituregrid.color_name()[i] == FurnitureGrid.ColorName.LightGreen)
            {
                Color_content.text += "黄緑";
            }
            else if (furnituregrid.color_name()[i] == FurnitureGrid.ColorName.Beige)
            {
                Color_content.text += "ベージュ";
            }
            else if (furnituregrid.color_name()[i] == FurnitureGrid.ColorName.Cream)
            {
                Color_content.text += "クリーム";
            }
            else if (furnituregrid.color_name()[i] == FurnitureGrid.ColorName.Brown)
            {
                Color_content.text += "茶";
            }
            else if (furnituregrid.color_name()[i] == FurnitureGrid.ColorName.Ocher)
            {
                Color_content.text += "黄土色";
            }
            else if (furnituregrid.color_name()[i] == FurnitureGrid.ColorName.Gold)
            {
                Color_content.text += "金";
            }
            else if (furnituregrid.color_name()[i] == FurnitureGrid.ColorName.Silver)
            {
                Color_content.text += "銀";
            }
            else if (furnituregrid.color_name()[i] == FurnitureGrid.ColorName.Purple)
            {
                Color_content.text += "紫";
            }

            if (i < furnituregrid.color_name().Count - 1)
            {
                Color_content.text += ",";
            }
        }

        for (int i = 0; i < furnituregrid.material_type().Count; i++)
        {
            if (furnituregrid.material_type()[i] == FurnitureGrid.MaterialType.ArtificialFoliage)
            {
                Material_content.text += "人工観葉植物";
            }
            else if (furnituregrid.material_type()[i] == FurnitureGrid.MaterialType.Wooden)
            {
                Material_content.text += "木製";
            }
            else if (furnituregrid.material_type()[i] == FurnitureGrid.MaterialType.Paper)
            {
                Material_content.text += "紙";
            }
            else if (furnituregrid.material_type()[i] == FurnitureGrid.MaterialType.Leather)
            {
                Material_content.text += "革";
            }
            else if (furnituregrid.material_type()[i] == FurnitureGrid.MaterialType.NaturalFibre)
            {
                Material_content.text += "天然繊維";
            }
            else if (furnituregrid.material_type()[i] == FurnitureGrid.MaterialType.ChemicalFibre)
            {
                Material_content.text += "化学繊維";
            }
            else if (furnituregrid.material_type()[i] == FurnitureGrid.MaterialType.Cotton)
            {
                Material_content.text += "綿";
            }
            else if (furnituregrid.material_type()[i] == FurnitureGrid.MaterialType.Plastic)
            {
                Material_content.text += "プラスチック";
            }
            else if (furnituregrid.material_type()[i] == FurnitureGrid.MaterialType.Ceramic)
            {
                Material_content.text += "陶磁器";
            }
            else if (furnituregrid.material_type()[i] == FurnitureGrid.MaterialType.Marble)
            {
                Material_content.text += "大理石";
            }
            else if (furnituregrid.material_type()[i] == FurnitureGrid.MaterialType.Metal)
            {
                Material_content.text += "金属";
            }
            else if (furnituregrid.material_type()[i] == FurnitureGrid.MaterialType.Mineral)
            {
                Material_content.text += "鉱物";
            }
            else if (furnituregrid.material_type()[i] == FurnitureGrid.MaterialType.Glass)
            {
                Material_content.text += "ガラス";
            }
            else if (furnituregrid.material_type()[i] == FurnitureGrid.MaterialType.Water)
            {
                Material_content.text += "水";
            }

            if (i < furnituregrid.material_type().Count - 1)
            {
                Material_content.text += ",";
            }
        }

        for (int i = 0; i < furnituregrid.pattern_type().Count; i++)
        {
            if (furnituregrid.pattern_type()[i] == FurnitureGrid.PatternType.Stripe)
            {
                Pattern_content.text += "ストライプ";
            }
            else if (furnituregrid.pattern_type()[i] == FurnitureGrid.PatternType.Leaf)
            {
                Pattern_content.text += "リーフパターン";
            }
            else if (furnituregrid.pattern_type()[i] == FurnitureGrid.PatternType.Flower)
            {
                Pattern_content.text += "花柄";
            }
            else if (furnituregrid.pattern_type()[i] == FurnitureGrid.PatternType.Star)
            {
                Pattern_content.text += "星柄";
            }
            else if (furnituregrid.pattern_type()[i] == FurnitureGrid.PatternType.Diamond)
            {
                Pattern_content.text += "ダイヤ柄";
            }
            else if (furnituregrid.pattern_type()[i] == FurnitureGrid.PatternType.Animal)
            {
                Pattern_content.text += "アニマル柄";
            }
            else if (furnituregrid.pattern_type()[i] == FurnitureGrid.PatternType.Zigzag)
            {
                Pattern_content.text += "ジグザグ";
            }
            else if (furnituregrid.pattern_type()[i] == FurnitureGrid.PatternType.Novel)
            {
                Pattern_content.text += "奇抜";
            }
            else if (furnituregrid.pattern_type()[i] == FurnitureGrid.PatternType.Border)
            {
                Pattern_content.text += "ボーダー";
            }
            else if (furnituregrid.pattern_type()[i] == FurnitureGrid.PatternType.Check)
            {
                Pattern_content.text += "チェック";
            }
            else if (furnituregrid.pattern_type()[i] == FurnitureGrid.PatternType.Tile)
            {
                Pattern_content.text += "タイル柄";
            }
            else if (furnituregrid.pattern_type()[i] == FurnitureGrid.PatternType.Dot)
            {
                Pattern_content.text += "ドット";
            }
            else if (furnituregrid.pattern_type()[i] == FurnitureGrid.PatternType.Round)
            {
                Pattern_content.text += "丸柄";
            }
            else if (furnituregrid.pattern_type()[i] == FurnitureGrid.PatternType.Arch)
            {
                Pattern_content.text += "アーチ";
            }
            else if (furnituregrid.pattern_type()[i] == FurnitureGrid.PatternType.Fruits)
            {
                Pattern_content.text += "フルーツ";
            }
            else if (furnituregrid.pattern_type()[i] == FurnitureGrid.PatternType.Luster)
            {
                Pattern_content.text += "光沢";
            }
            else if (furnituregrid.pattern_type()[i] == FurnitureGrid.PatternType.Wave)
            {
                Pattern_content.text += "ウェーブストライプ";
            }
            else if (furnituregrid.pattern_type()[i] == FurnitureGrid.PatternType.Irregularity)
            {
                Pattern_content.text += "不規則パターン";
            }
            else if (furnituregrid.pattern_type()[i] == FurnitureGrid.PatternType.Cloud)
            {
                Pattern_content.text += "雲柄";
            }

            if (i < furnituregrid.pattern_type().Count - 1)
            {
                Pattern_content.text += ",";
            }
        }

        for (int i = 0; i < furnituregrid.form_type().Count; i++)
        {
            if (furnituregrid.form_type()[i] == FurnitureGrid.FormType.High)
            {
                Form_content.text += "背が高い";
            }
            else if (furnituregrid.form_type()[i] == FurnitureGrid.FormType.Low)
            {
                Form_content.text += "背が低い";
            }
            else if (furnituregrid.form_type()[i] == FurnitureGrid.FormType.Vertical)
            {
                Form_content.text += "縦長";
            }
            else if (furnituregrid.form_type()[i] == FurnitureGrid.FormType.Oblong)
            {
                Form_content.text += "横長";
            }
            else if (furnituregrid.form_type()[i] == FurnitureGrid.FormType.Square)
            {
                Form_content.text += "正方形";
            }
            else if (furnituregrid.form_type()[i] == FurnitureGrid.FormType.Rectangle)
            {
                Form_content.text += "長方形";
            }
            else if (furnituregrid.form_type()[i] == FurnitureGrid.FormType.Round)
            {
                Form_content.text += "円形";
            }
            else if (furnituregrid.form_type()[i] == FurnitureGrid.FormType.Ellipse)
            {
                Form_content.text += "楕円形";
            }
            else if (furnituregrid.form_type()[i] == FurnitureGrid.FormType.Triangle)
            {
                Form_content.text += "三角形";
            }
            else if (furnituregrid.form_type()[i] == FurnitureGrid.FormType.Sharp)
            {
                Form_content.text += "尖っている";
            }
            else if (furnituregrid.form_type()[i] == FurnitureGrid.FormType.Novel)
            {
                Form_content.text += "奇抜";
            }

            if (i < furnituregrid.form_type().Count - 1)
            {
                Form_content.text += ",";
            }
        }

        for (int i = 0; i < furnituregrid.characteristic().Count; i++)
        {
            if (furnituregrid.characteristic()[i] == FurnitureGrid.Characteristic.Luxury)
            {
                Characteristic_content.text += "高級";
            }
            else if (furnituregrid.characteristic()[i] == FurnitureGrid.Characteristic.Sound)
            {
                Characteristic_content.text += "音が出る";
            }
            else if (furnituregrid.characteristic()[i] == FurnitureGrid.Characteristic.Smell)
            {
                Characteristic_content.text += "いいにおい";
            }
            else if (furnituregrid.characteristic()[i] == FurnitureGrid.Characteristic.Light)
            {
                Characteristic_content.text += "発光";
            }
            else if (furnituregrid.characteristic()[i] == FurnitureGrid.Characteristic.Hard)
            {
                Characteristic_content.text += "硬い";
            }
            else if (furnituregrid.characteristic()[i] == FurnitureGrid.Characteristic.Soft)
            {
                Characteristic_content.text += "やわらかい";
            }
            else if (furnituregrid.characteristic()[i] == FurnitureGrid.Characteristic.Warm)
            {
                Characteristic_content.text += "温かみ";
            }
            else if (furnituregrid.characteristic()[i] == FurnitureGrid.Characteristic.Cold)
            {
                Characteristic_content.text += "冷たさ";
            }
            else if (furnituregrid.characteristic()[i] == FurnitureGrid.Characteristic.Flower)
            {
                Characteristic_content.text += "花関連";
            }
            else if (furnituregrid.characteristic()[i] == FurnitureGrid.Characteristic.Wind)
            {
                Characteristic_content.text += "風関連";
            }
            else if (furnituregrid.characteristic()[i] == FurnitureGrid.Characteristic.Western)
            {
                Characteristic_content.text += "西洋風";
            }
            else if (furnituregrid.characteristic()[i] == FurnitureGrid.Characteristic.Clutter)
            {
                Characteristic_content.text += "乱雑";
            }

            if (i < furnituregrid.characteristic().Count - 1)
            {
                Characteristic_content.text += ",";
            }
        }
    }

    // コルーチン  
    private IEnumerator Sample(string name)
    {
        yield return new WaitForSeconds(2.0f);
        GameObject.Find("DataManager").GetComponent<AudioSource>().volume = 1.0f;
        SceneManager.LoadScene(name);
    }

    public void OK()
    {
        Button_Clicked();

        if (mode_ == Mode.Add)
        {
            FullWindow.SetActive(false);
            Destroy_furniture();
            Gallery_furniture.SetActive(false);
            Destroy_type();
            Gallery_type.SetActive(false);

            FurnitureManagement.AddFurniture(category_ID_temp, furniture_ID_temp);
        }
        else if (mode_ == Mode.Change)
        {
            FullWindow.SetActive(false);
            Destroy_furniture();
            Gallery_furniture.SetActive(false);

            FurnitureManagement.Menu.SetActive(false);
            FurnitureManagement.ChangeFurniture(category_ID_temp, furniture_ID_temp);
        }

        FurnitureManagement.Add_ChangeMode(true);
        FurnitureManagement.move_furniture = true;
    }

    IEnumerator IEMoveSide_level(bool left)
    {
        if (left)
        {
            while (ScrollList_level.horizontalNormalizedPosition > 0)
            {
                ScrollList_level.horizontalNormalizedPosition -= Time.deltaTime;
                yield return null;
            }
        }
        else
        {
            while (ScrollList_level.horizontalNormalizedPosition < 1)
            {
                ScrollList_level.horizontalNormalizedPosition += Time.deltaTime;
                yield return null;
            }
        }
    }

    IEnumerator IEMoveSide_type(bool left)
    {
        if (left)
        {
            while(ScrollList_type.horizontalNormalizedPosition > 0)
            {
                ScrollList_type.horizontalNormalizedPosition -= Time.deltaTime;
                yield return null;
            }
        }else
        {
            while (ScrollList_type.horizontalNormalizedPosition < 1)
            {
                ScrollList_type.horizontalNormalizedPosition += Time.deltaTime;
                yield return null;
            }
        }
    }

    IEnumerator IEMoveSide_furniture(bool left)
    {
        if (left)
        {
            while (ScrollList_furniture.horizontalNormalizedPosition > 0)
            {
                ScrollList_furniture.horizontalNormalizedPosition -= Time.deltaTime;
                yield return null;
            }
        }
        else
        {
            while (ScrollList_furniture.horizontalNormalizedPosition < 1)
            {
                ScrollList_furniture.horizontalNormalizedPosition += Time.deltaTime;
                yield return null;
            }
        }
    }
}
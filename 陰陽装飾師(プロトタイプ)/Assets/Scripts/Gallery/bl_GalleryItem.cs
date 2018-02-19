using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class bl_GalleryItem : MonoBehaviour
{
    [HideInInspector]public bool OnCenter = false;
    [HideInInspector]public string m_InfoItem = "";

    [Header("Settings")]
    [Range(0, 0.5f)]
    public float CrossFadeAnim = 0.4f;
    [Header("Effects")]
    public UGFType m_Type = UGFType.Reflection;
    public GameObject Reflection_object = null;
    public Image Reflection = null;
   
    [Header("References")]
    [SerializeField]private CanvasGroup GroupAlpha;
    [SerializeField]private Animator ContentAnim;

    private int ID;
    private Sprite Sprite;
    public enum Mode { Level, Type, Furniture};
    private Mode mode_;

    private bl_GalleryManager mGalleryManager;
    private bl_GalleryManager m_Manager { get { if (mGalleryManager == null) { mGalleryManager = GameObject.Find(bl_GalleryManager.UGFName).GetComponent<bl_GalleryManager>(); } return mGalleryManager; } }

    private Button.ButtonClickedEvent events = new Button.ButtonClickedEvent();//cache event of item
    private float currentAlpha = 1;

    private float center_select = 0.5f;
    private float center_game = 0.5f;

    private float center_width = 0.05f;
    private bool rotate = false;

    public IEnumerator Start()
    {
        yield return new WaitForSeconds(0.1f);

        if (rotate == true && OnCenter == false)
        {
            if (mode_ == Mode.Level)
            {
                if (ScreenHorizontalPosition < center_select - center_width || ScreenHorizontalPosition > center_select + center_width)
                {
                    Animation a = this.GetComponent<Animation>();
                    //その側はUIを終了しますか？
                    if (isRightPosition)
                    {
                        a.CrossFade("OnExitR", CrossFadeAnim);

                    }
                    else
                    {
                        a.CrossFade("OnExitL", CrossFadeAnim);

                    }
                }
            }
            else
            {
                if (ScreenHorizontalPosition < center_game - center_width || ScreenHorizontalPosition > center_game + center_width)
                {
                    Animation a = this.GetComponent<Animation>();
                    //その側はUIを終了しますか？
                    if (isRightPosition)
                    {
                        a.CrossFade("OnExitR", CrossFadeAnim);

                    }
                    else
                    {
                        a.CrossFade("OnExitL", CrossFadeAnim);

                    }
                }
            }
        }             
    }

    /// <summary>
    /// UIがセンターに出入りするとき
    /// </summary>
    public void CenterEvent(bool b)
    {
        Animation a = this.GetComponent<Animation>();

        if (a == null)
        {
            Debug.LogError("This don't have Animation component");
            return;
        }

        //入力するとき
        if (b)
        {
            if (rotate == true)
            {
                if (isRightPosition)
                {
                    a.CrossFade("OnCenterR", CrossFadeAnim);

                }
                else
                {
                    a.CrossFade("OnCenterL", CrossFadeAnim);

                }
            }

            if (ID == 0)
            {
                rotate = true;
            }
            
        }
        else//終了時
        {
            //その側はUIを終了しますか？
            if (isRightPosition)
            {
                a.CrossFade("OnExitR", CrossFadeAnim);
                
            }
            else
            {
                a.CrossFade("OnExitL", CrossFadeAnim);

            }
        }
    }

    /// <summary>
    /// ギャラリーマネージャーのアイテムの情報をキャッシュする
    /// </summary>
    public void GetInfo(int id, Sprite sprite, Mode mode)
    {
        mode_ = mode;

        if (mode_ == Mode.Level)
        {
            ID = id;
            gameObject.transform.Find("Content").gameObject.transform.Find("ItemButton").gameObject.GetComponent<RectTransform>().localScale = new Vector3(6.0f, 6.0f, 1);
            gameObject.transform.Find("Content").gameObject.transform.Find("ItemButton").gameObject.GetComponent<RectTransform>().localPosition += new Vector3(0.0f, 90.0f, 0.0f);            

            string temp = "";

            if (id == 0)
            {
                temp = "天国";
            }
            if (id == 1)
            {
                temp = "易しい";
            }
            if (id == 2)
            {
                temp = "普通";
            }
            if (id == 3)
            {
                temp = "難しい";
            }
            if (id == 4)
            {
                temp = "地獄";
            }

            GameObject difficulty = gameObject.transform.Find("Content").gameObject.transform.Find("Text").gameObject;
            difficulty.GetComponent<Text>().text = temp;
            difficulty.GetComponent<Text>().fontSize = 100;
            difficulty.GetComponent<Text>().color = new Color(255,255,255);

            difficulty.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 100);
            difficulty.GetComponent<RectTransform>().localPosition = new Vector3(0,-100,0);

        }
        else if (mode_ == Mode.Type)
        {
            ID = id;
            gameObject.transform.Find("Content").gameObject.transform.Find("ItemButton").gameObject.GetComponent<RectTransform>().localScale = new Vector3(7.0f, 7.0f, 1);
            gameObject.transform.Find("Content").gameObject.transform.Find("ItemButton").gameObject.GetComponent<RectTransform>().localPosition += new Vector3(0.0f, 30.0f, 0.0f);
            gameObject.transform.Find("Content").gameObject.transform.Find("Text").gameObject.SetActive(false);
        }
        else if (mode_ == Mode.Furniture)
        {
            ID = id + 1;
            gameObject.transform.Find("Content").gameObject.transform.Find("ItemButton").gameObject.GetComponent<RectTransform>().localScale = new Vector3(7.0f, 7.0f, 1);
            gameObject.transform.Find("Content").gameObject.transform.Find("ItemButton").gameObject.GetComponent<RectTransform>().localPosition += new Vector3(0.0f, 30.0f, 0.0f);
            gameObject.transform.Find("Content").gameObject.transform.Find("Text").gameObject.SetActive(false);
        }

        Sprite = sprite;

        Reflection.color = new Color(0, 0, 0, 0);
        gameObject.transform.Find("Content").gameObject.transform.Find("ItemButton").gameObject.GetComponent<Image>().sprite = Sprite;

        //if (Reflection != null)
        //{
        //    if (m_Type == UGFType.Reflection)
        //    {
        //        Reflection.sprite = Sprite;
        //    }
        //    else if (m_Type == UGFType.Shadown)
        //    {
        //        Reflection.color = new Color(0,0,0,0);
        //        //Reflection.color = Color.black;
        //    }
        //    else
        //    {
        //        Reflection.gameObject.SetActive(false);
        //    }
        //}

        if (ID == 0)
        {
            //CenterEvent(true);
        }
        else
        {
            rotate = true;
            CenterEvent(false);
        }
       
    }

    public void Button_Clicked()
    {
        GameObject.Find("DataManager").GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sound/SE/Button_Click"));
    }

    public void OpenFullWindow()
    {
        //これがターゲット（中央）のときにだけ、開くことができます
        if (!OnCenter)
            return;//中央にいなければ、戻ってくる

        if (mode_ == Mode.Level)
        {
            m_Manager.Level(ID);
        }
        else if (mode_ == Mode.Type)
        {
            Button_Clicked();
                
            m_Manager.NextGallery_type();
            m_Manager.OpenGallery_furniture();
            m_Manager.Init_furniture(ID);            
        }
        else if(mode_ == Mode.Furniture)
        {
            Button_Clicked();
            m_Manager.NextGallery_furniture();
            m_Manager.OpenFullWindow();
            m_Manager.FullWindow_furniture(ID, Sprite);           
        }
        
    }

    /// <summary>
    /// 
    /// </summary>
    public void OnUpdate()
    {
        float pos = ScreenHorizontalPosition;

        if (mode_ == Mode.Level)
        {
            if (pos < center_select - center_width || pos > center_select + center_width)
            {
                if (isRightPosition)
                {
                    currentAlpha = 1 - pos;
                }
                else
                {
                    currentAlpha = pos;
                }
            }
            else
            {
                currentAlpha = 1;
            }
        }
        else
        {
            if (pos < center_game - center_width || pos > center_game + center_width)
            {
                if (isRightPosition)
                {
                    currentAlpha = 1 - pos;
                }
                else
                {
                    currentAlpha = pos;
                }
            }
            else
            {
                currentAlpha = 1;
            }
        }
        
        GroupAlpha.alpha = Mathf.Lerp(GroupAlpha.alpha, currentAlpha, Time.deltaTime * 10);
    }

    /// <summary>
    /// UIの側面（左または右）を画面に表示する
    /// </summary>
    private bool isRightPosition
    {
        get
        {
            float temp = new float();

            if (mode_ == Mode.Level)
            {
                temp = center_select;
            }
            else
            {
                temp = center_game;
            }

            return (ScreenHorizontalPosition > temp);
        }
    }

    public float ScreenHorizontalPosition
    {
        get
        {
            Camera c = (Camera.main != null) ? Camera.main : Camera.current;
            Vector2 viewpoint = c.WorldToViewportPoint(transform.position);
            
            return viewpoint.x;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="e"></param>
    public void OnMouseEvent(bool e)
    {
        ContentAnim.SetBool("select", e);
    }

    private RectTransform m_Rect = null;
    private RectTransform mRect
    {
        get
        {
            if (m_Rect == null)
            {
                m_Rect = this.GetComponent<RectTransform>();
            }
            return m_Rect;
        }
    }

    private RectTransform m_ParentRect = null;
    private RectTransform ParentRect
    {
        get
        {
            if (m_ParentRect == null)
            {
                m_ParentRect = this.GetComponent<Transform>().parent.GetComponent<RectTransform>();
            }
            return m_ParentRect;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Result : MonoBehaviour {

    public Image BG;
    public Image Condition;
    public GameObject[] Shikigami = new GameObject[5];
    public GameObject[] Youkai = new GameObject[5];

    // Use this for initialization
    void Start () {

        GameObject DataManager = GameObject.Find("DataManager");

        if (DataManager.GetComponent<DataManager>().read_win() == true)
        {
            BG.sprite = Resources.Load<Sprite>("Background/win");
            Condition.sprite = Resources.Load<Sprite>("Effect/win");
            DataManager.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sound/SE/win"));
            Youkai[DataManager.GetComponent<DataManager>().read_advaice_mode()].GetComponent<RectTransform>().localScale = new Vector3(0.6f, 0.6f, 1);
        }
        else
        {
            BG.sprite = Resources.Load<Sprite>("Background/lose");
            Condition.sprite = Resources.Load<Sprite>("Effect/lose");
            DataManager.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sound/SE/lose"));
            Shikigami[DataManager.GetComponent<DataManager>().read_advaice_mode()].GetComponent<RectTransform>().localScale = new Vector3(0.6f, 0.6f, 1);
        }

        Shikigami[DataManager.GetComponent<DataManager>().read_advaice_mode()].SetActive(true);
        Youkai[DataManager.GetComponent<DataManager>().read_advaice_mode()].SetActive(true);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void To_Advice()
    {
        Button_Clicked();
    
        StartCoroutine(Sample("Advice"));
    }

    public void Button_Clicked()
    {
        GameObject.Find("DataManager").GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sound/SE/Button_Click"));
    }

    // コルーチン  
    private IEnumerator Sample(string name)
    {
        yield return new WaitForSeconds(3.0f);

        SceneManager.LoadScene(name);
    }
}

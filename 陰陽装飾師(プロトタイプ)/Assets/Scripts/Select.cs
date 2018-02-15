using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Select : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void To_Title()
    {
        StartCoroutine(Load_Scene("Title"));
    }

    // コルーチン  
    private IEnumerator Load_Scene(string name)
    {
        GameObject.Find("DataManager").GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sound/SE/Button_Click"));

        yield return new WaitForSeconds(2.0f);

        SceneManager.LoadScene(name);
    }

}

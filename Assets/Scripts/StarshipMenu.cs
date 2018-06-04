using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StarshipMenu : MonoBehaviour {

    private float yDisp;
    public float yDispMult;
    public Text hiscore;

	void Start () {
        yDisp = 0.0f;
        if (PlayerPrefs.HasKey("HI-SCORE")) {
            PlayerPrefs.GetInt("HI-SCORE");
        }
        else
        {
            PlayerPrefs.SetInt("HI-SCORE", 0);
        }

        hiscore.text = "HI-SCORE " + PlayerPrefs.GetInt("HI-SCORE");
    }
	
	// Update is called once per frame
	void Update () {
        yDisp += Time.deltaTime;
        transform.Translate(new Vector3(0.0f, Mathf.Sin(yDisp) * yDispMult, 0.0f));
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                SceneManager.LoadScene("mainGame", LoadSceneMode.Single);
            }
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AsteroidSpawner : MonoBehaviour {

    public float asteroidNumber;
    public Rigidbody asteroid;
    public float zMin, zMax, xMin, xMax, timeBetweenWaves;
    private float asteroidCount, time, previousTime;
    public float timePerWave;
    public TextMesh textNextWave;
    public TextMesh textWave;
    public TextMesh score;
    public TextMesh energy;
    public TextMesh gameOverText;
    public float waveTextTime;
    private int wave;
    private float timeWavePass;
    public int scoreAmount = 0;
    private Renderer textRender;
    private Renderer textRender2;
    private Renderer textRender3;
    private Renderer gameOverTextRenderer;
    public player playerScr;
    public UFOSpawner ufoSpawn;
    public AudioSource musicLoop, nextWaveSound;
    private bool dead;
    void Start()
    {
        wave = 0;
        dead = false;
        timeWavePass = waveTextTime;
        wave++;
        textRender = textWave.GetComponent<Renderer>();
        textRender2 = textNextWave.GetComponent<Renderer>();
        textRender3 = energy.GetComponent<Renderer>();
        gameOverTextRenderer = gameOverText.GetComponent<Renderer>();
        gameOverTextRenderer.enabled = false;
    }
    public void spawn () {
        time = 0;      
        previousTime = Time.time;
        for (int i = 0; i < asteroidNumber; ++i)
        {
            incAsteroidCount();
            int spawnLocation = (int)Random.Range(0.0f, 4.0f);
            Rigidbody newAsteroid1 = (Rigidbody)Instantiate(asteroid, transform.position, transform.rotation);
            asteroidScript newAsteroid1Scr = newAsteroid1.GetComponent<asteroidScript>();
            newAsteroid1Scr.newAsteroid();
            newAsteroid1Scr.setSize(1);
            switch (spawnLocation)
            {
                case 0:
                    newAsteroid1Scr.setPosition(new Vector3(Random.Range(xMin, xMax), 0.0f, zMin));
                    break;
                case 1:
                    newAsteroid1Scr.setPosition(new Vector3(Random.Range(xMin, xMax), 0.0f, zMax));
                    break;
                case 2:
                    newAsteroid1Scr.setPosition(new Vector3(xMin, 0.0f, Random.Range(zMin, zMax)));
                    break;
                case 3:
                    newAsteroid1Scr.setPosition(new Vector3(xMax, 0.0f, Random.Range(zMin, zMax)));
                    break;
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
        timeWavePass -= Time.deltaTime;
        if (dead == false)
        {
            int energyAmount = playerScr.getEnergy();
            if (energyAmount < 100 && energyAmount > 9)
            {
                energy.text = "ENERGY 0" + energyAmount + "%";
            }
            else if (energyAmount < 10)
            {
                energy.text = "ENERGY 00" + energyAmount + "%";
            }
            else energy.text = "ENERGY 100%";
            if (timeWavePass < 0 && textRender.enabled)
            {
                timeWavePass = 0;
                textRender.enabled = false;
                spawn();
            }

            time = Time.time - previousTime;
            int remainingWaveTime = (int)(timeBetweenWaves - time);
            textRender2.enabled = false;
            if (remainingWaveTime < 10 && !textRender.enabled)
            {
                Renderer textRender2 = textNextWave.GetComponent<Renderer>();
                textRender2.enabled = true;
                textNextWave.text = "NEXT WAVE " + remainingWaveTime.ToString();
            }

            if ((asteroidCount == 0 || time > timeBetweenWaves) && timeWavePass < 0)
            {
                if (wave < 10)
                {
                    timeBetweenWaves += timePerWave;
                    asteroidNumber++;
                }
                timeWavePass = waveTextTime;
                wave++;
                textWave.text = "WAVE " + wave.ToString();
                nextWaveSound.Play();
                textRender.enabled = true;
            }
            if (energyAmount == 0)
            {
                dead = true;
                gameOverTextRenderer.enabled = true;
                textRender.enabled = false;
                textRender2.enabled = false;
                playerScr.gameOver();
                ufoSpawn.gameOver();
                musicLoop.Stop();
                GetComponent<AudioSource>().Play();
            }
        }
        else {
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    SceneManager.LoadScene("menu", LoadSceneMode.Single);
                }
            }
        }
	}

    public void decAsteroidCount()
    {
        asteroidCount--;
        Renderer textRender = score.GetComponent<Renderer>();
        scoreAmount += 10;
        Debug.Log(scoreAmount);
        score.text = "SCORE " + scoreAmount.ToString();
    }
    public void incAsteroidCount()
    {
        asteroidCount++;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFOSpawner : MonoBehaviour {

    public Rigidbody rigidBody, UFO;
    public float speed;
    public float xMax, xMin;
    public float timeBetweenUFOs;
    private float timePassed;
	// Use this for initialization
	void Start () {
        timePassed = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
        timePassed -= Time.deltaTime;
        if (timePassed <= 0)
        {
            Debug.Log("UFO SPAWN " + transform.position);
            Rigidbody newUFO = (Rigidbody)Instantiate(UFO, transform.position, transform.rotation);
            timePassed = timeBetweenUFOs;
        }
		
	}

    public void gameOver()
    {
        Destroy(gameObject);
    }
}

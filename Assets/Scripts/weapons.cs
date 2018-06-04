using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weapons : MonoBehaviour {

    public GameObject cannon1;
    public GameObject cannon2;
    public Rigidbody projectile;
    public float timeBetweenShots;
    public bool fire1;
    private float waiting = 0.0f;
    void Start () {
        fire1 = true;
	}
	
	// Update is called once per frame
	void Update () {
        waiting -= Time.deltaTime;
        if (waiting < 0) waiting = 0.0f;
		
	}

    public void fire()
    {
        if (waiting <= 0.0f)
        {
            if (fire1)
            {
                fire1 = !fire1;
                Rigidbody newProjectile = (Rigidbody)Instantiate(projectile, cannon1.transform.position, cannon1.transform.rotation);
                waiting = timeBetweenShots;
            }
            else
            {
                fire1 = !fire1;
                Rigidbody newProjectile = (Rigidbody)Instantiate(projectile, cannon2.transform.position, cannon2.transform.rotation);
                waiting = timeBetweenShots;
            }
        }
    }
}

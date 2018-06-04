using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ufo : MonoBehaviour {

    public float speed;
    public float amplitude;
    private float sinus;
    private float xDir;
    public float maxDistance;
    public float timeBetweenShots;
    private float time;
    public int powerupNum;
    public Rigidbody rigidBody, enemyProjectile;
    public Rigidbody[] powerUps;
    private bool dead;
	void Start () {
        sinus = Random.Range(0.0f, 100.0f);
        time = timeBetweenShots;
        rigidBody.AddForce(new Vector3(0.0f, 0.0f, -speed));
        dead = false;
	}
	
	// Update is called once per frame
	void Update () {
        sinus += Time.deltaTime;
        time -= Time.deltaTime;
        rigidBody.velocity = new Vector3(Mathf.Sin(sinus) * amplitude, 0.0f, rigidBody.velocity.z);
        if (Vector3.Distance(transform.position, new Vector3(0.0f, 0.0f, 0.0f)) > maxDistance) Destroy(gameObject);
        if (!dead)
        {
            if (time <= 0)
            {
                time = timeBetweenShots;
                Rigidbody newEnemyShot = (Rigidbody)Instantiate(enemyProjectile, transform.position, transform.rotation);
                try
                {
                    newEnemyShot.transform.LookAt(GameObject.FindGameObjectsWithTag("Player")[0].transform.position);
                    Debug.Log(GameObject.FindGameObjectsWithTag("Player")[0].transform.position);
                }catch (System.Exception e)
                {
                    //Do nothing
                }
            }
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "projectile")
        {
            projectile proj = collider.GetComponentInParent<projectile>();
            proj.destroy();
            int i = Random.Range(0, 100) % powerupNum;
            Rigidbody newPowerup = Instantiate(powerUps[i], transform.position, transform.rotation);
            Destroy(this.gameObject);
        }
    }

    public void gameOver()
    {
        dead = true;
    }
}

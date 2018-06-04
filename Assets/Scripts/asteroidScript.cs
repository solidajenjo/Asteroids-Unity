using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class asteroidScript : MonoBehaviour {

    public Vector3 rotVec, directionVec;
    public Rigidbody rigidBody;
    public Rigidbody asteroid;
    public Rigidbody asteroidExplosion;
    private AsteroidSpawner asteroidSpawner;
    public Transform newPos1, newPos2;
    public float speed, maxSpeed, minSpeed, explosionTime;
    public float zMin, zMax, xMin, xMax;
    public float expRadius = 20000.0f;
    private int size;
    private float explosionTimer;
    private bool disableParticles;

    void Start()
    {
        asteroidSpawner = FindObjectOfType<AsteroidSpawner>();
        disableParticles = false;
    }
    public void newAsteroid () {
        Vector3 initialScale;
        initialScale.x = Random.Range(610.0f,850.0f);
        initialScale.y = Random.Range(610.0f,850.0f);
        initialScale.z = Random.Range(610.0f,850.0f);        
        rotVec.x = Random.Range(-8.0f, 8.0f);
        rotVec.y = Random.Range(-8.0f, 8.0f);
        rotVec.z = Random.Range(-8.0f, 8.0f);
        transform.localScale = initialScale;
        while (directionVec.x > -1.0f && directionVec.x < 1.0f)
        {
            directionVec.x = Random.Range(-2.0f, 2.0f);            
        }
        while (directionVec.z > -1.0f && directionVec.z < 1.0f)
        {
            directionVec.z = Random.Range(-2.0f, 2.0f);
        }
            directionVec.y = 0;
        rigidBody.AddForce(directionVec.normalized * speed);
        size = 1;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (Vector3.Distance(transform.position, new Vector3(0.0f, 0.0f, 0.0f)) > 150.0f) Destroy(gameObject);
        if (explosionTimer > 0) explosionTimer -= Time.deltaTime;
        transform.Rotate(rotVec * Time.deltaTime);
        if (transform.position.x < xMin) transform.position = new Vector3(xMax, 0.0f, transform.position.z);
        if (transform.position.x > xMax) transform.position = new Vector3(xMin, 0.0f, transform.position.z);
        if (transform.position.z < zMin) transform.position = new Vector3(transform.position.x, 0.0f, zMax);
        if (transform.position.z > zMax) transform.position = new Vector3(transform.position.x, 0.0f, zMin);
        if (transform.position.y != 0.0f) transform.position = new Vector3(transform.position.x, 0.0f, transform.position.z);
        if (rigidBody.velocity.magnitude > maxSpeed) rigidBody.velocity *= 0.7f;
        if (rigidBody.velocity.magnitude < minSpeed)
        {
            rigidBody.AddForce(directionVec.normalized * speed);
            rigidBody.velocity = new Vector3(rigidBody.velocity.x, 0.0f, rigidBody.velocity.z);
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "projectile")
        {
            projectile proj = collider.GetComponentInParent<projectile>();
            proj.destroy();
            divideAsteroid();
        }
    }

    public void divideAsteroid() {
        if (explosionTimer > 0) return;
        Rigidbody astExp = (Rigidbody)Instantiate(asteroidExplosion, transform.position, transform.rotation);
        astExp.velocity = rigidBody.velocity;
        if (size < 3)
        {
            Rigidbody newAsteroid1 = (Rigidbody)Instantiate(asteroid, transform.position, transform.rotation);
            asteroidScript newAsteroid1Scr = newAsteroid1.GetComponent<asteroidScript>();
            newAsteroid1Scr.newAsteroid();            
            newAsteroid1Scr.setSize(size + 1);
            newAsteroid1Scr.setPosition(newPos1.position);
            newAsteroid1Scr.setMass(rigidBody.mass / 2);
            newAsteroid1Scr.setExplosionTimer();
            asteroidSpawner.incAsteroidCount();
            Rigidbody newAsteroid2 = (Rigidbody)Instantiate(asteroid, transform.position, transform.rotation);
            asteroidScript newAsteroid2Scr = newAsteroid2.GetComponent<asteroidScript>();
            newAsteroid2Scr.newAsteroid();
            newAsteroid2Scr.setSize(size + 1);
            newAsteroid2Scr.setPosition(newPos2.position);
            newAsteroid2Scr.setMass(rigidBody.mass / 2);
            newAsteroid2Scr.setExplosionTimer();
            asteroidSpawner.incAsteroidCount();

            newAsteroid1.AddExplosionForce(1000.0f, transform.position, expRadius, 5.0f);
            newAsteroid1Scr.setVelocity(newAsteroid1.velocity + rigidBody.velocity);
            newAsteroid2.AddExplosionForce(1000.0f, transform.position, expRadius, 5.0f);
            newAsteroid1Scr.setVelocity(newAsteroid2.velocity + rigidBody.velocity);
        }
        Destroy(gameObject);
        asteroidSpawner.decAsteroidCount();
    }

    public void setExplosionTimer()
    {
        explosionTimer = explosionTime;
    }
    public void setSize(int size)
    {        
        transform.localScale /= (float)size;        
        this.size = size;        
    }

    public void setMass(float mass)
    {
        rigidBody.mass = mass;
    }

    public void setPosition(Vector3 position)
    {
        transform.position = position;
    }

    public void setVelocity(Vector3 velocity)
    {
        rigidBody.velocity = velocity;
    }

    public void recieveExplosion(Vector3 source)
    {
        directionVec = transform.position - source;
        rigidBody.AddForce((transform.position - source) * 500.0f);
        divideAsteroid();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{

    public Rigidbody rigidBody;
    public float throttle = 1.0f;
    public float rotSpeed = 1.0f;
    public float maxForce;
    public float maxSpeedBrake;
    public float inverseInertia;
    public float maxThruster;
    public float thrusterInc;
    public int fractionOfTouch, healingAmount;
    public weapons weaponsScr;
    public ParticleSystem thrusterPart;
    private float thruster;
    public float zMin, zMax, xMin, xMax, inmunityDuration;
    public Light inmunityLight;
    private int energy;
    private float inmunityTimer;
    private AudioSource hitSound;
    public AudioSource healSound;

    void Start()
    {
        Screen.orientation = ScreenOrientation.Landscape;
        thruster = 0.0f;
        inmunityLight.enabled = false;
        energy = 100;
        inmunityTimer = 0;
        hitSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (inmunityLight.enabled)
        {
            inmunityTimer += Time.deltaTime;
            if (inmunityTimer > inmunityDuration)
            {
                inmunityLight.enabled = false;
            }
        }
        bool mustBrake = true;
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Stationary)
            {
                if (touch.position.y > (Screen.height - Screen.height / 3) &&
                    (touch.position.x > (Screen.width - Screen.width / fractionOfTouch) ||
                    touch.position.x < (Screen.width / fractionOfTouch)))
                {
                    mustBrake = false;
                    if (thruster < maxThruster) thruster += thrusterInc;
                    Vector3 newForce = transform.forward * -throttle * Time.deltaTime;
                    if ((rigidBody.velocity + newForce).magnitude < maxForce)
                    {
                        rigidBody.AddForce(newForce);
                    }
                    else
                    {
                        rigidBody.velocity *= maxSpeedBrake;
                    }
                }
                else if (touch.position.y < (Screen.height - Screen.height / 3) &&
                        touch.position.y > (Screen.height / 3) &&
                        touch.position.x > (Screen.width - Screen.width / fractionOfTouch))
                {
                    transform.RotateAround(transform.position, transform.up, Time.deltaTime * rotSpeed);
                }
                else if (touch.position.y < (Screen.height - Screen.height / 3) &&
                            touch.position.y > (Screen.height / 3) &&
                            touch.position.x > (Screen.width - Screen.width / fractionOfTouch))
                {
                    transform.RotateAround(transform.position, transform.up, Time.deltaTime * rotSpeed);
                }
                else if (touch.position.y < (Screen.height - Screen.height / 3) &&
                            touch.position.y > (Screen.height / 3) &&
                            touch.position.x < (Screen.width / fractionOfTouch))
                {
                    transform.RotateAround(transform.position, transform.up, Time.deltaTime * -rotSpeed);
                }
                else
                {
                    weaponsScr.fire();
                }
            }
        }
        if (mustBrake)
        {
            if (thruster > 0.0f) thruster -= thrusterInc * 0.9f;
            if (Input.GetKey("down")) rigidBody.velocity *= inverseInertia * 0.985f;
            else rigidBody.velocity *= inverseInertia;
        }
        if (transform.position.x < xMin) transform.position = new Vector3(xMax, 0.0f, transform.position.z);
        if (transform.position.x > xMax) transform.position = new Vector3(xMin, 0.0f, transform.position.z);
        if (transform.position.z < zMin) transform.position = new Vector3(transform.position.x, 0.0f, zMax);
        if (transform.position.z > zMax) transform.position = new Vector3(transform.position.x, 0.0f, zMin);
        if (transform.position.y != 0.0f) transform.position = new Vector3(transform.position.x, 0.0f, transform.position.z);

        ParticleSystem.EmissionModule em = thrusterPart.emission;
        em.rateOverTime = thruster;
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision " + collision.gameObject.tag);
        if ((collision.gameObject.tag == "asteroid" || 
            collision.gameObject.tag == "enemy")
            && !inmunityLight.enabled)
        {
            inmunityLight.enabled = true;
            energy -= 10;
            hitSound.Play();
            inmunityTimer = 0.0f;
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "enemyProjectile" && !inmunityLight.enabled)
        {
            inmunityLight.enabled = true;
            energy -= 10;
            hitSound.Play();
            inmunityTimer = 0.0f;
        }
    }

    public int getEnergy()
    {
        return energy;
    }

    public void heal()
    {
        healSound.Play();
        energy += healingAmount;
        energy = Mathf.Clamp(energy, 0, 100);

    }

    public void gameOver()
    {
        int hiscore = PlayerPrefs.GetInt("HI-SCORE");
        int score = FindObjectOfType<AsteroidSpawner>().scoreAmount;
        if (score > hiscore) PlayerPrefs.SetInt("HI-SCORE", score);
        Destroy(gameObject);
    }
}
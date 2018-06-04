using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour {

    public float rotSpeed;
    public Transform background;
    public Rigidbody explosion;
    public float duration;
    public int type;
    
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        duration -= Time.deltaTime;
        if (duration < 0) Destroy(gameObject);
        this.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), rotSpeed * Time.deltaTime);
        background.Rotate(new Vector3(0.0f, 1.0f, 0.0f), -rotSpeed * Time.deltaTime * 2);
    }
    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            Destroy(this.gameObject);
            if (type == 0)
            {
                Rigidbody newExplosion = (Rigidbody)Instantiate(explosion, transform.position, transform.rotation);
            }
            if (type == 1)
            {
                GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<player>().heal();                
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

    public float explosionSpeed, duration;
    private float time;

	void Start () {
        time = 0;
	}
	
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime;
        transform.localScale = transform.localScale + new Vector3(explosionSpeed * Time.deltaTime, 0.0f, explosionSpeed * Time.deltaTime);// * explosionSpeed * Time.deltaTime;	
        if (time > duration) Destroy(this.gameObject);
	}

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "asteroid")
        {
            asteroidScript ast = collider.GetComponent<asteroidScript>();
            ast.recieveExplosion(transform.position);           
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile : MonoBehaviour {

    public float speed;
    public float maxDistance;
    public Rigidbody rigidBody;
	void Start () {
        rigidBody.AddForce(transform.forward * speed);
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, new Vector3(0.0f, 0.0f, 0.0f)) > maxDistance) this.destroy();
    }
    public void destroy()
    {
        Destroy(gameObject);
    }
}

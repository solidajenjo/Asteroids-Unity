using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidExplosion : MonoBehaviour {

	public ParticleSystem part;

	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        if (!part.isEmitting) Destroy(gameObject);
    }
}

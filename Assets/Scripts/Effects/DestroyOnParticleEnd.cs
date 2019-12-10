using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://answers.unity.com/questions/219609/auto-destroying-particle-system.html
public class DestroyOnParticleEnd : MonoBehaviour {
	private ParticleSystem particleSystem;

	// Start is called before the first frame update
	void Start() {
		particleSystem = GetComponent<ParticleSystem>();
	}

	// Update is called once per frame
	void Update() {
		if (particleSystem) {
			if (!particleSystem.IsAlive()) {
				Destroy(this.gameObject);
			}
		}
	}
}

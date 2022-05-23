using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour {
	public string id;
	public AudioClip placeSound;
	public AudioClip breakSound;
	private Animator animator;
	private AudioSource audioSource;

	private void Awake() {
		animator = GetComponent<Animator>();
		audioSource = GetComponent<AudioSource>();
	}

	public void placeBlock() {
		animator.SetTrigger("place");
		audioSource.clip = placeSound;
		audioSource.Play();
	}

	public void breakBlock() {
		animator.SetTrigger("break");
		audioSource.clip = breakSound;
		audioSource.Play();
		GetComponent<Collider>().enabled = false;
		StartCoroutine(breakWait());
	}

	private IEnumerator breakWait() {
		float waitUntil = Time.time + 0.3f;
		while (Time.time < waitUntil) {
			yield return null;
		}
		Destroy(gameObject);
	}
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	
	public enum PlayerType
	{
		PlayerOne,
		PlayerTwo
	}

	public PlayerType currentPlayer = PlayerType.PlayerOne;

	private Rigidbody2D rb;
	private Animator animator;
	public bool movementEnabled = false;
	private float moveHorizontal;
	private bool ladoD = true;
	private GameManager gM;
	private GameSettings gS;
	private GameObject[] chaves;
	private AudioClip keySound;
	private GameObject tiro;

	public Vector3 posicaoPlayerInicial;

	void Start ()
	{
		//Loading Resources
		keySound = Resources.Load<AudioClip> ("Musics/PickKey-Shnur");

		//Finding References
		rb = GetComponent<Rigidbody2D> ();
		animator = GetComponent<Animator> ();
		gM = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		gS = GameObject.Find ("GameSettings").GetComponent<GameSettings> ();
		posicaoPlayerInicial.y = transform.position.y;

		tiro = Resources.Load ("Prefabs/Key") as GameObject;
	}

	void Update ()
	{
		if (currentPlayer == PlayerType.PlayerOne && movementEnabled) {
			moveHorizontal = Input.GetAxis ("HorizontalP1");

		} else if (movementEnabled) {
			moveHorizontal = Input.GetAxis ("HorizontalP2");
		}
		Flip (moveHorizontal);

		if (rb.velocity.x < -0.1f || rb.velocity.x > 0.1f) {
			animator.SetBool ("andando", true);
		} else {
			animator.SetBool ("andando", false);
		}			

		// INICIO TIRO
		if (Input.GetButtonDown("Fire1")){
			Instantiate(tiro, transform.position, transform.rotation);
		}
	}

	void FixedUpdate ()
	{
		rb.velocity = new Vector2 (moveHorizontal * gS.velocidadePlayers, 0);
	}

	private void Flip (float horizontal)
	{
		if (horizontal > 0 && !ladoD || horizontal < 0 && ladoD) {
			TrocarDirecao ();
		}
	}

	public void TrocarDirecao ()
	{
		ladoD = !ladoD;
		transform.localScale = new Vector3 (transform.localScale.x * -1, transform.localScale.y * 1, transform.localScale.z * 1);
	}

	void OnTriggerEnter2D (Collider2D col)
	{
		if (col.gameObject.tag == "Chave") {
			chaves = GameObject.FindGameObjectsWithTag ("Chave");
			foreach (GameObject chaves in chaves) {
				Destroy (chaves);
			}
			gM.quantidadeChave++;

			AudioSource audioSource = this.GetComponent<AudioSource> ();
			audioSource.clip = keySound;
			audioSource.Play ();
		}

		if (col.gameObject.tag == "Porta") {
			gM.fim = true;
		}
	}
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pig : MonoBehaviour {

    public float maxSpeed = 10;
    public float minSpeed = 5;
    private SpriteRenderer render;
    public Sprite hurt;
    public GameObject boom;
    public GameObject score;

    public bool isPig = false;

    public AudioClip hurtClip;
    public AudioClip dead;
    public AudioClip birdCollision;

    private void Awake()
    {
        render = GetComponent<SpriteRenderer>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            AudioPlay(birdCollision);
            collision.transform.GetComponent<Bird>().Hurt();
        }

        if (collision.relativeVelocity.magnitude > maxSpeed)
        {
            Dead();

        }else if(collision.relativeVelocity.magnitude > minSpeed && collision.relativeVelocity.magnitude < maxSpeed)
        {
            render.sprite = hurt;
            AudioPlay(hurtClip);
        }
    }

    public void Dead()
    {
        if (isPig)
        {
            GameManager._instance.pigs.Remove(this);
        }
        Destroy(gameObject);
        Instantiate(boom, transform.position + new Vector3(0,0.2f,0), Quaternion.identity);
        GameObject go = Instantiate(score, transform.position + new Vector3(0,0.8f,0), Quaternion.identity);
        Destroy(go, 1.5f);
        AudioPlay(dead);
    }

    public void AudioPlay(AudioClip clip)
    {
        AudioSource.PlayClipAtPoint(clip, transform.position);
    }
}

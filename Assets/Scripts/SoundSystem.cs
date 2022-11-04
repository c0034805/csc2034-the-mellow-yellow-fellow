using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSystem : MonoBehaviour{

    [SerializeField]
    private Fellow player;

    [SerializeField]
    private AudioClip complete;

    [SerializeField]
    private AudioClip death;

    [SerializeField]
    private AudioClip buttonHover;

    private AudioSource gameAudio;

    private int previousLives;

    private bool flag = false;

    private void Start(){
        gameAudio = GetComponent<AudioSource>();
        previousLives = player.lives;
    }

    private void Update(){
        if(!flag && player.won){
            gameAudio.clip = complete;
            gameAudio.Play();

            flag = true;
        }

        if(player.lives - previousLives != 0){
            gameAudio.clip = death;
            gameAudio.Play();

            previousLives--;
        }
    }
}

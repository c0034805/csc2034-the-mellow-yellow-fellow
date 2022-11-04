using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour{

    [SerializeField]
    private Material transparentMaterial;

    private Material normalMaterial;

    private Fellow player;

    private void Start(){
        normalMaterial = transform.GetComponent<Renderer>().material;

        player = GameObject.FindGameObjectsWithTag("Fellow")[0].GetComponent<Fellow>();
    }

    void Update(){
        if(player.WallPowerupActive() || player.inWall){
            transform.GetComponent<Renderer>().material = transparentMaterial;
            transform.GetComponent<BoxCollider>().isTrigger = true;
        }
        else{
            transform.GetComponent<Renderer>().material = normalMaterial;
            transform.GetComponent<BoxCollider>().isTrigger = false;
        }
    }
}

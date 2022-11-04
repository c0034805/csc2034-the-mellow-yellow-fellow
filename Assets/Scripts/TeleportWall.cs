using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportWall : MonoBehaviour{

    [SerializeField]
    private TeleportWall pair;

    private void OnTriggerEnter(Collider other){
        other.transform.gameObject.SetActive(false);

        if(pair.transform.position.x > transform.position.x){
            other.transform.position = new Vector3(pair.transform.position.x - 1.5f, other.transform.position.y, transform.position.z);
        }
        else if(pair.transform.position.x < transform.position.x){
            other.transform.position = new Vector3(pair.transform.position.x + 1.5f, other.transform.position.y, transform.position.z);
        }
        else if(pair.transform.position.z > transform.position.z){
            other.transform.position = new Vector3(transform.position.x, other.transform.position.y, pair.transform.position.z - 1.5f);
        }
        else if(pair.transform.position.z < transform.position.z){
            other.transform.position = new Vector3(transform.position.x, other.transform.position.y, pair.transform.position.z + 1.5f);
        }

        other.transform.gameObject.SetActive(true);
    }
}

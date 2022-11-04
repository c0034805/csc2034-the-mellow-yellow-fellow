using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour{

    private bool firstUpdate = true;

    private void Update(){
        if(firstUpdate){
            firstUpdate = false;
            Manager.LoaderCallback();
        }
    }
}

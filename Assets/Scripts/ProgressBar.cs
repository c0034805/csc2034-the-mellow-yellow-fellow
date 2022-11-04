using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour{

    private Image bar;

    private void Awake(){
        bar = transform.GetComponent<Image>();
    }

    private void Update(){
        bar.fillAmount = Manager.LoadingProgress();
    }
}

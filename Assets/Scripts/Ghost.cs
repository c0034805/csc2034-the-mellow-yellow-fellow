using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour{
    
    [SerializeField]
    private Fellow player;

    [SerializeField]
    private Material scaredMaterial;

    [SerializeField]
    private Material slowMaterial;

    private Material normalMaterial;

    [SerializeField]
    private float speed = 3.1f;

    [SerializeField]
    private float speedMultiplier = 1.0f;

    [SerializeField]
    private Vector3 scatterTarget;

    [SerializeField]
    private float scatterThreshold;

    [SerializeField]
    private float thresholdIncrease;

    private float scatterTimer = 0.0f;

    [HideInInspector]
    public bool eaten = false;

    private Vector3 startLocation;

    private UnityEngine.AI.NavMeshAgent agent;
    
    private bool hiding = false;

    private bool inGhostHouse = false;

    private Transform ghostHouse;

    private void Start(){
        if(transform.tag == "GhostMad"){
            speed = 3.1f;
        }
        else if(transform.tag == "GhostIgnorant"){
            speed = 3.4f;
        }
        else if(transform.tag == "GhostChaser"){
            speed = 2.95f;
        }

        startLocation = transform.position;

        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.destination = PickRandomPosition();

        ghostHouse = transform.parent.parent.Find("Maze").Find("GhostHouse");

        normalMaterial = GetComponent<Renderer>().material;
    }

    private void Update(){
        if(player.won || player.lost){
            return;
        }

        if(GameObject.FindGameObjectsWithTag("Timer").Length < 1){
            scatterTimer += Time.deltaTime;
        }

        if(transform.tag == "GhostMad"){
            if(scatterTimer > scatterThreshold){
                agent.destination = scatterTarget;
                if(agent.remainingDistance < 0.2f){
                    scatterTimer = 0.0f;
                    scatterThreshold += thresholdIncrease;
               }
            }
            else if(player.PowerupActive() || CanSeePlayer()){
                agent.destination = player.transform.position;
            }
            else if(agent.remainingDistance < 0.2f){
                agent.destination = PickRandomPosition();
            }
        }
        else if(eaten){
            agent.destination = ghostHouse.position;
            hiding = false;
            GetComponent<Renderer>().material = scaredMaterial;
            speedMultiplier = 1.4f;
            
            if(inGhostHouse && !player.ScarePowerupActive()){
                eaten = false;

                if(player.SlowPowerupActive()){
                    GetComponent<Renderer>().material = slowMaterial;
                    speedMultiplier = 0.8f;
                }
                else{
                    GetComponent<Renderer>().material = normalMaterial;
                    speedMultiplier = 1.0f;
                }
            }
        }
        else if(player.ScarePowerupActive() && !eaten){
            if(!hiding || agent.remainingDistance < 0.5f){
                    hiding = true;
                    agent.destination = PickHidingPlace();
                    GetComponent<Renderer>().material = scaredMaterial;
                    speedMultiplier = 1.4f;
            }
        }
        else{
            if(player.SlowPowerupActive()){
                GetComponent<Renderer>().material = slowMaterial;
                speedMultiplier = 0.8f;
            }
            else{
                GetComponent<Renderer>().material = normalMaterial;
                speedMultiplier = 1.0f;
            }

            if(hiding){
                hiding = false;
            }

            if(scatterTimer > scatterThreshold){
                agent.destination = scatterTarget;
                if(agent.remainingDistance < 0.2f){
                    scatterTimer = 0.0f;
                    scatterThreshold += thresholdIncrease;
               }
            }
            else if(transform.tag == "GhostChaser"){
                agent.destination = player.transform.position;
            }
            else if(transform.tag == "GhostIgnorant"){
                if(agent.remainingDistance < 0.5f){
                        agent.destination = PickRandomPosition();
                }
            }
            else if(transform.tag == "Ghost"){
                if(CanSeePlayer()){
                    agent.destination = player.transform.position;
                }
                else{
                    if(agent.remainingDistance < 0.5f){
                        agent.destination = PickRandomPosition();
                    }
                }
            }
        }

        if(hiding){
            Physics.IgnoreLayerCollision(10, 13, true);
        }
        else{
            Physics.IgnoreLayerCollision(10, 13, false);
        }

        GetComponent<UnityEngine.AI.NavMeshAgent>().speed = speed * speedMultiplier;
    }

    private Vector3 PickRandomPosition(){
        Vector3 destination = transform.position;
        Vector2 randomDirection = UnityEngine.Random.insideUnitCircle * 8.0f;
        
        destination.x += randomDirection.x;
        destination.z += randomDirection.y;

        UnityEngine.AI.NavMeshHit navHit;
        UnityEngine.AI.NavMesh.SamplePosition(destination, out navHit, 8.0f, UnityEngine.AI.NavMesh.AllAreas);

        return navHit.position;
    }

    private bool CanSeePlayer(){
        Vector3 rayPos = transform.position;
        Vector3 rayDir = (player.transform.position - rayPos).normalized;

        RaycastHit info;
        if(Physics.Raycast(rayPos, rayDir, out info)){
            if(info.transform.CompareTag("Fellow")){
                return true;
            }
        }

        return false;
    }

    private Vector3 PickHidingPlace(){
        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;

        UnityEngine.AI.NavMeshHit navHit;
        UnityEngine.AI.NavMesh.SamplePosition(transform.position - (directionToPlayer * 8.0f), out navHit, 8.0f, UnityEngine.AI.NavMesh.AllAreas);

        return navHit.position;
    }

    private void OnTriggerEnter(Collider other){
        if(other.gameObject.CompareTag("GhostHouse")){
            inGhostHouse = true;
        }
    }

    private void OnTriggerExit(Collider other){
        if(other.gameObject.CompareTag("GhostHouse")){
            inGhostHouse = false;
        }
    }

    public void Freeze(){
        GetComponent<UnityEngine.AI.NavMeshAgent>().speed = 0;
        agent.ResetPath();
    }

    public void ResetPosition(){
        transform.position = startLocation;
    }
}

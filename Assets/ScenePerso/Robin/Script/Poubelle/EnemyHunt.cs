//using System.Collections;
//using System.Collections.Generic;
//using System.IO.Pipes;
//using Mirror;
//using UnityEngine;

//public class EnemyHunt : NetworkBehaviour
//{
//    //public Animator animator;
//    FieldOfView fov;
//    EnemyPathway enemyPathway;
//    public GameObject playerRef;
//    bool isHunting;
//    // Start is called before the first frame update
//    void Start()
//    {
//        isHunting = false;
//        fov = GetComponent<FieldOfView>();
//        enemyPathway = GetComponent<EnemyPathway>();
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if (playerRef == null)
//        {
//            playerRef = GameObject.FindGameObjectWithTag("Player");
//        }

//        if (fov.canSeePlayer)
//        {
//            isHunting = true;
//            StopCoroutine(HuntStateTimer());
//        }
//        else if (!fov.canSeePlayer)
//        {
//            StartCoroutine(HuntStateTimer());
//        }

//        if (isHunting)
//        {
//            Hunt();
//        }
//        else if (!isHunting)
//        {
//            if(enemyPathway.pathwayOver && enemyPathway.pathways.Count > 0)
//            {
//                enemyPathway.FindRoom();
//            }  
//        } 
        
//    }
//    void Hunt()
//    {
//        enemyPathway.pathwayOver = true;
//        enemyPathway.enemy.destination = playerRef.transform.position;
//    }

//    void OnTriggerEnter(Collider other)
//    {
//        if (other.tag == "Player")
//        {
//            StartCoroutine(Kill());
//        }
//    }

//    IEnumerator Kill()
//    {
//            yield return new WaitForSeconds(1);
//            //playerRef.SetActive(false);
//    }

//    IEnumerator HuntStateTimer()
//    {
//        yield return new WaitForSeconds(3);
//        isHunting = false;
//    }
//}

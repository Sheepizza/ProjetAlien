using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ControlRoomEvent : MonoBehaviour
{
    public int randomNumber;
    public int AlienFrequenceAttack;
    public AudioSource AlienDanger;
    public GameManager gameManager;
    //public GameObject monstre;
    public Alien alienScript;

    public bool alienAttack = false; 
    public bool playerDanger = false;

    [Space (20)]
    
    public Canvas canvaDeath;
    public Animator lightsWarning;

    public void Start()
    {
        /*monstre = gameManager.J1.transform.GetChild(gameManager.J1.transform.childCount-1).gameObject;
        monstre.SetActive (false);*/
        canvaDeath.enabled = false;
        StartCoroutine(AlienEvent());
    }
   
    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            playerDanger = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerDanger = false;
        }
    }

    IEnumerator AlienEvent()
    {
        yield return new WaitForSeconds(10);
        int randomNumber = Random.Range(0, AlienFrequenceAttack);

        Debug.Log("Tia tir�" + randomNumber);
        switch (randomNumber)
        {
            case 0:
                if (playerDanger == true)
                {
                    StartCoroutine(AlienAttack());
                }
                else
                {
                    StartCoroutine(AlienEvent());
                }
                break;

            default:
                StartCoroutine(AlienEvent());
                break;
        }
        
    }

    
    IEnumerator AlienAttack()
    {
        lightsWarning.SetBool("Warning", true);
        AlienDanger.Play();
        yield return new WaitForSeconds(21);

        if (playerDanger == true)
        {
            Debug.Log("Ti� mort");
            /*monstre.SetActive(true);*/
            alienScript.StartCoroutine(alienScript.Kill());
            //canvaDeath.enabled = true;
            //TUER JOUEUR
        }
        lightsWarning.SetBool("Warning", false);
        StartCoroutine(AlienEvent());
    }

}

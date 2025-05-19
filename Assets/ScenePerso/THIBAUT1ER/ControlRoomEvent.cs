using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ControlRoomEvent : MonoBehaviour
{
    public int randomNumber;
    public int AlienFrequenceAttack;

    public bool alienAttack = false; 
    public bool playerDanger = false;

    [Space (20)]
    
    public Canvas canvaDeath;
    public Animator lightsWarning;

    public void Start()
    {
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

        Debug.Log("Tia tiré" + randomNumber);
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
        //Animation Lights
        lightsWarning.SetBool("Warning", true);  
        Debug.Log("Ti va mourir");
        yield return new WaitForSeconds(5);
        //Animation Alien
        if (playerDanger == true)
        {
            Debug.Log("Tié mort");
            canvaDeath.enabled = true;
            //TUER JOUEUR
        }
        lightsWarning.SetBool("Warning", false);
        StartCoroutine(AlienEvent());
    }

}

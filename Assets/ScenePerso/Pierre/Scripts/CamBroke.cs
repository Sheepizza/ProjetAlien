using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

/// <summary>
/// Script permettant de changer le contraste d'une caméra pour donner un effet grisé et donc cassé 
/// 
/// Amélioration : Faire passer dans ce script également 
/// Ce qui donnera un effet cassé à la caméra dans les salles où elle sera
/// 
/// </summary>
public class CamBroke : MonoBehaviour
{
    public bool isBroke = false;

    [SerializeField]
    private Volume CamVolume;

    public void Start()
    {
        CamVolume  = GetComponent<Volume>();
    }

    public void Update()
    {
        //A enlever une fois le code relié
        if(isBroke)
            CameraBrokeVolume();
        else if (!isBroke)
            SetBaseVolume();

        //Debug.Log(CamVolume.profile.GetComponent<ColorAdjustments>().contrast.value);
    }

    public void SetBaseVolume()
    {
        //Remet le contraste à sa valeur de base
        if(CamVolume.profile.TryGet(out ColorAdjustments _colorAdjustments))
        {
            _colorAdjustments.contrast.value = 0;
        }

        //CamVolume.profile.GetComponent<ColorAdjustments>().saturation.value = -100;
        /*CamVolume.profile.GetComponent<Vignette>().intensity.value = 1;
        CamVolume.profile.GetComponent<Vignette>().smoothness.value = 0.01f;
        CamVolume.profile.GetComponent<FilmGrain>().intensity.value = 1;
        CamVolume.profile.GetComponent<FilmGrain>().response.value = 0;*/
    }

    public void CameraBrokeVolume()
    {
        //Changer le contraste permet de donné l'effet grisé
        if(CamVolume.profile.TryGet(out ColorAdjustments _colorAdjustments))
        {
            _colorAdjustments.contrast.value = -100;
        }
    }
}
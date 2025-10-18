using UnityEngine;
using System.Collections.Generic;

public class ActivateFire : MonoBehaviour
{
   [SerializeField] private List<GameObject> flames = new List<GameObject>();

   

    public void ActivateFlame(int flameID){
        if(flames[flameID-1]!=null){
            flames[flameID-1].SetActive(true);
        }
    }

}

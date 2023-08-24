using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatRotation : MonoBehaviour
{
    public GameObject catModel;
    void Awake()
    {
        if (catModel != null)
        {
            // Set the rotation of the targetObject to (0, 166, 0)
            catModel.transform.position = new Vector3(0.00234f, 0.709f, 0.8429f);
            catModel.transform.rotation = Quaternion.Euler(-27.314f, -27.66f, -17.708f);
        }
           
            
    }

}

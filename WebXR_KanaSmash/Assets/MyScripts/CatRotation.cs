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
            catModel.transform.rotation =  Quaternion.Euler(-27.314f, -109.26f, -17.708f);
            catModel.transform.position = new Vector3(-0.6531f, 0.6575f, 0.781f);
        }
    }

}

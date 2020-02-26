using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//SIMPLE SCRIPT TO MAKE SURE THE GAMEOBJECT THIS IS ATTACHED TO ALWAYS FACES THE CAMERA
public class LookAtCamera : MonoBehaviour
{
    


    void Update()
    {
        Camera camera = Camera.main;

        transform.LookAt(transform.position + camera.transform.rotation * Vector3.back, camera.transform.rotation * Vector3.up);
        this.transform.Rotate(0, 180, 0);
    }
}
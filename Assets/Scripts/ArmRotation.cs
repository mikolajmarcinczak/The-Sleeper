using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class ArmRotation : MonoBehaviour
{
    public int rotationOffset = 0;

    // Update is called once per frame
    void Update()
    {
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        difference.Normalize();          //Normalizacja vectora

        float rotZ = (float)Math.Atan2(difference.y, difference.x) * Mathf.Rad2Deg; //odnalezienie kąta
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ + rotationOffset);
    }
}

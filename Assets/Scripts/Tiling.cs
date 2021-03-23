using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]

public class Tiling : MonoBehaviour
{
    public int offsetX = 2;

    //Sprawdzanie czy nie trzeba zainicjowac nowych
    public bool hasARightBud = false;
    public bool hasALeftBud = false;

    public bool reverseScale = false; //jesli nie da sie dorobic

    private float spriteWidth = 0f;   //szerokosc elementu
    private Camera cam;
    private Transform myTransform;

    void Awake()
    {
        cam = Camera.main;
        myTransform = transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer sRenderer = GetComponent<SpriteRenderer>();
        spriteWidth = sRenderer.sprite.bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        //Czy nadal potrzebuje dogenerowania
        if (hasALeftBud == false || hasARightBud == false)
        {
            float camHorizontalExtent = cam.orthographicSize * Screen.width / Screen.height;

            float edgeVisiblePositionRight = (myTransform.position.x + spriteWidth / 2) - camHorizontalExtent;
            float edgeVisiblePositionLeft = (myTransform.position.x - spriteWidth / 2) + camHorizontalExtent;

            if (cam.transform.position.x >= edgeVisiblePositionRight - offsetX && hasARightBud == false)
            {
                MakeNewBuddy(1);
                hasARightBud = true;
            }
            else if (cam.transform.position.x <= edgeVisiblePositionLeft + offsetX && hasALeftBud == false)
            {
                MakeNewBuddy(-1);
                hasALeftBud = true;
            }
        }
    }

    //tworzenie kolejnych plansz
    void MakeNewBuddy(int rightOrLeft)
    {
        Vector3 newPosition = new Vector3(myTransform.position.x + spriteWidth * rightOrLeft, myTransform.position.y, myTransform.position.z);
        Transform newBuddy = Instantiate(myTransform, newPosition, myTransform.rotation) as Transform;
        
        if(reverseScale == true)
        {
            newBuddy.localScale = new Vector3(newBuddy.localScale.x * -1, newBuddy.localScale.y, newBuddy.localScale.z);
        }

        newBuddy.parent = myTransform;
        if (rightOrLeft > 0)
        {
            newBuddy.GetComponent<Tiling>().hasALeftBud = true;
        }
        else
        {
            newBuddy.GetComponent<Tiling>().hasARightBud = true;
        }
    }
}

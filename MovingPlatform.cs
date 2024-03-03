using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.04f);
            transform.position = new Vector2(transform.position.x, transform.position.y + 0.04f);
        }
    }


}

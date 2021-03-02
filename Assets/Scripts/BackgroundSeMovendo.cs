using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSeMovendo : MonoBehaviour
{
    public float velocidade;
    
    void Update()
    {
        transform.position -= new Vector3(velocidade * Time.deltaTime, transform.position.y);        
    }
}
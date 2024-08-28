using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class ScrollImage : MonoBehaviour
{
    public float speed;
    
    [SerializeField]
    private Image background;
    
    void Update()
    {
        background.material.mainTextureOffset += new Vector2(speed * Time.deltaTime, 0);
    }
}

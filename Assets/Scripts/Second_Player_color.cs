using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Second_Player_color : MonoBehaviour
{
    public Color newColor;
    private SpriteRenderer rend;
    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        rend.color = newColor;
    }
}

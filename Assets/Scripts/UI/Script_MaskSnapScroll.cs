﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Script_MaskSnapScroll : MonoBehaviour
{
    private RectTransform container;
    
    // Start is called before the first frame update
    void Start()
    {
        container = GetComponent<RectTransform>();

        print("container height: " + container.rect.height);
        print("container anchoredY: " + container.anchoredPosition.y);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
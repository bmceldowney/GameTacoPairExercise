using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ui : MonoBehaviour
{
    [SerializeField]
    weatherApi weatherApi;
    [SerializeField]
    TextMeshProUGUI resultDisplay;

    // Start is called before the first frame update
    void Start()
    {
        weatherApi.WeatherReceived += (bool result) => {
            resultDisplay.text = result.ToString();
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

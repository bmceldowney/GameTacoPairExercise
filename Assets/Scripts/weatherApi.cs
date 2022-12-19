using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class weatherApi : MonoBehaviour
{
    public so_key key;
    public float rangeBoundary = 5f;
    public event System.Action<bool> WeatherReceived;

    void Start(){
        // example
        GuessTemperature("Seattle", 30f);
    }

    IEnumerator GetWeather(string city, float guess){
        string url = "http://api.openweathermap.org/data/2.5/weather?q=" + city + "&units=imperial&appid=" + key.apiKey;
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = url.Split('/');
            int page = pages.Length - 1;
            string result = pages[page];

            switch (webRequest.result){
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    string[] splitResult = result.Split("\"temp\":");
                    Debug.Log(splitResult.Length);
                    string[] splitTemp = splitResult[1].Split(',');
                    string tempString = splitTemp[0];
                    float temp = float.Parse(tempString);
                    bool isInRange = Mathf.Abs(temp - guess) < rangeBoundary;
                    WeatherReceived?.Invoke(isInRange);
                    Debug.Log($"Actual Temp in {city} is {temp}, is in range? {isInRange}");
                    break;
            }
        }
    }

    public void GuessTemperature (string cityName, float temperature)
    {
        StartCoroutine( GetWeather( cityName, temperature ) );
    }

}

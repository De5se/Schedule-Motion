using System;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;

namespace _Scripts.SaveSystem
{
    public class WorldTimeAPI : MonoBehaviour
    {
        //json container
        private struct TimeData {
            //public string client_ip;
            //...
            public string datetime;
            //..
        }

        private const string API_URL = "http://worldtimeapi.org/api/ip";

        private static DateTime _currentDateTime;

        private void Start ( ) {
            StartCoroutine( GetRealDateTimeFromAPI ( ) );
        }

        public DateTime GetCurrentDateTime ( ) 
        {
            //here we don't need to get the datetime from the server again
            // just add elapsed time since the game start to _currentDateTime

            return _currentDateTime.AddSeconds ( Time.realtimeSinceStartup );
        }

        private static IEnumerator GetRealDateTimeFromAPI ( ) 
        {
            var webRequest = UnityWebRequest.Get ( API_URL );
            yield return webRequest.SendWebRequest ( );
            
            
            if ( webRequest.isNetworkError || webRequest.isHttpError ) {
                //error
                Debug.Log ( "Error: " + webRequest.error );

            } else {
                //success
                var timeData = JsonUtility.FromJson<TimeData> ( webRequest.downloadHandler.text );

                _currentDateTime = ParseDateTime ( timeData.datetime );
                Debug.Log("_current date tie is: " + _currentDateTime);
            }
        }
        //datetime format => 2020-08-14T15:54:04+01:00
        private static DateTime ParseDateTime ( string datetime ) 
        {
            //match 0000-00-00
            var date = Regex.Match ( datetime, @"^\d{4}-\d{2}-\d{2}" ).Value;
            //match 00:00:00
            var time = Regex.Match ( datetime, @"\d{2}:\d{2}:\d{2}" ).Value;

            return DateTime.Parse ($"{date} {time}");
        }
    }
}
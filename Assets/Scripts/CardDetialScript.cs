using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class CardDetialScript : MonoBehaviour
{
    [TextArea(5,1)]
    public string defaultText;
    public Text displayCardDetailText;
    public string cardHolderName;
    public int cardHolderAge;
    public string cardHolderCountry;
    public float timeToDisplayDetails = 0.2f;
    float timeFactor;

    [Button]
    void DisplayCardDetails()
    {
        if (!Application.isPlaying)
            return;

        Debug.Log("Display Card Detail Starts !");
        string sampleString = defaultText;
        sampleString = sampleString.Replace("$", cardHolderName);
        sampleString = sampleString.Replace("@", cardHolderAge.ToString());
        sampleString = sampleString.Replace("#", cardHolderCountry);

        char[] charArr = sampleString.ToCharArray();
        timeFactor = timeToDisplayDetails / charArr.Length;

        StartCoroutine(nameof(StartDisplayAnimation),charArr);
    }

    IEnumerator StartDisplayAnimation(char[] arr)
    {
        displayCardDetailText.text = "";

        for (int i = 0; i < arr.Length; i++)
        {
            displayCardDetailText.text += arr[i];
            yield return new WaitForSeconds(timeFactor);
        }
    }

    void Start()
    {
        
    }


    void Update()
    {
        
    }
}

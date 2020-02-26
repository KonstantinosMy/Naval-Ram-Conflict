using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//SCRIPT TO BE RUN WHEN BLUE SHIP WINS
public class RedWins : MonoBehaviour
{

    public GameObject text;
    public TextMeshProUGUI txt;
    private float rnd = 0f;

    //Trashtalk string list
    string[] Trashtalk = { "RED DESTROYED PEASANT BLUE", "BLUE HAS BEEN STOMPED BY RED", "RED SHOWED NO MERCY FOR BLUE", "RED HAS DOMINATED BLUE" };
    void Start()
    {
        //gets a random trashtalk sentence
        rnd = Random.Range(1f, 4f); ;
        Debug.Log("Decided Num: " + rnd);
        StartCoroutine(WaitforThree());
    }

    IEnumerator WaitforThree()
    {

        yield return new WaitForSeconds(3);
        ShowMessage();
    }

    //Prints trashtalk sentence
    void ShowMessage()
    {
        text.gameObject.SetActive(true);
        txt.text = Trashtalk[(int)Mathf.Round(rnd)].ToString();
        Debug.Log("Final value: " + Trashtalk[(int)Mathf.Round(rnd)] + "with int:" + rnd);
    }



}

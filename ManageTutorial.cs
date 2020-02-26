using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


//SIMPLE SCRIPT TO MANAGE THE FIRST TUTORIAL SCENE
public class ManageTutorial : MonoBehaviour
{

    public GameObject one;
    public GameObject two;
    public GameObject three;


    //Transitions from first slide to second
    public void OneToTwo()
    {
        one.SetActive(false);
        two.SetActive(true);
    }
    //Transitions from second slide to third
    public void TwoToThree()
    {
        two.SetActive(false);
        three.SetActive(true);
    }
    //Transitions from third slide to the main game
    public void ThreeToFour()
    {
        three.SetActive(false);
        SceneManager.LoadScene("MainScene");
    }
}

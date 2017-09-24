using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InformationPanel : MonoBehaviour {

    public Text textSpace;

    public void Start()
    {
        textSpace = this.GetComponent<Text>();
    }
    public void EditText(string textToDisplay)
    {
        //Create Scrolling effect
        textSpace.text = textToDisplay;
    }
}

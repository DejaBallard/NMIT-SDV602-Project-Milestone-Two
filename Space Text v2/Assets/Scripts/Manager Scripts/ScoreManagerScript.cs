using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SQLite4Unity3d;
//Script Purpose:
//Manages the text output for the score screen

//--------------------------------------- Start Of Score Manager Script : Mono Class ------------------------------------------------------------------------------
public class ScoreManagerScript : MonoBehaviour {
    //--------------------------------------- Start Of Top Level Variable Decalaring ------------------------------------------------------------
    public Text _Output;
    //--------------------------------------- End Of Top Level Variable Declaring ---------------------------------------------------------
    //-------------------------------------- Start Of Methods ----------------------------------------------------------------------------	
    // Update is called once per frame
    void Update () {
        _Output.text = "S:" + PlayerManagerScript._CurrentPlayer.PlayerScore.ToString() + "\n" +
                        "$:" + PlayerManagerScript._CurrentPlayer.PlayerMoney.ToString();
	}
    //-------------------------------------- End Of Methods ----------------------------------------------------------------------------	
}
//------------------------------------------ End Of Score Manager Script : Mono Class-------------------------------------------------------------------------------

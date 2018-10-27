using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Script Purpose:
//Gathers all canvas's within the scene and adds them to the instance

//--------------------------------------- Start Of CanvasManagerScript : Mono Class ------------------------------------------------------------------------------
public class CanvasManagerScript : MonoBehaviour {
	//--------------------------------------- Start Of Top Level Variable Decalaring ------------------------------------------------------------
	//--------------------------------------- End Of Top Level Variable Declaring ---------------------------------------------------------
	
	
	//-------------------------------------- Start Of Methods ----------------------------------------------------------------------------	
	void Awake () {
		//-------------- Adding canvas's to instance -----------------
		Canvas[] _CanvasArray = gameObject.GetComponentsInChildren<Canvas>();
		foreach (Canvas iCanvas in _CanvasArray) {
            //Adding each canvas to the instance
			GameManagerScript._Instance._CanvasDic.Add(iCanvas.name, iCanvas);
			Debug.Log(name + ": " + iCanvas.name + " has been added to GameManager");
		}
	}	
	//-------------------------------------- End Of Methods ----------------------------------------------------------------------------	
}
//------------------------------------------ End Of CanvasManagerScript : Mono Class-------------------------------------------------------------------------------

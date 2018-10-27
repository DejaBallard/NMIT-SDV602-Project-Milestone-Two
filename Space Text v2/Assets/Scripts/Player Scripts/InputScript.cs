using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//--------------------------------------- Start Of InputScript : Mono Class ------------------------------------------------------------------------------
public class InputScript : MonoBehaviour {
    //--------------------------------------- Start Of Top Level Variable Decalaring ------------------------------------------------------------
    public Text _TxtOutput;
    public Canvas _Canvas;
	private InputField _inputField;
	private InputField.SubmitEvent _submitEvent;
	private InputField.OnChangeEvent _onChangeEvent;

//--------------------------------------- End Of Top Level Variable Declaring ---------------------------------------------------------


//-------------------------------------- Start Of Methods ----------------------------------------------------------------------------	
	// Use this for initialization
	void Start () {
	// Adding the gameobject to the variable
		_inputField = this.GetComponent<InputField>();
		
		if (_inputField != null){
			_submitEvent = new InputField.SubmitEvent();
			_submitEvent.AddListener(submitInput);
			_inputField.onEndEdit = _submitEvent;
            //If this input field is attached to main canvas, show story
            if (_Canvas.name == "Main Canvas")
            {
                _TxtOutput.text = PlayerManagerScript.GetCurrentScene().Name + "\n" + PlayerManagerScript.GetCurrentScene().SceneStoryDescription;
            }
		}
	}

    private void Update()
    {
        //If on inventory canvas, display user inventory
        string lcList = "";
        if (_Canvas.name == "Inventory Canvas")
        {
            string[] Inventory = PlayerManagerScript._CurrentPlayer.InventoryList.Split(',');
            foreach(string i in Inventory)
            {
                if (i != "")
                {
                    GameItemScript lcItem = PlayerManagerScript.GetItem(i);
                    lcList = lcList + lcItem.Name + " - S"+lcItem.Score+" - $" + lcItem.Price+"\n";
                }
            }
            _TxtOutput.text = lcList;
            _inputField.ActivateInputField();
        }
    }

    private void submitInput(string prArg){
        if (!Input.GetKeyDown(KeyCode.Escape) && !Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1) && !Input.GetMouseButtonDown(2))
        {
            string lcCurrentText = _TxtOutput.text;
            CommandProccessorScript lcCommandPro = new CommandProccessorScript();
            //text being checked with command proccessor
            _TxtOutput.text = lcCommandPro.Parse(prArg);
            //Reset input field to blank
            _inputField.text = "";
        }
        //Allow input to be used again
        _inputField.ActivateInputField();
    }
	//-------------------------------------- End Of Methods ----------------------------------------------------------------------------	
}
//------------------------------------------ End Of InputScript : Mono Class-------------------------------------------------------------------------------

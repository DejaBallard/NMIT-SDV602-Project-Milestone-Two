using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnitySceneManager = UnityEngine.SceneManagement;
//--------------------------------------- Start Of GameManagerScript : Mono Class ------------------------------------------------------------------------------
public class GameManagerScript : MonoBehaviour {
	//--------------------------------------- Start Of Top Level Variable Decalaring ------------------------------------------------------------
	public static GameManagerScript _Instance;
    public static Camera _MainCamera;
    public Canvas _ActiveCanvas;
    public Dictionary<string, Canvas> _CanvasDic;
    public InputField _MainInput;
    public InputField _MapInput;
    public InputField _InvInput;
    public Animator _StarAnim;
    public Animator _PlanetAnim;
    public Animator _EnemyAnim;
    public Animator _ShopAnim;
    public string _StartCanvas;
    public DataService _Db = new DataService();
    private bool _gameRunning;
	//--------------------------------------- End Of Top Level Variable Declaring ---------------------------------------------------------
	
	
	//-------------------------------------- Start Of Methods ----------------------------------------------------------------------------	
	//Mono class that runs before start
	void Awake(){
	//if game manager hasn't been created yet
		if (_Instance ==null){
            Debug.Log(name + ": Created");
            _Instance = this;
			_gameRunning = true;
            attachObjects();
            _Db.CreateTables(new[] { typeof(PlayerScript),typeof(GameItemScript),typeof(SceneScript),typeof(SceneItemScript), typeof(SceneDirectionScript) });
            _CanvasDic = new Dictionary<string, Canvas>();

		}
		else {
			Destroy (gameObject);
            attachObjects();
            Debug.LogWarning (name +": Duplicate destoryed");
		}
	}
    //Mono class that runs after awake
    private void Start()
    {
        SetActiveCanvas(_StartCanvas);
    }
    //Chages the Active Cancvas
    public void SetActiveCanvas(string prCanvasName)
    {
        if (_CanvasDic.ContainsKey(prCanvasName))
        {
            foreach (Canvas iCanvas in _CanvasDic.Values)
            {
                iCanvas.gameObject.SetActive(false);
            }
            //set active canvas to the passed in value
            _ActiveCanvas = _CanvasDic[prCanvasName];
            Debug.Log(name + ": Loaded Canvas: " + prCanvasName);
            _ActiveCanvas.gameObject.SetActive(true);
            //not all canvas's use input, so try attach input if it needs it
            try
            {
                SetActiveInput();
            }catch(Exception e) { }
        }
        else Debug.LogError(name + ": " + prCanvasName + " doesn't exist within dictonary");
    }

    public void SetActiveInput()
    {
        //because user can not click the input to activate it, so keyboard is always stuck to the canvas
        switch (_ActiveCanvas.name)
        {
            case "Main Canvas":
                _MainInput.ActivateInputField();
                break;
            case "Map Canvas":
                _MapInput.ActivateInputField();
                break;
            case "Inventory Canvas":
                _InvInput.ActivateInputField();
                break;
        }
    }

    public string CurrentUnityScene()
    {
        return UnitySceneManager.SceneManager.GetActiveScene().name;
    }

    public void ChangeUnityScene(string prUnitySceneName)
    {
        UnitySceneManager.SceneManager.LoadScene("01_Game");
        Debug.Log(name + ": Loading " + prUnitySceneName);
    }

    public void NewGame() {
        //updates users information
        PlayerManagerScript._CurrentPlayer.CollectList = "";
        PlayerManagerScript._CurrentPlayer.InventoryList = "";
        PlayerManagerScript._CurrentPlayer.Visted = "E3";
        PlayerManagerScript._CurrentPlayer.Scanned = "";
        PlayerManagerScript._CurrentPlayer.PlayerScore = 0;
        PlayerManagerScript._CurrentPlayer.PlayerMoney = 0;
        PlayerManagerScript._CurrentPlayer.PlayerHealth = 3;
        PlayerManagerScript._CurrentPlayer.CurrentLocationID = "E3";
        _Db.Connection.InsertOrReplace(PlayerManagerScript._CurrentPlayer);
        ChangeUnityScene("01_Game");
    }
    public void ContinueGame() {
        ChangeUnityScene("01_Game");
    }

    private void attachObjects()
    {
        //not all unity scene use these items, so try attach or ignore
        try
        {
            _MainInput = GameObject.FindGameObjectWithTag("Main").GetComponent<InputField>() as InputField;
            _MapInput = GameObject.FindGameObjectWithTag("Map").GetComponent<InputField>() as InputField;
            _InvInput = GameObject.FindGameObjectWithTag("Inventory").GetComponent<InputField>() as InputField;
            _StarAnim = GameObject.FindGameObjectWithTag("Stars").GetComponent<Animator>() as Animator;
            _PlanetAnim = GameObject.FindGameObjectWithTag("Planet").GetComponent<Animator>() as Animator;
            _EnemyAnim = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Animator>() as Animator;
            _ShopAnim = GameObject.FindGameObjectWithTag("Shop").GetComponent<Animator>() as Animator;
            Debug.Log(name + ": All objects attached");
        }
        catch (Exception e)
        {
            Debug.Log(name + ": Objects didn't attach");
        }
    }
    //-------------------------------------- End Of Methods ----------------------------------------------------------------------------	
}
//------------------------------------------ End Of GameManagerScript : Mono Class-------------------------------------------------------------------------------
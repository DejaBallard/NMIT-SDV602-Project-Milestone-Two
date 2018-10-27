using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
//Script Purpose:
//Manage player login and database connection

//--------------------------------------- Start Of Player Manager Script : Mono Class ------------------------------------------------------------------------------
public class PlayerManagerScript : MonoBehaviour
{
    //--------------------------------------- Start Of Top Level Variable Decalaring ------------------------------------------------------------
    public static bool _Created = false;
    public static bool _LoggedIn = false;
    public static PlayerScript _CurrentPlayer { get; set; }
    public static DataService _db = new DataService();
    //--------------------------------------- End Of Top Level Variable Declaring ---------------------------------------------------------


    //-------------------------------------- Start Of Methods ----------------------------------------------------------------------------	
    // Use this for initialization
    void Awake()
    {
        //if not created, create this
        if (!_Created)
        {
            DontDestroyOnLoad(this.gameObject);
            _Created = true;
        }
    }
    public static SceneScript GetCurrentScene()
    {
        SceneScript lcresult;
        lcresult = _db.Connection.Table<SceneScript>().Where(x => x.Name == _CurrentPlayer.CurrentLocationID).First<SceneScript>();
        return lcresult;
    }
    public static GameItemScript GetItem(string prName)
    {
        GameItemScript lcresult;
        lcresult = _db.Connection.Table<GameItemScript>().Where(x => x.Name == prName).First<GameItemScript>();
        return lcresult;
    }
    //-------------------------------------- End Of Methods ----------------------------------------------------------------------------	
}
//------------------------------------------ End Of Player Manager Script : Mono Class-------------------------------------------------------------------------------
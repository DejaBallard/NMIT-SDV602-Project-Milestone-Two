using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SQLite4Unity3d;
//--------------------------------------- Start Of PlayerScript : Mono Class ------------------------------------------------------------------------------
public class PlayerScript {
    //--------------------------------------- Start Of Top Level Variable Decalaring ------------------------------------------------------------
	[PrimaryKey,AutoIncrement]
	public int Id {get; set;}
    public string CurrentLocationID { get; set; }
    public string PlayerName {get; set;}
    public string Password { get; set; }
	public int PlayerHealth {get; set;}
	public int PlayerScore {get; set;}
    public int PlayerMoney { get; set;}






































    public string InventoryList { get; set; }
    public string CollectList { get; set; }
    public string Visted { get; set; }
    public string Scanned { get; set; }

    //--------------------------------------- End Of Top Level Variable Declaring ---------------------------------------------------------


    //-------------------------------------- Start Of Methods ----------------------------------------------------------------------------	
    // Use this for initialization
    // Update is called once per frame
    //-------------------------------------- End Of Methods ----------------------------------------------------------------------------	
}
//------------------------------------------ End Of  : Mono Class-------------------------------------------------------------------------------
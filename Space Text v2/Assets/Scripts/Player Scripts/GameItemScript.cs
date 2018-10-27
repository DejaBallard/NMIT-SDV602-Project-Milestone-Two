using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SQLite4Unity3d;
//--------------------------------------- Start Of  : Mono Class ------------------------------------------------------------------------------
public class GameItemScript {
    //--------------------------------------- Start Of Top Level Variable Decalaring ------------------------------------------------------------
    [PrimaryKey, AutoIncrement]
    public int ItemId { get; set; }
    public string Name { get; set; }
    public int Price { get; set; }
    public int Score { get; set; }
    //--------------------------------------- End Of Top Level Variable Declaring ---------------------------------------------------------


    //-------------------------------------- Start Of Methods ----------------------------------------------------------------------------	
    //-------------------------------------- End Of Methods ----------------------------------------------------------------------------	
}
//------------------------------------------ End Of  : Mono Class-------------------------------------------------------------------------------
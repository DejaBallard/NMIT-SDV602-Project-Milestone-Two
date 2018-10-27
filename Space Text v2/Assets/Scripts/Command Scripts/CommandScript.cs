using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SQLite4Unity3d;
using System.Linq;
//Script Purpose:
//Location of Commands

//--------------------------------------- Start Of CommandScript : Mono Class ------------------------------------------------------------------------------
public class CommandScript : MonoBehaviour
{
    //--------------------------------------- Start Of Top Level Variable Decalaring ------------------------------------------------------------
    protected DataService _db = new DataService();
    //--------------------------------------- End Of Top Level Variable Declaring ---------------------------------------------------------

    //-------------------------------------- Start Of Methods ----------------------------------------------------------------------------	
    public virtual void Do(CommandMapScript prCommand)
    {
    }
    //-------------------------------------- End Of Methods ----------------------------------------------------------------------------	
}
//------------------------------------------ End Of CommandScript : Mono Class-------------------------------------------------------------------------------




//--------------------------------------- Start Of GoCommand : CommandScript Class ------------------------------------------------------------------------------
public class GoCommand : CommandScript
{
    //--------------------------------------- Start Of Top Level Variable Decalaring ------------------------------------------------------------
    //Direction the user inputs
    private string _direction;

    //General text outputs
    private string _NotScanned = "AI: We have not scanned this area yet";
    private string _Scanned = "AI: We have scanned this area";
    //--------------------------------------- End Of Top Level Variable Declaring ---------------------------------------------------------


    //-------------------------------------- Start Of Methods ----------------------------------------------------------------------------	

    public GoCommand(string prDirection)
    {
        //Assin the string to a local variable
        _direction = prDirection;
    }

    public override void Do(CommandMapScript prCommand)
    {
        //Turn off display events
        TurnOffEvent();

        //Assign players current local to a local variable 
        SceneScript lcCurrentScene = PlayerManagerScript.GetCurrentScene();

        //Get scene direction by selecting where From Scene name is current Scenes and direction is users input 
        SceneDirectionScript lcSceneDirection = _db.Connection.Table<SceneDirectionScript>().Where<SceneDirectionScript>(x => x.FromSceneName == lcCurrentScene.Name && x.Direction == _direction).ToList<SceneDirectionScript>().First<SceneDirectionScript>();
        
        //Get new scene by selecting where name is scene direction's to scene variable
        SceneScript lcNewScene = _db.Connection.Table<SceneScript>().Where<SceneScript>(x => x.Name == lcSceneDirection.ToSceneName).ToList<SceneScript>().First<SceneScript>();
        
        //Update current player with new scene
        PlayerManagerScript._CurrentPlayer.CurrentLocationID = lcNewScene.Name;
        
        //Save updated player in database
        _db.Connection.InsertOrReplace(PlayerManagerScript._CurrentPlayer);
        
        //check if new scene has a display event
        HasEvent(lcNewScene.Event);
        
        //if scene has been visited and scanned, Set this as text output
        if (Visited(lcNewScene) && Scanned(lcNewScene))
        {
            prCommand._Result = lcNewScene.Name + "\n" +
                     lcNewScene.SceneStoryDescription + "\n" +
                     _Scanned;
        }
        //Else if scene has only been visited, set this as text output
        else if (Visited(lcNewScene))
        {
            prCommand._Result = lcNewScene.Name + "\n" +
                     lcNewScene.SceneStoryDescription + "\n" +
                     _NotScanned;
        }
        else
        {
            //Update player saying they have visited this area before
            PlayerManagerScript._CurrentPlayer.Visted = PlayerManagerScript._CurrentPlayer.Visted + "," + lcNewScene.Name;
            //Update player with more score
            PlayerManagerScript._CurrentPlayer.PlayerScore += 10;
            //Save updated player into database
            _db.Connection.InsertOrReplace(PlayerManagerScript._CurrentPlayer);
            //Set this as text output
            prCommand._Result = lcNewScene.Name + "\n" + lcNewScene.SceneStoryDescription;
        }
    }
    //Check to see if user has visited this scene before
    private static bool Visited(SceneScript prScene)
    {
        //Create an array from visited list
        string[] lcArrayVisted = PlayerManagerScript._CurrentPlayer.Visted.Split(',');
        foreach (string SceneName in lcArrayVisted)
        {
            if (SceneName == prScene.Name)
            {
                return true;
            }
        }
        return false;
    }
    //Check to see if user has scanned this scene before
    private bool Scanned(SceneScript prScene)
    {
        //Create Array from scanned list
        string[] lcArrayScanned = PlayerManagerScript._CurrentPlayer.Scanned.Split(',');
        foreach (string SceneName in lcArrayScanned)
        {
            if (SceneName == prScene.Name)
            {
                return true;
            }
        }
        return false;
    }

    //Checks to see if scene has a disaply event to play
    public void HasEvent(string prEvent)
    {
        switch (prEvent)
        {
            case "Planet":
                GameManagerScript._Instance._StarAnim.SetTrigger("Warp");
                GameManagerScript._Instance._PlanetAnim.SetBool("ShipArrived", true);
                GameManagerScript._Instance._StarAnim.SetBool("Stop", true);
                break;
            case "Shop":
                GameManagerScript._Instance._StarAnim.SetTrigger("Warp");
                GameManagerScript._Instance._ShopAnim.SetBool("ShopArrived", true);
                GameManagerScript._Instance._StarAnim.SetBool("Stop", true);
                break;
            case "Enemy":
                GameManagerScript._Instance._StarAnim.SetTrigger("Warp");
                GameManagerScript._Instance._EnemyAnim.SetBool("EnemyArrived", true);
                GameManagerScript._Instance._StarAnim.SetBool("Stop", true);
                break;
            case "BrokenShip":
                GameManagerScript._Instance._StarAnim.SetTrigger("Warp");
                GameManagerScript._Instance._StarAnim.SetBool("Stop", true);
                break;
            default:
                GameManagerScript._Instance._StarAnim.SetTrigger("Warp");
                break;
        }
    }
    //turn off all display events
    public void TurnOffEvent()
    {
        GameManagerScript._Instance._StarAnim.SetBool("Stop", false);
        GameManagerScript._Instance._ShopAnim.SetBool("ShopArrived", false);
        GameManagerScript._Instance._PlanetAnim.SetBool("ShipArrived", false);
        GameManagerScript._Instance._EnemyAnim.SetBool("EnemyArrived", false);
    }
    //-------------------------------------- End Of Methods ----------------------------------------------------------------------------	
}
//------------------------------------------ End Of GoCommand : CommandScript Class -------------------------------------------------------------------------------





//--------------------------------------- Start Of PickCommand : CommandScript Class ------------------------------------------------------------------------------
public class PickCommand : CommandScript
{
    //--------------------------------------- Start Of Top Level Variable Decalaring ------------------------------------------------------------
    //User Input
    private string _item;
    //--------------------------------------- End Of Top Level Variable Declaring ---------------------------------------------------------


    //-------------------------------------- Start Of Methods ----------------------------------------------------------------------------	
    public PickCommand(string prItem)
    {
        //Assign users input to local variable
        _item = prItem;
    }

    public override void Do(CommandMapScript prCommand)
    {
        switch (_item)
        {
            case "up":
                SceneScript lcCurrentScene = PlayerManagerScript.GetCurrentScene();
                //if items havent been picked up
                if (!ItemsPickedUp(lcCurrentScene.Name))
                {
                    string lcInventoryList = null;
                    List<SceneItemScript> lcSceneItems = new List<SceneItemScript>();
                    //Gets all Scene Item that have the current Scene Name
                    lcSceneItems = _db.Connection.Table<SceneItemScript>().Where<SceneItemScript>(x => x.SceneName == lcCurrentScene.Name).ToList<SceneItemScript>();
                    foreach (SceneItemScript SceneItem in lcSceneItems)
                    {
                        //Get the Item with the same ID
                        GameItemScript Item = _db.Connection.Table<GameItemScript>().Where<GameItemScript>(x => x.ItemId == SceneItem.ItemId).ToList<GameItemScript>().First<GameItemScript>();
                        //Add Item name to local variable
                        lcInventoryList = lcInventoryList + Item.Name + ",";
                    }
                    //Add current scene name to collected list
                    PlayerManagerScript._CurrentPlayer.CollectList = PlayerManagerScript._CurrentPlayer.CollectList + lcCurrentScene + ",";
                    //Update players inventory with local variable
                    PlayerManagerScript._CurrentPlayer.InventoryList = PlayerManagerScript._CurrentPlayer.InventoryList + lcInventoryList;
                    //Save to database
                    _db.Connection.InsertOrReplace(PlayerManagerScript._CurrentPlayer);
                    //Text output
                    prCommand._Result = "AI: Items picked up";
                }
                else
                //Text Output
                { prCommand._Result = "AI: No Items to pick up"; }
                break;
        }
    }

    private bool ItemsPickedUp(string prScene)
    {
        bool lcResult = false;

        //needs Try/Catch because if user has null in collect list, it can not split
        try
        {
            //Put collected list into an array
            string[] lcCollectedArray = PlayerManagerScript._CurrentPlayer.CollectList.Split(',');
            foreach (string SceneName in lcCollectedArray)
            {
                if (SceneName == prScene)
                {
                    lcResult = true;
                }
            }
        }
        catch { }
        return lcResult;

    }
    //-------------------------------------- End Of Methods ----------------------------------------------------------------------------	
}
//------------------------------------------ End Of Command : CommandScript Class -------------------------------------------------------------------------------





//--------------------------------------- Start Of ShowCommand : CommandScript Class ------------------------------------------------------------------------------
public class ShowCommand : CommandScript
{
    //--------------------------------------- Start Of Top Level Variable Decalaring ------------------------------------------------------------
    //User Input
    private string _Display;
    //--------------------------------------- End Of Top Level Variable Declaring ---------------------------------------------------------


    //-------------------------------------- Start Of Methods ----------------------------------------------------------------------------	
    public ShowCommand(string prDisplay)
    {
        //Assigns user input to local variable
        _Display = prDisplay;
    }

    public override void Do(CommandMapScript prCommand)
    {
        //Default output
        string lcResult = "What are you wanting to show?";
        //Get current Scene
        SceneScript lcScene = PlayerManagerScript.GetCurrentScene();
        //Help Output
        string lcHelp = "Go: \n" +
                        "   Up - Down - Left - Right \n" +
                        "Show: \n" +
                        "   Map - Inventory - Terminal - Help\n" +
                        "Scan: \n" +
                        "   Area \n" +
                        "Pick: \n" +
                        "   Up \n" +
                        "Sell: \n" +
                        "   Items";

        switch (_Display)
        {

            case "map":
                lcResult = "";
                GameManagerScript._Instance.SetActiveCanvas("Map Canvas");
                break;
            case "inventory":
                GameManagerScript._Instance.SetActiveCanvas("Inventory Canvas");
                break;
            case "terminal":
                GameManagerScript._Instance.SetActiveCanvas("Main Canvas");
                lcResult = lcScene.SceneStoryDescription;
                break;
            case "help":
                lcResult = lcHelp;
                break;
        }

        prCommand._Result = lcResult;
    }
    //-------------------------------------- End Of Methods ----------------------------------------------------------------------------	
}
//------------------------------------------ End Of Command : CommandScript Class -------------------------------------------------------------------------------





//--------------------------------------- Start Of ScanCommand : CommandScript Class ------------------------------------------------------------------------------
public class ScanCommand : CommandScript
{
    //--------------------------------------- Start Of Top Level Variable Decalaring ------------------------------------------------------------
    //User Input
    private string _Scan;

    //--------------------------------------- End Of Top Level Variable Declaring ---------------------------------------------------------


    //-------------------------------------- Start Of Methods ----------------------------------------------------------------------------	
    public ScanCommand(string prScan)
    {
        //Assign User input to local variable
        _Scan = prScan;
    }

    public override void Do(CommandMapScript prCommand)
    {
        //Default Result
        string lcResult = "AI: Sorry my error, could you type that again?";

        switch (_Scan)
        {
            case "area":
                //Get players current scene
                SceneScript lcCurrentScene = PlayerManagerScript.GetCurrentScene();
                //Add to users score
                PlayerManagerScript._CurrentPlayer.PlayerScore += 25;
                //Add scene name to scanned list
                PlayerManagerScript._CurrentPlayer.Scanned += lcCurrentScene + ",";
                //update database
                _db.Connection.InsertOrReplace(PlayerManagerScript._CurrentPlayer);
                //Output
                lcResult = "AI: Scanning Area \n" +
                            lcCurrentScene.Scan +
                            "\n" +
                            "AI: Items found \n" +
                            ItemstoString(lcCurrentScene.Name);
                break;
                //-------------------------------------- End Of Methods ----------------------------------------------------------------------------	
        }
        prCommand._Result = lcResult;
    }

    //Convert items to a string
    private string ItemstoString(string prCurrentSceneName)
    {
        //if not already collected
        if (!AlreadyCollected(prCurrentSceneName))
        {
            string ItemList = null;
            List<SceneItemScript> lcSceneItems = new List<SceneItemScript>();
            //Get scene Items where scene name is the current scene name
            lcSceneItems = _db.Connection.Table<SceneItemScript>().Where<SceneItemScript>(x => x.SceneName == prCurrentSceneName).ToList<SceneItemScript>();
            foreach (SceneItemScript SceneItem in lcSceneItems)
            {
                //Get Item where ID is the current Scene Item ID
                GameItemScript Item = _db.Connection.Table<GameItemScript>().Where<GameItemScript>(x => x.ItemId == SceneItem.ItemId).ToList<GameItemScript>().First<GameItemScript>();
                //Add Item Name to list
                ItemList = ItemList + Item.Name + "\n";
            }
            return ItemList;
        }
        else { return "No Items Found"; }
    }

    //Check to see if items have already been collected
    private static bool AlreadyCollected(string prCurrentSceneName)
    {
        //Default Result
        bool lcResult = false;
        //Needs Try/Catch as if collect list is null, it will fail
        try
        {
            //Create array of scene names from collected list
            string[] lcCollected = PlayerManagerScript._CurrentPlayer.CollectList.Split(',');
            foreach (string SceneName in lcCollected)
            {
                if (SceneName == prCurrentSceneName)
                {
                    lcResult = true;
                }
            }
        }
        catch { }

        return lcResult;
    }
    //------------------------------------ End of Methods ------------------------------------------------------------------------------
}
//------------------------------------------ End Of ScanCommand : CommandScript Class-------------------------------------------------------------------------------




//--------------------------------------- Start Of SellCommandScript : CommandScript Class ------------------------------------------------------------------------------
public class SellCommand : CommandScript
{
    //--------------------------------------- Start Of Top Level Variable Decalaring ------------------------------------------------------------
    //User input
    private string _Item;
    //--------------------------------------- End Of Top Level Variable Declaring ---------------------------------------------------------
    //-------------------------------------- Start Of Methods ----------------------------------------------------------------------------	
    public SellCommand(string prItem)
    {
        //Assign user input to local variable
        _Item = prItem;
    }

    public override void Do(CommandMapScript prCommand)
    {
        //Default output
        string lcResult = "AI: No Items to Sell";

        //Check to see if user is in correct location to sell items
        if (PlayerManagerScript.GetCurrentScene().Name == "C4")
        {
            //Create Array of players inventory items
            string[] lcInventory = PlayerManagerScript._CurrentPlayer.InventoryList.Split(',');

            foreach (string ItemName in lcInventory)
            //Side effect of .Split, Creates one extra in Array
            {
                if (ItemName != "")
                {
                    //Get item with same name
                    GameItemScript lcItem = _db.Connection.Table<GameItemScript>().Where(x => x.Name == ItemName).First<GameItemScript>();
                    //Add item price to players money
                    PlayerManagerScript._CurrentPlayer.PlayerMoney += lcItem.Price;
                    //Add item score to players score
                    PlayerManagerScript._CurrentPlayer.PlayerScore += lcItem.Score;
                }
            }
            //Empty Inventory
            PlayerManagerScript._CurrentPlayer.InventoryList = "";
            //Update data base
            _db.Connection.InsertOrReplace(PlayerManagerScript._CurrentPlayer);
            //New Output
            lcResult = "AI: All items sold";
        }
        else
        {
            //New Output
            lcResult = "AI: Not at the shop, Go to C4";
        }

        prCommand._Result = lcResult;
    }
    //------------------------------------ End of Methods ------------------------------------------------------------------------------
}
//------------------------------------------ End Of SellCommand : CommandScript Class-------------------------------------------------------------------------------
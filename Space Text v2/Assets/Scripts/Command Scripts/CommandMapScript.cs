using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Script Purpose:
//Creates a map of possible commands that the user can enter then directs user to that command.

//--------------------------------------- Start Of CommandMapScript : Mono Class ------------------------------------------------------------------------------
public class CommandMapScript : MonoBehaviour
{
    //--------------------------------------- Start Of Top Level Variable Decalaring ------------------------------------------------
    // Variable being sent back to CommandProccessor for text output
    public string _Result = "";
    //Storing all directions of possible commands
    private Dictionary<string, CommandScript> _commandsDic;
    //--------------------------------------- End Of Top Level Variable Declaring ---------------------------------------------------------


    //-------------------------------------- Start Of Methods ----------------------------------------------------------------------------	
    public CommandMapScript()
    {
        _commandsDic = new Dictionary<string, CommandScript>();
        //Adds successful possible commands
        _commandsDic.Add("go up", new GoCommand("up"));
        _commandsDic.Add("go down", new GoCommand("down"));
        _commandsDic.Add("go left", new GoCommand("left"));
        _commandsDic.Add("go right", new GoCommand("right"));
        _commandsDic.Add("show map", new ShowCommand("map"));
        _commandsDic.Add("show terminal", new ShowCommand("terminal"));
        _commandsDic.Add("show inventory", new ShowCommand("inventory"));
        _commandsDic.Add("show help", new ShowCommand("help"));
        _commandsDic.Add("pick up", new PickCommand("up"));
        _commandsDic.Add("scan area", new ScanCommand("area"));
        _commandsDic.Add("sell items", new SellCommand("Items"));
    }

    //Checks to see if user command in part of dictionary	
    public bool KnowsCommand(string prCommand)
    {
        bool lcResult = false;
        CommandScript lcCommand;
        // If command string value is part of dictonary, do that command
        if (_commandsDic.ContainsKey(prCommand))
        {
            //Select that command
            lcCommand = _commandsDic[prCommand];
            // Do that command
            lcCommand.Do(this);
            lcResult = true;
        }
        else
        {
            lcResult = false;
        }
        return lcResult;
    }
    //-------------------------------------- End Of Methods ----------------------------------------------------------------------------	
}
//------------------------------------------ End Of CommandMapScript : Mono Class-------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
//Script Purpose:
//Proccesses data within the Main Menu
//--------------------------------------- Start Of  MenuProccessorScript: Mono Class ------------------------------------------------------------------------------
public class MenuProccessorScript : MonoBehaviour
{
    //--------------------------------------- Start Of Top Level Variable Decalaring ------------------------------------------------------------
    public DataService _db = new DataService();
    public InputField _UserNameInput;
    public InputField _PassInput;
    public InputField _RePassInput;
    public Text _Output;
    //--------------------------------------- End Of Top Level Variable Declaring ---------------------------------------------------------

    public void SignUp()
    {
        //Gathers Information and sets locally
        string lcUserName = _UserNameInput.text;
        string lcPassword = _PassInput.text;
        string lcRePassword = _RePassInput.text;

        //if Password and re-enter match
        if (lcPassword == lcRePassword)
        {
            RegisterPlayer(lcUserName, lcPassword);
            //If Register worked, the user will be logged in
            if (PlayerManagerScript._LoggedIn)
            {
                GameManagerScript._Instance.SetActiveCanvas("Lobby Canvas");
            }
        }
        else
        {
            _Output.text = "Password's Don't Match";
        }
    }

    public void RegisterPlayer(string prUserName, string prPassword)
    {
        //default result
        PlayerManagerScript._LoggedIn = false;

        //if name isn't taken already
        if (!NameTaken(prUserName))
        {
            //Create new user
            PlayerScript lcNewPlayer = new PlayerScript
            {
                PlayerName = prUserName,
                Password = prPassword,
                PlayerHealth = 3,
                PlayerScore = 0,
                CurrentLocationID = "E3",
                Visted = "E3"
            };

            //Add user to database
            _db.Connection.Insert(lcNewPlayer);

            //Assign to static script
            PlayerManagerScript._CurrentPlayer = lcNewPlayer;
            PlayerManagerScript._LoggedIn = true;
        }
        else
        {
            _Output.text = "Username Taken";
        }

    }

    public bool NameTaken(string prName)
    {
        //checks to see if there is a playername already entered in database
        return (_db.Connection.Table<PlayerScript>().Where<PlayerScript>(x => x.PlayerName == prName).ToList<PlayerScript>().Count > 0);
    }

    public void LogIn()
    {
        //Default result
        PlayerManagerScript._LoggedIn = false;
        //Gather Information and sets locally
        string lcUsername = _UserNameInput.text;
        string lcPassword = _PassInput.text;

        //Create a list of players who have the input name and password
        List<PlayerScript> lcPlayers = _db.Connection.Table<PlayerScript>().Where<PlayerScript>(x => x.PlayerName == lcUsername && x.Password == lcPassword).ToList<PlayerScript>();

        //if the list is more than 0, log the user in
        if (lcPlayers.Count > 0)
        {
            PlayerManagerScript._LoggedIn = true;
            PlayerManagerScript._CurrentPlayer = lcPlayers.First<PlayerScript>();
            GameManagerScript._Instance.SetActiveCanvas("Lobby Canvas");
        }
        else
        {
            _Output.text = "Incorrect Details";
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using DoorScript;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorTrigger : MonoBehaviour
{
    public Door linkedDoor;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && linkedDoor.open)
        {
            
            if (SceneManager.GetActiveScene().name == "End")
            {
                Game_Manager.instance._interstitialController.OnRoomPassed(() => {
                    Game_Manager.instance.SetLastScene("Cycle_Room");
                    SceneManager.LoadScene("Cycle_Room");
                    return;
                });
                Game_Manager.instance.SetLastScene("Cycle_Room");
                SceneManager.LoadScene("Cycle_Room");
                return;
            }
            
            if (SceneManager.GetActiveScene().name == "Black")
            {
                Game_Manager.instance.PassBlackRoom();
                if (Game_Manager.instance.blackRoomsPassed >= 3)
                {
                    SceneManager.LoadScene("Black_End");
                    return;
                }
                Game_Manager.instance.OnDoorEntered2(linkedDoor.doorColor);
                return;
            }
            
            if (Game_Manager.instance.roomsPassed >= 10)
            {
                Game_Manager.instance._interstitialController.OnRoomPassed(() => {
                    // Code to load next level here
                    Game_Manager.instance.SetLastScene("End");
                    SceneManager.LoadScene("End");
                    return;
                });
                
                Game_Manager.instance.SetLastScene("End");
                SceneManager.LoadScene("End");
                return;
            }
            
            if (SceneManager.GetActiveScene().name == "Begin")
            {
                Game_Manager.instance.yellowRoomsPassed++;
                Game_Manager.instance.roomsPassed++;
                Game_Manager.instance._interstitialController.OnRoomPassed(() => {
                    Game_Manager.instance.OnDoorEntered2(linkedDoor.doorColor);
                    return;
                });

                Game_Manager.instance.OnDoorEntered2(linkedDoor.doorColor);
                return;
            }
            
            if (SceneManager.GetActiveScene().name == "Yellow")
            {
                Game_Manager.instance.roomsPassed++;
                Game_Manager.instance.yellowRoomsPassed++;
                Game_Manager.instance._interstitialController.OnRoomPassed(() => {
                    Game_Manager.instance.OnDoorEntered2(linkedDoor.doorColor);
                    return;
                });

                Game_Manager.instance.OnDoorEntered2(linkedDoor.doorColor);
                return;
            }
            
            if (SceneManager.GetActiveScene().name == "Red")
            {
                Game_Manager.instance.roomsPassed++;
                Game_Manager.instance.redRoomsPassed++;
                Game_Manager.instance._interstitialController.OnRoomPassed(() => {
                    Game_Manager.instance.OnDoorEntered2(linkedDoor.doorColor);
                    return;
                });

                Game_Manager.instance.OnDoorEntered2(linkedDoor.doorColor);
                return;
            }
            
            
            
            Game_Manager.instance.OnDoorEntered2(linkedDoor.doorColor);
        }
    }
}

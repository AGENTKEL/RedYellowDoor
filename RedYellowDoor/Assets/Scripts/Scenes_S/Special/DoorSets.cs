using System.Collections;
using System.Collections.Generic;
using DoorScript;
using UnityEngine;

public class DoorSets : MonoBehaviour
{
[System.Serializable]
    public class DoorInfo
    {
        [HideInInspector] public DoorColor doorColor; // Assigned at runtime
        public Transform doorTransform;
    }

    [Header("Assign Door Prefabs")]
    public GameObject redDoorPrefab;
    public GameObject yellowDoorPrefab;
    public GameObject blackDoorPrefab;

    [Header("Assign Door Spawn Points")]
    public List<DoorInfo> doors = new List<DoorInfo>();

    void Start()
    {
        StartCoroutine(SpawnDoorsCE());
    }

    void SpawnDoors()
    {
        string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        bool isBlackRoom = currentScene == Game_Manager.instance.blackRoomScene;

        DoorColor expectedColor = isBlackRoom
            ? Game_Manager.instance.GetNextColorAfterBlackRoom() // After black room, follow main sequence
            : Game_Manager.instance.GetNextExpectedRoomColor();
        

        int doorCount = doors.Count;
        int colorAssigned = 0;

        for (int i = 0; i < doorCount; i++)
        {
            GameObject prefabToSpawn = null;
            DoorColor colorToAssign = DoorColor.Yellow;
            
            if (expectedColor == DoorColor.Red && colorAssigned == 0)
            {
                prefabToSpawn = redDoorPrefab;
                colorToAssign = DoorColor.Red;
                colorAssigned++;
            }
            else if (expectedColor == DoorColor.Yellow && colorAssigned == 0)
            {
                prefabToSpawn = yellowDoorPrefab;
                colorToAssign = DoorColor.Yellow;
                colorAssigned++;
            }
            else
            {
                // Optional: Skip spawning or handle extra doors here if needed
                continue;
            }

            // Assign to door info and spawn
            doors[i].doorColor = colorToAssign;
            Transform spawnPoint = doors[i].doorTransform;

            if (spawnPoint != null && prefabToSpawn != null)
            {
                GameObject doorObj = Instantiate(prefabToSpawn, spawnPoint.position, spawnPoint.rotation);

                // Assign door color to door script
                Door doorScript = doorObj.GetComponent<Door>();
                if (doorScript != null)
                    doorScript.doorColor = colorToAssign;
            }
        }
    }

    private IEnumerator SpawnDoorsCE()
    {
        yield return new WaitForSeconds(0.15f);
        SpawnDoors();
    }
    
    
}

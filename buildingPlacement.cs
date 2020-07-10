// WARNING, THIS CODE WAS MADE WHEN I WAS STILL GETTING INTO CODING.
// I have uploaded this file to show the progress of where I've come from.
// This code was made somewhere between 2016 - 2017
// Sadly there aren't much comments in this code, but as far as describing the code, the functionality seems pretty clear

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buildingPlacement : MonoBehaviour {
    public SettingsConfig Settings;
    [Header("General Items")]
    public DisplayBuildingStats buildingStats;
    [Space(10)]

    [Header("Ghost Buildings Array")]
    public GameObject[] GhostBuildings = new GameObject[6];
    [Space(10)]

    [Header("All Buildings")]
    public Building Mine_Building;
    public Building Gold_Building;
    public Building Wood_Building;
    public Building Barrack_Building;
    public Building Farm_Building;
    public Building guard_Tower;
    public Building Wall;
    public Building Gated_Wall;
    public Building WallCorner;
    [Space(10)]
    Vector3 defaultRotationLevel = new Vector3(0, 0, 0);
    GameObject[] layerChangeObject;
    Building Building;
    Vector3 newPosition;
    Vector3 newGhostBuildingPosition;
    public bool placement;
    public static bool lastplacementcheck;
    int buildingWoodCost;
    int buildingStoneCost;

    void Start()
    {
        layerChangeObject = GameObject.FindGameObjectsWithTag("Environment");
        newPosition = transform.position;
        placement = false;
        lastplacementcheck = false;
        resetGhostBuilding();
    }
    // every building in the game
    public void mineBuilding()
    {
        Building = Mine_Building;
        buildingWoodCost = Mine_Building.woodPrice;
        buildingStoneCost = Mine_Building.stonePrice;
        prepareBuildingPlacement();
    }
        public void woodBuilding()
    {
        Building = Wood_Building;
        buildingWoodCost = Wood_Building.woodPrice;
        buildingStoneCost = Wood_Building.stonePrice;
        prepareBuildingPlacement();
    }
    public void goldBuilding()
    {
        Building = Gold_Building;
        buildingWoodCost = Gold_Building.woodPrice;
        buildingStoneCost = Gold_Building.stonePrice;
        prepareBuildingPlacement();
    }
    public void farmBuilding()
    {
        Building = Farm_Building;
        buildingWoodCost = Farm_Building.woodPrice;
        buildingStoneCost = Farm_Building.stonePrice;
        prepareBuildingPlacement();
    }
    public void soldierBarrackBuilding()
    {
        Building = Barrack_Building;
        buildingWoodCost = Barrack_Building.woodPrice;
        buildingStoneCost = Barrack_Building.stonePrice;
        prepareBuildingPlacement();
    }
    public void guardTower()
    {
        Building = guard_Tower;
        buildingWoodCost = guard_Tower.woodPrice;
        buildingStoneCost = guard_Tower.stonePrice;
        prepareBuildingPlacement();
    }

    public void WallBuilding()
    {
        Building = Wall;
        buildingWoodCost = Wall.woodPrice;
        buildingStoneCost = Wall.stonePrice;
        prepareBuildingPlacement();
    }
    public void WallCornerBuilding()
    {
        Building = WallCorner;
        buildingWoodCost = WallCorner.woodPrice;
        buildingStoneCost = WallCorner.stonePrice;
        prepareBuildingPlacement();
    }
    public void GatedWallBuilding()
    {
        Building = Gated_Wall;
        buildingWoodCost = Gated_Wall.woodPrice;
        buildingStoneCost = Gated_Wall.stonePrice;
        prepareBuildingPlacement();
    }

    void Update()
    {
        if (placement == true) {
            RaycastHit hit2;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit2))
            {
            // finds raycasted position on map and makes it into a rounded number to create a grid
                newPosition = hit2.point;
                newPosition.y = newPosition.y - 0.03f;
                newPosition.x = Mathf.Round(newPosition.x / 1) * 1;
                newPosition.z = Mathf.Round(newPosition.z / 1) * 1;
                transform.position = newPosition;
                newGhostBuildingPosition = newPosition;
              //  newGhostBuildingPosition.y = -0.88f;
                //   newGhostBuildingPosition.y = newGhostBuildingPosition.y + 1f;

                if (Input.GetKeyDown(Settings.Rotate))
                {
                    defaultRotationLevel.y = defaultRotationLevel.y + 90f;
                }
                if (hit2.transform.tag == "World")
                {
                   // maak hier ff een switch case, dat is wat overzichtelijker
                   // ghost building is een gebouw dat overlayd word op de plaats waar hij mogenlijk wordt geplaatst
                    if (Building == Mine_Building)
                    {
                        GhostBuildings[0].transform.position = newGhostBuildingPosition;
                        GhostBuildings[0].transform.rotation = Quaternion.Euler(defaultRotationLevel);
                    }
                    else if (Building == Wood_Building)
                    {
                        GhostBuildings[1].transform.position = newGhostBuildingPosition;
                        GhostBuildings[1].transform.rotation = Quaternion.Euler(defaultRotationLevel);
                    }
                    else if (Building == Gold_Building)
                    {
                        GhostBuildings[2].transform.position = newGhostBuildingPosition;
                        GhostBuildings[2].transform.rotation = Quaternion.Euler(defaultRotationLevel);
                    }
                    else if (Building == Farm_Building)
                    {
                        GhostBuildings[3].transform.position = newGhostBuildingPosition;
                        GhostBuildings[3].transform.rotation = Quaternion.Euler(defaultRotationLevel);
                    }
                    else if (Building == Barrack_Building)
                    {
                        GhostBuildings[4].transform.position = newGhostBuildingPosition;
                        GhostBuildings[4].transform.rotation = Quaternion.Euler(defaultRotationLevel);
                    }
                    else if (Building == guard_Tower)
                    {
                        GhostBuildings[5].transform.position = newGhostBuildingPosition;
                        GhostBuildings[5].transform.rotation = Quaternion.Euler(defaultRotationLevel);
                    }
                    else if (Building == Wall)
                    {
                        GhostBuildings[6].transform.position = newGhostBuildingPosition;
                        GhostBuildings[6].transform.rotation = Quaternion.Euler(defaultRotationLevel);
                    }
                    else if (Building == Gated_Wall)
                    {
                        GhostBuildings[7].transform.position = newGhostBuildingPosition;
                        GhostBuildings[7].transform.rotation = Quaternion.Euler(defaultRotationLevel);
                    }
                    else if (Building == WallCorner)
                    {
                        GhostBuildings[8].transform.position = newGhostBuildingPosition;
                        GhostBuildings[8].transform.rotation = Quaternion.Euler(defaultRotationLevel);
                    }
                    // places building
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (lastplacementcheck)
                        {
                            if (resourcesGathering.wood >= buildingWoodCost && resourcesGathering.stone >= buildingStoneCost)
                            {
                                // hier kan je ook een mooie switch case maken
                                if(Building == Mine_Building)
                                {
                                    resourcesGathering.aantalStoneMines += 1;
                                }
                                else if(Building == Wood_Building)
                                {
                                    resourcesGathering.aantalWoodCutters += 1;
                                }
                                else if (Building == Gold_Building)
                                {
                                    resourcesGathering.aantalGoldMines += 1;
                                }
                                else if (Building == Farm_Building)
                                {
                                    resourcesGathering.aantalFarms += 1;
                                }
                                // plaats geluid
                                AudioManager.Instance.PlayAudioNonEnv(AudioManager.Instance.PlaceBuilding);
                                // instantieerd, achteraf met mijn huidige kennis kan ik dit object poolen
                                Instantiate(Building.thisBuilding, new Vector3(newPosition.x, newPosition.y, newPosition.z), Quaternion.Euler(defaultRotationLevel));
                                resourcesGathering.wood = resourcesGathering.wood - buildingWoodCost;
                                resourcesGathering.stone = resourcesGathering.stone - buildingStoneCost;
                                if (!Input.GetKey(Settings.Continuous))
                                {
                                    placement = false;
                                    resetGhostBuilding();
                                    foreach (GameObject Environment in layerChangeObject)
                                    {
                                        Environment.layer = 0;
                                    }
                                }
                            }
                            else
                            {
                                placement = false;
                                resetGhostBuilding();
                                DisplayBuildingStats.EnableStats = true;
                                AlertManager.Instance.BuildingAlert();
                                foreach (GameObject Environment in layerChangeObject)
                                {
                                    Environment.layer = 0;
                                }
                            }
                        }
                    }
                }
            }
        }

        if (Input.GetKeyDown("escape"))
        {
            placement = false;
            resetGhostBuilding();
            DisplayBuildingStats.EnableStats = true;
            foreach (GameObject Environment in layerChangeObject)
            {
                Environment.layer = 0;
            }
        }
    }

    void prepareBuildingPlacement()
    {
        placement = true;
        lastplacementcheck = true;
        resetGhostBuilding();
        UICameraOperatorHelper.enablezoom = true;
        DisplayBuildingStats.EnableStats = false;
        foreach (GameObject Environment in layerChangeObject)
        {
            Environment.layer = 2;
        }
    }

    void resetGhostBuilding()
    {
        // maak hier ff een for loopje of een foreach

        defaultRotationLevel = new Vector3(0, 0, 0);
        for(int i = 0; i < GhostBuildings.Length; i++)
        {
            GhostBuildings[i].transform.position = new Vector3(-255f, -20f, -255f);
            GhostBuildings[i].transform.rotation = Quaternion.Euler(defaultRotationLevel);
        }
    }
 }

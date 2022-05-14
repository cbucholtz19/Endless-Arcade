using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoomType
{
  Crossing, // 0 walls
  Junction, // 1 wall
  Corner, // 2 adjacent walls
  Hallway, // 2 opposite walls
  Deadend // 3 walls
}

enum Direction
{
  North,
  West,
  East,
  South
}

public class Room
{
  public bool visited;
  public bool containsItem;
  public Wall[] walls;
  public RoomType type;

  public Room() {
    this.visited = false;
    this.containsItem = false;
    this.walls = new Wall[4];
    for (int i=0; i<4; i++) {
      this.walls[i] = new Wall();
    }
  }
}

public class Wall
{
  public bool exists;
  public bool generated;

  public Wall() {
    this.exists = true;
    this.generated = false;
  }
}

public class GenerateLevel : MonoBehaviour
{
	
  public GameObject wallPrefab;
  public GameObject floorPrefab;
  public GameObject floorthPrefab;
  public GameObject floorbeamPrefab;
  public GameObject arcadeMachine1;
  public GameObject clockPrefab;
  public GameObject player;

  private bool all_visited(Room[,] maze, int mazeSize)
  {
    bool av = true;
    for(int i=0; i<mazeSize; i++) {
      for (int j=0; j<mazeSize; j++) {
        if (!maze[i,j].visited) {
          av = false;
        }
      }
    }
    return av;
  }

  // Start is called before the first frame update
  void Start()
  {

    // Parameters
    int mazeSize = 7 + (GameController.level);
    int clocksToPlace = 4 + (2*GameController.level);
    float gapChance = (float) (1.0f/((-0.5f*System.Math.Pow(System.Math.E,-0.4f*GameController.level))+0.5f));

    // Constants
    int arcadesToPlace = 3;
    const int NUM_WALLS = 4;

    Room[,] maze = new Room[mazeSize,mazeSize];
    for(int i=0; i<mazeSize; i++) {
      for (int j=0; j<mazeSize; j++) {
        maze[i,j] = new Room();
      }
    } 

    // Pick random starting room and mark as visited
    int currentX = Random.Range(0, mazeSize);
    int currentZ = Random.Range(0, mazeSize);
    maze[currentX,currentZ].visited = true;

    // While there are unvisited cells
    while (!all_visited(maze, mazeSize)) {
      
      // Find a valid random neighbor
      bool validStep = false;
      int nextStep = -1;
      int neighborX = -1;
      int neighborZ = -1;

      while (!validStep) {
        nextStep = Random.Range(0, NUM_WALLS);
        if ((Direction)nextStep == Direction.North) { // TODO switch case
          if (currentZ < mazeSize-1) {
            validStep = true;
            neighborX = currentX;
            neighborZ = currentZ + 1;
          }
        }
        else if ((Direction)nextStep == Direction.South) {
          if (currentZ > 0) {
            validStep = true;
            neighborX = currentX;
            neighborZ = currentZ - 1;
          }
        }
        else if ((Direction)nextStep == Direction.West) {
          if (currentX > 0) {
            validStep = true;
            neighborX = currentX - 1;
            neighborZ = currentZ;
          }
        }
        else if ((Direction)nextStep == Direction.East) {
          if (currentX < mazeSize-1) {
            validStep = true;
            neighborX = currentX + 1;
            neighborZ = currentZ;
          }
        }
      }

      // If neighbor hasn't been visited
      if (!maze[neighborX,neighborZ].visited) {
        // Remove wall between current and neighbor and mark as visited
        maze[currentX,currentZ].walls[nextStep].exists = false;
        maze[neighborX,neighborZ].walls[3-nextStep].exists = false;
        maze[neighborX,neighborZ].visited = true;
      }

      // Make neighbor cell the current
      currentX = neighborX;
      currentZ = neighborZ;
    }

    // Determine room types
    int numDeadends = 0;
    for(int i=0; i<mazeSize; i++) {
      for (int j=0; j<mazeSize; j++) {
        int numWalls = 0;
        for (int k=0; k<NUM_WALLS; k++) {
          if (maze[i,j].walls[k].exists) {
            numWalls++;
          }
        }
        switch(numWalls)
        {
          case 0:
            maze[i,j].type = RoomType.Crossing;
            break;
          case 1:
            maze[i,j].type = RoomType.Junction;
            break;
          case 2:
            if (maze[i,j].walls[(int)Direction.North].exists && maze[i,j].walls[(int)Direction.South].exists
            || maze[i,j].walls[(int)Direction.West].exists && maze[i,j].walls[(int)Direction.East].exists) {
              maze[i,j].type = RoomType.Hallway;
            }
            else {
              maze[i,j].type = RoomType.Corner;
            }
            break;
          case 3:
            maze[i,j].type = RoomType.Deadend;
            numDeadends++;
            break;
        }
        
      }
    }

    // Check for enough deadends (EXTREMELY UNLIKELY, otherwise I would redesign)
    if (numDeadends < 4) {
      Debug.Log("Error in maze generation algorithm.");
      GameController.gameover(GameoverCondition.None);
    }

    // Instantiate walls
    float wallLength = wallPrefab.transform.localScale.x;
    float wallHeight = wallPrefab.transform.localScale.y;
    for(int i=0; i<mazeSize; i++) {
      for (int j=0; j<mazeSize; j++) {
        for (int k=0; k<NUM_WALLS; k++) {
          if (maze[i,j].walls[k].exists && !maze[i,j].walls[k].generated) { // If wall exists and not already generated
            if ((Direction)k==Direction.North) {
              Instantiate(wallPrefab, new Vector3(i*wallLength+0.5f*wallLength,0.5f*wallHeight,(j+1)*wallLength), Quaternion.identity);
              if (j<mazeSize-1) {
                maze[i,j+1].walls[3-k].generated = true;
              }
            }
            else if ((Direction)k==Direction.South) {
              Instantiate(wallPrefab, new Vector3(i*wallLength+0.5f*wallLength,0.5f*wallHeight,j*wallLength), Quaternion.identity);
              if (j>0) {
                maze[i,j-1].walls[3-k].generated = true;
              }
            }
            else if ((Direction)k==Direction.West) {
              Instantiate(wallPrefab, new Vector3(i*wallLength,0.5f*wallHeight,j*wallLength+0.5f*wallLength), Quaternion.Euler(0,90,0));
              if (i>0) {
                maze[i-1,j].walls[3-k].generated = true;
              }
            }
            else if ((Direction)k==Direction.East) {
              Instantiate(wallPrefab, new Vector3((i+1)*wallLength,0.5f*wallHeight,j*wallLength+0.5f*wallLength), Quaternion.Euler(0,90,0));
              if (i<mazeSize-1) {
                maze[i+1,j].walls[3-k].generated = true;
              }
            }
          }
        }
      }
    }

    // Place arcades
    while (arcadesToPlace > 0) {
      int i = Random.Range(0, mazeSize);
      int j = Random.Range(0, mazeSize);
      if (maze[i,j].type == RoomType.Deadend && !maze[i,j].containsItem) {
        arcadeMachine1.transform.localPosition = new Vector3(i*wallLength+0.5f*wallLength,0,j*wallLength+0.5f*wallLength);
        /*
        if (!maze[i,j].walls[(int)Direction.North].exists) {
          arcadeMachine1.transform.localPosition = new Vector3(i*wallLength+0.5f*wallLength,0,j*wallLength+0.1f*wallLength);
          arcadeMachine1.transform.localRotation = Quaternion.Euler(0,0,0);
        }
        else if (!maze[i,j].walls[(int)Direction.South].exists) {
          arcadeMachine1.transform.localPosition = new Vector3(i*wallLength+0.5f*wallLength,0,j*wallLength+0.9f*wallLength);
          arcadeMachine1.transform.localRotation = Quaternion.Euler(0,180,0);
        }
        else if (!maze[i,j].walls[(int)Direction.West].exists) {
          arcadeMachine1.transform.localPosition = new Vector3(i*wallLength+0.9f*wallLength,0,j*wallLength+0.5f*wallLength);
          arcadeMachine1.transform.localRotation = Quaternion.Euler(0,270,0);
        }
        else if (!maze[i,j].walls[(int)Direction.East].exists) {
          arcadeMachine1.transform.localPosition = new Vector3(i*wallLength+0.1f*wallLength,0,j*wallLength+0.5f*wallLength);
          arcadeMachine1.transform.localRotation = Quaternion.Euler(0,90,0);
        }
        */
        maze[i,j].containsItem = true;
        arcadesToPlace--;
      }
    }

    // Place clocks
    while (clocksToPlace > 0) {
      int i = Random.Range(0, mazeSize);
      int j = Random.Range(0, mazeSize);
      if (maze[i,j].type != RoomType.Deadend && !maze[i,j].containsItem) {
        Instantiate(clockPrefab, new Vector3(i*wallLength+0.5f*wallLength,1,j*wallLength+0.5f*wallLength), Quaternion.identity);
        maze[i,j].containsItem = true;
        clocksToPlace--;
      }
    }

    // Place player
    bool playerPlaced = false;
    while (!playerPlaced) {
      int i = Random.Range(0, mazeSize);
      int j = Random.Range(0, mazeSize);
      if (maze[i,j].type == RoomType.Deadend && !maze[i,j].containsItem) {
        player.transform.position = new Vector3(i*wallLength+0.5f*wallLength,2,j*wallLength+0.5f*wallLength);
        maze[i,j].containsItem = true;
        playerPlaced = true;
      }
    }

    // Place floors and gaps
    for(int i=0; i<mazeSize; i++) {
      for (int j=0; j<mazeSize; j++) {

        // Chance of floor gaps increases as level increases
        bool hasGap = (Random.Range(0f,gapChance) < 1f);

        if (!maze[i,j].containsItem && !(maze[i,j].type==RoomType.Deadend) && hasGap) { // If nothing in room and not deadend
          if (maze[i,j].type == RoomType.Crossing) {
            int gapType = Random.Range(0,3);
            if (gapType == 0) { // Single center platform
              Instantiate(floorthPrefab, new Vector3(i*wallLength+0.5f*wallLength, 0, j*wallLength+0.5f*wallLength), Quaternion.identity);
            }
            else if (gapType == 1) { // Cross platforms
              Instantiate(floorbeamPrefab, new Vector3(i*wallLength+0.5f*wallLength, 0, j*wallLength+0.5f*wallLength), Quaternion.identity);
              Instantiate(floorbeamPrefab, new Vector3(i*wallLength+0.5f*wallLength, 0.01f, j*wallLength+0.5f*wallLength), Quaternion.Euler(0,90,0));
            }
            else if (gapType == 2) { // Single center hole
              Instantiate(floorbeamPrefab, new Vector3(i*wallLength+0.5f*wallLength, 0, j*wallLength+0.125f*wallLength), Quaternion.identity);
              Instantiate(floorbeamPrefab, new Vector3(i*wallLength+0.5f*wallLength, 0, j*wallLength+0.875f*wallLength), Quaternion.identity);
              Instantiate(floorbeamPrefab, new Vector3(i*wallLength+0.125f*wallLength, 0.01f, j*wallLength+0.5f*wallLength), Quaternion.Euler(0,90,0));
              Instantiate(floorbeamPrefab, new Vector3(i*wallLength+0.875f*wallLength, 0.01f, j*wallLength+0.5f*wallLength), Quaternion.Euler(0,90,0));
            }
          }
          else if (maze[i,j].type == RoomType.Hallway) {
            int gapType = Random.Range(0,2);
            if (maze[i,j].walls[(int)Direction.North].exists) { // East-west hall
              if (gapType == 0) {
                Instantiate(floorthPrefab, new Vector3(i*wallLength+0.5f*wallLength, 0, j*wallLength+0.25f*wallLength), Quaternion.identity);
                Instantiate(floorthPrefab, new Vector3(i*wallLength+0.5f*wallLength, 0, j*wallLength+0.75f*wallLength), Quaternion.Euler(0,180,0));
              }
              else if (gapType == 1) {
                Instantiate(floorbeamPrefab, new Vector3(i*wallLength+0.5f*wallLength, 0, j*wallLength+0.125f*wallLength), Quaternion.identity);
                Instantiate(floorbeamPrefab, new Vector3(i*wallLength+0.5f*wallLength, 0, j*wallLength+0.875f*wallLength), Quaternion.identity);
              }
            }
            else { // North-south hall
              if (gapType == 0) {
                Instantiate(floorthPrefab, new Vector3(i*wallLength+0.25f*wallLength, 0, j*wallLength+0.5f*wallLength), Quaternion.identity);
                Instantiate(floorthPrefab, new Vector3(i*wallLength+0.75f*wallLength, 0, j*wallLength+0.5f*wallLength), Quaternion.Euler(0,180,0));
              }
              else if (gapType == 1) {
                Instantiate(floorbeamPrefab, new Vector3(i*wallLength+0.125f*wallLength, 0, j*wallLength+0.5f*wallLength), Quaternion.Euler(0,90,0));
                Instantiate(floorbeamPrefab, new Vector3(i*wallLength+0.875f*wallLength, 0, j*wallLength+0.5f*wallLength), Quaternion.Euler(0,90,0));
              }
            }
          }
          else if (maze[i,j].type == RoomType.Corner) {
            int gapType = Random.Range(0,2);
            if (maze[i,j].walls[(int)Direction.North].exists) {
              if (maze[i,j].walls[(int)Direction.West].exists) { // North-west corner
                if (gapType == 0) {
                  Instantiate(floorthPrefab, new Vector3(i*wallLength+0.75f*wallLength, 0, j*wallLength+0.25f*wallLength), Quaternion.identity);
                }
                else if (gapType == 1) {
                  Instantiate(floorbeamPrefab, new Vector3(i*wallLength+0.125f*wallLength, 0, j*wallLength+0.5f*wallLength), Quaternion.Euler(0,90,0));
                  Instantiate(floorbeamPrefab, new Vector3(i*wallLength+0.5f*wallLength, 0.01f, j*wallLength+0.875f*wallLength), Quaternion.identity);
                }
              }
              else { // North-east corner
                if (gapType == 0) {
                  Instantiate(floorthPrefab, new Vector3(i*wallLength+0.25f*wallLength, 0, j*wallLength+0.25f*wallLength), Quaternion.identity);
                }
                else if (gapType == 1) {
                  Instantiate(floorbeamPrefab, new Vector3(i*wallLength+0.875f*wallLength, 0, j*wallLength+0.5f*wallLength), Quaternion.Euler(0,90,0));
                  Instantiate(floorbeamPrefab, new Vector3(i*wallLength+0.5f*wallLength, 0.01f, j*wallLength+0.875f*wallLength), Quaternion.identity);
                }
              }
            }
            else {
              if (maze[i,j].walls[(int)Direction.West].exists) { // South-west corner
                if (gapType == 0) {
                  Instantiate(floorthPrefab, new Vector3(i*wallLength+0.75f*wallLength, 0, j*wallLength+0.75f*wallLength), Quaternion.identity);
                }
                else if (gapType == 1) {
                  Instantiate(floorbeamPrefab, new Vector3(i*wallLength+0.125f*wallLength, 0, j*wallLength+0.5f*wallLength), Quaternion.Euler(0,90,0));
                  Instantiate(floorbeamPrefab, new Vector3(i*wallLength+0.5f*wallLength, 0.01f, j*wallLength+0.125f*wallLength), Quaternion.identity);
                }
              }
              else { // South-east corner
                if (gapType == 0) {
                  Instantiate(floorthPrefab, new Vector3(i*wallLength+0.25f*wallLength, 0, j*wallLength+0.75f*wallLength), Quaternion.identity);
                }
                else if (gapType == 1) {
                  Instantiate(floorbeamPrefab, new Vector3(i*wallLength+0.875f*wallLength, 0, j*wallLength+0.5f*wallLength), Quaternion.Euler(0,90,0));
                  Instantiate(floorbeamPrefab, new Vector3(i*wallLength+0.5f*wallLength, 0.01f, j*wallLength+0.125f*wallLength), Quaternion.identity);
                }
              }
            }
          }
          else if (maze[i,j].type == RoomType.Junction) {
            int gapType = Random.Range(0,2);
            if (maze[i,j].walls[(int)Direction.North].exists) {
              if (gapType==0) {
                Instantiate(floorbeamPrefab, new Vector3(i*wallLength+0.5f*wallLength, 0, j*wallLength+0.875f*wallLength), Quaternion.identity);
                Instantiate(floorthPrefab, new Vector3(i*wallLength+0.5f*wallLength, 0, j*wallLength+0.5f*wallLength), Quaternion.identity);
              }
              else if (gapType==1) {
                Instantiate(floorbeamPrefab, new Vector3(i*wallLength+0.5f*wallLength, 0, j*wallLength+0.625f*wallLength), Quaternion.identity);
                Instantiate(floorbeamPrefab, new Vector3(i*wallLength+0.5f*wallLength, 0.01f, j*wallLength+0.5f*wallLength), Quaternion.Euler(0,90,0));
              }
            }
            else if (maze[i,j].walls[(int)Direction.South].exists) {
              if (gapType==0) {
                Instantiate(floorbeamPrefab, new Vector3(i*wallLength+0.5f*wallLength, 0, j*wallLength+0.125f*wallLength), Quaternion.identity);
                Instantiate(floorthPrefab, new Vector3(i*wallLength+0.5f*wallLength, 0, j*wallLength+0.5f*wallLength), Quaternion.identity);
              }
              else if (gapType==1) {
                Instantiate(floorbeamPrefab, new Vector3(i*wallLength+0.5f*wallLength, 0, j*wallLength+0.375f*wallLength), Quaternion.identity);
                Instantiate(floorbeamPrefab, new Vector3(i*wallLength+0.5f*wallLength, 0.01f, j*wallLength+0.5f*wallLength), Quaternion.Euler(0,90,0));
              }
            }
            else if (maze[i,j].walls[(int)Direction.West].exists) {
              if (gapType==0) {
                Instantiate(floorbeamPrefab, new Vector3(i*wallLength+0.125f*wallLength, 0, j*wallLength+0.5f*wallLength), Quaternion.Euler(0,90,0));
                Instantiate(floorthPrefab, new Vector3(i*wallLength+0.5f*wallLength, 0, j*wallLength+0.5f*wallLength), Quaternion.identity);
              }
              else if (gapType==1) {
                Instantiate(floorbeamPrefab, new Vector3(i*wallLength+0.5f*wallLength, 0.01f, j*wallLength+0.5f*wallLength), Quaternion.identity);
                Instantiate(floorbeamPrefab, new Vector3(i*wallLength+0.625f*wallLength, 0, j*wallLength+0.5f*wallLength), Quaternion.Euler(0,90,0));
              }
            }
            else if (maze[i,j].walls[(int)Direction.East].exists) {
              if (gapType==0) {
                Instantiate(floorbeamPrefab, new Vector3(i*wallLength+0.875f*wallLength, 0, j*wallLength+0.5f*wallLength), Quaternion.Euler(0,90,0));
                Instantiate(floorthPrefab, new Vector3(i*wallLength+0.5f*wallLength, 0, j*wallLength+0.5f*wallLength), Quaternion.identity);
              }
              else if (gapType==1) {
                Instantiate(floorbeamPrefab, new Vector3(i*wallLength+0.5f*wallLength, 0.01f, j*wallLength+0.5f*wallLength), Quaternion.identity);
                Instantiate(floorbeamPrefab, new Vector3(i*wallLength+0.375f*wallLength, 0, j*wallLength+0.5f*wallLength), Quaternion.Euler(0,90,0));
              }
            }
          }
        }
        else { // Default to full floor
          GameObject plane = Instantiate(floorPrefab, new Vector3(i*wallLength+0.5f*wallLength, 0, j*wallLength+0.5f*wallLength), Quaternion.identity);
          //plane.transform.localScale = new Vector3(1,0,1);
        }
      }
    }
  }
}
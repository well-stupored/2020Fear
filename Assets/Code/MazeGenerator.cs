using UnityEngine;
using System.Collections.Generic;

public class MazeGenerator : MonoBehaviour
{
    public GameObject[] WallPrefabs;

    public GameObject[] SpecailWalls;

	//These should never block the path!!!
	public GameObject[] Ob_sickles;
    
	public float ChanceForSpezcuialVValltz;

	public int AmountofAwsomenessOb_sickles;

    public Vector2 TopLeftOfOpenArea;
    public int OpenAreaDemention;

    public int MazeDimension;
    public float GrindSpacing;

    struct MazeSpot
    {
        public Transform Spot;
        public bool IsWall;
        public bool Touched;
    }

    List<Coord> Walls = new List<Coord>();
	List<Vector3> OpenSpaces = new List<Vector3>();

    struct Coord
    {
        public int x;
        public int y;
        public int pX;
        public int pY;

        public Coord(int inX, int inY, int inpX, int inpY)
        {
            x = inX;
            y = inY;
            pX = inpX;
            pY = inpY;
        }
    }

    MazeSpot[,] Maze;

	public Vector3 GetRandomOpenPosition()
	{
        if (OpenSpaces.Count == 0)
            return Vector3.zero;

		int temp = Random.Range (0, OpenSpaces.Count);

		return OpenSpaces [temp];
	}

    public Vector3 GetRandomOpenPositionAndRemove()
    {
        if (OpenSpaces.Count == 0)
            return Vector3.zero;

        int temp = Random.Range(0, OpenSpaces.Count);

        Vector3 temparoo = OpenSpaces[temp];

        OpenSpaces.RemoveAt(temp);

        return temparoo;
    }

    // Use this for initialization
    public void Build()
    {
        Maze = new MazeSpot[MazeDimension, MazeDimension];
        ConstructGrind();
        MakeMaze();
        PlaceWalls();
    }

    void ConstructGrind()
    {
        for (int i = 0; i < MazeDimension; i++)
        {
            for (int j = 0; j < MazeDimension; j++)
            {
                Maze[i, j].Spot = new GameObject("Wall Grid").transform;
                Maze[i, j].Spot.transform.parent = transform;
                Maze[i, j].Spot.position = new Vector3(transform.position.x + i * GrindSpacing, 0, transform.position.z + j * GrindSpacing);
                Maze[i, j].IsWall = true;
                Maze[i, j].Touched = false;
            }
        }
    }

    void PlaceWalls()
    {
        for (int i = 0; i < MazeDimension; i++)
        {
            for (int j = 0; j < MazeDimension; j++)
            {
                if(Maze[i,j].IsWall)
                {
                    // we get a random platonic (only acceptable if we aren't on an edge
                    var platonic = (Random.Range(0.0f, 1.0f) < ChanceForSpezcuialVValltz &&
                                    i != 0 && i != 1 && j != 0 && j != 1 && i != Maze.GetLength(0) - 1 && j != Maze.GetLength(1) - 1)
                                            ? SpecailWalls[Random.Range(0, SpecailWalls.Length)]
                                            : WallPrefabs [Random.Range(0, WallPrefabs.Length )];

                    var temp = Instantiate(platonic, Maze[i, j].Spot.position, Quaternion.identity) as GameObject;
                    temp.transform.parent = transform;
                }
				else
				{
					if(i >= (int)TopLeftOfOpenArea.x && i < TopLeftOfOpenArea.x + OpenAreaDemention &&
					   j >= (int)TopLeftOfOpenArea.y && j < TopLeftOfOpenArea.y + OpenAreaDemention)
						continue;

					OpenSpaces.Add(Maze[i,j].Spot.position);
				}
            }
        }

		for (int i = 0; i < AmountofAwsomenessOb_sickles; i ++)
		{
			if(Ob_sickles.Length == 0 || OpenSpaces.Count == 0)
				continue;

			int ChoooooosinWone = Random.Range(0, Ob_sickles.Length);

			GameObject temp;
			temp = Instantiate(Ob_sickles[ChoooooosinWone], GetRandomOpenPositionAndRemove(), Quaternion.identity) as GameObject;
			temp.transform.parent = transform;
		}

    }

    void MakeMaze()
    {
        //Prim's Algorithm
        //1. Start with a grid full of walls.
        //2. Pick a cell, mark it as part of the maze. Add the walls of the cell to the wall list.
        //3. While there are walls in the list:
        //1. Pick a random wall from the list. If the cell on the opposite side isn't in the maze yet:
        //1. Make the wall a passage and mark the cell on the opposite side as part of the maze.
        //2. Add the neighboring walls of the cell to the wall list.
        //2. Remove the wall from the list


        //we are always going to start at 0,0
        Maze[0, 0].IsWall = false;
        Maze[0, 0].Touched = true;
        //Add possible connections
        Walls.Add(new Coord(2, 0,  0,0));
        Walls.Add(new Coord(0, 2,  0,0));


        while (Walls.Count > 0)
        {
            Chung();
        }

        int MaxI = (int)TopLeftOfOpenArea.x + OpenAreaDemention + 1;
        int MaxY = (int)TopLeftOfOpenArea.y + OpenAreaDemention + 1;

        //Opening the end space hard coded bra
        for (int i = (int)TopLeftOfOpenArea.x; i < MaxI; i++)
        {
            for(int j = (int)TopLeftOfOpenArea.y; j < MaxY; j++)
            {
                Maze[i, j].IsWall = false;
                Maze[i, j].Touched = true;
            }
        }
    }

    void Chung()
    {
        CleanWalls();
        //chose a spot to go to
        int temp = Random.Range(0, Walls.Count);
        //Debug.Log(temp);
        //clear the wall to that spot
        if (Walls[temp].pX != Walls[temp].x)
        {//this is where the connection is
            float dirtemp = Walls[temp].pX - Walls[temp].x;
            if (dirtemp < 0)
            {
                Maze[Walls[temp].pX + 1, Walls[temp].pY].IsWall = false;
                Maze[Walls[temp].pX + 1, Walls[temp].pY].Touched = true;
            }
            else
            {
                Maze[Walls[temp].pX - 1, Walls[temp].pY].IsWall = false;
                Maze[Walls[temp].pX - 1, Walls[temp].pY].Touched = true;
            }
        }

        if (Walls[temp].pY != Walls[temp].y)
        {//this is where the connection is
            float dirtemp = Walls[temp].pY - Walls[temp].y;
            if (dirtemp < 0)
            {
                Maze[Walls[temp].pX, Walls[temp].pY + 1].IsWall = false;
                Maze[Walls[temp].pX, Walls[temp].pY + 1].Touched = true;
            }
            else
            {
                Maze[Walls[temp].pX, Walls[temp].pY - 1].IsWall = false;
                Maze[Walls[temp].pX, Walls[temp].pY - 1].Touched = true;
            }
        }

        //Clear the chosen Spot
        Maze[Walls[temp].x, Walls[temp].y].IsWall = false;
        Maze[Walls[temp].x, Walls[temp].y].Touched = true;

        //Get the spot the chosen spot connects to
        if (Walls[temp].x - 2 > 0)
        {//we can add this connection
            if (Maze[Walls[temp].x - 2, Walls[temp].y].Touched == false)
            {//check if the spot was already touched
                Walls.Add(new Coord(Walls[temp].x - 2, Walls[temp].y, Walls[temp].x, Walls[temp].y));
            }
        }
        if (Walls[temp].x + 2 < MazeDimension)
        {//we can add this connection
            if (Maze[Walls[temp].x + 2, Walls[temp].y].Touched == false)
            {
                Walls.Add(new Coord(Walls[temp].x + 2, Walls[temp].y, Walls[temp].x, Walls[temp].y));
            }
        }
        if (Walls[temp].y - 2 > 0)
        {//We can add this connection
            if (Maze[Walls[temp].x, Walls[temp].y - 2].Touched == false)
            {
                Walls.Add(new Coord(Walls[temp].x, Walls[temp].y - 2, Walls[temp].x, Walls[temp].y));
            }
        }
        if (Walls[temp].y + 2 < MazeDimension)
        {//we can add this connection
            if (Maze[Walls[temp].x, Walls[temp].y + 2].Touched == false)
            {
                Walls.Add(new Coord(Walls[temp].x, Walls[temp].y + 2, Walls[temp].x, Walls[temp].y));
            }
        }

        //delete the chosen one from the list
        Walls.RemoveAt(temp);         
    }

    void CleanWalls()
    {
        for(int i = 0; i < Walls.Count; i++)
        {
            if(Maze[Walls[i].x, Walls[i].y].Touched)
            {//remove it from the list
                Walls.RemoveAt(i);
                i--;
            }
        }
    }

    public void TearDown()
    {
        for(var i = 0; i < transform.childCount; i++)
            Destroy(transform.GetChild(i).gameObject);
    }
}
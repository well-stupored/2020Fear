using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MazeGenerator : MonoBehaviour
{

    public GameObject Wall;

    public int MazeDimension;
    public float GrindSpacing;

    public struct MazeSpot
    {
        public Transform Spot;
        public bool IsWall;
        public bool Touched;
    }

    List<Coord> Walls = new List<Coord>();

    public struct Coord
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

    public MazeSpot[,] Maze;


    // Use this for initialization
    void Start()
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
                Maze[i, j].Spot = new GameObject().transform;
                Maze[i, j].Spot.position = new Vector3(this.transform.position.x + i * GrindSpacing, 0, this.transform.position.z + j * GrindSpacing);
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
                if(Maze[i,j].IsWall == true)
                {
                    GameObject temp;
                    temp = Instantiate(Wall, Maze[i, j].Spot.position, Quaternion.identity) as GameObject;
                }
            }
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
            if(Maze[Walls[i].x, Walls[i].y].Touched == true)
            {//remove it from the list
                Walls.RemoveAt(i);
                i--;
            }
        }
    }

   
}
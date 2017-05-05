using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public enum Color
	{
		White, Red, Green, Blue
	}
	public enum Direction
	{
		Right, Left, Up, Down
	}

	public GameObject[] Tile0;
	public GameObject[] Tile1;
	public GameObject[] Tile2;
	public GameObject[] Tile3;
	public GameObject[] Tile4;
	public GameObject[] Tile5;
	public GameObject[] Tile6;

	public int amountOfGoalIn = 0;

	public GameObject Tile(int n, int m)
	{
		Debug.Assert(n > 2 || n < -2 || m > 3 || m < -3);
		switch(m)
		{
			case -3:
			return Tile0[n+2];
			case -2:
			return Tile1[n+2];
			case -1:
			return Tile2[n+2];
			case 0:
			return Tile3[n+2];
			case 1:
			return Tile4[n+2];
			case 2:
			return Tile5[n+2];
			case 3:
			return Tile6[n+2];
			default:
			Debug.Log("Tile is out of range");
			return null;
		}
	}
}
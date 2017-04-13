using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
	Right, Left, Up, Down
}

public class CreateLight : MonoBehaviour
{
	public List<LightSource> lightSources;
	public GameObject lightPrefab;

	private GameObject mapManager;
	private GameObject createdLight;

	private void Start()
	{
		mapManager = GameObject.Find("Map Manager");

		foreach(LightSource source in lightSources)
		{
			source.startPos = transform.position;

			createdLight = Instantiate(lightPrefab, source.startPos, Quaternion.Euler(0, 0, 0));
			createdLight.transform.localScale = new Vector3 (DistanceFromPrism(source.startPos, DirectionVector(source.direction)), 1, 1);
		}
	}

	private void Update()
	{
	}

	private Vector2 DirectionVector(Direction direction)
	{
		switch(direction)
		{
			case Direction.Right:
				return new Vector2 (1, 0);
			case Direction.Left:
				return new Vector2 (-1, 0);
			case Direction.Up:
				return new Vector2 (0, 1);
			case Direction.Down:
				return new Vector2 (0, -1);
			default:
				return new Vector2 (0, 0);
		}
	}

	private float DistanceFromPrism(Vector2 sourcePos, Vector2 dir)
	{
		float distance = 0f;

		while (distance < 50f)
		{
			Vector2 checkBase = sourcePos + dir * distance;

			if(mapManager.GetComponent<MapManager>().IsPrismOnBasebyVector2(checkBase) == true)
			{
				return distance;
			}

			distance++;
		}

		return distance;
	}
}

[System.SerializableAttribute]
public class LightSource
{
	public Direction direction;
	public float length;
	public Vector2 startPos;
}
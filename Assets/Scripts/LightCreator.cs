using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightCreator : MonoBehaviour
{
	public GameManager.Color myLaserColor;
	public GameManager.Direction myLaserDirection;
	public GameObject laserPrefab;

	private GameManager GM;
	private Vector2 myPos;
	private bool isLengthDecided;
	private int length;

	private void Start()
	{
		myPos = transform.position;
		GM = GameObject.Find("Game Manager").GetComponent<GameManager>();
	}

	private void Update()
	{
		if(Input.GetMouseButtonDown(0))
		{
			InstantiateLightCreator(myLaserColor, myLaserDirection);
		}
	}

	private void InstantiateLightCreator(GameManager.Color laserColor, GameManager.Direction laserDirection)
	{
		GameObject laser = Instantiate(laserPrefab);
		//laser.GetComponent<SpriteRenderer>().sprite = laserImage(color);
		laser.transform.localScale = new Vector3 (DecideLaserLength(laserDirection), 1, 1);
		switch(laserDirection)
		{
			case GameManager.Direction.Right:
				laser.transform.rotation = Quaternion.Euler(0,0,0);
				break;
			case GameManager.Direction.Left:
				laser.transform.rotation = Quaternion.Euler(0,0,180);
				break;
			case GameManager.Direction.Up:
				laser.transform.rotation = Quaternion.Euler(0,0,90);
				break;
			case GameManager.Direction.Down:
				laser.transform.rotation = Quaternion.Euler(0,0,270);
				break;
		}
	}

	private TileController.State CheckGivenfPos(Vector2 posToCheck) // 주어진 위치가 어떤 상태인지 체크
	{
		Debug.Log(posToCheck);
		if(posToCheck.x % 10 != 0)
			return TileController.State.None;
		if(posToCheck.y % 10 != 0)
			return TileController.State.None;
		
		int x = (int)(posToCheck.x/10);
		int y = (int)(posToCheck.y/10);

		if(x > 2 || x < -2 || y > 3 || y < -3)
			return TileController.State.None;

		return GM.Tile(x, y).GetComponent<TileController>().state;
	}

	private int DecideLaserLength(GameManager.Direction laserDirection) //자기자신은 체크하지 않음 selfCheck는 따로 해야함
	{
		length = 0;
		isLengthDecided = false;

		while(length < 50 || isLengthDecided == false)
		{
			length = length + 1;

			Vector2 posToCheck = myPos + DirectionVector(laserDirection) * length;
			
			ActionAcordingToState(posToCheck);
			Debug.Log(length);
			if(length > 50)
			{
				isLengthDecided = true;
			}
		}

		return length;
	}
	private Vector2 DirectionVector(GameManager.Direction laserDirection)
	{
		switch(laserDirection)
		{
			case GameManager.Direction.Right:
				return new Vector2(1, 0);
			case GameManager.Direction.Left:
				return new Vector2(-1, 0);
			case GameManager.Direction.Up:
				return new Vector2(0, 1);
			case GameManager.Direction.Down:
				return new Vector2(0, -1);
		}

		return new Vector2(0, 0);
	}

	private void ActionAcordingToState(Vector2 posToCheck)
	{
		if(CheckGivenfPos(posToCheck) == TileController.State.Prism)
		{
			switch(myLaserDirection)
			{
				case GameManager.Direction.Right:
					if(GM.Tile((int)posToCheck.x, (int)posToCheck.y).GetComponent<TileController>().prism.leftFilter.color != myLaserColor)
					{
						length = length - 2;
					}
					else
					{
						// InstantiateLightCreator(myLaserColor, Right);
						// InstantiateLightCreator(myLaserColor, Up);
						// InstantiateLightCreator(myLaserColor, Down);
					}
					break;
				case GameManager.Direction.Left:
					if(GM.Tile((int)posToCheck.x, (int)posToCheck.y).GetComponent<TileController>().prism.rightFilter.color != myLaserColor)
					{
						length = length - 2;
					}
					else
					{
						// InstantiateLightCreator(myLaserColor, Right);
						// InstantiateLightCreator(myLaserColor, Up);
						// InstantiateLightCreator(myLaserColor, Down);
					}
					break;
				case GameManager.Direction.Up:
					if(GM.Tile((int)posToCheck.x, (int)posToCheck.y).GetComponent<TileController>().prism.downFilter.color != myLaserColor)
					{
						length = length - 2;
					}
					else
					{
						// InstantiateLightCreator(myLaserColor, Right);
						// InstantiateLightCreator(myLaserColor, Up);
						// InstantiateLightCreator(myLaserColor, Down);
					}
					break;
				case GameManager.Direction.Down:
					if(GM.Tile((int)posToCheck.x, (int)posToCheck.y).GetComponent<TileController>().prism.upFilter.color != myLaserColor)
					{
						length = length - 2;
					}
					else
					{
						// InstantiateLightCreator(myLaserColor, Right);
						// InstantiateLightCreator(myLaserColor, Up);
						// InstantiateLightCreator(myLaserColor, Down);
					}
					break;
			}

			isLengthDecided = true;
		}
		if(CheckGivenfPos(posToCheck) == TileController.State.Goal)
		{
			GM.GetComponent<GameManager>().amountOfGoalIn = GM.GetComponent<GameManager>().amountOfGoalIn + 1;
		}
	}
}
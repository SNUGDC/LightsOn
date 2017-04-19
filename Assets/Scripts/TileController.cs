using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
	public GameObject GM;
	public GameObject laser;
	public enum Color {
		None, White, Red, Green, Blue
	}

	public enum Direction {
		None, Right, Left, Up, Down
	}

	public enum State
	{
		None, Base, prism, LightSource
	}

	public State state;
	public Color laserColor;
	public Direction laserDir;

	public int myPosX;
	public int myPosY;

	[System.Serializable]
	public class LaserQueue
	{
		public Color color;
		public Direction dir;
		public LaserQueue(Color color, Direction dir)
		{
			this.color = color;
			this.dir = dir;
		}
	}
	public List<LaserQueue> queue;

	private GameObject myLaser;

	private void Start()
	{
		myPosX = (int)transform.position.x/10;
		myPosY = (int)transform.position.y/10;

		queue = new List<LaserQueue>();

		switch(state)
		{
			case State.None:
			GetComponent<SpriteRenderer>().sprite = null;
			break;
			case State.LightSource:
			queue.Add(new LaserQueue(laserColor, laserDir));
			break;
		}
	}

	private void Update()
	{
		if(Input.GetMouseButtonDown(0) || queue.Count != 0)
		{
			Main();
		}
	}

	private void Main()
	{
		if(queue.Count == 0)
		{
			return;
		}

		Direction direction = queue[0].dir;
		Color color = queue[0].color;

		CreateLaser(direction, color);
		PassQueueToNextTile(direction, color);		
	}

	private void CreateLaser(Direction direction, Color color)
	{
		int laserLength;
		myLaser = Instantiate(laser, transform.position, SpawnAngle(direction));

		laserLength = DecideLaserLength(direction, color);
		myLaser.transform.localScale = new Vector3(laserLength,1,1);
	}

	private int DecideLaserLength(Direction direction, Color color)
	{
		/*foreach(GameManager.Prism prism in GM.GetComponent<GameManager>().prism)
		{
			if(NextPos(direction) != prism.pos)
			{
				Debug.Log("프리즘이 없어서 길이가 5가 된 위치는 " + myPosX + ", " + myPosY);
				return 5;
			}
		}
		foreach(GameManager.Prism prism in GM.GetComponent<GameManager>().prism)
		{
			if(FilterToCheck(prism, direction).color == color)
			{
				Debug.Log("올바른 필터로 인해 길이가 5가 된 위치는 " + myPosX + ", " + myPosY);
				return 5;
			}
		}*/
		foreach(GameManager.Prism prism in GM.GetComponent<GameManager>().prism)
		{
			if(NextPos(direction) == prism.pos && FilterToCheck(prism, direction).color != color)
			{
				//Debug.Log("올바르지 않은 필터로 인해 길이가 3이 된 위치는 " + myPosX + ", " + myPosY);
				return 3;
			}
		}
		
		return 5;
	}

	private TileController Tile(int n, int m)
	{
		if(n > 2 || n <-2 || m > 3 || m < -3)
			return null;
		return GM.GetComponent<GameManager>().Tile(n,m).GetComponent<TileController>();
	}

	private Quaternion SpawnAngle(Direction dir)
	{
		Quaternion angle = new Quaternion (0,0,0,0);

		switch(dir)
		{
			case Direction.Right:
			angle.eulerAngles = new Vector3(0,0,0);
			return angle;
			case Direction.Left:
			angle.eulerAngles = new Vector3(0,0,180);
			return angle;
			case Direction.Up:
			angle.eulerAngles = new Vector3(0,0,90);
			return angle;
			case Direction.Down:
			angle.eulerAngles = new Vector3(0,0,270);
			return angle;
		}

		Debug.Log("Sth is wrong at DecideSpawnAngle");
		return angle;
	}

	private void PassQueueToNextTile(Direction direction, Color color)
	{
		queue.RemoveAt(0);

		if(PrismCheck(direction) == false)
		{
			if (Tile((int)NextPos(direction).x, (int)NextPos(direction).y) == null)
				return;
			// Tile((int)NextPos(direction).x, (int)NextPos(direction).y).laserColor = laserColor;
			// Tile((int)NextPos(direction).x, (int)NextPos(direction).y).laserDir = laserDir;

			Tile((int)NextPos(direction).x, (int)NextPos(direction).y).queue.Add(new LaserQueue(color, direction));
		}
		else if(PrismCheck(direction) == true)
		{
			if(FilterToCheck(GetPrism(direction), direction).color != color)
				return;
			else if (FilterToCheck(GetPrism(direction), direction).color == color)
			{
				switch(direction)
				{
					case Direction.Right:
					if(GetPrism(direction).upFilter.color != Color.None)
					{
						Color filterColor = GetPrism(direction).upFilter.color;
						Direction filterDirection = Direction.Up;

						// Tile((int)NextPos(direction).x, (int)NextPos(direction).y).laserColor = filterColor;
						// Tile((int)NextPos(direction).x, (int)NextPos(direction).y).laserDir = filterDirection;

						Tile((int)NextPos(direction).x, (int)NextPos(direction).y).queue.Add(new LaserQueue(filterColor, filterDirection));
					}
					break;
				}
			}
		}
		
	}

	private bool PrismCheck(Direction direction)
	{
		foreach(GameManager.Prism prism in GM.GetComponent<GameManager>().prism)
		{
			if(NextPos(direction) == prism.pos)
			{
				return true;
			}
		}
		return false;
	}

	private GameManager.Prism GetPrism(Direction direction)
	{
		foreach(GameManager.Prism prism in GM.GetComponent<GameManager>().prism)
		{
			if(NextPos(direction) == prism.pos)
			{
				return prism;
			}
		}
		return null;
	}

	private Vector2 NextPos(Direction dir)
	{
		int nextPosX = 0;
		int nextPosY = 0;

		switch(dir)
		{
			case Direction.Right:
				nextPosX = myPosX + 1;
				nextPosY = myPosY;
				break;
			case Direction.Left:
				nextPosX = myPosX - 1;
				nextPosY = myPosY;
				break;
			case Direction.Up:
				nextPosX = myPosX;
				nextPosY = myPosY + 1;
				break;
			case Direction.Down:
				nextPosX = myPosX;
				nextPosY = myPosY - 1;
				break;
			default:
				Debug.Log("Sth is Wrong at PassQueueToNextTile Function");
				break;
		}

		return new Vector2(nextPosX, nextPosY);
	}

	public GameManager.Filter FilterToCheck(GameManager.Prism prism, Direction dir)
	{
		switch(dir)
		{
			case Direction.Right:
			return prism.leftFilter;
			case Direction.Left:
			return prism.rightFilter;
			case Direction.Up:
			return prism.downFilter;
			case Direction.Down:
			return prism.upFilter;
			default:
			Debug.Log("Sth is wrong at FilterToCheck Function");
			return null;
		}
	}
}
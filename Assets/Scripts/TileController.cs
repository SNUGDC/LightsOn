using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
		None, Base, prism, LightSource, Goal
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
			case State.Goal:
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

		CreateLaser(queue[0]);
		PassQueueToNextTile(queue[0]);
	}

	private void CreateLaser(LaserQueue firstQueue)
	{
		Direction direction = firstQueue.dir;
		Color color = firstQueue.color;

		int laserLength;
		myLaser = Instantiate(laser, transform.position, SpawnAngle(direction));

		laserLength = DecideLaserLength(direction, color);
		myLaser.transform.localScale = new Vector3(laserLength,1,1);
		myLaser.GetComponent<SpriteRenderer>().sprite = GM.GetComponent<GameManager>().LaserSprite(color);

		CheckGoalIn(color);
	}

	private int DecideLaserLength(Direction direction, Color color)
	{
		foreach(GameManager.Prism prism in GM.GetComponent<GameManager>().prism)
		{
			if(NextPos(direction) == prism.pos && FilterToCheck(prism, direction).color != color)
			{
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

	private void PassQueueToNextTile(LaserQueue firstQueue)
	{
		Direction inputDirection = firstQueue.dir;
		Color inputColor = firstQueue.color;

		queue.RemoveAt(0);

		if(PrismCheck(inputDirection) == false) // 다음 타일에 프리즘이 없을 경우
		{
			if (Tile((int)NextPos(inputDirection).x, (int)NextPos(inputDirection).y) == null)
				return;

			Tile((int)NextPos(inputDirection).x, (int)NextPos(inputDirection).y).queue.Add(new LaserQueue(inputColor, inputDirection));
		}
		else if(PrismCheck(inputDirection) == true) // 다음 타일에 프리즘이 있을 경우
		{
			if(FilterToCheck(GetPrism(inputDirection), inputDirection).color != inputColor) // 잘못된 필터를 통과할 경우
				return;

			var allOutputDirections = new List<Direction> {									// 올바른 필터를 통과할 경우
				Direction.Right, Direction.Left, Direction.Up, Direction.Down
			};
			var outputDirections = allOutputDirections.Where(d => d != Opposite(inputDirection)).ToList();

			var nextPrism = GetPrism(inputDirection);
			foreach (var outputDir in outputDirections)
			{
				var nextPrismFilter = nextPrism.GetFilter(outputDir);
				if (nextPrismFilter.color == inputColor)
				{
					Color filterColor = nextPrismFilter.color;
					Direction filterDirection = outputDir;
					Tile((int)NextPos(inputDirection).x, (int)NextPos(inputDirection).y).queue.Add(new LaserQueue(filterColor, filterDirection));
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

	private Direction Opposite(Direction direction)
	{
		switch(direction)
		{
			case Direction.Right:
				return Direction.Left;
			case Direction.Left:
				return Direction.Right;
			case Direction.Up:
				return Direction.Down;
			case Direction.Down:
				return Direction.Up;
		}
		return Direction.None;
	}

	private void CheckGoalIn(Color color)
	{
		if(state != State.Goal)
			return;

		foreach(GameManager.Goal goal in GM.GetComponent<GameManager>().goal)
		{
			if(goal.color != color)
				return;

			goal.isOn = true;
		}
	}
}
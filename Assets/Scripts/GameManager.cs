using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public GameObject[] Tile0;
	public GameObject[] Tile1;
	public GameObject[] Tile2;
	public GameObject[] Tile3;
	public GameObject[] Tile4;
	public GameObject[] Tile5;
	public GameObject[] Tile6;

	public Vector2[] lightSource;
	public GameObject lightSourcePrefab;

	[System.Serializable]
	public class Prism
	{
		public GameObject prismObject;
		public Vector2 pos;
		public Filter rightFilter;
		public Filter leftFilter;
		public Filter upFilter;
		public Filter downFilter;

	}
	[System.Serializable]
	public class Filter
	{
		public TileController.Color color;
	}

	public Prism[] prism;
	public GameObject prismPrefab;
	public Sprite[] normalFilterImage;

	public GameObject Tile(int n, int m)
	{
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

	private void Awake()
	{
		foreach(Vector2 pos in lightSource)
		{
			int x = (int)pos.x;
			int y = (int)pos.y;
			TileController tile = Tile(x,y).GetComponent<TileController>();
			tile.state = TileController.State.LightSource;
		}
		foreach (Prism prism in prism)
		{
			int x = (int)prism.pos.x;
			int y = (int)prism.pos.y;
			TileController tile = Tile(x,y).GetComponent<TileController>();
			tile.state = TileController.State.prism;
		}
	}

	private void Start()
	{
		foreach(Vector2 pos in lightSource)
		{
			Instantiate(lightSourcePrefab, 10*pos, new Quaternion(0, 0, 0, 0));
		}
		foreach (Prism prism in prism)
		{
			prism.prismObject = Instantiate(prismPrefab, 10*prism.pos, new Quaternion(0, 0, 0, 0));
			InitializePrism(prism);
		}
	}

	private void InitializePrism(Prism prism)
	{
		if(prism.rightFilter.color == TileController.Color.None)
		{
			Destroy(prism.prismObject.GetComponent<PrismController>().rightFilter);
		}
		else
		{
			prism.prismObject.GetComponent<PrismController>().rightFilter.GetComponent<SpriteRenderer>().sprite = FilterColor(prism.rightFilter.color);
		}
		if(prism.leftFilter.color == TileController.Color.None)
		{
			Destroy(prism.prismObject.GetComponent<PrismController>().leftFilter);
		}
		else
		{
			prism.prismObject.GetComponent<PrismController>().leftFilter.GetComponent<SpriteRenderer>().sprite = FilterColor(prism.leftFilter.color);
		}
		if(prism.upFilter.color == TileController.Color.None)
		{
			Destroy(prism.prismObject.GetComponent<PrismController>().upFilter);
		}
		else
		{
			prism.prismObject.GetComponent<PrismController>().upFilter.GetComponent<SpriteRenderer>().sprite = FilterColor(prism.upFilter.color);
		}
		if(prism.downFilter.color == TileController.Color.None)
		{
			Destroy(prism.prismObject.GetComponent<PrismController>().downFilter);
		}
		else
		{
			prism.prismObject.GetComponent<PrismController>().downFilter.GetComponent<SpriteRenderer>().sprite = FilterColor(prism.downFilter.color);
		}
	}

	private Sprite FilterColor(TileController.Color color)
	{
		switch(color)
		{
			case TileController.Color.White:
			return normalFilterImage[0];
			case TileController.Color.Red:
			return normalFilterImage[1];
			case TileController.Color.Green:
			return normalFilterImage[2];
			case TileController.Color.Blue:
			return normalFilterImage[3];
			default:
			Debug.Log("Sth is wrong at FilterColor Function");
			return normalFilterImage[0];
		}
	}
}
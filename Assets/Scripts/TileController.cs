using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
	public Vector2 position;
	public GameObject prism;
	public GameObject[] laser; //방향을 숫자가 아닌 laser.right 같은 것으로 쓰고 싶음!

	public Sprite[] laserColor; //색을 숫자가 아닌 laser.red 같은 것으로 쓰고 싶음!
	public Sprite[] prismState; //상태를 숫자가 아닌 prism.normal 같은 것으로 쓰고 싶음!
	public bool prismOn = false;
	public bool[] laserState;

	private void Start()
	{
		position = new Vector2(transform.position.x/10, transform.position.y/10);
	}

	private void Update()
	{
		PrismStateChange();
		LaserStateChange();
	}

	private void PrismStateChange()
	{
		if(prismOn == false)
		{
			prism.SetActive(false);
		}
		else if(prismOn == true)
		{
			prism.SetActive(true);
		}
	}

	private void LaserStateChange()
	{
		for(int i = 0; i < 4; i++)
		{
			if(laserState[i] == true)
			{
				laser[i].SetActive(true);
			}
			else
			{
				laser[i].SetActive(false);
			}
		}
	}

	public void LaserStateChanger(int i, bool isOn)
	{
		laserState[i] = isOn;
	}

	public void PrismStateChanger(bool isOn)
	{
		prismOn = isOn;
	}
}
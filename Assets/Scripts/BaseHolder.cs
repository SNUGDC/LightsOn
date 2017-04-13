using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseHolder : MonoBehaviour
{
	public GameObject prism;
	public Vector2 basePos;
	public bool isPrism;
	public string[] glassInfo; // Right Left Up Down, None, White, Red, Blue, Green

	private void Start()
	{
		if(isPrism == true)
		{
			prism.SetActive(true);
		}
		else
		{
			prism.SetActive(false);
		}
	}
}
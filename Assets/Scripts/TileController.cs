using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TileController : MonoBehaviour
{
	public GameObject GM;

	[System.Serializable]
	public enum State
	{
		None, Base, Prism, Goal
	}

	public State state;

	[System.Serializable]
	public class Prism
	{
		public Filter rightFilter;
		public Filter leftFilter;
		public Filter upFilter;
		public Filter downFilter;
	}
	[System.Serializable]
	public class Filter
	{
		public bool isOn;
		public GameManager.Color color;
	}

	public Prism prism;
}

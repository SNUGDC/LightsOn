using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
	public GameObject[] Base;
	public Sprite[] BaseImage;

	private GameObject startPrismBase;
	private GameObject finishPrismBase;
	private bool isHoldingPrism = false;

	private void Update()
	{
		foreach(GameObject baseTile in Base) // 모든 타일 베이스 이미지 초기화
		{
			baseTile.GetComponent<SpriteRenderer>().sprite = BaseImage[0];
		}

		if(Input.GetMouseButtonDown(0)) // 마우스 눌렀을 때
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			Vector2 mousePos = ray.origin;

			float distance = Mathf.Sqrt(Mathf.Pow(mousePos.x - NearestBase(mousePos).transform.position.x, 2) + Mathf.Pow(mousePos.y - NearestBase(mousePos).transform.position.y, 2));

			if(distance > 5f)
				return;
			if(NearestBase(mousePos).GetComponent<BaseHolder>().prism.activeInHierarchy == false)
				return;
			
			startPrismBase = NearestBase(mousePos);
			isHoldingPrism = true;
		}

		if(isHoldingPrism == true && Input.GetMouseButton(0)) // 프리즘이 잡혔고 마우스가 눌러진 상태일 때
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			Vector2 mousePos = ray.origin;

			if(IsPrismOnBasebyObject(NearestBase(mousePos)) == true)
			{
				return;
			}

			NearestBase(mousePos).GetComponent<SpriteRenderer>().sprite = BaseImage[2];
		}

		if(isHoldingPrism == true && Input.GetMouseButtonUp(0)) // 프리즘이 잡혔고 마우스를 뗐을 때
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			Vector2 mousePos = ray.origin;

			if(IsPrismOnBasebyObject(NearestBase(mousePos)) == true)
			{
				return;
			}

			NearestBase(mousePos).GetComponent<BaseHolder>().prism.SetActive(true);
			finishPrismBase = NearestBase(mousePos);

			startPrismBase.GetComponent<BaseHolder>().prism.SetActive(false);
			finishPrismBase.GetComponent<BaseHolder>().prism.SetActive(true);

			isHoldingPrism = false;
		}
	}

	private GameObject NearestBase(Vector2 pos)
	{
		GameObject nearBase = null;
		float distance = 999f;

		foreach(GameObject baseTile in Base)
		{
			float nowDistance;
			Vector2 tilePos = baseTile.transform.position;
			nowDistance = Mathf.Sqrt(Mathf.Pow(pos.x - tilePos.x, 2) + Mathf.Pow(pos.y - tilePos.y, 2));

			if(distance > nowDistance)
			{
				distance = nowDistance;
				nearBase = baseTile;
			}
		}

		return nearBase;
	}

	private bool IsPrismOnBasebyObject(GameObject Base)
	{
		return Base.GetComponent<BaseHolder>().prism.activeInHierarchy;
	}

	public bool IsPrismOnBasebyVector2(Vector2 pos)
	{
		foreach(GameObject baseTile in Base)
		{
			if(new Vector2 (baseTile.transform.position.x, baseTile.transform.position.y) == pos)
			{
				return true;
			}
		}

		return false;
	}
}
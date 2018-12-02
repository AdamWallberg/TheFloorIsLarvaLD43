using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
	public Room _nextRoom;

	void OnTriggerEnter2D(Collider2D collision)
	{
		if(_nextRoom)
		{
			StartCoroutine(Transition(collision.transform));
		}
	}

	IEnumerator Transition(Transform player)
	{
		for(float f = 0.0f; f < 1.0f; f += Time.deltaTime)
		{

		}

		_nextRoom.SetActive(player);
		yield return null;
	}
}

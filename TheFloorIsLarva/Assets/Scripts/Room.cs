using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
	public Transform _playerSpawn;

	public void SetActive(Transform player)
	{
		Vector3 camPos = Camera.main.transform.position;
		camPos.x = transform.position.x;
		camPos.y = transform.position.y;
		Camera.main.transform.position = camPos;
		player.position = _playerSpawn.position;
	}
}

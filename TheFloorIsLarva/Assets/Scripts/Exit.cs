using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Exit : MonoBehaviour
{
	public Room _currentRoom;
	public Room _nextRoom;
	public Image _fadePanel;
	public float _fadeTime = 0.5f;

	void OnTriggerEnter2D(Collider2D collision)
	{
		if(_nextRoom)
		{
			StartCoroutine(Transition(collision.transform));
		}
		else
		{
			SceneManager.LoadScene("Win");
		}
	}

	IEnumerator Transition(Transform player)
	{
		for(float f = 0.0f; f < 1.0f; f += Time.deltaTime / _fadeTime)
		{
			_fadePanel.color = new Color(0f, 0f, 0f, f);
			yield return null;
		}

		_nextRoom.SetActive(player);

		for (float f = 1.0f; f > 0.0f; f -= Time.deltaTime / _fadeTime)
		{
			_fadePanel.color = new Color(0f, 0f, 0f, f);
			yield return null;
		}
		_fadePanel.color = new Color(0f, 0f, 0f, 0f);

		_currentRoom.gameObject.SetActive(false);

		yield return null;
	}
}

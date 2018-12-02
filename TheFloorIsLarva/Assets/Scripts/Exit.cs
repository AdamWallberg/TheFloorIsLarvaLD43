using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Exit : MonoBehaviour
{
	public Room _currentRoom;
	public Room _nextRoom;
	public CanvasGroup _fadePanel;
	public float _fadeTime = 0.5f;
	public float _readTime = 4.0f;

	[TextArea] public string _text;
	public TextMeshProUGUI _tmpText;

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
		_tmpText.text = _text;

		for(float f = 0.0f; f < 1.0f; f += Time.deltaTime / _fadeTime)
		{
			_fadePanel.alpha = f;
			yield return null;
		}

		yield return new WaitForSeconds(_readTime);
		_nextRoom.SetActive(player);

		for (float f = 1.0f; f > 0.0f; f -= Time.deltaTime / _fadeTime)
		{
			_fadePanel.alpha = f;
			yield return null;
		}
		_fadePanel.alpha = 0.0f;

		_currentRoom.gameObject.SetActive(false);

		yield return null;
	}
}

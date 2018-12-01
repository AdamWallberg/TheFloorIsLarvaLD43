using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
	enum InputType
	{
		NONE,
		CLICK,
		DRAG,
	}
	InputType _currentInput = InputType.NONE;
	float _timeAtLastClick;
	Vector2 _lastClickPos;
	MaggotChunk _lastSelectedChunk;
	bool _mouseHeld = false;

	public float _doubleClickThreshold = 0.5f;
	public float _dragThreshold = 1.0f;
	
	void Update ()
	{
		HandleMaggotChunks();
	}
	
	void HandleMaggotChunks()
	{
		bool mouseClicked = Input.GetMouseButtonDown(0);
		bool mouseReleased = Input.GetMouseButtonUp(0);
		Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		// Release mouse button
		if (mouseReleased)
		{
			_mouseHeld = false;
			if (_currentInput == InputType.DRAG)
				_currentInput = InputType.NONE;
		}

		// Check for clicking of maggot chunks
		if (_currentInput == InputType.NONE && mouseClicked)
		{
			MaggotChunk selectedChunk = null;
			foreach (var c in FindObjectsOfType<MaggotChunk>())
			{
				CircleCollider2D coll = c.GetComponent<CircleCollider2D>();
				float r = coll.radius;

				if (CheckMouseOver(c))
				{
					_timeAtLastClick = Time.time;
					selectedChunk = c;
					break;
				}
			}

			if (selectedChunk)
			{
				_lastSelectedChunk = selectedChunk;
				_currentInput = InputType.CLICK;
				_lastClickPos = pos;
				_mouseHeld = true;
			}
		}
		// Check for double clicking & dragging
		else if(_currentInput == InputType.CLICK)
		{
			if(_lastSelectedChunk != null)
			{
				if(mouseClicked) // Double click
				{
					if (CheckMouseOver(_lastSelectedChunk))
					{
						if (Time.time - _timeAtLastClick < _doubleClickThreshold)
						{
							_currentInput = InputType.NONE;
							_lastSelectedChunk.Split();
							_lastSelectedChunk = null;
							print("Doubleclicked the thing");
						}
					}
				}
				else if(_mouseHeld) // Dragging
				{
					Vector2 diff = pos - (Vector2)_lastSelectedChunk.transform.position;
					float distSquared = diff.sqrMagnitude;
					if(distSquared > _dragThreshold * _dragThreshold)
					{
						_currentInput = InputType.DRAG;
					}
				}
			}
		}

		if(_currentInput == InputType.DRAG)
		{
			if(_lastSelectedChunk)
			{
				Vector2 diff = pos - (Vector2)_lastSelectedChunk.transform.position;
				_lastSelectedChunk.Drag(diff);
			}
		}

		// Check double click threshold
		if (_currentInput == InputType.CLICK && 
			Time.time - _timeAtLastClick > _doubleClickThreshold)
		{
			_currentInput = InputType.NONE;
		}
	}

	bool CheckMouseOver(MaggotChunk mc)
	{
		float radius = mc.GetComponent<CircleCollider2D>().radius;
		radius *= mc.transform.localScale.x;

		return 
			Vector2.Distance(
				mc.transform.position, 
				Camera.main.ScreenToWorldPoint(Input.mousePosition)) 
			< radius;
	}
}

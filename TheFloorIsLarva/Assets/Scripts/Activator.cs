using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Activator : MonoBehaviour
{
	public float Value;
	public bool On
	{
		get { return Value > 0.5f; }
	}
}

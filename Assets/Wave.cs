using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour {

	[SerializeField]
	ComputeShader computeShader;

	[SerializeField]
	RenderTexture texture;

	void Start () {
		computeShader.SetTexture(0, "Result", texture);
		computeShader.Dispatch(0, texture.width / 16, texture.height / 16, 1);
	}

}

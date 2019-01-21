using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputeExamle : MonoBehaviour {

	[SerializeField]
	RenderTexture result;
	[SerializeField]
	ComputeShader computeShader;
	[SerializeField]
	float ss;
	[SerializeField]
	float a;

	int initKernel;
	int velocityKernel;
	int positionKernel;
	int addWaveKernel;

	void Start () {
		initKernel = computeShader.FindKernel("Init");
		velocityKernel = computeShader.FindKernel("VelocityUpdate");
		positionKernel = computeShader.FindKernel("PositionUpdate");
		addWaveKernel = computeShader.FindKernel("AddWave");

		computeShader.SetTexture(initKernel, "Result", result);
		computeShader.Dispatch(initKernel, result.width / 8, result.height / 8, 1);
	}

	void FixedUpdate() {
		computeShader.SetFloat("deltaTime", Time.deltaTime);
		computeShader.SetFloat("ss", ss);
		computeShader.SetFloat("a", a);

		computeShader.SetTexture(velocityKernel, "Result", result);
		computeShader.Dispatch(velocityKernel, result.width / 8, result.height / 8, 1);
		computeShader.SetTexture(positionKernel, "Result", result);
		computeShader.Dispatch(positionKernel, result.width / 8, result.height / 8, 1);
	}

	void OnCollisionEnter(Collision other) {
		var size = new Vector3(result.width, result.height);
		foreach (var item in other.contacts) {
			var pos = (Vector3.one * 5 - new Vector3(item.point.x, item.point.z)) / 10;
			AddWave(Vector3.Scale(pos, size), 10);
		}
	}

	void AddWave(Vector2 pos, float radius) {
		computeShader.SetVector("position", pos);
		computeShader.SetFloat("radius", radius);
		computeShader.SetTexture(addWaveKernel, "Result", result);
		computeShader.Dispatch(addWaveKernel, result.width / 8, result.height / 8, 1);
	}
}

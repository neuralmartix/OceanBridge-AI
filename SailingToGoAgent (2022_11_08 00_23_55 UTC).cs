using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class SailingToGoAgent : Agent
{

	[SerializeField] private Transform targetTransform;
	[SerializeField] private Material winMaterial;
	[SerializeField] private Material loseMaterial;
	[SerializeField] private MeshRenderer floorMeshRenderer;
	private Rigidbody m_Rigidbody;
	private bool isForced = false;
	private DateTime startTime;
	infoTransfer infos;
	public Transform target;
	void Start()
    {
        infos = FindObjectOfType<infoTransfer>();
    }
	public override void OnEpisodeBegin()
	{
		Debug.Log("STARTED");
		transform.localPosition = Vector3.zero;
		m_Rigidbody = GetComponent<Rigidbody>();
		m_Rigidbody.velocity = Vector3.zero;
		m_Rigidbody.angularVelocity = Vector3.zero;
		isForced = false;
		startTime = DateTime.Now;
	}

	public override void CollectObservations(VectorSensor sensor)
	{
		sensor.AddObservation(transform.localPosition);
		sensor.AddObservation(targetTransform.localPosition);
	}

	public override void OnActionReceived(ActionBuffers actions)
	{
		//Debug.Log("OnActionReceived: " + " X: " + actions.ContinuousActions[0] + "  Y: " + actions.ContinuousActions[1]);
		// Debug.Log("X: " + actions.ContinuousActions[0]);
		// Debug.Log("Y: " + actions.ContinuousActions[1]);
		float moveX = actions.ContinuousActions[0];
		float moveZ = actions.ContinuousActions[1];

		// float moveX = 0f;
		// float moveZ = 1f;

		float moveSpeed = infos.b*100f;
		// Debug.Log("Foced: " + isForced);
		// Debug.Log(DateTime.Now - startTime);
		if (DateTime.Now > startTime.AddSeconds(10))
		{
			Debug.Log("Too slow! EndEpisode");
			SetReward(-1f);
			floorMeshRenderer.material = loseMaterial;
			EndEpisode();
		}
		if (!isForced)
		{
			Debug.Log("Applied the force!");
			Debug.Log("OnActionReceived: " + " X: " + actions.ContinuousActions[0] + "  Y: " + actions.ContinuousActions[1]);
			m_Rigidbody.AddForce(new Vector3(moveX, 0, moveZ) * moveSpeed);
			isForced = true;
		}
		// transform.localPosition += new Vector3(moveX, 0, moveZ) * Time.deltaTime * moveSpeed;
	}

	public override void Heuristic(in ActionBuffers actionsOut)
	{
		//Debug.Log("Calll!");
		ActionSegment<float> countinousActions = actionsOut.ContinuousActions;
		countinousActions[0] = Input.GetAxisRaw("Horizontal");
		countinousActions[1] = Input.GetAxisRaw("Vertical");
	}

	private void OnTriggerEnter(Collider other)
	{
		Debug.Log("Triggered!");
		if (other.TryGetComponent<Goal>(out Goal goal))
		{
			SetReward(+1f);
			floorMeshRenderer.material = winMaterial;
			Debug.Log("Win!");
			EndEpisode();
		}
		if (other.TryGetComponent<Wall>(out Wall wall))
		{
			float distance = (transform.localPosition.z + infos.a) / infos.a;
			float distance2 = ((transform.localPosition.x + 4) / 4f);
			distance2 = Math.Abs(distance2);
			// Debug.Log("Distance: " + distance);
			SetReward(-1f*distance*distance2);
			floorMeshRenderer.material = loseMaterial;
			Debug.Log("Lose!");
			EndEpisode();
		}
	}
}



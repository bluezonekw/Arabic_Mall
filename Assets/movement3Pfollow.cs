using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class movement3Pfollow : MonoBehaviour
{
	private CinemachineVirtualCamera Cinemachine;
	private Player playerinput;
	bool isclick = false;
	[SerializeField]
	private float lookspeed = 0.05f;

	private void Awake()
	{
		playerinput = new Player();
		Cinemachine = GetComponent<CinemachineVirtualCamera>();
	}
	private void OnEnable()
	{
		playerinput.Enable();
	}
	private void OnDisable()
	{

		playerinput.Disable();
	}


	void Update()
	{

		Vector2 delta = playerinput.PlayerMan.MoveAround.ReadValue<Vector2>();


	//	Cinemachine.m..Value += delta.x * lookspeed * 50;

//Cinemachine.m_YAxis.Value += delta.y * lookspeed * 0.5f;


	}
}

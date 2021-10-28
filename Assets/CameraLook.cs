//Fixed the issues with the previous controller
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
[RequireComponent(typeof(CinemachineFreeLook))]

public class CameraLook : MonoBehaviour
{
	private CinemachineFreeLook Cinemachine;
	private Player playerinput;

	[SerializeField]
	private float lookspeed = 1.0f;

    private void Awake()
    {
		playerinput = new Player();
		Cinemachine = GetComponent<CinemachineFreeLook>();
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
		Vector2 delta = playerinput.PlayerMan.Lock.ReadValue<Vector2>();
		
		
		Cinemachine.m_XAxis.Value += delta.x * lookspeed *200* Time.deltaTime;

		Cinemachine.m_YAxis.Value += delta.y * lookspeed * Time.deltaTime;


	}
}
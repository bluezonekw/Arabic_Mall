using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapController : MonoBehaviour
{
    public Vector3[] storesLocations;
    public int[] yStoresRotation;
    public Transform Player;
    public OwnThirdPersonController PlayerRotation;
    public GameObject map;
    public void movePlayer(int id)
    {
        print(storesLocations[id - 1]);
        Player.localPosition = storesLocations[id - 1];
        print(yStoresRotation[id - 1]);
        PlayerRotation.CameraAngleY = yStoresRotation[id - 1];
        //Player.eulerAngles = new Vector3(0f, yStoresRotation[id - 1], 0f);
        map.SetActive(false);
    }
}

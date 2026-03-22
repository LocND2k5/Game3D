using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerControlls controlls {  get; private set; }

    public PlayerAim aim {  get; private set; }

    private void Awake()
    {
        controlls= new PlayerControlls();
        aim = GetComponent<PlayerAim>();
    }

    private void OnEnable()
    {
        controlls.Enable();
    }

    private void OnDisable()
    {
        controlls.Disable();
    }
}

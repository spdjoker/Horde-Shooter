using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    public static TeamManager Instance { get; private set; }

    public List<Material> teams;
    public Material testCaptureMaterial;

    public Transform gem;
    public Transform player;
    public int assignedTeam = -1;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public Material GetColorMaterial() => teams[assignedTeam % teams.Count];
}

using System.Net.NetworkInformation;
using UnityEngine;

public class PlayerLocator : MonoBehaviour
{
    public static Transform Player
    {
        get; private set;
    } 
    void Awake()
    {
        Player = transform;
    }
}

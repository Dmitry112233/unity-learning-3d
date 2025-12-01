using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class WayPoint : MonoBehaviour
{
    [SerializeField] [Range(0f, 5f)] public float radius = 2f;
}
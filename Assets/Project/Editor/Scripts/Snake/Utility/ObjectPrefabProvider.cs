using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPrefabProvider : MonoBehaviour, IObjectPrefabProvider
{
    public GameObject headPrefab;
    public GameObject bodyPrefab;
    public GameObject tailPrefab;

    public GameObject HeadPrefab => headPrefab;
    public GameObject BodyPrefab => bodyPrefab;
    public GameObject TailPrefab => tailPrefab;
}

public interface IObjectPrefabProvider
{
    GameObject HeadPrefab { get; }
    GameObject BodyPrefab { get; }
    GameObject TailPrefab { get; }
}

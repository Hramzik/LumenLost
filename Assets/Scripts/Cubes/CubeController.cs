using UnityEngine;

public class CubeController : MonoBehaviour
{
    public enum CubeType { Void, Basic, JumpBoost, Blindness, Trigger, Appearing, Explosion, Portal }
    public CubeType cubeType = CubeType.Basic;
}
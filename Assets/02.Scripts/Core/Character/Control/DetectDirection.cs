using UnityEngine;

using Sirenix.OdinInspector;

public class DetectDirection : MonoBehaviour
{
    private const string TITLE_OPTIONAL = "[Optional]";

    // variables
    [Title(TITLE_OPTIONAL), Range(0f, 10f)]
    [SerializeField] private float _radius = 5f;

    private float _angle = 90f;

    // properties
    public float ThisRadius => _radius;
    public float ThisAngle => _angle;
}

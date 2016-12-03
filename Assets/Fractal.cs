using UnityEngine;
using System.Collections;

public class Fractal : MonoBehaviour
{
    public int maxDepth;
    private int depth;

    public float childScale;

    public Mesh mesh;
    public Material material;

    private static Vector3[] childDirections =
    {
        Vector3.up, Vector3.right, Vector3.left
    };

    private static Quaternion[] childOrientations =
    {
        Quaternion.identity,
        Quaternion.Euler(0f, 0f, -90f),
        Quaternion.Euler(0f, 0f, 90f),
    };

    private Material[] materials;

    private void InitMaterials()
    {
        materials = new Material[maxDepth + 1];

        for (int i = 0; i <= maxDepth; i++)
        {
            materials[i] = new Material(material);
            materials[i].color = Color.Lerp(Color.white, Color.black, (float)i / maxDepth);
        }
    }


    public float maxRotationSpeed;
    private float rotationSpeed;

    // Use this for initialization
    void Start()
    {
        rotationSpeed = Random.Range(-maxRotationSpeed, maxRotationSpeed);
        if (materials == null) { InitMaterials(); }

        gameObject.AddComponent<MeshFilter>().mesh = mesh;
        gameObject.AddComponent<MeshRenderer>().material = materials[depth];
        if (depth < maxDepth)
        {
            StartCoroutine(CreateChildren());
        }
    }

    private IEnumerator CreateChildren()
    {
        for (int i = 0; i < childDirections.Length; i++)
        {
            yield return new WaitForSeconds(0.5f);
            new GameObject("Fractal Child").AddComponent<Fractal>().Initialize(this, i);
        }

    }


    private void Initialize(Fractal parent, int i)
    {
        maxRotationSpeed = parent.maxRotationSpeed;
        mesh = parent.mesh;
        materials = parent.materials;
        maxDepth = parent.maxDepth;
        depth = parent.depth + 1;
        transform.parent = parent.transform;
        childScale = parent.childScale;

        transform.localScale = Vector3.one * childScale;
        transform.localPosition = childDirections[i] * (.5f + .5f * childScale);
        transform.localRotation = childOrientations[i];
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
        transform.localScale = Vector3.one * rotationSpeed * Time.deltaTime;
    }
}

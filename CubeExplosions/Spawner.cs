using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Explosion))]
public class Spawner : MonoBehaviour
{
    [SerializeField] private int _minCountClone = 2;
    [SerializeField] private int _maxCountClone = 6;

    private Explosion _explosion;

    private void Awake()
    {
        _explosion = GetComponent<Explosion>();
    }

    private void OnEnable()
    {
        var handler = GetComponent<Handler>();
        handler.OnDivide += HandleDivide;
    }

    private void OnDisable()
    {
        var handler = GetComponent<Handler>();
        handler.OnDivide -= HandleDivide;
    }

    private void HandleDivide(GameObject originalCube)
    {
        ExplosionData originalData = originalCube.GetComponent<ExplosionData>();
        Vector3 originalPosition = originalCube.transform.position;
        Vector3 originalScale = originalCube.transform.localScale;
        float originalChance = originalData.GetCurrentChance();
        
        Divide(originalCube, originalPosition, originalScale, originalChance);
    }

    private void Divide(GameObject originalCube, Vector3 position, Vector3 scale, float chance)
    {
        List<GameObject> createdCubes = CreateClones(originalCube, position, scale, chance);
        Destroy(originalCube);
        StartCoroutine(_explosion.ApplyExplosionToCubes(position, createdCubes));
    }

    private List<GameObject> CreateClones(GameObject originalCube, Vector3 position, Vector3 scale, float chance)
    {
        int cloneCount = Random.Range(_minCountClone, _maxCountClone + 1);
        List<GameObject> createdCubes = new List<GameObject>();

        for (int i = 0; i < cloneCount; i++)
        {
            GameObject cube = Instantiate(originalCube);
            cube.transform.position = position;
            cube.transform.rotation = Random.rotation;
            cube.transform.localScale = scale / 2f;
            ExplosionData explosionData = cube.GetComponent<ExplosionData>();
            explosionData.InitializeFromParent(chance);
            ChangeColor changeColor = cube.GetComponent<ChangeColor>();
            changeColor.Change();
            Rigidbody rigidbody = cube.GetComponent<Rigidbody>();
            rigidbody.useGravity = true;
            createdCubes.Add(cube);
        }

        return createdCubes;
    }
}
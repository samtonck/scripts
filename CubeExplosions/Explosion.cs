using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private float _explosionRadius = 10f;
    [SerializeField] private float _explosionForce = 1000f;
    [SerializeField] private float _explosionDelay = 0.1f;

    private void OnEnable()
    {
        var handler = GetComponent<Handler>();
        handler.OnExplode += HandleExplode;
    }

    private void OnDisable()
    {
        var handler = GetComponent<Handler>();
        handler.OnExplode -= HandleExplode;
    }

    private void HandleExplode(GameObject cube)
    {
        Vector3 explosionCenter = cube.transform.position;
        Destroy(cube);
        StartCoroutine(ApplyExplosionToCubes(explosionCenter, FindCubesInRadius(explosionCenter)));
    }

    public System.Collections.IEnumerator ApplyExplosionToCubes(Vector3 explosionCenter, List<GameObject> cubes)
    {   
        yield return new WaitForSeconds(_explosionDelay);
        foreach (GameObject cube in cubes)
        {
            if (cube != null)
            {
                ApplyForceToCube(explosionCenter, cube);
            }
        }
    }

    private List<GameObject> FindCubesInRadius(Vector3 explosionCenter)
    {
        List<GameObject> cubes = new List<GameObject>();
        Collider[] colliders = Physics.OverlapSphere(explosionCenter, _explosionRadius);

        foreach (Collider hit in colliders)
        {
            if (hit.GetComponent<ExplosionData>() != null)
                cubes.Add(hit.gameObject);
        }

        return cubes;
    }

    private void ApplyForceToCube(Vector3 explosionCenter, GameObject cube)
    {
        Rigidbody rigidbody = cube.GetComponent<Rigidbody>();
        Vector3 cubePosition = rigidbody.transform.position;
        Vector3 direction = cubePosition - explosionCenter;
        float distance = direction.magnitude;

        if (distance < Mathf.Epsilon) return;

        direction.Normalize();

        float percentage = 1f - Mathf.Clamp01(distance / _explosionRadius);
        float force = _explosionForce * percentage;

        rigidbody.AddForce(direction * force, ForceMode.Impulse);
    }
}
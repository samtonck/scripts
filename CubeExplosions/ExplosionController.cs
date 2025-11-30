using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Clone))]
[RequireComponent(typeof(ChangeColor))]
[RequireComponent(typeof(Explosion))]
public class ExplosionController : MonoBehaviour
{
    [SerializeField] private float _explosionDelay = 0.1f;
    
    private Clone _clone;
    private ChangeColor _changeColor;
    private Explosion _explosion;

    private void Awake()
    {
        _clone = GetComponent<Clone>();
        _changeColor = GetComponent<ChangeColor>();
        _explosion = GetComponent<Explosion>();
    }

    private void OnMouseUpAsButton()
    {
        if (_clone.CanDivide())
        {
            StartCoroutine(ExplodeWithDivision());
        }
        else
        {
            StartCoroutine(ExplodeWithoutDivision());
        }
    }

    private IEnumerator ExplodeWithDivision()
    {
        List<GameObject> clones = _clone.CreateClones();
        
        foreach (GameObject clone in clones)
        {
            ChangeColor colorScript = clone.GetComponent<ChangeColor>();
            colorScript.Change();
        }
        
        yield return StartCoroutine(_explosion.ExplodeAndScatterClones(_explosionDelay, clones));
        Destroy(gameObject);
    }

    private IEnumerator ExplodeWithoutDivision()
    {
        yield return StartCoroutine(_explosion.ExplodeArea(_explosionDelay));
        Destroy(gameObject);
    }
}


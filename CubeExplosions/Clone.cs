using System.Collections.Generic;
using UnityEngine;

public class Clone : MonoBehaviour
{
    [SerializeField] private int _minCountClone = 2;
    [SerializeField] private int _maxCountClone = 6;
    [SerializeField] private int _decreaseInChanceOf = 2;
    
    private float _chanceOfDividing = 100f;

    public void SetChance(float chance)
    {
        _chanceOfDividing = chance;
    }

    public bool CanDivide()
    {
        float randomValue = Random.Range(0f, 100f);
        return randomValue < _chanceOfDividing;
    }

    public List<GameObject> CreateClones()
    {
        List<GameObject> clones = new List<GameObject>();
        int cloneCount = Random.Range(_minCountClone, _maxCountClone + 1);
        
        for (int i = 0; i < cloneCount; i++)
        {
            GameObject clone = Instantiate(gameObject, transform.position, Random.rotation);
            clone.transform.localScale = transform.localScale / 2f;
            Clone cloneScript = clone.GetComponent<Clone>();
            cloneScript.SetChance(_chanceOfDividing / _decreaseInChanceOf);
            clones.Add(clone);
        }
        
        return clones;
    }
}


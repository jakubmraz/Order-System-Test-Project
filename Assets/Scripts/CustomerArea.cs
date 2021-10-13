using System.Collections;
using UnityEngine;

public class CustomerArea : MonoBehaviour
{
    [SerializeField] private Customer customerPrefab;

    void Awake()
    {
        StartCoroutine(SpawnCustomersCoroutine());
    }

    IEnumerator SpawnCustomersCoroutine()
    {
        while(true)
        {
            Customer newCustomer = Instantiate(customerPrefab,
                new Vector3(Random.Range(transform.position.x - 5, transform.position.x + 5), transform.position.y + 2.95f,
                    Random.Range(transform.position.z - 5, transform.position.z + 5)), Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(10f, 15f));
        }
    }
}

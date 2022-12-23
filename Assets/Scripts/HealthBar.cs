using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Transform _Scale;
    [SerializeField] private Transform _CameraTransform;

    private DamagableObject _DamagableObject;
    private Transform target;
    // Start is called before the first frame update
    void Start()
    {
        _CameraTransform = Camera.main.transform;
        transform.rotation = _CameraTransform.transform.rotation;
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        if(target == null) { OnTargetNull(); return; }
        transform.position = target.position;
    }

    private void OnTargetNull()
    {
        _DamagableObject.OnHealthChanged -= SetHealth;
        Destroy(gameObject);
    }
    private void SetHealth(int health, int maxHealth)
    {
        _Scale.localScale = new Vector3(Mathf.Clamp01((float)health / maxHealth), 1f, 1f);
        gameObject.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(TimeActiveGameObject());
    }
    public void Setup(Transform HealthBarPosition, DamagableObject damagableObject)
    {
        _DamagableObject = damagableObject;
        _DamagableObject.OnHealthChanged += SetHealth;
        target = HealthBarPosition;
        gameObject.SetActive(false);
    }

    private IEnumerator TimeActiveGameObject()
    {
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }
}

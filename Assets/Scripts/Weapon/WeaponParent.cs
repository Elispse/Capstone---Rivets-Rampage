using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponParent : MonoBehaviour
{
    [SerializeField]
    public SpriteRenderer characterRenderer;
    [SerializeField]
    public SpriteRenderer weaponRenderer { get; set; }
    public Vector2 pointerPosition { get; set; }
    private Vector3 spriteScale;

    private void Awake()
    {
        weaponRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        Vector2 direction = (pointerPosition - (Vector2)transform.position).normalized;
        transform.right = direction;
        Vector2 scale = transform.localScale;

        if (weaponRenderer != null)
        {
            if (direction.x < 0)
            {
                scale.y = -1;
            }
            else if (direction.x > 0)
            {
                scale.y = 1;
            }
            transform.localScale = scale;

            if (transform.eulerAngles.z > 0 && transform.eulerAngles.z < 180)
            {
                weaponRenderer.sortingOrder = characterRenderer.sortingOrder - 1;
            }
            else
            {
                weaponRenderer.sortingOrder = characterRenderer.sortingOrder + 1;
            }
        }
    }
}

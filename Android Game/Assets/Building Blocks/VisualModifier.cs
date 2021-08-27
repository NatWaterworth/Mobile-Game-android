using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Alter the sprite of this 2D Sprite Renderer based on it's state.
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class VisualModifier : MonoBehaviour
{
   [SerializeField] Sprite[] sprites;
    SpriteRenderer spriteRenderer;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetSprite(int _spriteIndex)
    {
        if (sprites == null || sprites.Length == 0 || spriteRenderer == null)
            return;

        Sprite _sprite = sprites[_spriteIndex % sprites.Length];

        if (_sprite != null)
            spriteRenderer.sprite = _sprite;
        else
            Debug.LogError(this + " has no sprite set at this index: " + _spriteIndex);
    }
}

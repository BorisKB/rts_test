using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnBuildingQueueUI : MonoBehaviour
{
    [SerializeField] private Image _Image;

    [SerializeField] private Sprite _BaseSprite;

    public void SetSprite(Sprite sprite)
    {
        _Image.sprite = sprite == null ?_BaseSprite : sprite;
    }
}

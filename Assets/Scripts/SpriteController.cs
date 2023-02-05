using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteController : Singleton<SpriteController>
{
    public List<SpriteRenderer> sprites;
    public List<Button> buttons;

    public List<Sprite> growSprites;
}
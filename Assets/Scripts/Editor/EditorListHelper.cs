using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EditorListHelper {





    public static Sprite DrawListofSprites(List<Sprite> sprites, int index) {
        Sprite result = EditorHelper.ObjectField<Sprite>("Sprite", sprites[index]);
        return result;
    }

}

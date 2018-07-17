using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facepunch.Steamworks;

public class Utils {

    public static Texture ConvertSteamImage(Facepunch.Steamworks.Image image)
    {
        var texture = new Texture2D(image.Width, image.Height);

        for (int x = 0; x < image.Width; x++)
            for (int y = 0; y < image.Height; y++)
            {
                var p = image.GetPixel(x, y);

                texture.SetPixel(x, image.Height - y, new UnityEngine.Color(p.r / 255.0f, p.g / 255.0f, p.b / 255.0f, p.a / 255.0f));
            }

        texture.Apply();
        return texture;
    }
}

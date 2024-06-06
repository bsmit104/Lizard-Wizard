using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlatformDecorator : MonoBehaviour
{
    public TextMeshPro textMesh;
    public SpriteRenderer leftLineRenderer;
    public SpriteRenderer rightLineRenderer;
    public SpriteRenderer platformRenderer; // Assuming the platform is represented by a sprite renderer

    private void Start()
    {
        DecoratePlatform();
    }

    void DecoratePlatform()
    {
        // Generate random colors for text, lines, and platform
        Color textColor = Random.ColorHSV();
        Color lineColor = Random.ColorHSV();
        Color platformColor = Random.ColorHSV();

        // Ensure platform color is different from line color
        while (ColorUtility.ToHtmlStringRGB(platformColor) == ColorUtility.ToHtmlStringRGB(lineColor))
        {
            platformColor = Random.ColorHSV();
        }

        // Set text and color procedurally
        textMesh.text = GenerateRandomText();
        textMesh.color = textColor;

        // Set the same random color for both lines
        leftLineRenderer.color = lineColor;
        rightLineRenderer.color = lineColor;

        // Set platform color
        platformRenderer.color = platformColor;
    }

    string GenerateRandomText()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        char[] stringChars = new char[8];

        for (int i = 0; i < stringChars.Length; i++)
        {
            stringChars[i] = chars[Random.Range(0, chars.Length)];
        }

        return new string(stringChars);
    }
}


// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using TMPro;

// public class PlatformDecorator : MonoBehaviour
// {
//     public TextMeshPro textMesh;
//     public SpriteRenderer leftLineRenderer;
//     public SpriteRenderer rightLineRenderer;

//     private void Start()
//     {
//         DecoratePlatform();
//     }

//     void DecoratePlatform()
//     {
//         // Generate a random color
//         Color randomColor = Random.ColorHSV();

//         // Set text and color procedurally
//         textMesh.text = GenerateRandomText();
//         textMesh.color = randomColor;

//         // Set the same random color for both lines
//         leftLineRenderer.color = randomColor;
//         rightLineRenderer.color = randomColor;
//     }

//     string GenerateRandomText()
//     {
//         const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
//         char[] stringChars = new char[8];

//         for (int i = 0; i < stringChars.Length; i++)
//         {
//             stringChars[i] = chars[Random.Range(0, chars.Length)];
//         }

//         return new string(stringChars);
//     }
// }




// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using TMPro;

// public class PlatformDecorator : MonoBehaviour
// {
//     public TextMeshPro textMesh;
//     public SpriteRenderer leftLineRenderer;
//     public SpriteRenderer rightLineRenderer;

//     private void Start()
//     {
//         DecoratePlatform();
//     }

//     void DecoratePlatform()
//     {
//         // Set text and color procedurally
//         textMesh.text = GenerateRandomText();
//         textMesh.color = Random.ColorHSV();

//         // Set color for lines
//         leftLineRenderer.color = Random.ColorHSV();
//         rightLineRenderer.color = Random.ColorHSV();
//     }

//     string GenerateRandomText()
//     {
//         const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
//         char[] stringChars = new char[8];

//         for (int i = 0; i < stringChars.Length; i++)
//         {
//             stringChars[i] = chars[Random.Range(0, chars.Length)];
//         }

//         return new string(stringChars);
//     }
// }





// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using TMPro;

// public class PlatformDecorator : MonoBehaviour
// {
//     public GameObject linePrefab;
//     public GameObject textPrefab;
//     public float lineOffsetX = 0.5f;
//     public float lineHeight = 0.1f;
//     public float textHeight = 0.2f;

//     private void Start()
//     {
//         DecoratePlatform();
//     }

//     void DecoratePlatform()
//     {
//         // Left line
//         CreateLine(new Vector3(-transform.localScale.x / 2 + lineOffsetX, 0, 0));

//         // Right line
//         CreateLine(new Vector3(transform.localScale.x / 2 - lineOffsetX, 0, 0));

//         // Random text in the middle
//         CreateText(new Vector3(0, 0, 0));
//     }

//     void CreateLine(Vector3 position)
//     {
//         GameObject line = Instantiate(linePrefab, transform);
//         line.transform.localPosition = position;
//         line.transform.localScale = new Vector3(line.transform.localScale.x, lineHeight, line.transform.localScale.z);
//         line.GetComponent<SpriteRenderer>().color = Random.ColorHSV();
//     }

//     void CreateText(Vector3 position)
//     {
//         GameObject textObj = Instantiate(textPrefab, transform);
//         textObj.transform.localPosition = position;

//         TextMeshPro textComponent = textObj.GetComponent<TextMeshPro>();
//         textComponent.text = GenerateRandomText();
//         textComponent.color = Random.ColorHSV();
//     }

//     string GenerateRandomText()
//     {
//         const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
//         char[] stringChars = new char[8];

//         for (int i = 0; i < stringChars.Length; i++)
//         {
//             stringChars[i] = chars[Random.Range(0, chars.Length)];
//         }

//         return new string(stringChars);
//     }
// }
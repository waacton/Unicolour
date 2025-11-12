using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Wacton.Unicolour;

public class ColourController : MonoBehaviour
{
    private readonly Dictionary<ColourSpace, (Limits first, Limits second, Limits third)> colourSpaces = GetColourSpaces();
    private int currentIndex;
    private int LastIndex => colourSpaces.Count - 1;
    private ColourSpace CurrentSpace => colourSpaces.Keys.ElementAt(currentIndex);

    private Material sphereMaterial;
    private const int AxisScale = 25;
    private TMP_Text colourSpaceTextComponent;
    private readonly Dictionary<GameObject, Unicolour> sphereLookup = new();
    
    
    private void Start()
    {
        sphereMaterial = Resources.Load<Material>("Sphere");
        
        var cameraComponent = GameObject.Find("Main Camera").GetComponent<Camera>();
        var cameraTransform = cameraComponent.transform;
        cameraTransform.position = new Vector3(-AxisScale, AxisScale, -AxisScale);
        cameraTransform.rotation = Quaternion.Euler(AxisScale / 2.0f, 45f, 0f);
        
        DrawAxis(Vector3.zero, Vector3.right * AxisScale, Color.red);
        DrawAxis(Vector3.zero, Vector3.up * AxisScale, Color.green);
        DrawAxis(Vector3.zero, Vector3.forward * AxisScale, Color.blue);
        
        colourSpaceTextComponent = GameObject.Find("Colour Space Text").GetComponent<TMP_Text>();
        
        const int spheresPerAxis = 16;
        for (var r = 0; r < spheresPerAxis; r++)
        {
            for (var g = 0; g < spheresPerAxis; g++)
            {
                for (var b = 0; b < spheresPerAxis; b++)
                {
                    double To255(int sphereIndex) => sphereIndex / (double)(spheresPerAxis - 1) * 255;
                    var unicolour = new Unicolour(ColourSpace.Rgb255, To255(r), To255(g), To255(b));
                    GenerateSphere(unicolour);
                }
            }
        }

        ArrangeSpheres();
    }
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // left mouse
        {
            currentIndex = currentIndex == LastIndex ? 0 : currentIndex + 1;
        }
        else if (Input.GetMouseButtonDown(1)) // right mouse
        {
            currentIndex = currentIndex == 0 ? LastIndex : currentIndex - 1;
        }
        
        ArrangeSpheres();
    }

    private void GenerateSphere(Unicolour unicolour)
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        var sphereRenderer = sphere.GetComponent<Renderer>();
        sphereRenderer.material = sphereMaterial;
        sphereRenderer.material.color = ToUnityColor(unicolour);
        sphereLookup.Add(sphere, unicolour);
    }

    private void ArrangeSpheres()
    {
        colourSpaceTextComponent.text = CurrentSpace.ToString();
        var limits = colourSpaces[CurrentSpace];    
        
        foreach (var (sphere, unicolour) in sphereLookup)
        {
            var (first, second, third) = unicolour.GetRepresentation(CurrentSpace).Triplet;
            var normalisedFirst = (float)limits.first.Normalise(first);
            var normalisedSecond = (float)limits.second.Normalise(second);
            var normalisedThird = (float)limits.third.Normalise(third);

            Vector3 position = new Vector3(normalisedFirst, normalisedSecond, normalisedThird) * AxisScale;
            sphere.transform.position = Vector3.Lerp(sphere.transform.position, position, Time.deltaTime);
        }
    }
    
    private static void DrawAxis(Vector3 start, Vector3 end, Color color)
    {
        GameObject lineObject = new GameObject("Line");
        LineRenderer lineRenderer = lineObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));

        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.positionCount = 2;

        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }

    private static Color ToUnityColor(Unicolour unicolour)
    {
        return new Color((float)unicolour.Rgb.R, (float)unicolour.Rgb.G, (float)unicolour.Rgb.B);
    }

    private static Dictionary<ColourSpace, (Limits first, Limits second, Limits third)> GetColourSpaces()
    {
        // precalculated min & max values of sRGB in all colour spaces
        return new()
        {
            { ColourSpace.Rgb, (new(0, 1), new(0, 1), new(0, 1)) },
            { ColourSpace.RgbLinear, (new(0, 1), new(0, 1), new(0, 1)) },
            { ColourSpace.Hsb, (new(0, 360), new(0, 1), new(0, 1)) },
            { ColourSpace.Hsl, (new(0, 360), new(0, 1), new(0, 1)) },
            { ColourSpace.Hwb, (new(0, 360), new(0, 1), new(0, 1)) },
            { ColourSpace.Hsi, (new(0, 360), new(0, 1), new(0, 1)) },
            { ColourSpace.Xyz, (new(0, 0.9505), new(0, 1), new(0, 1.0888)) },
            { ColourSpace.Xyy, (new(0.15, 0.64), new(0.06, 0.6), new(0, 1)) },
            { ColourSpace.Wxy, (new(-566.4, 700), new(0, 1), new(0, 1)) },
            { ColourSpace.Lab, (new(0, 100), new(-86.2, 98.2), new(-108, 94.5)) },
            { ColourSpace.Lchab, (new(0, 100), new(0, 134), new(0, 360)) },
            { ColourSpace.Luv, (new(0, 100), new(-83.1, 175), new(-134, 107)) },
            { ColourSpace.Lchuv, (new(0, 100), new(0, 179), new(0, 360)) },
            { ColourSpace.Hsluv, (new(0, 360), new(0, 100), new(0, 100)) },
            { ColourSpace.Hpluv, (new(0, 360), new(0, 1780), new(0, 100)) },
            { ColourSpace.Ypbpr, (new(0, 1), new(-0.5, 0.5), new(-0.5, 0.5)) },
            { ColourSpace.Ycbcr, (new(0, 255), new(0, 255), new(0, 255)) },
            { ColourSpace.Ycgco, (new(0, 1), new(-0.5, 0.5), new(-0.5, 0.5)) },
            { ColourSpace.Yuv, (new(0, 1), new(-0.436, 0.436), new(-0.614, 0.614)) },
            { ColourSpace.Yiq, (new(0, 1), new(-0.595, 0.595), new(-0.522, 0.522)) },
            { ColourSpace.Ydbdr, (new(0, 1), new(-1.33, 1.33), new(-1.33, 1.33)) },
            { ColourSpace.Tsl, (new(0, 360), new(0, 1), new(0, 1)) },
            { ColourSpace.Xyb, (new(-0.0154, 0.0281), new(0, 0.845), new(-0.278, 0.388)) },
            { ColourSpace.Lms, (new(0, 1), new(0, 1), new(0, 1)) },
            { ColourSpace.Ipt, (new(0, 1), new(-0.453, 0.662), new(-0.748, 0.651)) },
            { ColourSpace.Ictcp, (new(0, 0.581), new(-0.281, 0.277), new(-0.161, 0.279)) },
            { ColourSpace.Jzazbz, (new(0, 0.222), new(-0.109, 0.130), new(-0.186, 0.135)) },
            { ColourSpace.Jzczhz, (new(0, 0.222), new(0, 0.190), new(0, 360)) },
            { ColourSpace.Oklab, (new(0, 1), new(-0.234, 0.276), new(-0.312, 0.199)) },
            { ColourSpace.Oklch, (new(0, 1), new(0, 0.322), new(0, 360)) },
            { ColourSpace.Okhsv, (new(0, 360), new(0, 1), new(0, 1)) },
            { ColourSpace.Okhsl, (new(0, 360), new(0, 1), new(0, 1)) },
            { ColourSpace.Okhwb, (new(0, 360), new(0, 1), new(0, 1)) },
            { ColourSpace.Oklrab, (new(0, 1), new(-0.234, 0.276), new(-0.312, 0.199)) },
            { ColourSpace.Oklrch, (new(0, 1), new(0, 0.322), new(0, 360)) },
            { ColourSpace.Cam02, (new(0, 100), new(-32.2, 41.5), new(-39.2, 35.6)) },
            { ColourSpace.Cam16, (new(0, 100), new(-35.5, 43.2), new(-37.9, 33)) },
            { ColourSpace.Hct, (new(0, 360), new(0, 113), new(0, 100)) },
            { ColourSpace.Munsell, (new(0, 360), new(0, 10), new(0, 26)) }
        };
    }

    private record Limits(double Min, double Max)
    {
        internal double Min { get; } = Min;
        internal double Max { get; } = Max;
        
        internal double Normalise(double value) => (value - Min) / (Max - Min);
    }
}

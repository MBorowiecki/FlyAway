using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class GroundGeneration : MonoBehaviour
{
    public SpriteShapeController shape;
    public int startIndex = 1;
    public int endIndex = 2;
    public int numberOfPoints = 150;
    public int distanceBetweenPoints = 5;
    public float scale = 2000f;
    public float minimumRunway = 200f;
    public float heightFactor = 5f;
    public float planePositionRegenerateBuffer = 1000f;
    private Transform planeTransform;
    // Start is called before the first frame update
    void Start()
    {
        shape = GetComponent<SpriteShapeController>();

        GenerateSpriteShape();
    }

    void Update()
    {
        if (!planeTransform)
        {
            planeTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        }
        else
        {
            if (planeTransform.position.x >= shape.spline.GetPosition(shape.spline.GetPointCount() - 2).x - planePositionRegenerateBuffer)
            {
                GenerateSpriteShape();
            }
        }
    }

    void GenerateSpriteShape()
    {
        shape.spline.SetPosition(endIndex, shape.spline.GetPosition(endIndex) + Vector3.right * scale);
        shape.spline.SetPosition(endIndex + 1, new Vector3(shape.spline.GetPosition(endIndex).x, shape.spline.GetPosition(endIndex + 1).y));

        numberOfPoints = (int)((shape.spline.GetPosition(endIndex).x - shape.spline.GetPosition(startIndex).x) / distanceBetweenPoints);

        for (int i = endIndex - 2; i < numberOfPoints - 1; i++)
        {
            Debug.Log(i);
            float xPos = shape.spline.GetPosition(i + startIndex).x + distanceBetweenPoints;
            float yPos = i > minimumRunway / distanceBetweenPoints ? Mathf.PerlinNoise(i * Random.Range(0f, 500f), 0) * heightFactor : 0f;
            shape.spline.InsertPointAt(i + 2, new Vector3(xPos, yPos, 0));
        }

        for (int i = endIndex - 1; i < numberOfPoints - 1 + startIndex; i++)
        {
            shape.spline.SetTangentMode(i, ShapeTangentMode.Continuous);
            shape.spline.SetLeftTangent(i, new Vector3(-distanceBetweenPoints / 2, 0, 0));
            shape.spline.SetRightTangent(i, new Vector3(distanceBetweenPoints / 2, 0, 0));
        }

        endIndex = shape.spline.GetPointCount() - 2;
    }
}

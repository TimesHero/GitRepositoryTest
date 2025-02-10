using UnityEngine;

public class RotaryMenu : MonoBehaviour
{
    public int bulletType;  // 0, 1, or 2 for different bullet types
    public bool right;  // Determines if the rotation should happen clockwise (right)
    public bool left;   // Determines if the rotation should happen counterclockwise (left)
    private float currentRotation = 0f; // Tracks the total rotation applied
    public float rotationSpeed = 200f; // The amount of rotation applied per frame (degrees per second)
    float rotationLocation;

    void Start()
    {
    }

    void Update()
    {
        if (bulletType==0)
        {
            rotationLocation=0;
        }
        if (bulletType==1)
        {
            rotationLocation=120;
        }
        if (bulletType==2)
        {
            rotationLocation=240;
        }

        if (right==true)
        {
            rotationSpeed = 30f;
        }
        if (left==true)
        {
            rotationSpeed = -30f;
        }
        if (currentRotation!=rotationLocation)
        {
            transform.Rotate(0f,0f,rotationSpeed);
            currentRotation+=rotationSpeed;
            print(currentRotation);
            if (currentRotation==360)
            {
                currentRotation=0;
            }
            if (currentRotation==-30)
            {
                currentRotation=330;
            }
        }
    }
}

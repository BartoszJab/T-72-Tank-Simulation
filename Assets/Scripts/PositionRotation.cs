using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionRotation
{
    public Vector3 position;
    public Quaternion rotation;

    public PositionRotation(Vector3 position, Quaternion rotation) {
        this.position = position;
        this.rotation = rotation;
    }

    public PositionRotation(Vector3 position) {
        this.position = position;
    }

    public PositionRotation(Quaternion rotation) {
        this.rotation = rotation;
    }
}

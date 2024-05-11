using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityRandom = UnityEngine.Random;

namespace PIzafiyet
{
    public class GettingColor : MonoBehaviour
    {
        [SerializeField] private Texture2D heightmap;
        [SerializeField] private Vector3 size = new(100, 10, 100);

        void Update()
        {
            int x = Mathf.FloorToInt(transform.position.x / size.x * heightmap.width);
            int z = Mathf.FloorToInt(transform.position.z / size.z * heightmap.height);
            Vector3 pos = transform.position;
            pos.y = heightmap.GetPixel(x, z).grayscale * size.y;
            transform.position = pos;
        }
    }
}
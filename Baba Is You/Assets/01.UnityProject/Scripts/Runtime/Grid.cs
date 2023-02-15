using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    private int width_;
    private int height_;

    private int [,] gridArray_;

    public Grid(int width, int height)
    {
        width_ = width;
        height_ = height;

        gridArray_ = new int[height, width];
    }
}

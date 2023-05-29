using System;
using System.Collections.Generic;
using System.Globalization;
using Unity.Mathematics;
using UnityEngine;

public struct Hex3 : IEquatable<Hex3>
{
    /// <summary>
    ///   <para>X component of the hex.</para>
    /// </summary>
    public int x;
    
    /// <summary>
    ///   <para>Y component of the hex.</para>
    /// </summary>
    public int y;
    
    /// <summary>
    ///   <para>Z component of the hex.</para>
    /// </summary>
    public int z;
    
    private static readonly Hex3 _zeroHex = new Hex3(0, 0, 0);
    private static readonly Hex3 _upLeftHex = new Hex3(0, 1, -1); 
    private static readonly Hex3 _upRightHex = new Hex3(1, 0, -1);
    private static readonly Hex3 _downLeftHex = new Hex3(-1, 0, 1);
    private static readonly Hex3 _downRightHex = new Hex3(0, -1, 1);
    private static readonly Hex3 _leftHex = new Hex3(-1, 1, 0);
    private static readonly Hex3 _rightHex = new Hex3(1, -1, 0);

    /// <summary>
    ///   <para>Creates a new hex with given x, y, z components.</para>
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    public Hex3(int x, int y, int z)
    {
        if (x+y+z != 0)
        {
            throw new ArgumentException("Sum of components are not equal to 0");
        }
        this.x = x;
        this.y = y;
        this.z = z;
    }
    
    /// <summary>
    ///   <para>Creates a new hex with given x, y components and sets z to x+y+z be 0.</para>
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public Hex3(int x, int y)
    {
        this.x = x;
        this.y = y;
        this.z = 0-x-y;
    }
    
    /// <summary>
    ///   <para>Shorthand for writing Hex3(0, 0, 0).</para>
    /// </summary>
    public static Hex3 zero => _zeroHex;
    
    /// <summary>
    ///   <para>Shorthand for writing Hex3(0, 1, -1).</para>
    /// </summary>
    public static Hex3 upLeft => _upLeftHex;
    
    /// <summary>
    ///   <para>Shorthand for writing Hex3(1, 0, -1).</para>
    /// </summary>
    public static Hex3 upRight => _upRightHex;
    
    /// <summary>
    ///   <para>Shorthand for writing Hex3(-1, 0, 1).</para>
    /// </summary>
    public static Hex3 downLeft => _downLeftHex;
    
    /// <summary>
    ///   <para>Shorthand for writing Hex3(0, -1, 1).</para>
    /// </summary>
    public static Hex3 downRight => _downRightHex;
    
    /// <summary>
    ///   <para>Shorthand for writing Hex3(-1, 1, 0).</para>
    /// </summary>
    public static Hex3 left => _leftHex;
    
    /// <summary>
    ///   <para>Shorthand for writing Hex3(1, -1, 0).</para>
    /// </summary>
    public static Hex3 right => _rightHex;
    
    /// <summary>
    ///   <para>Get all directions of Hex3.zero.</para>
    /// </summary>
    public static Hex3[] directions
    {
        get
        {
            return new[]
            {
                _rightHex, _upRightHex, _upLeftHex, 
                _leftHex, _downLeftHex, _downRightHex
            };
        }
    }
    
    /// <summary>
    ///   <para>Get all corners of Hex3.zero.</para>
    /// </summary>
    public static Vector2[] corners
    {
        get
        {
            return new[]
            {
                new Vector2(Mathf.Sqrt(3) / 2f,1/2f), // Up Right
                new Vector2(0,1),                      // Up
                new Vector2(-Mathf.Sqrt(3)/2f,1/2f),  // Up Left
                new Vector2(-Mathf.Sqrt(3)/2f,-1/2f), // Down Left
                new Vector2(0,-1),                     // Down
                new Vector2(Mathf.Sqrt(3)/2f,-1/2f)   // Down Right
            };
        }
    }

    /// <summary>
    ///   <para>Converts Hex3 into Vector2.</para>
    /// </summary>
    /// <param name="value"></param>
    /// <param name="scale"></param>
    /// <param name="orientation"></param>
    public static Vector2 ToVector2(Hex3 value, float scale, Orientation orientation = Orientation.Pointy)
    {
        var x = orientation == Orientation.Pointy ? 
            scale * (Mathf.Sqrt(3) * value.x + Mathf.Sqrt(3) / 2f * value.z):
            scale * (1.5f * value.x);
        var y = orientation == Orientation.Pointy ? 
            scale * (1.5f * value.z):
            scale * (Mathf.Sqrt(3) / 2f * value.x + Mathf.Sqrt(3) * value.z);
        return new Vector2(x, y);
    }
    
    public Vector2 ToVector2(float scale, Orientation orientation = Orientation.Pointy)
    {
        var x = orientation == Orientation.Pointy ? 
            scale * (Mathf.Sqrt(3) * this.x + Mathf.Sqrt(3) / 2f * this.z):
            scale * (1.5f * this.x);
        var y = orientation == Orientation.Pointy ? 
            scale * (1.5f * this.z):
            scale * (Mathf.Sqrt(3) / 2f * this.x + Mathf.Sqrt(3) * this.z);
        return new Vector2(x, y);
    }

    /// <summary>
    ///   <para>Converts Hex3 into Vector3 on XY0 format.</para>
    /// </summary>
    /// <param name="value"></param>
    /// <param name="scale"></param>
    /// <param name="orientation"></param>
    public static Vector3 ToVector3XY(Hex3 value, float scale, Orientation orientation = Orientation.Pointy)
    {
        var pos = ToVector2(value, scale, orientation);
        return new Vector3(pos.x, pos.y, 0);
    }
    
    public Vector3 ToVector3XY(float scale, Orientation orientation = Orientation.Pointy)
    {
        var pos = ToVector2(new Hex3(this.x,this.y,this.z), scale, orientation);
        return new Vector3(pos.x, pos.y, 0);
    }

    /// <summary>
    ///   <para>Converts Hex3 into Vector3 on X0Z format.</para>
    /// </summary>
    /// <param name="value"></param>
    /// <param name="scale"></param>
    /// <param name="orientation"></param>
    public static Vector3 ToVector3XZ(Hex3 value, float scale, Orientation orientation = Orientation.Pointy)
    {
        var pos = ToVector2(value, scale, orientation);
        return new Vector3(pos.x, 0, pos.y);
    }
    
    public Vector3 ToVector3XZ(float scale, Orientation orientation = Orientation.Pointy)
    {
        var pos = ToVector2(new Hex3(this.x,this.y,this.z), scale, orientation);
        return new Vector3(pos.x, 0, pos.y);
    }

    /// <summary>
    ///   <para>Converts Hex3 into Vector3 on 0YZ format.</para>
    /// </summary>
    /// <param name="value"></param>
    /// <param name="scale"></param>
    /// <param name="orientation"></param>
    public static Vector3 ToVector3YZ(Hex3 value, float scale, Orientation orientation = Orientation.Pointy)
    {
        var pos = ToVector2(value, scale, orientation);
        return new Vector3(0, pos.x, pos.y);
    }
    
    public Vector3 ToVector3YZ(float scale, Orientation orientation = Orientation.Pointy)
    {
        var pos = ToVector2(new Hex3(this.x,this.y,this.z), scale, orientation);
        return new Vector3(0, pos.x, pos.y);
    }
    
    /// <summary>
    ///   <para>Converts Vector3 on XY0 format into Hex3.</para>
    /// </summary>
    /// <param name="value"></param>
    /// <param name="scale"></param>
    public static Hex3 XYToHex3(Vector3 value, float scale)
    {
        var x = (Mathf.Sqrt(3) / 3f * value.x - value.y / 3f)/scale;
        var y = -(Mathf.Sqrt(3)/3f * value.x + value.y / 3f)/scale;
        var z = 2f / 3f * value.y/scale;

        var rx = Mathf.RoundToInt(x);
        var ry = Mathf.RoundToInt(y);
        var rz = Mathf.RoundToInt(z);

        var xDiff = Mathf.Abs(rx - x);
        var yDiff = Mathf.Abs(ry - y);
        var zDiff = Mathf.Abs(rz - z);

        if (xDiff > yDiff && xDiff > zDiff)
        {
            rx = -ry - rz;
        }
        else if (yDiff > zDiff)
        {
            ry = -rx - rz;
        }
        else
        {
            rz = -rx - ry;
        }

        return new Hex3(rx,ry,rz);
    }

    /// <summary>
    ///   <para>Converts Vector3 on X0Z format into Hex3.</para>
    /// </summary>
    /// <param name="value"></param>
    /// <param name="scale"></param>
    public static Hex3 XZToHex3(Vector3 value, float scale)
    {
        var x = (Mathf.Sqrt(3) / 3f * value.x - value.z / 3f)/scale;
        var y = -(Mathf.Sqrt(3)/3f * value.x + value.z / 3f)/scale;
        var z = 2f / 3f * value.z/scale;

        var rx = Mathf.RoundToInt(x);
        var ry = Mathf.RoundToInt(y);
        var rz = Mathf.RoundToInt(z);

        var xDiff = Mathf.Abs(rx - x);
        var yDiff = Mathf.Abs(ry - y);
        var zDiff = Mathf.Abs(rz - z);

        if (xDiff > yDiff && xDiff > zDiff)
        {
            rx = -ry - rz;
        }
        else if (yDiff > zDiff)
        {
            ry = -rx - rz;
        }
        else
        {
            rz = -rx - ry;
        }

        return new Hex3(rx,ry,rz);
    }
    /// <summary>
    ///   <para>Converts Vector3 on 0YZ format into Hex3.</para>
    /// </summary>
    /// <param name="value"></param>
    /// <param name="scale"></param>
    public static Hex3 YZToHex3(Vector3 value, float scale)
    {
        var x = (Mathf.Sqrt(3) / 3f * value.y - value.z / 3f)/scale;
        var y = -(Mathf.Sqrt(3)/3f * value.y + value.z / 3f)/scale;
        var z = 2f / 3f * value.z/scale;

        var rx = Mathf.RoundToInt(x);
        var ry = Mathf.RoundToInt(y);
        var rz = Mathf.RoundToInt(z);

        var xDiff = Mathf.Abs(rx - x);
        var yDiff = Mathf.Abs(ry - y);
        var zDiff = Mathf.Abs(rz - z);

        if (xDiff > yDiff && xDiff > zDiff)
        {
            rx = -ry - rz;
        }
        else if (yDiff > zDiff)
        {
            ry = -rx - rz;
        }
        else
        {
            rz = -rx - ry;
        }

        return new Hex3(rx,ry,rz);
    }
    
    /// <summary>
    ///   <para>Converts a offset coordinate to a cube coordinate.</para>
    /// </summary>
    /// <param name="value">Offset coordinate</param>
    public static Hex3 OffsetToCubeCoord(Vector2Int value)
    {
        var x = value.x - (value.y - (value.y & 1)) / 2;
        var z = value.y;
        var y = -x - z;
        
        return new Hex3(x, y, z);
    }
    
    /// <summary>
    ///   <para>Converts a cube coordinate to a offset coordinate.</para>
    /// </summary>
    /// <param name="value">Cube coordinate</param>
    public static Vector2Int CubeToOffsetCoord(Hex3 value)
    {
        var col = value.x + (value.z - (value.z & 1)) / 2;
        var row = value.z;
            
        return new Vector2Int(col, row);
    }
    
    /// <summary>
    ///   <para>Returns the distance between a and b.</para>
    /// </summary>
    /// <param name="a">Center hex</param>
    /// <param name="b">Ring radius</param>
    public static float Distance(Hex3 a, Hex3 b)
    {
        return (Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y) + Mathf.Abs(a.z - b.z)) / 2f;
    }
    
    /// <summary>
    ///   <para>Returns neighbor hexes of center hex.</para>
    /// </summary>
    /// <param name="center"></param>
    public static Hex3[] Neighbors(Hex3 center)
    {
        return new[]
        {
            center+_rightHex, center+_upRightHex, center+_upLeftHex, 
            center+_leftHex, center+_downLeftHex, center+_downRightHex
        };
    }
    
    /// <summary>
    ///   <para>Returns neighbor hexes of center hex.</para>
    /// </summary>
    /// <param name="center"></param>
    public Hex3[] Neighbors()
    {
        var center = new Hex3(x, y, z);
        
        return new[]
        {
            center+_rightHex, center+_upRightHex, center+_upLeftHex, 
            center+_leftHex, center+_downLeftHex, center+_downRightHex
        };
    }
    
    public static Hex3 GetNeighbor(Hex3 center, int index)
    {
        return index switch
        {
            0 => center + new Hex3(1,-1,0),
            1 => center + new Hex3(1,0,-1),
            2 => center + new Hex3(0,1,-1),
            3 => center + new Hex3(-1,1,0),
            4 => center + new Hex3(-1,0,1),
            5 => center + new Hex3(0,-1,1),
            _ => center
        };
    }
    
    public static Hex3 GetDirection(int index)
    {
        return index switch
        {
            0 => new Hex3(1,-1,0),
            1 => new Hex3(1,0,-1),
            2 => new Hex3(0,1,-1),
            3 => new Hex3(-1,1,0),
            4 => new Hex3(-1,0,1),
            5 => new Hex3(0,-1,1),
            _ => Hex3.zero
        };
    }

    /// <summary>
    ///   <para>Returns neighbor hexes of center hex in radius of radius center.</para>
    /// </summary>
    /// <param name="center"></param>
    /// <param name="radiusCenter"></param>
    /// <param name="radius"></param>
    public static List<Hex3> NeighborsInRadius(Hex3 center, Hex3 radiusCenter, int radius)
    {
        var neighbors = new List<Hex3>();
        
        for (int i = 0; i < directions.Length; i++)
        {
            var neighbor = directions[i] + center;
            if (Distance(radiusCenter,neighbor) < radius) neighbors.Add(neighbor);
        }

        return neighbors;
    }
    
    /// <summary>
    ///   <para>Returns rotated clockwise hex around of origin.</para>
    /// </summary>
    /// <param name="value"></param>
    public static Hex3 RotateClockwise(Hex3 value)
    {
        return new Hex3(-value.y,-value.z,-value.x);
    }
    
    /// <summary>
    ///   <para>Returns rotated clockwise hex around of origin.</para>
    /// </summary>
    /// <param name="value"></param>
    public static Hex3 RotateCounterClockwise(Hex3 value)
    {
        return new Hex3(-value.z,-value.x,-value.y);
    }

    /// <summary>
    ///   <para>Returns hex value for wraparound map of radius.</para>
    /// </summary>
    /// <param name="value"></param>
    /// <param name="radius"></param>
    public static Hex3 Wraparound(Hex3 value, int radius)
    {
        var cornerHex = new Hex3(2 * radius + 1, -radius, -radius - 1);
        
        for (int j = 0; j < 6; j++)
        {
            var distance = Distance(cornerHex, value);
            
            if (distance <= radius)
            {
                return value - cornerHex;
            }

            cornerHex = RotateClockwise(cornerHex);
        }

        return value;
    }

    /// <summary>
    ///   <para>Returns hexes only on the ring of center hex.</para>
    /// </summary>
    /// <param name="value">Center hex</param>
    /// <param name="radius">Ring radius</param>
    public static List<Hex3> Ring(Hex3 value, int radius)
    {
        var ring = new List<Hex3>();
        var cornerPos = value + Hex3.directions[4]*radius;
        
        for (int i = 0; i < 6; i++)
        {
            for (int k = 0; k < radius; k++)
            {
                ring.Add(cornerPos);
                cornerPos += Hex3.directions[i];
            }
        }

        return ring;
    }
    
    /// <summary>
    ///   <para>Returns hexes only on the pie of center hex.</para>
    /// </summary>
    /// <param name="value">Center hex</param>
    /// <param name="radius">Pie radius</param>
    public static List<Hex3> Pie(Hex3 value, int radius, Direction direction)
    {
        var pie = new List<Hex3>();

        for (int i = 0; i < radius; i++)
        {
            var cornerPos = value + Hex3.directions[(int) direction] * (i + 1);
            
            for (int k = 0; k < i+1; k++)
            {
                pie.Add(cornerPos);
                if (k == i) break;
                cornerPos += Hex3.directions[(int)(direction + 2) % 6];
            }
        }

        return pie;
    }

    /// <summary>
    ///   <para>Returns all hexes that are inside given radius from the center hex.</para>
    /// </summary>
    /// <param name="value">Center hex</param>
    /// <param name="radius">Spiral Ring radius</param>
    public static List<Hex3> SpiralRing(Hex3 value, int radius)
    {
        List<Hex3> spiralRing = new List<Hex3>{value};
        
        for (int x = -radius; x <= radius; x++)
        {
            var min = x <= 0 ? -radius - x : -radius;
            var max = x <= 0 ? radius : radius - x;
        
            for (int y = min; y <= max; y++)
            {
                var hex = new Hex3(x, y, 0 - (x + y));
                spiralRing.Add(hex+value);
            }
        }
        
        return spiralRing;
    }

    /// <summary>
    ///   <para>Returns hex count of the Spiral Ring.</para>
    /// </summary>
    /// <param name="radius">Spiral Ring radius</param>
    public static int SpiralRingCount(int radius)
    {
        return 1 + 3 * radius * (radius + 1);
    }
    
    /// <summary>
    ///   <para>Returns all hexes that are inside given radius from the center hex.</para>
    /// </summary>
    /// <param name="value">Center hex</param>
    /// <param name="radius">Retrieved hex position radius</param>
    /// <param name="action">Action</param>
    public static void ForEachHex3(Hex3 value, int radius, Action<Hex3> action)
    {
        for (int x = -radius; x <= radius; x++)
        {
            var min = x <= 0 ? -radius - x : -radius;
            var max = x <= 0 ? radius : radius - x;
        
            for (int y = min; y <= max; y++)
            {
                var hex = new Hex3(x, y, 0 - (x + y)) + value;
                action?.Invoke(hex);
            }
        }
    }
    
    /// <summary>
    ///   <para>Returns hexes on the line from current hex to target hex using Bresenham algorithm.</para>
    /// </summary>
    /// <param name="current">Line start hex point</param>
    /// <param name="target">Line end hex point</param>
    public static List<Hex3> Line(Hex3 current, Hex3 target)
    {
        var line = new List<Hex3>();
        
        var p0 = current;
        var p1 = target;

        int dx = Mathf.Abs(p1.x-p0.x), sx = p0.x<p1.x ? 1 : -1;
        int dy = Mathf.Abs(p1.y-p0.y), sy = p0.y<p1.y ? 1 : -1; 
        int dz = Mathf.Abs(p1.z-p0.z), sz = p0.z<p1.z ? 1 : -1; 
        int dm = Mathf.Max(dx,dy,dz), i = dm;
        
        p1.x = p1.y = p1.z = dm/2;
 
        while(true) 
        {
            if (p0.x+p0.y+p0.z == 0) line.Add(p0);
            if (i-- == 0) return line;
            
            p1.x -= dx; if (p1.x < 0) { p1.x += dm; p0.x += sx; } 
            p1.y -= dy; if (p1.y < 0) { p1.y += dm; p0.y += sy; } 
            p1.z -= dz; if (p1.z < 0) { p1.z += dm; p0.z += sz; } 
        }
    }
    
    /// <summary>
    ///   <para>Returns hexes on the thick line from current hex to target hex using Bresenham algorithm.</para>
    /// </summary>
    /// <param name="current">Line start hex point</param>
    /// <param name="target">Line end hex point</param>
    /// <param name="wd">Line width</param>
    public static HashSet<Hex3> ThickLine(Hex3 current, Hex3 target, float wd)
    {
        var line = new HashSet<Hex3>();
        
        var startPoint = CubeToOffsetCoord(current);
        var endPoint = CubeToOffsetCoord(target);
        
        var x0 = startPoint.x; var x1 = endPoint.x;
        var y0 = startPoint.y; var y1 = endPoint.y;
        
        int dx = Mathf.Abs(x1-x0), sx = x0 < x1 ? 1 : -1; 
        int dy = Mathf.Abs(y1-y0), sy = y0 < y1 ? 1 : -1;

        var err = dx-dy;
        var ed = dx+dy == 0 ? 1 : Mathf.Sqrt((float)dx*dx+(float)dy*dy);

        wd = (wd + 1) / 2;
        while (true) 
        {
            line.Add(OffsetToCubeCoord(new Vector2Int(x0, y0)));
            var e2 = err; var x2 = x0;
            if (2*e2 >= -dx) 
            {
                int y2;
                for (e2 += dy, y2 = y0; e2 < ed*wd && (y1 != y2 || dx > dy); e2 += dx)
                {
                    y2 += sy;
                    line.Add(OffsetToCubeCoord(new Vector2Int(x0, y2)));
                }
                if (x0 == x1) return line;
                e2 = err; err -= dy; x0 += sx; 
            } 
            if (2*e2 <= dy) 
            {
                for (e2 = dx-e2; e2 < ed*wd && (x1 != x2 || dx < dy); e2 += dy)
                {
                    x2 += sx;
                    line.Add(OffsetToCubeCoord(new Vector2Int(x2, y0)));
                }
                if (y0 == y1) return line;
                err += dx; y0 += sy; 
            }
        }
    }
    
    /// <summary>
    ///   <para>Returns corners of the hex in Vector2.</para>
    /// </summary>
    /// <param name="value"></param>
    /// <param name="scale"></param>
    public static Vector2[] CornersInVector2(Hex3 value,float scale)
    {
        return new[]
        {
            corners[0] + ToVector2(value, scale),
            corners[1] + ToVector2(value, scale),
            corners[2] + ToVector2(value, scale),
            corners[3] + ToVector2(value, scale),
            corners[4] + ToVector2(value, scale),
            corners[5] + ToVector2(value, scale)
        };
    }
    
    public static bool BoundsCheck(Hex3 value, int size)
    {
        if (value.x < -size || value.x > size)
        {
            return false;
        }
        if (value.y < -size || value.y > size)
        {
            return false;
        }
        return value.z >= -size && value.z <= size;
    }
    
    public static Hex3 SmallToBigHex(Hex3 smallHex, int radius) 
    {
        var area = 3f * (radius * radius) + 3f * radius + 1f;
        var shift = 3f * radius + 2f;
        var xh = Mathf.FloorToInt((smallHex.y + shift * smallHex.x) / area);
        var yh = Mathf.FloorToInt((smallHex.z + shift * smallHex.y) / area);
        var zh = Mathf.FloorToInt((smallHex.x + shift * smallHex.z) / area);
        
        var i = Mathf.FloorToInt((1 + xh - yh) / 3f);
        var j = Mathf.FloorToInt((1 + yh - zh) / 3f);
        var k = Mathf.FloorToInt((1 + zh - xh) / 3f);

        return new Hex3(i, j, k);
    }

    public static int HexMod(Hex3 smallHex, int radius)
    {
        var area = 3f * (radius * radius) + 3f * radius + 1f;
        var shift = 3f * radius + 2f;
        var v = smallHex.y + shift * smallHex.x;

        return (int) ((v % area + area) % area);
    }

    public static Hex3 InverseHexMod(int m, int radius)
    {
        var shift = 3f * radius + 2f;
        var ms = (int) ((m + radius) / shift);
        var mcs = (int) ((m + 2 * radius) / (shift - 1));
        var x = ms * (radius + 1) + mcs * -radius;
        var y = m + ms * (-2 * radius - 1) + mcs * (-radius - 1);
        var z = -m + ms * radius + mcs * (2 * radius + 1);
        return new Hex3(x, y, z);
    }

    /// <summary>
    ///   <para>Returns true if the given hex is exactly equal to this hex.</para>
    /// </summary>
    /// <param name="other"></param>
    public override bool Equals(object other)
    {
        return other is Hex3 other1 && this.Equals(other1);
    }

    public bool Equals(Hex3 other)
    {
        return x == other.x && y == other.y && z == other.z;
    }

    public override int GetHashCode()
    {
        return x.GetHashCode() + y.GetHashCode() + z.GetHashCode();
    }

    public static Hex3 operator + (Hex3 a, Hex3 b)
    {
        return new Hex3(a.x + b.x, a.y + b.y, a.z + b.z);
    }
    public static Hex3 operator - (Hex3 a, Hex3 b)
    {
        return new Hex3(a.x - b.x, a.y - b.y, a.z - b.z);
    }
    public static Hex3 operator - (Hex3 a)
    {
        return new Hex3(-a.x, -a.y, -a.z);
    }
    public static Hex3 operator * (Hex3 a, float d)
    {
        return new Hex3((int) (a.x * d), (int) (a.y * d), (int) (a.z * d));
    }
    public static Hex3 operator * (float d, Hex3 a)
    {
        return new Hex3((int) (a.x * d), (int) (a.y * d), (int) (a.z * d));
    }
    public static Hex3 operator / (Hex3 a, float d)
    {
        return new Hex3((int) (a.x / d), (int) (a.y / d), (int) (a.z / d));
    }
    public static bool operator == (Hex3 lhs, Hex3 rhs)
    {
        return !(lhs.x != rhs.x || lhs.y != rhs.y || lhs.z != rhs.z);
    }
    public static bool operator != (Hex3 lhs, Hex3 rhs)
    {
        return !(lhs == rhs);
    }
    public static implicit operator int3(Hex3 hex)
    {
        return new int3(hex.x, hex.y, hex.z);
    }
    public static implicit operator Vector3Int(Hex3 hex)
    {
        return new Vector3Int(hex.x, hex.y, hex.z);
    }
    public static implicit operator Hex3(int3 hex)
    {
        return new Hex3(hex.x, hex.y, hex.z);
    }
    public static implicit operator Hex3(Vector3Int hex)
    {
        return new Hex3(hex.x, hex.y, hex.z);
    }

    /// <summary>
    ///   <para>Returns a formatted string for this vector.</para>
    /// </summary>
    public override string ToString() => ToString(null, null);

    /// <summary>
    ///   <para>Returns a formatted string for this vector.</para>
    /// </summary>
    /// <param name="format">A numeric format string.</param>
    public string ToString(string format) => ToString(format, null);
    
    /// <summary>
    ///   <para>Returns a formatted string for this hex.</para>
    /// </summary>
    /// <param name="format">A numeric format string.</param>
    /// <param name="formatProvider">An object that specifies culture-specific formatting.</param>
    public string ToString(string format, IFormatProvider formatProvider)
    {
        if (string.IsNullOrEmpty(format))
            format = "F2";
        formatProvider ??= CultureInfo.InvariantCulture.NumberFormat;
        return
            $"({x.ToString(format, formatProvider)}, {y.ToString(format, formatProvider)}, {z.ToString(format, formatProvider)})";
    }
    
    public enum Orientation
    {
        Flat,
        Pointy
    }

    public enum Direction
    {
        Right,
        UpRight,
        UpLeft,
        Left,
        DownLeft,
        DownRight,
    }
}
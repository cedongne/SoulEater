using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

public class MeshGenerator : MonoBehaviour
{
    public GameObject portalObject;
    public float yCorrect;
    public float frontCorrect;

    public bool isBoss;

    public SquareGrid squareGrid;
    public MeshFilter walls;
    public MeshFilter cave;

    public bool is2D;

    List<Vector3> vertices;
    List<int> triangles;

    public int nextSpawnDir;
    public int MapNum;

    Dictionary<int, List<Triangle>> triangleDictionary = new Dictionary<int, List<Triangle>>();
    List<List<int>> outlines = new List<List<int>> ();
    HashSet<int> checkedVertices = new HashSet<int>();
    List<Vector3> wallVertices;
    public void GenerateMesh(int[,] map, float squareSize)
    {
        triangleDictionary.Clear();
        outlines.Clear();
        checkedVertices.Clear();

        squareGrid = new SquareGrid(map, squareSize);

        vertices = new List<Vector3>();
        triangles = new List<int>();

        for (int x = 0; x < squareGrid.squares.GetLength(0); x++)
        {
            for (int y = 0; y < squareGrid.squares.GetLength(1); y++)
            {
                TriangulateSquare(squareGrid.squares[x, y]);
            }
        }

        Mesh mesh = new Mesh();
        cave.mesh = mesh;

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();


        float tileAmount = 5;
        Vector2[] uvs = new Vector2[vertices.Count];
        for (int i = 0; i < vertices.Count; i++)
        {
            float percentX = Mathf.InverseLerp(-map.GetLength(0) / 2 * squareSize, map.GetLength(0) / 2 * squareSize, vertices[i].x) * tileAmount;
            float percentZ = Mathf.InverseLerp(-map.GetLength(0) / 2 * squareSize, map.GetLength(0) / 2 * squareSize, vertices[i].z) * tileAmount;
            uvs[i] = new Vector2(percentX, percentZ);
        }
        mesh.uv = uvs;

        if (!is2D)
        {
            CreateWallMesh();
            walls.tag = "Wall";
        }
    }

    public Vector3 getSpawnPos(int spawnDir)
    {
        Vector3 pos = new Vector3(0,0,0);
        for (int i = 1; i < wallVertices.Count; i++)
        {
            if (spawnDir == 0) //xMax(Right)
            {
                if (wallVertices[i].x > pos.x)
                {
                    pos = wallVertices[i];
                    pos.y = 0;
                }
            }
            else if (spawnDir == 1) //xMin(Left)
            {
                if (wallVertices[i].x < pos.x)
                {
                    pos = wallVertices[i];
                    pos.y = 0;
                }
            }
            else if(spawnDir == 3) //zMin(Down)
            {
                if (wallVertices[i].z < pos.z)
                {
                    pos = wallVertices[i];
                    pos.y = 0;
                }
            }
        }

        if (spawnDir == 0)
            pos.x -= frontCorrect;
        else if (spawnDir == 1)
            pos.x += frontCorrect;
        else if (spawnDir == 3)
            pos.z += frontCorrect;

            return transform.position + pos;
    }
    void CreatePortal(List<Vector3> wall)
    {
        int portalDir = Random.Range(0, 3);
        Vector3 portalPos = new Vector3(0, 0, 0);

        for (int i = 1; i < wall.Count; i++)
        {
            if (portalDir == 0) //xMax(Right)
            {
                if (wall[i].x > portalPos.x)
                {
                    portalPos = wall[i];
                }
            }
            else if (portalDir == 1) //xMin(Left)
            {
                if (wall[i].x < portalPos.x)
                {
                    portalPos = wall[i];
                }
            }
            else //zMax(Up)
            {
                if (wall[i].z > portalPos.z)
                {
                    portalPos = wall[i];
                }
            }
        }

        GameObject instance;
        if (portalDir == 0)
        {
            instance = Instantiate(portalObject, new Vector3(this.transform.position.x + portalPos.x - frontCorrect, yCorrect, this.transform.position.z + portalPos.z), Quaternion.Euler(0, 270, 0));
            nextSpawnDir = 1;
        }
        else if (portalDir == 1)
        {
            instance = Instantiate(portalObject, new Vector3(this.transform.position.x + portalPos.x + frontCorrect, yCorrect, this.transform.position.z + portalPos.z), Quaternion.Euler(0, 90, 0));
            nextSpawnDir = 0;
        }
        else
        {
            instance = Instantiate(portalObject, new Vector3(this.transform.position.x + portalPos.x, yCorrect, this.transform.position.z + portalPos.z - frontCorrect), Quaternion.Euler(0, 180, 0));
            nextSpawnDir = 3;
        }
        
        instance.transform.parent = this.transform;
    }

    void CreateWallMesh()
    {
        CalculateMeshOutlines();

        wallVertices = new List<Vector3>();
        List<int> wallTriangles = new List<int>();
        Mesh wallMesh = new Mesh();
        float wallHeight = 2;

        foreach(List<int> outline in outlines)
        {
            for(int i = 0; i < outline.Count-1; i++)
            {
                int startIndex = wallVertices.Count;

                wallVertices.Add(vertices[outline[i]]); //left
                wallVertices.Add(vertices[outline[i + 1]]); //right
                wallVertices.Add(vertices[outline[i]] - Vector3.up*wallHeight); //bottom left
                wallVertices.Add(vertices[outline[i + 1]] - Vector3.up * wallHeight); //bottom right

                wallTriangles.Add(startIndex + 0);
                wallTriangles.Add(startIndex + 2);
                wallTriangles.Add(startIndex + 3);

                wallTriangles.Add(startIndex + 3);
                wallTriangles.Add(startIndex + 1);
                wallTriangles.Add(startIndex + 0);
            }
        }

        wallMesh.vertices = wallVertices.ToArray();
        wallMesh.triangles = wallTriangles.ToArray();
        wallMesh.RecalculateNormals();

        if (!isBoss)
            CreatePortal(wallVertices);

        walls.mesh = wallMesh;

        Vector2[] uvs = new Vector2[wallVertices.Count];
        float tileAmount = 0.3f;
        for (int i=0; i<uvs.Length; i++)
        {
            if(wallMesh.normals[i] == new Vector3(1.0f, 0, 0) || wallMesh.normals[i] == new Vector3(-1.0f, 0, 0))
                uvs[i] = new Vector2(wallMesh.vertices[i].y * tileAmount, wallMesh.vertices[i].z * tileAmount);
            else
                uvs[i] = new Vector2(wallMesh.vertices[i].y * tileAmount, wallMesh.vertices[i].x * tileAmount);
        }
        wallMesh.uv = uvs;

        MeshCollider wallCollider = walls.gameObject.AddComponent<MeshCollider>();
        wallCollider.sharedMesh = wallMesh;
    }

    void TriangulateSquare(Square square)
    {
        switch (square.configuration)
        {
            case 0:
                break;

            // 1 points:
            case 1:
                MeshFromPoints(square.centreLeft, square.centreBottom, square.bottomLeft);
                break;
            case 2:
                MeshFromPoints(square.bottomRight, square.centreBottom, square.centreRight);
                break;
            case 4:
                MeshFromPoints(square.topRight, square.centreRight, square.centreTop);
                break;
            case 8:
                MeshFromPoints(square.topLeft, square.centreTop, square.centreLeft);
                break;

            // 2 points:
            case 3:
                MeshFromPoints(square.centreRight, square.bottomRight, square.bottomLeft, square.centreLeft);
                break;
            case 6:
                MeshFromPoints(square.centreTop, square.topRight, square.bottomRight, square.centreBottom);
                break;
            case 9:
                MeshFromPoints(square.topLeft, square.centreTop, square.centreBottom, square.bottomLeft);
                break;
            case 12:
                MeshFromPoints(square.topLeft, square.topRight, square.centreRight, square.centreLeft);
                break;
            case 5:
                MeshFromPoints(square.centreTop, square.topRight, square.centreRight, square.centreBottom, square.bottomLeft, square.centreLeft);
                break;
            case 10:
                MeshFromPoints(square.topLeft, square.centreTop, square.centreRight, square.bottomRight, square.centreBottom, square.centreLeft);
                break;

            // 3 point:
            case 7:
                MeshFromPoints(square.centreTop, square.topRight, square.bottomRight, square.bottomLeft, square.centreLeft);
                break;
            case 11:
                MeshFromPoints(square.topLeft, square.centreTop, square.centreRight, square.bottomRight, square.bottomLeft);
                break;
            case 13:
                MeshFromPoints(square.topLeft, square.topRight, square.centreRight, square.centreBottom, square.bottomLeft);
                break;
            case 14:
                MeshFromPoints(square.topLeft, square.topRight, square.bottomRight, square.centreBottom, square.centreLeft);
                break;

            // 4 point:
            case 15:
                MeshFromPoints(square.topLeft, square.topRight, square.bottomRight, square.bottomLeft);
                checkedVertices.Add(square.topLeft.vertexIndex);
                checkedVertices.Add(square.topRight.vertexIndex);
                checkedVertices.Add(square.bottomRight.vertexIndex);
                checkedVertices.Add(square.bottomLeft.vertexIndex);
                break;
        }
    }

    void MeshFromPoints(params Node[] points)
    {
        AssignVertices(points);

        if (points.Length >= 3)
            CreateTriangle(points[0], points[1], points[2]);
        if (points.Length >= 4)
            CreateTriangle(points[0], points[2], points[3]);
        if (points.Length >= 5)
            CreateTriangle(points[0], points[3], points[4]);
        if (points.Length >= 6)
            CreateTriangle(points[0], points[4], points[5]);
    }

    void AssignVertices(Node[] points)
    {
        for (int i = 0; i < points.Length; i++)
        {
            if (points[i].vertexIndex == -1)
            {
                points[i].vertexIndex = vertices.Count;
                vertices.Add(points[i].position);
            }
        }
    }

    void CreateTriangle(Node a, Node b, Node c)
    {
        triangles.Add(a.vertexIndex);
        triangles.Add(b.vertexIndex);
        triangles.Add(c.vertexIndex);

        Triangle triangle = new Triangle(a.vertexIndex, b.vertexIndex, c.vertexIndex);
        AddTriangleToDictionary(triangle.vertexIndexA, triangle);
        AddTriangleToDictionary(triangle.vertexIndexB, triangle);
        AddTriangleToDictionary(triangle.vertexIndexC, triangle);
    }

    void AddTriangleToDictionary(int vertexIndexKey, Triangle triangle)
    {
        if (triangleDictionary.ContainsKey(vertexIndexKey))
        {
            triangleDictionary[vertexIndexKey].Add(triangle);
        }
        else
        {
            List<Triangle> triangleList = new List<Triangle>();
            triangleList.Add(triangle);
            triangleDictionary.Add(vertexIndexKey, triangleList);
        }
    }
    
    void CalculateMeshOutlines()
    {
        for(int vertexIndex = 0; vertexIndex < vertices.Count; vertexIndex++)
        {
            if (!checkedVertices.Contains(vertexIndex))
            {
                int newOutlineVertex = GetConnectedOutlineVertex(vertexIndex);
                if(newOutlineVertex != -1)
                {
                    checkedVertices.Add(vertexIndex);

                    List<int> newOutline = new List<int>();
                    newOutline.Add(vertexIndex);
                    outlines.Add(newOutline);
                    FollowOutline(newOutlineVertex, outlines.Count - 1);
                    outlines[outlines.Count - 1].Add(vertexIndex);
                }
            }
        }
    }

    void FollowOutline(int vertexindex, int outlineIndex)
    {
        outlines[outlineIndex].Add(vertexindex);
        checkedVertices.Add(vertexindex);
        int nextVertexIndex = GetConnectedOutlineVertex(vertexindex);

        if(nextVertexIndex != -1)
        {
            FollowOutline(nextVertexIndex, outlineIndex);
        }
    }
    int GetConnectedOutlineVertex(int vertexIndex)
    {
        List<Triangle> trianglesContainingVertex = triangleDictionary[vertexIndex];
        for(int i = 0; i < trianglesContainingVertex.Count; i++)
        {
            Triangle triangle = trianglesContainingVertex[i];

            for(int j = 0; j < 3; j++)
            {
                int vertexB = triangle[j];

                if(vertexB != vertexIndex && !checkedVertices.Contains(vertexB))
                {
                    if (IsOutlineEdge(vertexIndex, vertexB))
                    {
                        return vertexB;
                    }
                }
            }
        }

        return -1;
    }

    bool IsOutlineEdge(int vertexA, int vertexB)
    {
        List<Triangle> trianglesContainingVertexA = triangleDictionary[vertexA];
        int sharedTriangleCount = 0;

        for(int i = 0; i<trianglesContainingVertexA.Count; i++)
        {
            if (trianglesContainingVertexA[i].Contains(vertexB))
            {
                sharedTriangleCount++;
                if(sharedTriangleCount > 1)
                {
                    break;
                }
            }
        }
        return sharedTriangleCount == 1;
    }

    struct Triangle
    {
        public int vertexIndexA;
        public int vertexIndexB;
        public int vertexIndexC;
        int[] vertices;
        public Triangle(int a, int b, int c)
        {
            vertexIndexA = a;
            vertexIndexB = b;
            vertexIndexC = c;

            vertices = new int[3];
            vertices[0] = a;
            vertices[1] = b;
            vertices[2] = c;
        }

        public int this[int i]
        {
            get
            {
                return vertices[i];
            }
        }

        public bool Contains(int vertexIndex)
        {
            return vertexIndex == vertexIndexA || vertexIndex == vertexIndexB || vertexIndex == vertexIndexC;
        }
    }

    void OnDrawGizmos()
    {
        /*
        if (squareGrid != null) {
            for (int x = 0; x < squareGrid.squares.GetLength(0); x ++) {
                for (int y = 0; y < squareGrid.squares.GetLength(1); y ++) {
                    Gizmos.color = (squareGrid.squares[x,y].topLeft.active)?Color.black:Color.white;
                    Gizmos.DrawCube(squareGrid.squares[x,y].topLeft.position, Vector3.one * .4f);
                    Gizmos.color = (squareGrid.squares[x,y].topRight.active)?Color.black:Color.white;
                    Gizmos.DrawCube(squareGrid.squares[x,y].topRight.position, Vector3.one * .4f);
                    Gizmos.color = (squareGrid.squares[x,y].bottomRight.active)?Color.black:Color.white;
                    Gizmos.DrawCube(squareGrid.squares[x,y].bottomRight.position, Vector3.one * .4f);
                    Gizmos.color = (squareGrid.squares[x,y].bottomLeft.active)?Color.black:Color.white;
                    Gizmos.DrawCube(squareGrid.squares[x,y].bottomLeft.position, Vector3.one * .4f);
                    Gizmos.color = Color.grey;
                    Gizmos.DrawCube(squareGrid.squares[x,y].centreTop.position, Vector3.one * .15f);
                    Gizmos.DrawCube(squareGrid.squares[x,y].centreRight.position, Vector3.one * .15f);
                    Gizmos.DrawCube(squareGrid.squares[x,y].centreBottom.position, Vector3.one * .15f);
                    Gizmos.DrawCube(squareGrid.squares[x,y].centreLeft.position, Vector3.one * .15f);
                }
            }
        }
        */
    }

    public class SquareGrid
    {
        public Square[,] squares;

        public SquareGrid(int[,] map, float squareSize)
        {
            int nodeCountX = map.GetLength(0);
            int nodeCountY = map.GetLength(1);
            float mapWidth = nodeCountX * squareSize;
            float mapHeight = nodeCountY * squareSize;

            ControlNode[,] controlNodes = new ControlNode[nodeCountX, nodeCountY];

            for (int x = 0; x < nodeCountX; x++)
            {
                for (int y = 0; y < nodeCountY; y++)
                {
                    Vector3 pos = new Vector3(-mapWidth / 2 + x * squareSize + squareSize / 2, 0, -mapHeight / 2 + y * squareSize + squareSize / 2);
                    controlNodes[x, y] = new ControlNode(pos, map[x, y] == 1, squareSize);
                }
            }

            squares = new Square[nodeCountX - 1, nodeCountY - 1];
            for (int x = 0; x < nodeCountX - 1; x++)
            {
                for (int y = 0; y < nodeCountY - 1; y++)
                {
                    squares[x, y] = new Square(controlNodes[x, y + 1], controlNodes[x + 1, y + 1], controlNodes[x + 1, y], controlNodes[x, y]);
                }
            }

        }
    }

    public class Square
    {

        public ControlNode topLeft, topRight, bottomRight, bottomLeft;
        public Node centreTop, centreRight, centreBottom, centreLeft;
        public int configuration;

        public Square(ControlNode _topLeft, ControlNode _topRight, ControlNode _bottomRight, ControlNode _bottomLeft)
        {
            topLeft = _topLeft;
            topRight = _topRight;
            bottomRight = _bottomRight;
            bottomLeft = _bottomLeft;

            centreTop = topLeft.right;
            centreRight = bottomRight.above;
            centreBottom = bottomLeft.right;
            centreLeft = bottomLeft.above;

            if (topLeft.active)
                configuration += 8;
            if (topRight.active)
                configuration += 4;
            if (bottomRight.active)
                configuration += 2;
            if (bottomLeft.active)
                configuration += 1;
        }
    }

    public class Node
    {
        public Vector3 position;
        public int vertexIndex = -1;

        public Node(Vector3 _pos)
        {
            position = _pos;
        }
    }

    public class ControlNode : Node
    {

        public bool active;
        public Node above, right;

        public ControlNode(Vector3 _pos, bool _active, float squareSize) : base(_pos)
        {
            active = _active;
            above = new Node(position + Vector3.forward * squareSize / 2f);
            right = new Node(position + Vector3.right * squareSize / 2f);
        }
    }
}
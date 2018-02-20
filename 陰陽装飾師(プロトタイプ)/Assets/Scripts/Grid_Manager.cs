using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point
{
    public Vector3 pos = new Vector3(0,0,0);
    public bool inside = false;
    public bool wall = false;
}

public class Grid_Manager : MonoBehaviour
{
    private int Grid_x_min_ = 0;
    private int Grid_y_min_ = 0;
    private int Grid_x_max_;
    private int Grid_y_max_;

    private int To_center_x_;
    private int To_center_y_;

    private float Grid_density_ = 0.15f;

    private Point[,] point_;

    public int Grid_x_min()
    {
        return Grid_x_min_;
    }

    public int Grid_y_min()
    {
        return Grid_y_min_;
    }

    public int Grid_x_max()
    {
        return Grid_x_max_;
    }

    public int Grid_y_max()
    {
        return Grid_y_max_;
    }

    public int To_center_x()
    {
        return To_center_x_;
    }

    public int To_center_y()
    {
        return To_center_y_;
    }

    public float Grid_density()
    {
        return Grid_density_;
    }

    public Point point(int i, int j)
    {
        return point_[i, j];
    }

    private Vector3[] line_start = new Vector3[4];
    private Vector3[] line_end = new Vector3[4];

    // 左クリックしたオブジェクトを取得する関数(3D)
    public GameObject getClickObject()
    {
        GameObject result = null;

        // 左クリックされた場所のオブジェクトを取得
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(ray, out hit))
            {
                result = hit.collider.gameObject;
            }
        }
        return result;
    }

    public void Instantiate(int room_id)
    {
        if (Grid_density_ == 0.15f)
        {
            if (room_id == 0)
            {
                Grid_x_max_ = 116;
                Grid_y_max_ = 65;

                To_center_x_ = 0;
                To_center_y_ = -13;
            }
            else if (room_id == 1)
            {
                Grid_x_max_ = 115;
                Grid_y_max_ = 64;

                To_center_x_ = 2;
                To_center_y_ = -14;
            }
            else if (room_id == 2)
            {
                Grid_x_max_ = 114;
                Grid_y_max_ = 63;

                To_center_x_ = 1;
                To_center_y_ = -13;
            }
            else if (room_id == 3)
            {
                Grid_x_max_ = 114;
                Grid_y_max_ = 63;

                To_center_x_ = 0;
                To_center_y_ = -12;
            }
            else if (room_id == 4)
            {
                Grid_x_max_ = 115;
                Grid_y_max_ = 64;

                To_center_x_ = 1;
                To_center_y_ = -11;
            }
        }
        
        point_ = new Point[Grid_x_max_, Grid_y_max_];

        for (int i = Grid_y_min_; i < Grid_y_max_; i++)
        {
            for (int j = Grid_x_min_; j < Grid_x_max_; j++)
            {
                point_[j, i] = new Point();
                
                point_[j, i].pos.x = (j - (Grid_x_max_ * 0.5f) + To_center_x_) * Grid_density_;
                point_[j, i].pos.y = (i - (Grid_y_max_ * 0.5f) + To_center_y_) * Grid_density_;

                if (j == Grid_x_min_ || j == Grid_x_max_ - 1 || i == Grid_y_min_ || i == Grid_y_max_ - 1)
                {
                    point_[j, i].wall = true;
                    //Test_sphere(j, i, 2, Color.red);
                }
                //Grid_all(j, i);
            }
        }

        Line(point_[Grid_x_min_, Grid_y_min_].pos, point_[Grid_x_max_ - 1, Grid_y_min_].pos, room_id);
        Line(point_[Grid_x_min_, Grid_y_max_ - 1].pos, point_[Grid_x_max_ - 1, Grid_y_max_ - 1].pos, room_id);

        Line(point_[Grid_x_min_, Grid_y_min_].pos, point_[Grid_x_min_, Grid_y_max_ - 1].pos, room_id);
        Line(point_[Grid_x_max_ - 1, Grid_y_min_].pos, point_[Grid_x_max_ - 1, Grid_y_max_ - 1].pos, room_id);

    }
    
    //public void Square_measure()
    //{
    //    bool mode = false;
    //    int start = 0;
    //    int end = 0;
    //
    //    for (int i = Grid_y_min_; i < Grid_y_max_; i++)
    //    {
    //        for (int j = Grid_x_min_; j < Grid_x_max_; j++)
    //        {
    //            if (mode == false)
    //            {
    //                if (point_[j, i].inside == true)
    //                {
    //                    mode = true;
    //                    start = j;
    //                }
    //            }
    //            else if (mode == true)
    //            {
    //                if (point_[j, i].inside == true)
    //                {
    //                    mode = false;
    //                    end = j;
    //
    //                    for (int k = start; k < end; k++)
    //                    {
    //                        point_[k, i].inside = true;
    //
    //                        //Test_sphere(k, i);
    //                    }
    //
    //                    j--;
    //                }
    //            }
    //        }
    //
    //        if (mode == true)
    //        {
    //            mode = false;
    //
    //            point_[end, i].inside = true;
    //
    //            //Test_sphere(end, i);              
    //        }            
    //    }
    //}

    public void Grid_all(int x, int y)
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.localScale = new Vector3(0.1f,0.1f,0.1f);
        sphere.GetComponent<MeshRenderer>().material.color = Color.black;
        sphere.transform.position = point_[x, y].pos;
    }

    public void Line(Vector3 start, Vector3 end, int room_id)
    {
        GameObject Line = new GameObject();
        LineRenderer renderer = Line.AddComponent<LineRenderer>();
        renderer.SetWidth(0.1f, 0.1f);
        renderer.SetVertexCount(2);
        renderer.SetPosition(0, start);
        renderer.SetPosition(1, end);

        if (room_id == 0)
        {
            renderer.material.color = new Color(0, 255, 255);
        }
        if (room_id == 1)
        {
            renderer.material.color = new Color(255, 0, 0);
        }
        if (room_id == 2)
        {
            renderer.material.color = new Color(0, 255, 0);
        }
        if (room_id == 3)
        {
            renderer.material.color = new Color(255, 255, 0);
        }
        if (room_id == 4)
        {
            renderer.material.color = new Color(255, 0, 200);
        }

    }

    public void Test_sphere(int x, int y, float scale, Color color)
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.localScale = new Vector3(scale, scale, scale);
        sphere.GetComponent<MeshRenderer>().material.color = color;
        sphere.transform.position = point_[x, y].pos;
    }

    // Use this for initialization
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

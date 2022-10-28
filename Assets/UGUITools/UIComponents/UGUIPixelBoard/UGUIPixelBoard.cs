/*************
因需求问题暂时封板不做
整体思路：
1.接到底层数据
2.根据底层数据格式修改UI控件
3.根据zoom缩放数据矩阵至高分辨率图片
4.分别计算出左、右、全三个重心，并将数组中的位置映射至position
5.根据底层值按一定梯度给板子对应像素颜色（可能有更好看的方法？）
6.画出对应的颜色，并画出重心连线
todo:
1. 缩放还没写完
2. 组件的生命周期管理，入口函数设计 
******/

using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UGUIPixelBoard : MonoBehaviour
{
    #region varialbles
    private byte[,] valueMatrix; // 输入的板子数值

    [Header("面板")]
    public RawImage board;
    public int zoom;

    [Tooltip("面板颜色梯度")]
    [Space(5)]
    public Color32[] stepColors;

    [Tooltip("面板颜色阈值 (length = colors+1、值为0~255递增)")]
    [Space(5)]
    public int[] thresholdValue;//length = colors+1 值为0~255递增

    [Header("重心线")]
    [Space(5)]
    public bool isBarycentric = true;
    public GameObject leftLine;
    public GameObject rightLine;

    public GameObject leftPoint;
    public GameObject rightPoint;
    public GameObject wholePoint;

    public float lineWidth;

    private byte[,] leftValueMatrix;
    private byte[,] rightValueMatrix;
    private Texture2D Boardtexture;

    private int BASE_COLUMN_SIZE;
    private int BASE_RAW_SIZE;

    #endregion

    // Start is called before the first frame update
    #region LifeCycle
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #endregion

    #region BoardBehavior
    public void InitBoard()
    {
        Texture2D boardTtexture = new Texture2D(BASE_COLUMN_SIZE, BASE_RAW_SIZE, TextureFormat.RGBA32, true);
        this.board.texture = boardTtexture;
    }
    /// <summary>
    /// 该函数按数据源帧率实时调用 TODO：
    /// 1.拿到压力数值矩阵矩阵
    /// 2.缩放矩阵
    /// 3.计算需要显示的数值
    /// 4.画板子
    /// </summary>
    public void UpdateBoard()
    {

    }
    
    public void ClearBoard()
    {
        //Clear
    }
    #endregion

    #region Geter Setter
    public void SetValueMatrix(byte[,] valueMatrix)
    {
        this.valueMatrix = valueMatrix;
    }
    #endregion

    #region DrawBoard
    void DrawBoard()
    {
        DrawValue(ZoomValueMatrix(valueMatrix));

        DrawLine(leftLine, GetBarycentricIndex(leftValueMatrix), GetBarycentricIndex(valueMatrix));
        DrawLine(rightLine, GetBarycentricIndex(valueMatrix), GetBarycentricIndex(rightValueMatrix));

        DrawPoint(leftPoint, GetBarycentricIndex(leftValueMatrix));
        DrawPoint(rightPoint, GetBarycentricIndex(rightValueMatrix));
        DrawPoint(wholePoint, GetBarycentricIndex(valueMatrix));
    }

    void DrawLine(GameObject line, Vector2 startPosition, Vector2 endPosition)
    {
        var heading = endPosition - startPosition;
        var distance = heading.magnitude;
        var direction = heading / distance;

        RectTransform rtLine = line.GetComponent<RectTransform>();

        //对齐左下角
        rtLine.anchorMin = Vector2.zero;
        rtLine.anchorMax = Vector2.zero;

        //计算线条位置
        Vector2 centerPos = new Vector2(startPosition.x + endPosition.x, startPosition.y + endPosition.y) / 2;
        rtLine.anchoredPosition = centerPos;

        //计算线条角度
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        line.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        //yf th
        rtLine.sizeDelta = new Vector2(distance, lineWidth);
    }

    void DrawPoint(GameObject point, Vector2 position)
    {
        position *= 2;
        point.GetComponent<RectTransform>().anchorMin = Vector2.zero;
        point.GetComponent<RectTransform>().anchorMax = Vector2.zero;
        point.GetComponent<RectTransform>().localPosition = new Vector3(position.x, position.y, 0);
    }

    void DrawValue(byte[,] values)
    {
        Boardtexture.SetPixels32(ByteToColor32(values));
        Boardtexture.Apply(false);
    }

 

    #endregion

    #region Cal func
    Vector2Int GetBarycentricIndex(byte[,] byteMatrix)
    {
        Vector2Int barycentric = new Vector2Int();
        int cx = 0;
        int cy = 0;
        int m = 0;

        for (int x = 0; x < byteMatrix.GetLength(0); x++)
        {
            for (int y = 0; y < byteMatrix.GetLength(1); y++)
            {
                cx += byteMatrix[x, y] * x;//x轴 m * r
                cy += byteMatrix[x, y] * y;//y轴 m * r
                m += byteMatrix[x, y];
            }
        }

        barycentric.x = (cx / m);
        barycentric.y = (cy / m);
        return barycentric;
    }
    Color32[] ByteToColor32(byte[,] byteArray)
    {
        int x = 0;
        int y = 0;
        Color32[] color32 = new Color32[byteArray.Length];
        //遍历值矩阵根据thresholdValue规定的梯度给颜色
        //循环顺序 [0,0]-> [x,0]-> [0,y]->[x,y]
        for (int i = 0; i < byteArray.Length; i++)
        {
            for (int j = 0; j < stepColors.Length; j++)
            {
                if (byteArray[x, y] >= thresholdValue[j] && byteArray[x, y] < thresholdValue[j + 1])
                {
                    color32[i] = stepColors[j];
                    break;
                }
            }
            x++;
            if (x > byteArray.GetLength(0) - 1)
            {
                x = 0;
                y++;
            }
        }
        return color32;
    }

    /// <summary>
    /// 缩放值矩阵
    /// </summary>
    /// <param name="valueMatrix"></param>
    /// <returns></returns>
    byte[,] ZoomValueMatrix(byte[,] valueMatrix)
    {
        byte[,] bytes = new byte[BASE_RAW_SIZE * zoom, BASE_COLUMN_SIZE * zoom];
        return bytes;
    }
    #endregion
}

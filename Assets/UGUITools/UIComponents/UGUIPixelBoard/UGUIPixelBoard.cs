using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UGUIPixelBoard : MonoBehaviour
{
    #region varialbles
    private byte[,] valueMatrix; // ����İ�����ֵ

    [Header("���")]
    public RawImage Board;
    public int zoom;

    [Tooltip("�����ɫ�ݶ�")]
    [Space(5)]
    public Color32[] stepColors;

    [Tooltip("�����ɫ��ֵ (length = colors+1��ֵΪ0~255����)")]
    [Space(5)]
    public int[] thresholdValue;//length = colors+1 ֵΪ0~255����

    [Header("������")]
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
    }
    /// <summary>
    /// �ú���������Դ֡��ʵʱ���� TODO��
    /// 1.�õ�ѹ����ֵ�������
    /// 2.���ž���
    /// 3.������Ҫ��ʾ����ֵ
    /// 4.������
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

    void DrawGravityCenter()
    {

    }

    void DrawLine(GameObject line, Vector2 startPosition, Vector2 endPosition)
    {
        var heading = endPosition - startPosition;
        var distance = heading.magnitude;
        var direction = heading / distance;

        RectTransform rtLine = line.GetComponent<RectTransform>();

        //�������½�
        rtLine.anchorMin = Vector2.zero;
        rtLine.anchorMax = Vector2.zero;

        //��������λ��
        Vector2 centerPos = new Vector2(startPosition.x + endPosition.x, startPosition.y + endPosition.y) / 2;
        rtLine.anchoredPosition = centerPos;

        //���������Ƕ�
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
                cx += byteMatrix[x, y] * x;//x�� m * r
                cy += byteMatrix[x, y] * y;//y�� m * r
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
        //����ֵ�������thresholdValue�涨���ݶȸ���ɫ
        //ѭ��˳�� [0,0]-> [x,0]-> [0,y]->[x,y]
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

    byte[,] ZoomValueMatrix(byte[,] valueMatrix)
    {
        byte[,] bytes = new byte[BASE_RAW_SIZE * zoom, BASE_COLUMN_SIZE * zoom];
        return bytes;
    }
    #endregion
}

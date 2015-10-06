//#############################################################################
//  フェードする
//  作者：稲垣達也
//#############################################################################

//名前空間/////////////////////////////////////////////////////////////////////
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//クラス///////////////////////////////////////////////////////////////////////
[RequireComponent(typeof(CanvasRenderer))]
[RequireComponent(typeof(Image))]
public class CanvasFade : MonoBehaviour {

    private Image m_Image;
    private Color m_Color;
    private float m_Time;
    private float m_TimeMax;
    private int   m_Flag;       // 1 = In / -1 = Out


    //非公開関数///////////////////////////////////////////////////////////////
    //初期化===================================================================
    void Awake() {
        m_Image = GetComponent<Image>();
        m_Image.enabled = false;
    }
    void Start() {
        if(m_Image == null) m_Image = GetComponent<Image>();
        m_Color      = m_Image.color;
        m_Flag       = 0;
        m_Time       = 0f;
        m_TimeMax    = 0f;
    }

    //更新=====================================================================
    void Update() {
        if(m_Flag == 0) return;

        m_Time -= Time.deltaTime;
        m_Color.a = m_Time / m_TimeMax;

        if(m_Flag == -1) { m_Color.a = 1f - m_Color.a; }

        if(m_Time <= 0.0f) {
            m_Color.a        = (m_Flag == 1)? 0.0f: 1.0f;
            m_Image.enabled  = (m_Flag != 1);
            m_Flag           = 0;
        }
    
        m_Image.color = m_Color;
    }

    //公開関数/////////////////////////////////////////////////////////////////
    //フェードイン=============================================================
    public void In(float aTime) {
        m_TimeMax = m_Time = aTime;
        m_Flag    = 1;
        m_Color.a = 1.0f;
        m_Image.color = m_Color;
        m_Image.enabled = true;
    }

    //フェードアウト=============================================================
    public void Out(float aTime) {
        m_TimeMax = m_Time = aTime;
        m_Flag    = -1;
        m_Color.a = 0.0f;
        m_Image.color = m_Color;
        m_Image.enabled = true;
    }

}

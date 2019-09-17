using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GyroController : MonoBehaviour
{
    // キーボードでの操作用
#if UNITY_EDITOR || UNITY_STANDALONE
    private Vector3 rot;
#endif
    public Vector3 fixrot;          // 視点リセット用修正角度変数
    public Vector3 nowrot;          // 視点リセット用現在角度変数
    public Vector3 delrot;          // 視点リセット用オフセット変数

    public GameObject exitButtonOBJ;
    public GameObject resetButtonOBJ;
    public GameObject modeButtonOBJ;

    public float visibleTimeCount = 2.0f;
    public float timeCount = 0.0f;

    public bool modeFlag;                   // VR操作モード:true/タッチ操作モード:false
    public GameObject targetObject;

    public float minCameraAngleX = 340.0f;  // カメラの最小角度
    public float maxCameraAngleX = 20.0f;   // カメラの最大角度
    public float swipeTurnSpeed = 10.0f;    // スワイプで回転するときの回転スピード

    private Vector3 baseMousePos;           // 基準となるタップの座標
    private Vector3 baseCameraPos;          // 基準となるカメラの座標
    private bool isMouseDown = false;       // マウスが押されているかのフラグ

    private Button modeButton;
    public Sprite grayModeButton_image;
    public Sprite whiteModeButton_image;

    // Use this for initialization
    void Start()
    {
        Debug.Log("stated");    // 動作確認用のログ

        fixrot = new Vector3(0, 0, 0);
        nowrot = new Vector3(0, 0, 0);
        delrot = new Vector3(0, 0, 0);

        modeButton = modeButtonOBJ.GetComponent<Button>();
        timeCount = visibleTimeCount;
        modeFlag = true;

#if UNITY_EDITOR || UNITY_STANDALONE
        rot = transform.rotation.eulerAngles;
        Debug.Log("non-smartphone");    // 動作環境の判別用のログ
#else
        Input.gyro.enabled = true;
#endif
    }

    // Update is called once per frame
    void Update()
    {
        // PCの場合はキーボードで視点変更、スマホはジャイロで視点変更
#if UNITY_EDITOR || UNITY_STANDALONE
        float spd = Time.deltaTime * 100.0f;
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rot.y -= spd;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            rot.y += spd;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            rot.x -= spd;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            rot.x += spd;
        }
        transform.rotation = Quaternion.Euler(rot);
#else
        if (modeFlag)
        {
            Quaternion gattitude = Input.gyro.attitude;
            gattitude.x *= -1;
            gattitude.y *= -1;
            transform.localRotation = Quaternion.Euler(90, -fixrot.y, 0) * gattitude;   // Y軸に対して修正角度を反映
        }
        else
        {
            SwipeCameraView();
        }
#endif

        if (timeCount > 0)
        {
            timeCount -= Time.deltaTime;
            exitButtonOBJ.SetActive(true);
            resetButtonOBJ.SetActive(true);
            modeButtonOBJ.SetActive(true);
        }
        else
        {
            exitButtonOBJ.SetActive(false);
            resetButtonOBJ.SetActive(false);
            modeButtonOBJ.SetActive(false);
        }

        if (Input.GetMouseButtonDown(0))
        {
            timeCount = visibleTimeCount;
        }
    }

    public void OnClickExitButton()
    {
        Application.Quit();     // アプリメーションを終了
    }

    public void OnClickResetButton()
    {
        Debug.Log("Reset");    // 動作確認用のログ

        nowrot = transform.localEulerAngles;
        fixrot += (nowrot + delrot);                   // 修正角度変数に、現在の角度を加算
    }

    public void OnClickModeButton()
    {
        Debug.Log("Mode");    // 動作確認用のログ

        if (modeFlag)
        {
            modeFlag = false;
            modeButton.image.sprite = whiteModeButton_image;
        }
        else
        {
            modeFlag = true;
            modeButton.image.sprite = grayModeButton_image;
        }
    }

    public void SwipeCameraView()
    {
        // タップの種類の判定 & 対応処理
        if ((Input.touchCount == 1 && !isMouseDown) || Input.GetMouseButtonDown(0))
        {
            baseMousePos = Input.mousePosition;
            isMouseDown = true;
        }

        // 指を離した時の処理
        if (Input.GetMouseButtonUp(0))
        {
            isMouseDown = false;
        }

        // スワイプ回転処理
        if (isMouseDown)
        {
            Vector3 mousePos = Input.mousePosition;
            Vector3 distanceMousePos = (mousePos - baseMousePos);
            float angleX = targetObject.transform.eulerAngles.x - distanceMousePos.y * swipeTurnSpeed * 0.01f;
            float angleY = targetObject.transform.eulerAngles.y + distanceMousePos.x * swipeTurnSpeed * 0.01f;

            if ((angleX >= -10f && angleX <= maxCameraAngleX) || (angleX >= minCameraAngleX && angleX <= 370f))
            {
                targetObject.transform.eulerAngles = new Vector3(angleX, angleY, 0);
            }
            else
            {
                targetObject.transform.eulerAngles = new Vector3(targetObject.transform.eulerAngles.x, angleY, 0);
            }
            baseMousePos = mousePos;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GazeUIController : MonoBehaviour
{

    public GameObject mainCamera;
    public GameObject movePlane_1;  // 視点UIの対象Plane
    public GameObject movePlane_2;  // 視点UIの対象Plane

    public Material[] _material;    // 対象ボタンの色 0:通常/1:ヒット

    public SpriteRenderer movePlaneSprite_1;
    public SpriteRenderer movePlaneSprite_2;

    public Sprite[] pointer;        // ポインター画像
    public float gazeTimeCount = 3.0f;      // 確定までの時間(秒)

    void Start()
    {
        Debug.Log("Gaze UI Controller is started");
    }

    void Update()
    {
        Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.black);     // Sceneでのみ線が見える

            if (hit.collider.gameObject.tag == "VRUI")          // 視点UIの対象かをタグで判定
            {
                Debug.Log("hit");
                if (CheckHitGameObject(hit, movePlane_1) == true)
                {
                    movePlane_1.GetComponent<Renderer>().material = _material[1];   // ヒットの色
                    if (DrawSpriteFromGazeTimeCount(movePlaneSprite_1) == true)
                    {
                        mainCamera.transform.position = new Vector3(0, 1.2f, -5);
                        mainCamera.transform.rotation = Quaternion.Euler(0, 0, 0);    // 角度を変えるとき
                    }
                }
                if (CheckHitGameObject(hit, movePlane_2) == true)
                {
                    movePlane_2.GetComponent<Renderer>().material = _material[1];   // ヒットの色
                    if (DrawSpriteFromGazeTimeCount(movePlaneSprite_2) == true)
                    {
                        mainCamera.transform.position = new Vector3(0, 1.2f, -10);
                        mainCamera.transform.rotation = Quaternion.Euler(0, 180f, 0);    // 角度を変えるとき
                    }
                }
            }
            else
            {
                gazeOff();
            }
        }
        else
        {
            gazeOff();
        }
    }

    public void gazeOff()
    {
        movePlane_1.GetComponent<Renderer>().material = _material[0];       // 通常の色
        movePlane_2.GetComponent<Renderer>().material = _material[0];       // 通常の色

        movePlaneSprite_1.sprite = pointer[0];
        movePlaneSprite_2.sprite = pointer[0];

        gazeTimeCount = 3.0f;
    }

    public bool CheckHitGameObject(RaycastHit hit, GameObject obj)
    {
        bool result = false;
        if (hit.collider.gameObject == obj) result = true;
        return result;
    }

    // 注視カウントの値によりポインタ画像を変更
    public bool DrawSpriteFromGazeTimeCount(SpriteRenderer spr)
    {
        bool result = false;

        if (gazeTimeCount > 0)
        {
            int idx = 6 - (int)(gazeTimeCount * 10) / 5;
            spr.sprite = pointer[idx];
            gazeTimeCount -= Time.deltaTime;
        }
        else
        {
            result = true;
            spr.sprite = pointer[0];
        }
        return result;
    }
}

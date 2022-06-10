using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // подключаем для работы с текстом канваса

public class PlayerController : MonoBehaviour
{
    Transform transform;
    CharacterController characterController; 
    public float move_speed = 12f; 
    float gravityValue = -9.81f; 
    bool isGrounded = false;
    float jumpHeight = 5f; 
    int seconds_left = 10;
    [SerializeField] TextMeshProUGUI timerText; 
    public GameObject losePanel; 
    public GameObject winPanel;
    public GameObject die;
    public GameObject crown;
    public bool isPlayerControl = true;

    void TimerWork() {
        seconds_left--; 
        timerText.text = "ОСТАЛОСЬ: " + seconds_left; 
        if(seconds_left == 0) { 
            CancelInvoke(); 
            losePanel.SetActive(true); 
            isPlayerControl = false;
        }
        
    }

    
    void Start()
    {
        InvokeRepeating("TimerWork", 1f, 1f); 
        transform = GetComponent<Transform>(); 
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isPlayerControl){
            characterController.Move(transform.up * gravityValue * Time.deltaTime); 

            float mouseX = Input.GetAxis("Mouse X") * 2; // для поворота мышки только налево и направо, т.е. по горизонтали (вдоль оси Х)
            float vertical = Input.GetAxis("Vertical"); // для перемещения капсулы с помощью кнопок W и S (вперед, назад)

            characterController.Move(transform.forward * vertical * move_speed * Time.deltaTime); // капсула будет двигаться при нажатии W и S вперед-назад со скоростью move_speed

            transform.Rotate(0, mouseX, 0); // капсула будет делать поворот в зависимости от движения мыши (влево или вправо)

            if(Input.GetKeyDown("space") && isGrounded == true) { // если нажат пробел и мы на земле, то...
                characterController.Move(transform.up * jumpHeight); // ...двигаемся вверх на высотку прыжка
            }

            isGrounded = false; // по дефолту мы не на земле (пока не коснёмся её коллайдера)
        }
        if (Input.GetKeyDown(KeyCode.R))
            {
                 SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
    }

    void OnControllerColliderHit(ControllerColliderHit col) {
        if(col.gameObject.tag == "ground") {
            isGrounded = true; // если капсула касается коллайдера земли, то сохраняем это состояние в переменной isGrounded
        }
        if(col.gameObject.tag == "ground1") {           
            CancelInvoke(); 
            losePanel.SetActive(true);
            isPlayerControl = false;      
        }

        if(col.gameObject.tag == "cube") {
            Destroy(col.gameObject); // если капсула касается коллайдера куба (не трамплина), то уничтожаем модельку этого куба
        }

        if(col.gameObject.tag == "springboard") { // springboard переводится как "трамплин" 
            col.gameObject.GetComponent<Renderer>().material.color = Color.green; // если капсула касается коллайдера трамплина (рядом с лестницей), то у трамплина
            // ищем компонент Renderer, затем заходим в настройки материала, берём цвет и меняем его на зеленый (или любой другой цвет).
        }

        if(col.gameObject.tag == "WinObject") { // если сталкиваемся с невидимым объектом с именем WinObject
            CancelInvoke("TimerWork"); // останавливаем таймер игрока (интервал для функции TimerWork)
            Destroy(col.gameObject);
            crown.SetActive(true);
            winPanel.SetActive(true); // показываем игроку панель выигрыша
        }

        if(col.gameObject.tag == "arrow") { // если сталкиваемся с невидимым объектом с именем WinObject
            CancelInvoke("TimerWork"); // останавливаем таймер игрока (интервал для функции TimerWork)
            die.SetActive(true); // показываем игроку панель выигрыша
            isPlayerControl = false;
        }
    }
}

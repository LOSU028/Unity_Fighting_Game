using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Referi : MonoBehaviour
{   
    
    public TMP_Text hp1;
    public TMP_Text hp2;
    private int HP_Player2 = 15;
    private int HP_Player1 = 15;

    
    private void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.tag == "Enemy_Player2"){
            HP_Player2 --;
            hp2.text = "HP: " + HP_Player2;
        }else if(collision.gameObject.tag == "Enemy_Player1"){
            HP_Player1--;
            hp1.text = "HP: " + HP_Player1;
        }
        if(HP_Player2 == 0 || HP_Player1 == 0){
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    
}

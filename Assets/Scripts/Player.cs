using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb2D;
    public Animator animator;

    public float speed;
    public float jumpForce;

    public Transform posicaoDoPe;
    public Transform posicaoDoPe2;
    public bool isGrounded;
    public float radius;
    public LayerMask ground;

    private float direction;
    
    private bool facingright = true;
    private int nJump = 1;

    private float dashAtual;
    private bool canDash = true;
    private bool isDashing;
    private bool isCrouching;
    public float duracaoDash;
    public float dashSpeed;

    void Start()
    {
        dashAtual = duracaoDash;
    }

    void Update()
    {
        animator.SetBool("isGround", isGrounded);
        animator.SetFloat("speedY", rb2D.velocity.y);
        animator.SetFloat("speedX", Mathf.Abs(direction));
        animator.SetBool("isCrouching", isCrouching);
 
        Flip();
        MovePlayer();
        CheckInput();
        CheckGround();
        Dash();
        Crouch();
    }

    void Flip()
    {
        if((direction < 0 && facingright) || (direction > 0 && !facingright))
        {
            facingright = !facingright;
            transform.Rotate(0f, 180f, 0f);
        }
    }

    void MovePlayer()
    {
        direction = Input.GetAxisRaw("Horizontal");//se esquerda <- = -1  ou se direita -> = 1 
        rb2D.velocity = new Vector2(direction * speed, rb2D.velocity.y); 
    }

    void CheckGround()
    {
        //cria um circulo na posição do pé que devera encostar no chao
        isGrounded = Physics2D.OverlapCircle(posicaoDoPe.position, 0.3f, ground);//ground anula pulo infinito
        isGrounded = Physics2D.OverlapCircle(posicaoDoPe2.position, 0.3f, ground);
    }

    void CheckInput()
    {
        if(isGrounded)
        {
            nJump = 1;
        }

        if(Input.GetKeyDown(KeyCode.Space) && nJump > 0)
        {
            Jump();
        }
    }

    void Jump()
    {
        nJump--;
        rb2D.velocity = Vector2.up * jumpForce;

        //para pular, se o usuario pressionar a tecla espaço
        if (isGrounded && Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            //altera a velocidade no eixo y para multiplicando pela força do pulo
            rb2D.velocity = Vector2.up * jumpForce;
        }
    }

    void Crouch()
    {
        isCrouching = (Input.GetKey("down")) ? true : false;//esta agachado?
        animator.SetBool("isCrouching", isCrouching);

        if(isCrouching)//cancelar andar agachado
        {
            animator.SetFloat("speedX", 0);//zera o parametro da animação
            rb2D.velocity = new Vector2(0, rb2D.velocity.y);//zera a velocidade
        }
    }

    void Dash()
    {
        if(Input.GetKey(KeyCode.Q) && isGrounded && canDash)
        {
            if(dashAtual <= 0)
            {
                StopDash();
            }
            else
            {
                isDashing = true;
                dashAtual -= Time.deltaTime;

                if(facingright)
                {
                    rb2D.velocity = Vector2.right * dashSpeed;
                    animator.SetBool("isCrouching", isCrouching);
                }
                else
                    rb2D.velocity = Vector2.left * dashSpeed;
                    animator.SetBool("isCrouching", isCrouching);
            }
        }

        if(Input.GetKeyDown(KeyCode.Q))
        {
            isDashing = false;
            canDash = true;
            dashAtual = duracaoDash;
        }
    }

    private void StopDash ()
    {
        rb2D.velocity = Vector2.zero;
        dashAtual = duracaoDash;
        isDashing = false;
        canDash = false;
    }

    //Mostra os Gizmos na tela para orientação
/*   void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(posicaoDoPe.position, radius);//pé esquerdo
        Gizmos.DrawWireSphere(posicaoDoPe2.position, radius);//pé direito
    }
*/
    //Faz o player permanecer parado sobre a colisao da plataforma
    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.name.Equals("Plataforma"))//O objeto deve conter o mesmo nome
            this.transform.parent = col.transform;
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if(col.gameObject.name.Equals("Plataforma"))
            this.transform.parent = null;
    }
}
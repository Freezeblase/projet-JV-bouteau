using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private bool isGrounded;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float gravity;
    [SerializeField] private Vector3 velocity;
    [SerializeField] private float jumpHeight;

    [SerializeField] private float turnSpeed;
    

    private Vector3 moveDirection;

    // Références
    private CharacterController controller;
    private Animator anim;
    

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>(); // Récupère le character controller
        anim =  GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        // On déclance l'atk si tu click
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            StartCoroutine(Attack());
        }
    }

    void Move(){

        isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundMask);

        float moveZ = Input.GetAxis("Vertical");
        moveDirection = new Vector3(0,0,moveZ);

        moveDirection = transform.TransformDirection(moveDirection); // permet de se déplacer dans l'axe du personnage

        if( isGrounded ){
            if( moveDirection != Vector3.zero && !Input.GetKey(KeyCode.RightShift)){
            Walk();
            }
            else if( moveDirection != Vector3.zero && Input.GetKey(KeyCode.RightShift)){
                Run();
            }
            else if( moveDirection == Vector3.zero){
                Idle();
            }

            if( Input.GetKeyDown(KeyCode.Space)){ // On teste le saut (espace)
                Jump();
            }
        }
       

        // Applique le mouvement dans le plan horizontal
        moveDirection *= moveSpeed;
        controller.Move(moveDirection*Time.deltaTime);

        // Applique la gravité (déplacement vertical)
        if( isGrounded && velocity.y<= 0){ // si on est déjà au sol, on ne bouge pas
            velocity.y = 0;
        }
        else{ // On applique la gravité
            velocity.y += gravity*Time.deltaTime;
            controller.Move(velocity*Time.deltaTime);
        }
        
        transform.Rotate(0, Input.GetAxis("Horizontal")*turnSpeed*Time.deltaTime,0);
    }

    void Idle(){
        anim.SetFloat("Speed", 0, 0.1f, Time.deltaTime);
    }

    void Walk(){
        moveSpeed = walkSpeed;
        anim.SetFloat("Speed", 0.5f, 0.1f, Time.deltaTime);
    }

    void Run(){
        moveSpeed = runSpeed;
        anim.SetFloat("Speed", 1, 0.1f, Time.deltaTime);
    }

    void Jump(){
        velocity.y = Mathf.Sqrt(jumpHeight*-2*Physics.gravity.y);
    }
    IEnumerator Attack(){
        anim.SetLayerWeight(anim.GetLayerIndex("Attack Layer"), 1);
        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(0.9f);
        anim.SetLayerWeight(anim.GetLayerIndex("Attack Layer"), 0);
    }

}
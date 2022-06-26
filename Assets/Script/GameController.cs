using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public float requiredDistance = 1f;
    public float requiredDistanceToHolder = 1f;
    [SerializeField] private Material changeMaterial;
    public Material changeMaterialDefense;
    [SerializeField]public Material defaultMeshRenderer;
    private List<EachGridInfo> meshRendererHolder = new List<EachGridInfo>();
    private PlayerController player;
    private DefenceController defence;
    [SerializeField] private GameObject turn1, turn2;
    public List<Transform> holders = new List<Transform>();
    public List<Transform> actualHolder = new List<Transform>();
    [SerializeField] private PlayerController player1;
    [SerializeField] private PlayerController player2;
    public Vector2 oppPlayer;
    [SerializeField] GameObject wonCanvas;
    [SerializeField] private Text wonText;
    public static event Action<Vector3> onDefenceAdded;

  

    enum PlayerTurn
    {
        player1,
        player2
    }

    PlayerTurn playerTurn = PlayerTurn.player1;

    public bool GameOver { get; private set; }
    public List<Transform> finalPoints = new List<Transform>();
   

    void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
    }
   
    // Update is called once per frame
    void Update()
    {
        if (GameOver) return;
        HandleClick();
    }
    /// <summary>
    /// Handles click by the user.
    /// </summary>
    private void HandleClick()
    {
        if (Input.GetMouseButtonDown(0))
        {          
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                hit = HandleGrid(hit);
                RemoveGridRendererEffect();
                CheckPlayerTurn(hit);
            }
            else             
               RemoveGridRendererEffect();
        }      
    }
    /// <summary>
    /// Calls when a game completes.
    /// </summary>
    private void GameComplete()
    {
        Time.timeScale = 0f;
        if(playerTurn == PlayerTurn.player1)
            WonGame("Player 1 won");

        else
            WonGame("Player 2 won");

    }
    /// <summary>
    /// This function do changes needed when a game completes.
    /// </summary>
    /// <param name="v"></param>
    private void WonGame(string v)
    {
        wonCanvas.SetActive(true);
        wonText.text = v;
    }
    /// <summary>
    /// If touch any grid it handles the information.
    /// </summary>
    /// <param name="hit"></param>
    /// <returns></returns>
    private RaycastHit HandleGrid(RaycastHit hit)
    {
        if (hit.collider.CompareTag("Grid"))
        {
            EachGridInfo eachGridInfo = hit.transform.GetComponent<EachGridInfo>();
            if (eachGridInfo.IsMovable())
            {
                player.Move(hit.transform);
                if (eachGridInfo.FinalGrid())
                {
                    GameOver = true;
                    Invoke(nameof(GameComplete), 0.5f);
                    return hit;
                }
                ChangeTurn();
            }
        }
        return hit;
    }
    /// <summary>
    /// It checks which players turn currently is.
    /// </summary>
    /// <param name="hit"></param>
    private void CheckPlayerTurn(RaycastHit hit)
    {
        if (playerTurn == PlayerTurn.player1)      
            HandlePlayer1(hit);
        
        else if (playerTurn == PlayerTurn.player2) 
            HandlePlayer2(hit);

    }
    /// <summary>
    /// Handles player two action when its player one turn.
    /// </summary>
    /// <param name="hit"></param>
    private void HandlePlayer2(RaycastHit hit)
    {
        if (hit.collider.CompareTag("Player2"))
        {
            player = hit.transform
            .GetComponent<PlayerController>();
            player.OnClickByUser();
        }
        if (hit.collider.CompareTag("Defence2"))
        {
            defence = hit.transform
            .GetComponent<DefenceController>();
            defence.OnClickByUser();
        }
    }
    /// <summary>
    /// Handles player one action when its player one turn.
    /// </summary>
    /// <param name="hit"></param>
    private void HandlePlayer1(RaycastHit hit)
    {
        if (hit.collider.CompareTag("Player1"))
        {
            player = hit.transform
            .GetComponent<PlayerController>();
            player.OnClickByUser();
        }
        if (hit.collider.CompareTag("Defence1"))
        {
            defence= hit.transform
            .GetComponent<DefenceController>();
            defence.OnClickByUser();
        }
    }
    /// <summary>
    /// Change turn after the player does any moves.
    /// Since it is a turn based game.
    /// </summary>
    public void ChangeTurn()
    {
        bool player1 = playerTurn == PlayerTurn.player1;
        
        playerTurn =player1  ? PlayerTurn.player2 : PlayerTurn.player1;
        oppPlayer = playerTurn == PlayerTurn.player2 ? this.player1.TransformPointVector2 :
            this.player2.TransformPointVector2;
        turn1.SetActive(!player1);
        turn2.SetActive(player1);

    }
    /// <summary>
    /// Remove the grid effects happening on the grid on choosing.
    /// </summary>
    private void RemoveGridRendererEffect()
    {
    if (meshRendererHolder.Count > 0)
        {
            foreach (EachGridInfo r in meshRendererHolder)
            {
                r.render.material = defaultMeshRenderer;
                r.Movable = false;
            }
            player = null;
            meshRendererHolder.Clear();
        }
    }
    public void ChangedMeshrenderer(EachGridInfo grid)
    {
        grid.render.material = changeMaterial;
        meshRendererHolder.Add(grid);
    }
}


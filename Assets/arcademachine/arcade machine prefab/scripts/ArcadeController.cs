using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class ArcadeController : MonoBehaviour
{

    public CinemachineFreeLook playerCam;
    public CinemachineVirtualCamera arcadeCam;

    public Transform player;
    public Transform machine;

    public InputAction machineControl;

    public GameObject arcadeGame;
    public GameObject sceneManager;

    public Texture m_mainTex, m_instructions, m_grid;
    Renderer m_Renderer;
    public int currentTexture = 0;

    //private bool notinInstructions = true;

    float distance;

    
    


    private void SwitchPriority()
    {
        if(playerCam.Priority == 1)
        {
            playerCam.Priority = 0;
            arcadeCam.Priority = 1;
        }
        else
        {
            playerCam.Priority = 1;
            arcadeCam.Priority = 0;
        }
    }

    private void OnEnable()
    {
        machineControl.Enable();
    }
    private void OnDisable()
    {
        machineControl.Disable();
    }

    private void Awake()
    {
        arcadeGame.gameObject.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        arcadeCam.Priority = 0;    
        m_Renderer = GetComponent<Renderer>();
        m_Renderer.material.SetTexture("_MainTex", m_mainTex);
    }

    // Update is called once per frame
    void Update()
    {
        distance = Mathf.Abs(Vector3.Distance(player.transform.position, machine.transform.position));
        if(distance < 2f)
        {
            if (machineControl.WasPressedThisFrame())
            {
                Debug.Log("true");

                if (m_Renderer.material.mainTexture == m_mainTex)
                {
                    SwitchPriority();
                    m_Renderer.material.SetTexture("_MainTex", m_instructions);
                    player.SendMessage("switchInput");
                }
                else if (m_Renderer.material.mainTexture == m_instructions)
                {
                    m_Renderer.material.SetTexture("_MainTex", m_grid);
                    //sceneManager.SendMessage("minigame");
                    arcadeGame.gameObject.SetActive(true);
                    arcadeGame.SendMessage("spawnToggle");
                }
                else if (m_Renderer.material.mainTexture == m_grid)
                {
                    SwitchPriority();
                    m_Renderer.material.SetTexture("_MainTex", m_mainTex);
                    player.SendMessage("switchInput");
                    //arcadeGame.SendMessage("spawnToggle");
                    arcadeGame.gameObject.SetActive(false);
                }
            }
        }
    }

    private void win()
    {
        MazeController.puzzleCompleted();
    }
}

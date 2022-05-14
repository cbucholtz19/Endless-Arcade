using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosive : MonoBehaviour
{
    public GameObject _explosive;
    public GameObject _gamemanager;
    public GameObject _hotLavaController;
    public ParticleSystem _explosion;
    public GameObject _player;

    private ParticleSystem temp;

    // Start is called before the first frame update
    void Start()
    {
        _explosion.Play(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Water") || collision.gameObject.layer == LayerMask.NameToLayer("Wall")) 
        {
  
            //temp = Instantiate(_explosion, _explosive.transform.position , Quaternion.identity);
            Destroy(_explosive);
            new WaitForSeconds(2);
            Destroy(temp);
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player")) 
        {
            _player.SendMessage("freeze");
            _hotLavaController.SendMessage("spawnToggle");
            temp = Instantiate(_explosion, _explosive.transform.position, Quaternion.identity);
            Destroy(_explosive);
            new WaitForSeconds(2);
            Destroy(temp);
            GameController.gameover(GameoverCondition.Arcade);
        }
    }
}

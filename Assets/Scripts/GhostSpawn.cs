using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostSpawn : MonoBehaviour
{
    Vector3 prev_pos, prev_rot;
    [SerializeField] GameObject ghost, player, walk,hip;
    GameManager manager;

    void Start()
    {
        ghost.SetActive(false);
        manager = (GameManager)player.GetComponent(typeof(GameManager));
        prev_pos = player.transform.position;
    }

    void Spawn()
    {
        //Debug.Log("Spawned ghost");
        manager.sound_player.PlaySFX("ghost");
        int r = Random.Range(15, 30);
        Vector3 newpos = player.transform.forward * r + player.transform.position;
        newpos.y -= 5f;
        transform.position = newpos;
        ghost.SetActive(true);
        ghost.GetComponent<Animator>().SetTrigger("walk");
        StartCoroutine(Expired());
    }

    void Hide()
    {
        //Debug.Log("Hid ghost");
        ghost.SetActive(false);
    }

    void FixedUpdate()
    {
        if (!manager.finish0)
            return;
        Vector3 p_pos = player.transform.position;
        Vector3 p_rot = player.transform.eulerAngles;
        Vector3 g_pos = hip.transform.position;
        Vector3 me_pos = walk.transform.position;
        if (p_pos != prev_pos || p_rot != prev_rot)
        {
            prev_pos = p_pos;
            p_rot = prev_rot;
            if (Random.Range(1, 3500) == 799 && !ghost.activeSelf)
                Spawn();
        }
        if (ghost.activeSelf)
        {
            float y_tan = me_pos.z - p_pos.z, x_tan = me_pos.x - p_pos.x;
            Vector3 newrot = transform.eulerAngles;
            newrot.y = Mathf.Atan2(x_tan, y_tan) * Mathf.Rad2Deg + 190;
            walk.transform.eulerAngles = newrot;
        }
        if (ghost.activeSelf)
        {
            float dis = Mathf.Abs(p_pos.z - g_pos.z) + Mathf.Abs(p_pos.x - g_pos.x);
            if (dis <= 12)
                Hide();
        }
    }

    IEnumerator Expired()
    {
        yield return new WaitForSeconds(8);
        Hide();
    }
}

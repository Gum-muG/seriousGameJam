using System;
using Unity.Mathematics;
using UnityEngine;

public class spikes : MonoBehaviour
{
    [SerializeField] GameObject spikeMesh;
    [SerializeField] Material spikeMaterial;
    [SerializeField] int densityX = 3;
    [SerializeField] int densityZ = 3;
    [SerializeField] float width = 1;
    GameObject[] spikeArray = new GameObject[0];

    bool attackWait = false;

    [SerializeField] bool continuousUpdate = false;
    void Start()
    {
        spawnSpikes();
    }
    void Update()
    {
        if (continuousUpdate) spawnSpikes();
    }

    void spawnSpikes()
    {
        densityX = (int) math.clamp(densityX, 1, transform.localScale.x/width-1);
        densityZ = (int) math.clamp(densityZ, 1, transform.localScale.z/width-1);
        if (spikeArray.Length > 0)
        {
            foreach(GameObject spike in spikeArray)
            {
                Destroy(spike);
            }
        }
        spikeArray = new GameObject[densityX * densityZ];
        float gapX;
        float gapZ;
        float height;
        float startX;
        float startZ;
        float startY;
        if (densityX > 1)
        {
            gapX = transform.localScale.x / (densityX - 1);
            startX = transform.position.x - transform.localScale.x/2;
        } else
        {
            gapX = 1;
            startX = transform.position.x;
        }
        if (densityZ > 1)
        {
            gapZ = transform.localScale.z / (densityZ - 1);
            startZ = transform.position.z - transform.localScale.z/2;
        } else
        {
            gapZ = 1;
            startZ = transform.position.z;
        }
        height = transform.localScale.y;
        startY = transform.position.y - transform.localScale.y/2;

        

        for (int x = 0; x < densityX; x++)
        {
            for (int z = 0; z < densityZ; z++)
            {
                float currX = startX + x * gapX;
                float currZ = startZ + z * gapZ;
                GameObject spike = Instantiate(spikeMesh, new Vector3(currX, startY, currZ), Quaternion.identity);
                spike.transform.localScale = new Vector3(width, height, width);
                spike.GetComponent<Renderer>().material = spikeMaterial;
                spikeArray[x + (z * densityX)] = spike;
            } 
        } 
    }

    void OnTriggerStay(Collider other)
    {
        if(!attackWait && other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            GameManager.instance.playerHealth.Damage(2);
            HUD.instance.SetHealth(GameManager.instance.playerHealth.Health);
            attackWait = true;
            Invoke("resetAttack", 0.3f);
        }
    }

    void resetAttack()
    {
        attackWait = false;
    }
}

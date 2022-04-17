using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigEnemyPatrol : Patrol
{

    public override State RunCurrentState(EnemyNavMesh enm, FOV fov)
    {
        enm.SetSpeed(navMeshSpeed);
        this.enm = enm;
        this.fov = fov;
        fov.patrolling = true;
        HandlePatrolPoints();

        switch (HandleSus())
        {
            case -1:
                enm.PatrolNav(HandleWait());
                break;
            case 0:
                enm.StopNav(fov.susLocation);
                break;
            case 1:
                if (endTime == 0f)
                {
                    endTime = Time.time + 1f;
                }
                if (HandleWait(susLocation) == susLocation)
                {
                    SetPatrol();
                    searchPatrol.checkLocation = susLocation;
                    return searchPatrol;
                }
                else
                {
                    enm.StopNav(susLocation);
                }
                break;
        }
        return this;
    }

    protected int HandleSus()
    {
        Transform gore = null;
        int susLevel = base.HandleSus();

        if (susLevel == 1)
        {
            foreach (Collider collider in fov.nearbyColliders)
            {
                if (collider.gameObject.TryGetComponent(out BodyPartGore goreScript)){
                    gore = collider.gameObject.transform;
                }
            }

            if (gore != null)
            {
                susLocation = gore.position;
                susLevel = 1;
            }
        }

        if (fov.targetNotPlayer && gore == null)
        {
            susLocation = Vector3.zero;
            susLevel = -1;
        }


        return susLevel;
    }
}

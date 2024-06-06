using UnityEngine;
using UnityEngine.AI;

public class ThiefScript : MonoBehaviour
{
    private NavMeshAgent agentRef;
    private Drive driveRef;
    [SerializeField] private GameObject copRef;

    void Start()
    {
        agentRef = GetComponent<NavMeshAgent>();
        driveRef = copRef.GetComponent<Drive>();
    }

    private void Seek(Vector3 targetLocation)
    {
        agentRef.SetDestination(targetLocation);
    }

    private void Flee(Vector3 targetLocation) 
    {
        Vector3 newLocation = targetLocation - transform.position;
        agentRef.SetDestination(transform.position - newLocation);
    }

    private void Pursue()
    {
        Vector3 targetDirection = copRef.transform.position - transform.position;

        float fwdAngle = Vector3.Angle(transform.forward, transform.TransformVector(copRef.transform.forward));
        float dirAngle = Vector3.Angle(transform.forward, transform.TransformVector(targetDirection));

        if ((dirAngle > 90f && fwdAngle < 20f) || driveRef.currentSpeed < 0.01f)
        {
            Seek(copRef.transform.position);
            return;
        }

        float predictedLocation = targetDirection.magnitude/(agentRef.speed + driveRef.currentSpeed);
        Seek(copRef.transform.position + copRef.transform.forward * predictedLocation);
    }

    private void Evade()
    {
        Vector3 targetDirection = copRef.transform.position - transform.position;
        float predictedLocation = targetDirection.magnitude / (agentRef.speed + driveRef.currentSpeed);
        Flee(copRef.transform.position + copRef.transform.forward * predictedLocation);
    }

    void Hide()
    {

        float dist = Mathf.Infinity;
        Vector3 chosenSpot = Vector3.zero;
        Vector3 chosenDir = Vector3.zero;
        GameObject chosenGO = HidingController.Instance.GetHidingObjects()[0];

        for (int i = 0; i < HidingController.Instance.GetHidingObjects().Count - 1; ++i)
        {

            Vector3 hideDir = HidingController.Instance.GetHidingObjects()[i].transform.position - copRef.transform.position;
            Vector3 hidePos = HidingController.Instance.GetHidingObjects()[i].transform.position + hideDir.normalized * 10.0f;

            if (Vector3.Distance(transform.position, hidePos) < dist)
            {

                chosenSpot = hidePos;
                chosenDir = hideDir;
                chosenGO = HidingController.Instance.GetHidingObjects()[i];
                dist = Vector3.Distance(transform.position, hidePos);
            }
        }

        Collider hideCol = chosenGO.GetComponent<Collider>();
        Ray backRay = new(chosenSpot, -chosenDir.normalized);
        float distance = 100.0f;
        hideCol.Raycast(backRay, out RaycastHit info, distance);

        Seek(info.point + chosenDir.normalized * 10.0f);
    }

    void Update()
    {
        //Seek(copRef.transform.position);
        //Flee(copRef.transform.position);
        //Pursue();
        //Evade();
        Hide();
    }
}

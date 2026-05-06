using UnityEngine;
using TMPro;

public class ChairAssemblyManager : MonoBehaviour
{
    [System.Serializable]
    public class AssemblyStep
    {
        public string stepName;
        public Transform[] parts;
        public Transform[] targets;

        [HideInInspector] public Vector3[] startPositions;
        [HideInInspector] public Quaternion[] startRotations;
    }

    [Header("Étapes de montage")]
    [SerializeField] private AssemblyStep[] steps;

    [Header("Animation")]
    [SerializeField] private float moveSpeed = 2.5f;
    [SerializeField] private float rotateSpeed = 4f;

    [Header("UI")]
    [SerializeField] private TMP_Text stepText;
    [SerializeField] private TMP_Text progressText;

    private int currentStep = 0;
    private bool isMoving = false;

    void Start()
    {
        SaveStartPositions();
        UpdateUI();
    }

    void Update()
    {
        if (!isMoving) return;

        AssemblyStep step = steps[currentStep - 1];
        bool allArrived = true;

        for (int i = 0; i < step.parts.Length; i++)
        {
            Transform part = step.parts[i];
            Transform target = step.targets[i];

            part.position = Vector3.MoveTowards(
                part.position,
                target.position,
                moveSpeed * Time.deltaTime
            );

            part.rotation = Quaternion.RotateTowards(
                part.rotation,
                target.rotation,
                rotateSpeed * 100f * Time.deltaTime
            );

            if (Vector3.Distance(part.position, target.position) > 0.01f)
            {
                allArrived = false;
            }
        }

        if (allArrived)
        {
            isMoving = false;
        }
    }

    public void NextStep()
    {
        if (isMoving) return;
        if (currentStep >= steps.Length) return;

        isMoving = true;
        currentStep++;

        UpdateUI();
    }

    public void ResetAssembly()
    {
        currentStep = 0;
        isMoving = false;

        foreach (AssemblyStep step in steps)
        {
            for (int i = 0; i < step.parts.Length; i++)
            {
                step.parts[i].position = step.startPositions[i];
                step.parts[i].rotation = step.startRotations[i];
            }
        }

        UpdateUI();
    }

    private void SaveStartPositions()
    {
        foreach (AssemblyStep step in steps)
        {
            step.startPositions = new Vector3[step.parts.Length];
            step.startRotations = new Quaternion[step.parts.Length];

            for (int i = 0; i < step.parts.Length; i++)
            {
                step.startPositions[i] = step.parts[i].position;
                step.startRotations[i] = step.parts[i].rotation;
            }
        }
    }

    private void UpdateUI()
    {
        if (stepText != null)
        {
            if (currentStep < steps.Length)
            {
                stepText.text = steps[currentStep].stepName;
            }
            else
            {
                stepText.text = "Montage terminé !";
            }
        }

        if (progressText != null)
        {
            progressText.text = "Progression : " + currentStep + " / " + steps.Length;
        }
    }
}
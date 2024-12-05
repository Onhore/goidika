using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Item : MonoBehaviour, IInteractable
{
    [SerializeField] protected string itemName;
    [TextArea(15,20)] [SerializeField] protected string description;
    [SerializeField] protected GameObject prefab;

    // Поля для регулировки смещения
    [SerializeField] private float offsetX = 1f;
    [SerializeField] private float offsetY = 0.5f;
    [SerializeField] private float offsetZ = 1f;
    [SerializeField] private float lerpSpeed = 5f;
    [SerializeField] private float dropForce =5f;

    // Поля для регулировки собственного вращения предмета
    [SerializeField] private Vector3 rotationOffset = Vector3.zero;

    public string Name => itemName;
    public string Description => description;
    public GameObject Prefab => prefab;

    private Rigidbody rigidbody;
    private Collider collider;

    private bool PickedUp = false;
    protected GameObject pickedUpBy;

    [SerializeField] protected UnityEvent OnClick;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
    }

    public void Interact()
    {
        Debug.Log("PickUp!");
        Player.instance.Inventory?.PickUpItem(this);
        pickedUpBy = Player.instance.gameObject;
    }

    public void PickUp()
    {
        PickedUp = true;
        rigidbody.isKinematic = true;
        collider.isTrigger = true;
    }

    public void Drop()
    {
        rigidbody.isKinematic = false;
        collider.isTrigger = false;
        PickedUp = false;
        rigidbody.AddForce(Camera.main.transform.forward * dropForce, ForceMode.Impulse);
    }

    private void Update()
    {
        if (PickedUp)
        {
            if(Input.GetMouseButtonDown(0))
                OnClick.Invoke();
            // Определяем целевую позицию и вращение предмета
            Vector3 cameraPosition = Camera.main.transform.position;
            Vector3 cameraForward = Camera.main.transform.forward;
            Vector3 cameraRight = Camera.main.transform.right;
            Vector3 cameraUp = Camera.main.transform.up;

            Vector3 targetPosition = cameraPosition + cameraForward * offsetZ + cameraRight * offsetX + cameraUp * offsetY;
            Quaternion targetRotation = Quaternion.LookRotation(cameraForward, cameraUp) * Quaternion.Euler(rotationOffset);

            // Плавно перемещаем и поворачиваем предмет
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * lerpSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * lerpSpeed);
        }
    }
}
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Barista : MonoBehaviour, IPointerClickHandler
{
    public static Barista Instance { get; private set; }
    [SerializeField] private Canvas orderCanvas;
    [SerializeField] private Canvas preparationCanvas;
    [SerializeField] private Transform readyConsumableSpawnPoint;
    
    public BaristaMenuHandler menuHandler;

    private bool _isPreparing = false;
    private bool _readyConsumableSpawnPointOccupied = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        HighscoreManager.Instance.FindUIElements();
        HighscoreManager.Instance.ResetScore();

        menuHandler = GetComponent<BaristaMenuHandler>();
        SetOrderCanvasActive(false);
        SetPreparationCanvasActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_isPreparing) return;

        if (eventData.pointerEnter != null &&
            eventData.pointerEnter.transform.IsChildOf(orderCanvas.transform))
        {
            return;
        }

        Debug.Log("Barista clicked");
        ToggleOrderCanvas();
    }

    private void SetPreparationCanvasActive(bool isActive)
    {
        preparationCanvas.gameObject.SetActive(isActive);
    }

    private void ToggleOrderCanvas()
    {
        orderCanvas.gameObject.SetActive(!orderCanvas.gameObject.activeSelf);
    }

    private void SetOrderCanvasActive(bool isActive)
    {
        orderCanvas.gameObject.SetActive(isActive);
    }

    public void ItemOrdered(Consumable consumable)
    {
        Debug.Log("Item ordered: " + consumable.name);
        if (_isPreparing) return;
        if (_readyConsumableSpawnPointOccupied)
        {
            Debug.Log("Ready consumable spawn point is occupied!");
            return;
        }
        SetOrderCanvasActive(false);
        Consumable preparedConsumable = Instantiate(consumable);
        StartCoroutine(PrepareConsumable(preparedConsumable));
    }

    private IEnumerator PrepareConsumable(Consumable consumable)
    {
        _isPreparing = true;

        consumable.transform.SetParent(preparationCanvas.transform, false);
        consumable.transform.localPosition = new Vector3(0f, -1.5f, 0f);
        consumable.transform.localScale = new Vector3(2f, 2f, 1.0f);

        var mask = consumable.transform.Find("Mask").GetComponent<SpriteMask>();

        float prepTime = consumable.difficulty.preparationTime;
        float elapsedTime = 0f;

        SetPreparationCanvasActive(true);

        Vector3 endPos = mask.transform.localPosition;
        float maskScaleOffset = mask.transform.localScale.y;
        Vector3 startPos = new Vector3(endPos.x, endPos.y - maskScaleOffset, endPos.z);

        mask.transform.localPosition = startPos;

        while (elapsedTime < prepTime)
        {
            elapsedTime += Time.deltaTime;

            float t = Mathf.Clamp01(elapsedTime / prepTime);
            mask.transform.localPosition = Vector3.Lerp(startPos, endPos, t);

            yield return null;
        }

        mask.transform.localPosition = endPos;
        SetPreparationCanvasActive(false);
        _isPreparing = false;
        ServeConsumable(consumable);
    }

    private void ServeConsumable(Consumable consumable)
    {
        _readyConsumableSpawnPointOccupied = true;
        consumable.transform.SetParent(null, true);
        consumable.transform.localScale = new Vector3(0.3f, 0.3f, 1.0f);
        Vector3 servePosition = readyConsumableSpawnPoint.position;
        consumable.transform.position = servePosition;
        consumable.isPreview = false;

        Debug.Log("Consumable served!");
    }

    public void FreeReadyConsumableSpawnPoint()
    {
        _readyConsumableSpawnPointOccupied = false;
    }
}

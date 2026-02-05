using System;
using DotsRts;
using DotsRts.MonoBehaviours;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

public class BuildingBarracksUI : MonoBehaviour
{
    [SerializeField] private Button _soldierButton;
    [SerializeField] private Button _scoutButton;
    [SerializeField] private Image _progressBarImage;
    [SerializeField] private RectTransform _unitQueueContainer;
    [SerializeField] private RectTransform _unitQueueTemplate;

    private Entity _buildingBarracksEntity;
    private EntityManager _entityManager;

    private void Awake()
    {
        _soldierButton.onClick.AddListener(() =>
        {
            _entityManager.SetComponentData(_buildingBarracksEntity, new BuildingBarracksUnitEnqueue
            {
                UnitType = UnitType.Soldier,
            });
            _entityManager.SetComponentEnabled<BuildingBarracksUnitEnqueue>(_buildingBarracksEntity, true);

            _unitQueueTemplate.gameObject.SetActive(false);
        });

        _scoutButton.onClick.AddListener(() =>
        {
            _entityManager.SetComponentData(_buildingBarracksEntity, new BuildingBarracksUnitEnqueue
            {
                UnitType = UnitType.Scout,
            });
            _entityManager.SetComponentEnabled<BuildingBarracksUnitEnqueue>(_buildingBarracksEntity, true);

            _unitQueueTemplate.gameObject.SetActive(false);
        });
    }

    private void Start()
    {
        UnitSelectionManager.Instance.OnSelectedEntitiesChanged += UnitSelectionManager_OnSelectedEntitiesChanged;
        DOTSEventsManager.Instance.OnBarracksUnitQueueChanged += DOTSEventManagers_OnBarracksUnitQueueChanged;

        Hide();
    }

    private void DOTSEventManagers_OnBarracksUnitQueueChanged(object sender, EventArgs e)
    {
        var entity = (Entity)sender;
        if (entity == _buildingBarracksEntity)
        {
            UpdateUnitQueueVisual();
        }
    }

    private void Update()
    {
        UpdateProgressBarVisual();
    }

    private void UnitSelectionManager_OnSelectedEntitiesChanged(object sender, EventArgs e)
    {
        _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        var entityQuery = new EntityQueryBuilder(Allocator.Temp).WithAll<Selected, BuildingBarracks>()
            .Build(_entityManager);

        var entityArray = entityQuery.ToEntityArray(Allocator.Temp);

        if (entityArray.Length > 0)
        {
            _buildingBarracksEntity = entityArray[0];

            Show();
            UpdateProgressBarVisual();
            UpdateUnitQueueVisual();
        }
        else
        {
            _buildingBarracksEntity = Entity.Null;

            Hide();
        }
    }

    private void UpdateProgressBarVisual()
    {
        if (_buildingBarracksEntity == Entity.Null)
        {
            _progressBarImage.fillAmount = 0f;
            return;
        }

        var buildingBarracks = _entityManager.GetComponentData<BuildingBarracks>(_buildingBarracksEntity);

        if (buildingBarracks.ActiveUnitType == UnitType.None)
        {
            _progressBarImage.fillAmount = 0f;
        }
        else
        {
            _progressBarImage.fillAmount = buildingBarracks.Progress / buildingBarracks.ProgressMax;
        }
    }

    private void UpdateUnitQueueVisual()
    {
        foreach (Transform child in _unitQueueContainer)
        {
            if (child == _unitQueueTemplate)
            {
                continue;
            }

            Destroy(child.gameObject);
        }

        var spawnUnitTypeDynamicBuffer = _entityManager.GetBuffer<SpawnUnitTypeBuffer>(_buildingBarracksEntity, true);
        foreach (var spawnUnitTypeBuffer in spawnUnitTypeDynamicBuffer)
        {
            var unitQueueRectTransform = Instantiate(_unitQueueTemplate, _unitQueueContainer);
            unitQueueRectTransform.gameObject.SetActive(true);

            var unitTypeSo = GameAssets.Instance.UnitTypeListSO.GetUnitTypeSO(spawnUnitTypeBuffer.UnitType);
            unitQueueRectTransform.GetComponent<Image>().sprite = unitTypeSo.Sprite;
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
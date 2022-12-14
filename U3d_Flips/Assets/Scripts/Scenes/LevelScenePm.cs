using System;
using System.Collections.Generic;
using Configs;
using Mouse;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Random = UnityEngine.Random;

public class LevelScenePm : IDisposable
{
    public struct Ctx
    {
        public Camera camera;
        public GameSet gameSet;
        public OperationsSet operationsSet;
        public ReactiveCommand<List<OperationTypes>> onSelectInteractable;
        public ReactiveCommand<OperationTypes> onInteractionButtonClick;
        public List<Texture2D> textures;
    }

    private Ctx _ctx;
    private List<InteractableEntity> _interactables;
    private ReactiveProperty<InteractableEntity> _current;
    private InteractableEntity _previous;
    private ReactiveCommand<OperationTypes> _onDoOperation;
    private CompositeDisposable _disposables;
    private ReactiveProperty<Vector3> _mousePosition;
    private ReactiveCommand _onDragObject;
    private ReactiveCommand<(InteractableEntity entity, bool isSelected)> _onColorChange;
    private bool _dragEnabled;
    private List<GameObject> _cashedObjects;
    public LevelScenePm(Ctx ctx)
    {
        _ctx = ctx;
        _disposables = new CompositeDisposable();
        _interactables = new List<InteractableEntity>();
        _current = new ReactiveProperty<InteractableEntity>();
        _mousePosition = new ReactiveProperty<Vector3>();
        _onDragObject = new ReactiveCommand();
        _onColorChange = new ReactiveCommand<(InteractableEntity entity, bool isSelected)>();
        _cashedObjects = new List<GameObject>();
        
        var dragOperation = _ctx.operationsSet.GetOperation(OperationTypes.Drag);
        _dragEnabled = dragOperation != null && dragOperation.enabled;

        var mouseHandler = new MouseHandler(new MouseHandler.Ctx
        {
            camera = _ctx.camera,
            current = _current,
            interactables = _interactables,
            onDragObject = _onDragObject,
            mousePosition = _mousePosition,
        }).AddTo(_disposables);

        _current.SkipLatestValueOnSubscribe().Subscribe(OnCurrentChange).AddTo(_disposables);
        _onDragObject.Subscribe(_ => OnDragObject()).AddTo(_disposables);
        _ctx.onInteractionButtonClick.Subscribe(OnInteractionButtonClick).AddTo(_disposables);

        CreateObjects();
    }

    private async void CreateObjects()
    {
        // TODO here can be used overlap box
        var handle = Addressables.InstantiateAsync(_ctx.gameSet.tableRef);
        var cashedTable = await handle.Task;
        _cashedObjects.Add(cashedTable);
        
        var tBounds = cashedTable.GetComponent<Renderer>().bounds;
        var tCenter = tBounds.center;
        var tExtents = tBounds.extents;

        var textures = new List<Texture2D>();
        textures.AddRange(_ctx.textures);

        foreach (var set in _ctx.gameSet.interactableSets)
        {
            handle = Addressables.LoadAssetAsync<GameObject>(set.interactableRef);
            var cashed = await handle.Task;
            _cashedObjects.Add(cashed);
            var interactable = cashed.GetComponent<InteractableView>();
            
            var oBounds = cashed.GetComponent<Renderer>().bounds;
            var oExtents = oBounds.extents;
            
            for (var i = 0; i < set.amount; i++)
            {
                var x = Random.Range(-tExtents.x + oExtents.x, tExtents.x - oExtents.x);
                var z = Random.Range(-tExtents.z + oExtents.z, tExtents.z - oExtents.z);
                var position = new Vector3(x, tExtents.y + oExtents.y + tExtents.y, z) + tCenter;

                Texture2D texture = RandomTexture(textures);

                var interactableEntity = new InteractableEntity(new InteractableEntity.Ctx
                {
                    camera = _ctx.camera,
                    prefab = interactable,
                    operationsSet = _ctx.operationsSet,
                    type = set.type,
                    operations = set.operations,
                    mousePosition = _mousePosition,
                    position = position,
                    extents = oExtents,
                    texture = texture,
                    onColorChange = _onColorChange,
                });

                _interactables.Add(interactableEntity);
            }
        }
    }

    private Texture2D RandomTexture(List<Texture2D> textures)
    {
        Texture2D texture;
        if (textures.Count > 0)
        {
            texture = textures[Random.Range(0, textures.Count)];
            textures.Remove(texture);
        }
        else
        {
            texture = null;
        }

        return texture;
    }

    private void OnCurrentChange(InteractableEntity current)
    {
        if (current == null)
        {
            if (_previous != null)
            {
                _onColorChange.Execute((_previous, false));
                _previous = null;
            }

            _ctx.onSelectInteractable.Execute(new List<OperationTypes>());
        }
        else
        {
            if (_previous != current)
            {
                _onColorChange.Execute((_previous, false));
                _previous = current;
                _onColorChange.Execute((current, true));
                _ctx.onSelectInteractable.Execute(current.Data.operations);
            }
        }
    }

    private void OnDragObject()
    {
        if (!_dragEnabled)
            return;

        // Debug.Log($"[LevelScenePm] OnMouseDrag, _current = {_current.Value.View.name} :  {_mousePosition}");
        // TODO The drag operation way to different from the rest so it is better to separate from common operations logic
        _current.Value?.DoOperation(OperationTypes.Drag);
    }

    private void OnInteractionButtonClick(OperationTypes operation)
    {
        Debug.Log($"[LevelScenePm] OnInteractionButtonClick");
        _current.Value?.DoOperation(operation);
    }

    private void UnloadAssets()
    {
        Debug.Log($"[{this}] Released Addressables");

        foreach (var cashedObject in _cashedObjects)
        {
            if (cashedObject == null) continue;

            cashedObject.SetActive(false);
            Addressables.ReleaseInstance(cashedObject);
        }
        
        _cashedObjects.Clear();
    }
    
    public void Dispose()
    {
        UnloadAssets();
        _disposables.Dispose();
    }
}
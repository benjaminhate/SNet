using SNet.Core.Models;

namespace SNet.Core
{
    public class SNetTransform : SNetEntity
    {
        public delegate void TransformChanged();

        public event TransformChanged OnTransformChanged;

        private void OnEnable()
        {
            OnTransformChanged += OnTransformChangedInternal;
        }

        private void OnDisable()
        {
            OnTransformChanged -= OnTransformChangedInternal;
        }

        protected override void Setup()
        {
            NetworkRouterRegister(OnClientTransformReceive);
        }

        private void Update()
        {
            if (!transform.hasChanged) return;
            transform.hasChanged = false;
            OnTransformChanged?.Invoke();
        }

        private void OnTransformChangedInternal()
        {
            if (IsServer)
            {
                ServerBroadcastSerializable(SNetTransformSerializer.Serialize(transform));
            }
        }

        private void OnClientTransformReceive(uint peerId, byte[] data)
        {
            SNetTransformSerializer.Deserialize(data, out var position, out var rotation, out var scale);
            var localTransform = transform;
            localTransform.position = position;
            localTransform.rotation = rotation;
            localTransform.localScale = scale;
        }
    }
}
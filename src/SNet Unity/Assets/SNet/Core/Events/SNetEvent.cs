using SNet.Core.Models.Router;

namespace SNet.Core.Events
{
    public abstract class SNetEvent : SNetEntity
    {
        protected RouterCallback clientReceive;
        protected RouterCallback serverReceive;

        protected new void Awake()
        {
            base.Awake();

            Setup();

            if (IsClient && clientReceive != null)
            {
                // TODO Change to NetworkRouter.RegisterClientCallback(identity.Id, clientEventCallbacks);
                NetworkRouterRegister(clientReceive);
                //NetworkRouter.Register(ChannelType.Base, HeaderType.Base, (value) => InternalClientReceive?.Invoke(value));
            }

            if (IsServer && serverReceive != null)
            {
                // TODO Change to NetworkRouter.RegisterServerCallback(identity.Id, serverEventCallbacks);
            }
        }

        protected abstract void Setup();
    }
}
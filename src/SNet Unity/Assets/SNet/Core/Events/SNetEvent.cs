using SNet.Core.Models.Router;

namespace SNet.Core.Events
{
    public abstract class SNetEvent : SNetEntity
    {
        protected RouterCallback ClientReceive;
        protected RouterCallback ServerReceive;

        protected new void Awake()
        {
            base.Awake();

            Setup();

            if (IsClient && ClientReceive != null)
            {
                NetworkRouterRegister(ClientReceive);
            }

            if (IsServer && ServerReceive != null)
            {
                NetworkRouterRegister(ServerReceive);
            }
        }

        protected abstract void Setup();
    }
}
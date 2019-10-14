using SNet.Core.Models;
using SNet.Core.Models.Router;

namespace SNet.Core.Events
{
    public abstract class SNetEvent : SNetEntity
    {
        protected RouterCallback ClientReceive;
        protected RouterCallback ServerReceive;

        public override void Setup()
        {
            PreSetup();

            if (IsClient && ClientReceive != null)
            {
                NetworkRouterRegister(ClientReceive);
            }

            if (IsServer && ServerReceive != null)
            {
                NetworkRouterRegister(ServerReceive);
            }
        }

        protected abstract void PreSetup();
    }
}
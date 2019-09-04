using System;
using System.Collections.Generic;

namespace SNet.Core.Models.Router
{
    public class TypedCallbackHandlers
    {
        public Type Type;
        public readonly List<CallbackHandler> Callbacks = new List<CallbackHandler>();
    }
}
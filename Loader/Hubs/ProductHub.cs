using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Loader.SqlServerNotifier;
using Microsoft.AspNet.SignalR.Hubs;

namespace Loader.Hubs
{
    [HubName("productHub")]
public class ProductHub : Hub
{
    internal NotifierEntity NotifierEntity { get; private set; }

    public void DispatchToClient()
    {
        Clients.All.broadcastMessage("Refresh");
    }

    public void Initialize(String value)
    {
        NotifierEntity = NotifierEntity.FromJson(value);
        if (NotifierEntity == null)
            return;
        Action<String> dispatcher = (t) => { DispatchToClient(); };
        PushSqlDependency.Instance(NotifierEntity, dispatcher);
    }
}
}
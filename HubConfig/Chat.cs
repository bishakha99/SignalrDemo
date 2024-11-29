using SignalrDemo.HubModels;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
namespace SignalrDemo.HubConfig
{
  public partial class MyHub
  {
    //public async Task getOnlineUsers()
    //{
    //  Guid? currUserId = ctx.Connections.Where(c => c.SignalrId == Context.ConnectionId).Select(c => c.PersonId).SingleOrDefault();
    //  List<User> onlineUsers = ctx.Connections
    //      .Where(c => c.PersonId != currUserId)
    //      .Select(c =>
    //          new User(c.PersonId, ctx.People.Where(p => p.Id == c.PersonId).Select(p => p.Name).SingleOrDefault(), c.SignalrId)
    //      ).ToList();
    //  await Clients.Caller.SendAsync("getOnlineUsersResponse", onlineUsers);
    //}
    public async Task getOnlineUsers()
    {
      var currSignalrID = Context.ConnectionId;
      var currUserId = ctx.Connections.Where(c => c.SignalrId == currSignalrID).Select(c => c.PersonId).FirstOrDefault();

      var onlineUsers = ctx.Connections
          .Where(c => c.PersonId != currUserId)
          .Select(c => new User(
              c.PersonId,
              ctx.People.Where(p => p.Id == c.PersonId).Select(p => p.Name).FirstOrDefault(),
              c.SignalrId
          )).ToList();

      await Clients.Caller.SendAsync("getOnlineUsersResponse", onlineUsers);
    }
    //public async Task sendMsg(string connId, string msg)
    //{
    //  await Clients.Client(connId).SendAsync("sendMsgResponse", Context.ConnectionId, msg);
    //}
    public async Task sendMsg(string connId, string msg)
    {
      Console.WriteLine($"sendMsg called with connId: {connId}, msg: {msg}");
      await Clients.Client(connId).SendAsync("sendMsgResponse", Context.ConnectionId, msg);
    }

  }
}

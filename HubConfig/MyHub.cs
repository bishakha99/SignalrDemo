using Microsoft.AspNetCore.SignalR;
using SignalrDemo.DAL;
using SignalrDemo.EFModels;

using SignalrDemo.HubModels;

namespace SignalrDemo.HubConfig
{
    public partial class MyHub : Hub

    {
    private readonly SignalrContext ctx;

    public MyHub(SignalrContext context)
    {
      ctx = context;
    }
    public override Task OnDisconnectedAsync(Exception exception)
    {
      Guid? currUserId = ctx.Connections.Where(c => c.SignalrId == Context.ConnectionId).Select(c => c.PersonId).SingleOrDefault();
      ctx.Connections.RemoveRange(ctx.Connections.Where(p => p.PersonId == currUserId).ToList());
      ctx.SaveChanges();
      Clients.Others.SendAsync("userOff", currUserId);
      return base.OnDisconnectedAsync(exception);
    }


    //2Tutorial
    //public async Task authMe(PersonInfo personInfo)
    //{
    //  string currSignalrID = Context.ConnectionId;
    //  Person tempPerson = ctx.People.Where(p => p.Username == personInfo.userName && p.Password == personInfo.password)
    //      .SingleOrDefault();

    //  if (tempPerson != null) //if credentials are correct
    //  {
    //    Console.WriteLine("\n" + tempPerson.Name + " logged in" + "\nSignalrID: " + currSignalrID);

    //    Connection currUser = new Connection
    //    {
    //      PersonId = tempPerson.Id,
    //      SignalrId = currSignalrID,
    //      TimeStamp = TimeOnly.FromDateTime(DateTime.Now)
    //    };
    //    await ctx.Connections.AddAsync(currUser);
    //    await ctx.SaveChangesAsync();

    //    User newUser = new User(tempPerson.Id, tempPerson.Name, currSignalrID);
    //    await Clients.Caller.SendAsync("authMeResponseSuccess", newUser);//4Tutorial
    //    await Clients.Others.SendAsync("userOn", newUser);//4Tutorial
    //  }

    //  else //if credentials are incorrect
    //  {
    //    await Clients.Caller.SendAsync("authMeResponseFail");
    //  }
    //}
    //<__________new
    //public async Task authMe(PersonInfo personInfo)
    //{
    //  string currSignalrID = Context.ConnectionId;
    //  Person tempPerson = ctx.People.Where(p => p.Username == personInfo.userName && p.Password == personInfo.password)
    //      .SingleOrDefault();

    //  if (tempPerson != null) // if credentials are correct
    //  {
    //    Console.WriteLine("\n" + tempPerson.Name + " logged in" + "\nSignalrID: " + currSignalrID);

    //    // Logging to check person name and ID
    //    Console.WriteLine($"Sending data: personId = {tempPerson.Id}, personName = {tempPerson.Name}");

    //    Connection currUser = new Connection
    //    {
    //      PersonId = tempPerson.Id,
    //      SignalrId = currSignalrID,
    //      TimeStamp = TimeOnly.FromDateTime(DateTime.Now)
    //    };
    //    await ctx.Connections.AddAsync(currUser);
    //    await ctx.SaveChangesAsync();

    //    User newUser = new User(tempPerson.Id, tempPerson.Name, currSignalrID);
    //    // Ensure you're sending the correct values to the client
    //    await Clients.Caller.SendAsync("authMeResponseSuccess", newUser.id.ToString(), newUser.name);
    //    await Clients.Others.SendAsync("userOn", newUser);
    //  }
    //  else // if credentials are incorrect
    //  {
    //    await Clients.Caller.SendAsync("authMeResponseFail");
    //  }
    //}


    //2Tutorial
    public async Task authMe(PersonInfo personInfo)
    {
      string currSignalrID = Context.ConnectionId;
      Person tempPerson = ctx.People.Where(p => p.Username == personInfo.userName && p.Password == personInfo.password)
          .SingleOrDefault();

      if (tempPerson != null) //if credentials are correct
      {
        Console.WriteLine("\n" + tempPerson.Name + " logged in" + "\nSignalrID: " + currSignalrID);

        Connection currUser = new Connection
        {
          PersonId = tempPerson.Id,
          SignalrId = currSignalrID,
          TimeStamp = TimeOnly.FromDateTime(DateTime.Now) 
        };
        await ctx.Connections.AddAsync(currUser);
        await ctx.SaveChangesAsync();

        User newUser = new User(tempPerson.Id, tempPerson.Name, currSignalrID);
        await Clients.Caller.SendAsync("authMeResponseSuccess", newUser);//4Tutorial
        await Clients.Others.SendAsync("userOn", newUser);//4Tutorial
      }

      else //if credentials are incorrect
      {
        await Clients.Caller.SendAsync("authMeResponseFail");
      }
    }
    //public async Task authMe(PersonInfo personInfo)
    //{
    //  try
    //  {
    //    string currSignalrID = Context.ConnectionId;

    //    // Validate inputs
    //    if (personInfo == null || string.IsNullOrWhiteSpace(personInfo.userName) || string.IsNullOrWhiteSpace(personInfo.password))
    //    {
    //      Console.WriteLine("Invalid input received for authMe.");
    //      await Clients.Caller.SendAsync("authMeResponseFail");
    //      return;
    //    }

    //    // Fetch user from database
    //    Person tempPerson = ctx.People
    //        .Where(p => p.Username == personInfo.userName && p.Password == personInfo.password)
    //        .SingleOrDefault();

    //    if (tempPerson != null) // If credentials are correct
    //    {
    //      Console.WriteLine("\n" + tempPerson.Name + " logged in" + "\nSignalrID: " + currSignalrID);
    //      Console.WriteLine($"Sending data: personId = {tempPerson.Id}, personName = {tempPerson.Name}");

    //      // Save connection info
    //      Connection currUser = new Connection
    //      {
    //        PersonId = tempPerson.Id,
    //        SignalrId = currSignalrID,
    //        TimeStamp = TimeOnly.FromDateTime(DateTime.Now)
    //      };

    //      await ctx.Connections.AddAsync(currUser);
    //      await ctx.SaveChangesAsync();

    //      User newUser = new User(tempPerson.Id, tempPerson.Name, currSignalrID);

    //      // Ensure you're sending correct data to the client
    //      await Clients.Caller.SendAsync("authMeResponseSuccess", newUser);
    //      await Clients.Others.SendAsync("userOn", newUser);
    //    }
    //    else // If credentials are incorrect
    //    {
    //      Console.WriteLine("Invalid credentials.");
    //      await Clients.Caller.SendAsync("authMeResponseFail");
    //    }
    //  }
    //  catch (Exception ex)
    //  {
    //    Console.WriteLine($"Error in authMe: {ex.Message}");
    //    await Clients.Caller.SendAsync("authMeResponseFail");
    //  }
    //}


    //3Tutorial
    public async Task reauthMe(Guid? personId)
    {
      string currSignalrID = Context.ConnectionId;
      Person tempPerson = ctx.People.Where(p => p.Id == personId)
          .SingleOrDefault();

      if (tempPerson != null) //if credentials are correct
      {
        Console.WriteLine("\n" + tempPerson.Name + " logged in" + "\nSignalrID: " + currSignalrID);

        Connection currUser = new Connection
        {
          PersonId = tempPerson.Id,
          SignalrId = currSignalrID,
          TimeStamp = TimeOnly.FromDateTime(DateTime.Now)
        };
        await ctx.Connections.AddAsync(currUser);
        await ctx.SaveChangesAsync();

        User newUser = new User(tempPerson.Id, tempPerson.Name, currSignalrID);
        await Clients.Caller.SendAsync("reauthMeResponse", newUser);//4Tutorial
        await Clients.Others.SendAsync("userOn", newUser);//4Tutorial
      }
    } //end of reauthMe
    //public async Task reauthMe(string personId)
    //{
    //  try
    //  {
    //    // Ensure that personId is a valid Guid (assuming user.Id is Guid)
    //    if (Guid.TryParse(personId, out Guid personGuid))
    //    {
    //      var user = ctx.People.SingleOrDefault(p => p.Id == personGuid);
    //      if (user == null)
    //      {
    //        throw new Exception("User not found");
    //      }

    //      // Proceed with re-authentication logic
    //      await Clients.Caller.SendAsync("reauthMeResponse", user.Id, user.Name);
    //    }
    //    else
    //    {
    //      throw new Exception("Invalid personId format");
    //    }
    //  }
    //  catch (Exception ex)
    //  {
    //    // Log the error and notify the client
    //    Console.Error.WriteLine(ex.Message);
    //    await Clients.Caller.SendAsync("reauthMeResponse", null, null);  // Send null values to indicate failure
    //  }
    //}

    //4Tutorial
    public void logOut(Guid personId)
    {
      ctx.Connections.RemoveRange(ctx.Connections.Where(p => p.PersonId == personId).ToList());
      ctx.SaveChanges();
      Clients.Caller.SendAsync("logoutResponse");
      Clients.Others.SendAsync("userOff", personId);
    }
  }
}

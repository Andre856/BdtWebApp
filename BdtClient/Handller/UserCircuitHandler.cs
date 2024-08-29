//using BdtClient.AppServices.User;
//using Microsoft.AspNetCore.Components.Authorization;
//using Microsoft.AspNetCore.Components.Server.Circuits;
//using System.Security.Claims;

//namespace BdtClient.Handller;

//class UserCircuitHandler : CircuitHandler, IDisposable
//{
//    private readonly AuthenticationStateProvider _authenticationStateProvider;
//    private readonly IUserService _userService;
//    private readonly ILogger _logger;

//    public UserCircuitHandler(AuthenticationStateProvider authenticationStateProvider, 
//        IUserService userService, ILogger<UserCircuitHandler> logger)
//    {
//        _authenticationStateProvider = authenticationStateProvider;
//        _userService = userService;
//        _logger = logger;
//    }

//    public override Task OnCircuitOpenedAsync(Circuit circuit, CancellationToken cancellationToken)
//    {
//        _authenticationStateProvider.AuthenticationStateChanged += task => AuthenticationStateChanged(task, circuit);
//        return base.OnCircuitOpenedAsync(circuit, cancellationToken);
//    }

//    private void AuthenticationStateChanged(Task<AuthenticationState> task, Circuit circuit) 
//    {
//        _ = UpdateAuthentication(task, circuit);

//        async Task UpdateAuthentication(Task<AuthenticationState> task, Circuit circuit)
//        {
//            try
//            {
//                var state = await task;
//                _userService.SetUser(state.User);
//                UserStaticConnections.ConnectedUsers.Add(new()
//                {
//                    CircuitId = circuit.Id,
//                    User = state.User,
//                    ConnectionStartDate = DateTime.UtcNow
//                });
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error updating the authentication.");
//            }
//        }
//    }

//    public override async Task OnConnectionUpAsync(Circuit circuit, CancellationToken cancellationToken)
//    {
//        var state = await _authenticationStateProvider.GetAuthenticationStateAsync();

//        UserStaticConnections.ConnectedUsers.Add(new()
//        {
//            CircuitId = circuit.Id,
//            User = state.User,
//            ConnectionStartDate = DateTime.UtcNow
//        });

//        _userService.SetUser(state.User);
//    }

//    public override async Task OnConnectionDownAsync(Circuit circuit, CancellationToken cancellationToken)
//    {
//        UserStaticConnections.ConnectedUsers.RemoveAll(x => x.CircuitId == circuit.Id);
//        //return base.OnConnectionDownAsync(circuit, cancellationToken);
//    }

//    public void Dispose()
//    {
//        _authenticationStateProvider.AuthenticationStateChanged -= task => AuthenticationStateChanged(task, null);
//    }

//    private static class UserStaticConnections
//    {
//        public static List<UserSessionModel> ConnectedUsers = [];
//    }

//    private class UserSessionModel
//    {
//        public string CircuitId { get; set; }
//        public ClaimsPrincipal User { get; set; }
//        public DateTime ConnectionStartDate { get; set; }
//    }
//}

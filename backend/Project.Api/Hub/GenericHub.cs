using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Project.CrossCutting.Base;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Project.CrossCutting.DTO;

namespace Project.Api.Hub
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class GenericHub<T> : Hub<T> where T : class
    {

        private readonly IHubConnection _hubConnection;
        private readonly string _hubName;

        public GenericHub(IHubConnection hubConnection, string hubName)
        {
            _hubConnection = hubConnection;
            _hubName = hubName;
        }

        #region Utils

        public IReadOnlyList<string> FilterPlayers(Func<KeyValuePair<string, AuthHubDto>, bool> predicate)
        {
            bool Query(KeyValuePair<string, AuthHubDto> x) => x.Value.Hub == _hubName && predicate(x);
            return _hubConnection.FilterPlayers(Query);
        }

        public dynamic GetUser(string connection)
        {
            return _hubConnection.GetUser(connection);
        }
        #endregion


        #region OnConnectDisconnect

        public override async Task OnConnectedAsync()
        {
            Console.ResetColor();
            var authKey = Context.User.Identity.Name;

            if (!string.IsNullOrEmpty(authKey))
            {
                var players = FilterPlayers(t => t.Value.AuthKey == authKey);

                foreach (var player in players)
                {
                    _hubConnection.RemovePlayer(player);
                    await Groups.RemoveFromGroupAsync(player, "Players");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine($"Desconectado {_hubName} - ConnectionId:{player} - AuthKey:{authKey}");
                }

                var authDto = new AuthHubDto
                {
                    AuthKey = authKey,
                    Hub = _hubName
                };

                _hubConnection.AddOrUpdate(Context.ConnectionId, authDto);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Conectado {_hubName} - ConnectionId:{Context.ConnectionId} - - AuthKey:{authKey}");

                await base.OnConnectedAsync();
            }
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Desconectado {_hubName} - ConnectionId:{Context.ConnectionId}");
            _hubConnection.RemovePlayer(Context.ConnectionId);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "Players");
            await base.OnDisconnectedAsync(exception);
        }

        #endregion OnConnectDisconnect

        protected async Task OnConnectedBaseAsync()
        {
            await base.OnConnectedAsync();
        }
    }
}

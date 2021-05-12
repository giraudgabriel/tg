using Project.CrossCutting.Base;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Project.CrossCutting.DTO;

namespace Project.Service
{
    public class HubConnections : IHubConnection
    {
        private static readonly ConcurrentDictionary<string, AuthHubDto> PlayersDictionary = new();

        public void AddOrUpdate(string connectionId, AuthHubDto authDto)
        {
            PlayersDictionary.AddOrUpdate(connectionId, authDto, (_, _) => authDto);
            RemoveOtherConnections(connectionId, authDto);
        }

        private void RemoveOtherConnections(string connectionId, AuthHubDto authDto)
        {
            var players = PlayersDictionary
                .Where(x => x.Value.AuthKey == authDto.AuthKey
                            && x.Value.Hub == authDto.Hub
                            && x.Key != connectionId)
                .Select(x => x.Key);
            Parallel.ForEach(players, RemovePlayer);
        }

        public IReadOnlyList<string> FilterPlayers(Func<KeyValuePair<string, AuthHubDto>, bool> predicate) =>
            PlayersDictionary.Where(predicate).Select(p => p.Key).ToList().AsReadOnly();

        public AuthHubDto GetUser(string connectionId)
        {
            return PlayersDictionary.FirstOrDefault(c => c.Key == connectionId).Value;
        }

        public void RemovePlayer(string player)
        {
            PlayersDictionary.TryRemove(player, out _);
        }
    }
}

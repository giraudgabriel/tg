using System;
using System.Collections.Generic;
using Project.CrossCutting.DTO;

namespace Project.CrossCutting.Base
{
    public interface IHubConnection
    {
        IReadOnlyList<string> FilterPlayers(Func<KeyValuePair<string, AuthHubDto>, bool> predicate);
        void RemovePlayer(string player);
        void AddOrUpdate(string connectionId, AuthHubDto authDto);
        AuthHubDto GetUser(string connectionId);
    }
}

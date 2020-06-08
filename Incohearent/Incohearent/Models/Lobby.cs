using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Incohearent.Models
{
    public class Lobby
    {
        [PrimaryKey]
        [AutoIncrement]
        public string LobbyId { get; set; }
        public string GatewayIp { get; set; }
        public string IspIp { get; set; }
        public string UserId { get; set; }

        public Lobby()
        {

        }
    }
}

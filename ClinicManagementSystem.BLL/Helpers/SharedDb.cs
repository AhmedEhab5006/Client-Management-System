using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.BLL.Helpers
{
    public class SharedDb
    {

        private readonly ConcurrentDictionary<string, UserConnection> _connection = new ConcurrentDictionary<string, UserConnection>();

        public ConcurrentDictionary<string, UserConnection> Connection => _connection;

    }
}

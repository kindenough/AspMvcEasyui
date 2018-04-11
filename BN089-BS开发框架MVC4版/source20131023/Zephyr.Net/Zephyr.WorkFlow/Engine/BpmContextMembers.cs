using System;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using System.Reflection;
using Zephyr.Data;

namespace Zephyr.WorkFlow
{
    public partial class BpmContext
    {
        private string _currentActor;
        private IDbContext _db;
        private BpmConfiguration _bpmConfiguration;

        public BpmConfiguration getBpmConfiguration()
        {
            return _bpmConfiguration;
        }
        public BpmContext SetCurrentActor(string actor) 
        {
            _currentActor = actor;
            return this;
        }
    }
}

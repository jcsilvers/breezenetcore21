using Breeze.Persistence.EFCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace breezenetcore21.Contexts
{
    public class PersistenceManager : EFPersistenceManager<MITSContext>
    {
        public PersistenceManager(MITSContext context) : base(context) { }
    }
}

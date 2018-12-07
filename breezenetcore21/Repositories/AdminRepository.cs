using breezenetcore21.Contexts;
using breezenetcore21.Repositories.Interfaces;
using Breeze.AspNetCore;
using Breeze.Persistence.EFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace breezenetcore21.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly MITSContext context;
        private PersistenceManager persistenceManager;
        

        public AdminRepository(MITSContext context)
        {
            this.context = context;
            persistenceManager = new PersistenceManager(context);

        }

        public string GetMetadata
        {
            get
            {
                return persistenceManager.Metadata();
            }
        }
    }
}

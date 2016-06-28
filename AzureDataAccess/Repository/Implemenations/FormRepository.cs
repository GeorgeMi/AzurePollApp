/* Copyright (C) Miron George - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Miron George <george.miron2003@gmail.com>, 2016
 *
 * Role:
 * Repository implementation. 
 *
 * History:
 * 09.02.2016    Miron George       Created class.
 */
using Entities;
using AzureDataAccess.Repository.Interfaces;
using AzureDataAccess.Context;

namespace AzureDataAccess.Repository.Implementations
{
    /// <summary>
    /// Repository implementare interfata "IFormRepository"
    /// </summary>
    public class FormRepository : GenericRepository<Form>, IFormRepository
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public FormRepository(AzurePollAppDBContext context) : base(context)
        {
        }

        /// <summary>
        /// Inchidere sondaje expirate
        /// </summary>
        public void ScheduleUpdateForms()
        {
            Context.Database.ExecuteSqlCommand("update [dbo].[Form] set State = 'closed' where Deadline < SYSDATETIME()");
        }

        /// <summary>
        /// Votare sondaj
        /// </summary>
        /// <param name="id">sondajul careia i s-a acordat votul</param>
        public void AddVote(int id)
        {
            Form f = Context.Forms.Find(id);
            f.NrVotes = f.NrVotes + 1;
            Context.SaveChanges();
        }
    }
}

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
using System;

namespace AzureDataAccess.Repository.Implementations
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(AzurePollAppDBContext context) : base(context)
        {

        }

        public void ChangeRole(int userID, string role)
        {
            User u = Context.Users.Find(userID);
            u.Role = role;

            Context.SaveChanges();
        }
    }
}

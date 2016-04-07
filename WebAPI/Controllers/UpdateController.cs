/* Copyright (C) Miron George - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Miron George <george.miron2003@gmail.com>, 2016
 */
using System.Web.Http;
using WebAPI.ActionFilters;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    /// <summary>
    /// handles HTTP database update requests
    /// </summary>
    public class UpdateController : ApiController
    {
        /// <summary>
        /// delete unverified accounts and set to closed outdated forms
        /// </summary>
        [RequirePasswordForScheduler]
        public void Get()
        {
            UsersModel model = new UsersModel();

            model.ScheduleUpdates();
        }
    }
}
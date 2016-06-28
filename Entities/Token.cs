/* Copyright (C) Miron George - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Miron George <george.miron2003@gmail.com>, 2016
 */

namespace Entities
{
    /// <summary>
    /// Clasa ce corespunde tabelului "Token" din baza de date
    /// </summary>
    public partial class Token
    {
        public int TokenID { get; set; }
        public int UserID { get; set; }
        public string TokenString { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.DateTime ExpirationDate { get; set; }
        public virtual User User { get; set; }
    }
}

using DataLayer.UnitOfWork;
using System;

namespace DataLayer.Domin.Models.Identity
{
    public class RelationBetweenUser
    {
        public virtual int Id { get; set; }


        public Guid RequestedById { get; set; }
        public Guid RequestedToId { get; set; }

        //public virtual User RequestedBy { get; set; }

        //public virtual User RequestedTo { get; set; }

        public DateTime? RequestTime { get; set; }

        public FriendRequestFlag FriendRequestFlag { get; set; }
    }
}

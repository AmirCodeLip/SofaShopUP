using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Domin
{
    public enum FriendRequestFlag
    {
        None,
        Approved,
    };

    public enum RelationStatus
    {
        outgoing, incoming
    }

    public enum ActionStatus
    {
        Success, NotFound, NotValid
    }

   
}

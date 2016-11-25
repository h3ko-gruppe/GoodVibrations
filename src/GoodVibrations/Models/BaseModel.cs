using System;
using Realms;

namespace GoodVibrations.Models
{
    public class BaseModel : RealmObject
    {
        public Guid? Id {
            get;
            set;
        }
    }
}

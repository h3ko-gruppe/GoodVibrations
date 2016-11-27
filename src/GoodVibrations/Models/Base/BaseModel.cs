using System;
using SQLite.Net.Attributes;

namespace GoodVibrations.Models.Base
{
    public class BaseModel : ReactiveUI.ReactiveObject
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id {
            get;
            set;
        }
    }
}
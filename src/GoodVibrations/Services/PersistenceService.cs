using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GoodVibrations.Models;
using Realms;

namespace GoodVibrations.Services
{
    public class PersistenceService
    {
        public PersistenceService ()
        {
            _realm = Realm.GetInstance ();
        }

        private readonly Realm _realm;

        public IEnumerable<T> LoadWhere<T> (Expression<Func<T, bool>> filter) where T : BaseModel, new()
        {
            return _realm.All<T> ().Where(filter);
        }

        public IEnumerable<T> LoadAll<T> () where T : BaseModel
        {
            return _realm.All<T> ();
        }

        public T LoadFirstOrDefault<T> (Expression<Func<T, bool>> filter) where T : BaseModel, new()
        {
            return _realm.All<T> ().Where (filter).FirstOrDefault();
        }

        public async Task<Guid> Save<T>(T item) where T : BaseModel, new()
        {
            if (!item.Id.HasValue) 
            {
                //create new id
                item.Id = Guid.NewGuid ();
            }


            //https://realm.io/
           // _realm.
           // var mydog = _realm.CreateObject<T> ();
           // await _realm.WriteAsync (_ => item);


            _realm.Write (() => {
                
                var mydog = _realm.CreateObject<Sample> ();

            });

            return item.Id.Value;
        }

    }
}

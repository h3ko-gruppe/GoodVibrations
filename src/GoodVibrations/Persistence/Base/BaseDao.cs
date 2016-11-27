using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Extensions;
using SQLite.Net;
using GoodVibrations.Models.Base;

namespace GoodVibrations.Persistence.Base
{
    /// <summary>
    /// Base DAO.
    /// </summary>
    public abstract class BaseDao<TModel> where TModel : BaseModel, new()
    {
        /// <summary>
        /// Get the current connection.
        /// </summary>
        /// <returns>The connection.</returns>
        protected abstract SQLiteConnection Connection { get; }

        /// <summary>
        /// Loads the by identifier.
        /// </summary>
        /// <returns>The by identifier.</returns>
        /// <param name="id">Identifier.</param>
        public virtual TModel LoadById (int id)
        {
            lock (Connection) {
                try {
                    var result = Connection.Find<TModel> (id);
                    return result;
                } catch (Exception ex) {
                    DebugTraceListener.Instance.WriteLine (GetType ().Name, $"Cannot find item with ID: {id}\n\n{ex}");
                    return null;
                }
            }
        }

        /// <summary>
        /// Delete the specified model.
        /// </summary>
        /// <param name="model">Model.</param>
        public virtual int Delete (TModel model)
        {
            return DeleteById (model.Id);
        }

        /// <summary>
        /// Deletes the by identifier.
        /// </summary>
        /// <returns>The by identifier.</returns>
        /// <param name="id">Identifier.</param>
        public virtual int DeleteById (int id)
        {
            lock (Connection) {
                var result = Connection.Delete<TModel> (id);
                return result;
            }
        }

        /// <summary>
        /// Loads all.
        /// </summary>
        /// <returns>The all.</returns>
        public virtual List<TModel> LoadAll ()
        {
            lock (Connection) {
                var result = Connection.Table<TModel> ().ToList ();
                return result;
            }
        }

        /// <summary>
        /// Inserts the or replace.
        /// </summary>
        /// <returns>The or replace.</returns>
        /// <param name="obj">Object.</param>
        public virtual int InsertOrReplace (TModel obj)
        {
            int result = InvalidId;
            lock (Connection) {
                Connection.BeginTransaction ();
                try {
                    var rowsAffected = Connection.Update (obj);
                    if (rowsAffected == 0) {
                        // The item does not exists in the database so lets insert it
                        rowsAffected = Connection.Insert (obj);
                    }
                    Connection.Commit ();
					result = obj.Id;
                } catch (Exception ex) {
                    Connection.Rollback ();
                    DebugTraceListener.Instance.WriteLine (GetType ().Name, ex.ToString ());
                }

                return result;
            }
        }

        /// <summary>
        /// Inserts the or replace.
        /// </summary>
        /// <param name="objs">Objects.</param>
        public virtual void InsertOrReplace (IEnumerable<TModel> objs)
        {
            foreach (var obj in objs) {
                InsertOrReplace (obj);
            }
        }

        /// <summary>
        /// Inserts all items in a single transaction
        /// </summary>
        /// <param name="objs">Objects.</param>
        public virtual long InsertBulk (IEnumerable<TModel> objs)
        {
            long rowsAffected = 0;
            lock (Connection) {
                Connection.BeginTransaction ();
                try {
                    rowsAffected = objs.Sum (obj => Connection.Insert (obj));
                    Connection.Commit ();
                } catch (Exception ex) {
                    Connection.Rollback ();
                    DebugTraceListener.Instance.WriteLine (GetType ().Name, ex.ToString ());
                }

                return rowsAffected;
            }
        }

        /// <summary>
        /// Invalid Id.
        /// </summary>
        public virtual int InvalidId => -1;

        /// <summary>
        /// Loads the children.
        /// </summary>
        /// <param name="item">Item.</param>
        public virtual void LoadChildren (TModel item)
        {
            lock (Connection) {
                Connection.GetChildren (item, true);
            }
        }

        /// <summary>
        /// Loads the child.
        /// </summary>
        /// <param name="obj">Object.</param>
        /// <param name="property">Property.</param>
        /// <param name="recursive">If set to <c>true</c> recursive.</param>
        public virtual void LoadChild (TModel obj, Expression<Func<TModel, object>> property, bool recursive = false)
        {
            lock (Connection) {
                Connection.GetChild (obj, property, recursive);
            }
        }


        /// <summary>
        /// Loads the by identifier with children.
        /// </summary>
        /// <returns>The by identifier with children.</returns>
        /// <param name="id">Identifier.</param>
        public virtual TModel LoadByIdWithChildren (int id)
        {
            lock (Connection) {
                var result = Connection.GetWithChildren<TModel> (id);
                return result;
            }
        }

        /// <summary>
        /// Loads all with children.
        /// </summary>
        /// <returns>The all with children.</returns>
        public virtual List<TModel> LoadAllWithChildren ()
        {
            lock (Connection) {
                var result = LoadAll ();
                foreach (var item in result) {
                    try {
                        Connection.GetChildren (item);
                    } catch (Exception e) {
                        DebugTraceListener.Instance.WriteLine (GetType ().Name,
                             "Can not load Child for itemType: " + item.GetType ().Name + " reason: " + e);
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// Loads the where.
        /// </summary>
        /// <returns>The where.</returns>
        /// <param name="expression">Expression.</param>
        public virtual List<TModel> LoadWhere (Expression<Func<TModel, bool>> expression)
        {
            lock (Connection) {
                var result = Connection.Table<TModel> ().Where (expression).ToList ();
                return result;
            }
        }

        /// <summary>
        /// Gets the name of the tablen.
        /// </summary>
        /// <value>The name of the tablen.</value>
        public static string TablenName {
            get {
                string result;
                if (!GetNameFromTableAttribute (out result))
                    result = typeof (TModel).Name;

                return result;
            }
        }

        /// <summary>
        /// Gets the name from table attribute.
        /// </summary>
        /// <returns><c>true</c>, if name from table attribute was gotten, <c>false</c> otherwise.</returns>
        /// <param name="name">Name.</param>
        protected static bool GetNameFromTableAttribute (out string name)
        {
            name = string.Empty;
            try {
                var tableAttribute = typeof (TModel).GetAttribute<TableAttribute> ();
                if (tableAttribute != null) {
                    name = tableAttribute.Name;
                    return true;
                }
                return false;
            } catch (Exception) {
                return false;
            }
        }

        /// <summary>
        /// Exists the specified id.
        /// </summary>
        /// <param name="id">Identifier.</param>
        public virtual bool Exists (int id)
        {
            lock (Connection) {
                try {
                    var count = Connection.ExecuteScalar<int> (
                        $"SELECT EXISTS(SELECT 1 FROM {TablenName} WHERE Id={id} LIMIT 1);");
                    return count > 0;
                } catch (Exception) {
                    throw;
                }
            }
        }

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <returns>The count.</returns>
        public virtual int GetCount ()
        {
            lock (Connection) {
                try {
                    var count = Connection.ExecuteScalar<int> ($"SELECT count(*) FROM {TablenName} AS Count");
                    return count;
                } catch (Exception) {
                    throw;
                }
            }
        }

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <returns>The count.</returns>
        /// <param name="expression">Expression.</param>
        public virtual int GetCount (Expression<Func<TModel, bool>> expression)
        {
            lock (Connection) {
                var result = Connection.Table<TModel> ().Count (expression);
                return result;
            }
        }
    }
}
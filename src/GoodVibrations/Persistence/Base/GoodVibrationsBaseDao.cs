using GoodVibrations.Models.Base;
using GoodVibrations.Persistence.ConnectionFactories;
using SQLite.Net;

namespace GoodVibrations.Persistence.Base
{
    /// <summary>
    /// Base DAO.
    /// </summary>
    public class GoodVibrationsBaseDao<TModel> : BaseDao<TModel> where TModel : BaseModel, new()
    {
        /// <summary>
        /// Get the current connection.
        /// </summary>
        /// <returns>The connection.</returns>
		protected override SQLiteConnection Connection => GoodVibrationsConnectionFactory.Instance.GetConnection ();
    }
}
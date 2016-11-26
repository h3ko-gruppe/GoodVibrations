using System;
using GoodVibrations.Utils;
using SQLite.Net;
using SQLite.Net.Interop;

namespace GoodVibrations.Persistence.ConnectionFactories
{
    /// <summary>
    /// Db connection factory. Needs to be initialized before its first usage by calling Initialize() method 
    /// </summary>
    public class GoodVibrationsConnectionFactory
    {      
        private GoodVibrationsConnectionFactory ()
        {
            _uniqueInstance = null;
        }

        /// <summary>
        /// The unique instance for singleton design pattern
        /// </summary>
        private static volatile GoodVibrationsConnectionFactory _uniqueInstance;

        /// <summary>
        /// Lock object for creating thread safe instance of singleton
        /// </summary>
        private static readonly object LockObj = new object ();

        /// <summary>
        /// Gets the singleton instance.
        /// </summary>
        /// <value>The instance.</value>
        public static GoodVibrationsConnectionFactory Instance {
            get {
                if (_uniqueInstance == null) {
                    lock (LockObj) {
                        if (_uniqueInstance == null)
                            _uniqueInstance = new GoodVibrationsConnectionFactory ();
                    }
                }
                return _uniqueInstance;
            }
        }

        /// <summary>
        /// Initialize the specified platform and databaseFolder.
        /// </summary>
        /// <param name="platform">Platform.</param>
        /// <param name="databasePath">Database path</param>
        public void Initialize (ISQLitePlatform platform, string databasePath)
        {
            DatabasePath = databasePath;
            Platform = platform;
            IsInitialized = true;
        }

        /// <summary>
        /// Returns the current schema version string
        /// </summary>
        public string SchemaVersion => AssemblyVersionHelper.GetAssemblyVersion ();

        /// <summary>
        /// Gets or sets a value indicating whether this instance is initialized.
        /// </summary>
        /// <value><c>true</c> if this instance is initialized; otherwise, <c>false</c>.</value>
        public bool IsInitialized {
            get;
            protected set;
        } = false;

        /// <summary>
        /// Platform of Db
        /// </summary>
        public ISQLitePlatform Platform { get; private set; }

        private SQLiteConnection _connection;

        /// <summary>
        /// Gets or sets the database path.
        /// </summary>
        /// <value>The database path.</value>
        public string DatabasePath {
            get;
            protected set;
        }

        /// <summary>
        /// Creates the connection.
        /// </summary>
        /// <returns>The connection.</returns>
        public SQLiteConnection GetConnection ()
        {
            if (!IsInitialized)
                throw new InvalidOperationException ($"{GetType().Name} is not initialized properly. Please Call {GetType ().Name}.Instance.Initialize() method before its first usage!");

            _connection = _connection ?? new SQLiteConnection (Platform, DatabasePath, false);
            return _connection;
        }

        /// <summary>
        /// Releasing the SQLite connection.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void CloseConnection ()
        {
            _connection?.Dispose ();
            _connection = null;
        }
    }
}


using System;
namespace GoodVibrations.Consts
{
	public static class Constants
	{
		public static class DataBase
		{
			public const string DatabaseFileName = "GoodVibration_SQLite_Database.db";
		}

        public static class RestApi
        {
            public const string HostUrl = "https://goodvibrations-app.azurewebsites.net";
        }

		public static class Band
		{
			public const string BandTileId = "012131414";
		}

		public static class KeyChain
		{
			public static readonly string CommonKeyChainUsername = "CommonKeyChainUsername";
			public static readonly string CommonKeyChainPassword = "CommonKeyChainPasssword";
			public static readonly string CommonKeyChainNamespace = "com.draeger.sc";
			public static readonly string CommonKeyChainKeyStoreFileProtectionPassword = "SdXsUg547GDFs7fGNHtLVPFdas978FIDASTohGhkxBU6ZsxtAFtgKa6hZDuweGA9YbV5ASuzXcVSdfz8";
		}
	}
}
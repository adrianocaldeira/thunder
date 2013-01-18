using System;
using System.Reflection;

namespace $rootnamespace$.Library
{
    public static class HardCode
    {
        public static string Version
        {
            get
            {
                Version version = Assembly.GetExecutingAssembly().GetName().Version;
                return string.Format("{0}.{1}.{2}", version.Major, version.Minor, version.Build);
            }
        }

        #region Nested type: Constants

        public static class Constants
        {
            public const string MessageWithSuccess = "Informações salvas com sucesso.";
            public const string ImageLoadingForm = "~/content/manager/images/loading.gif";
        }

        #endregion

        #region Nested type: Session

        public static class Session
        {
            public static string ConnectedUser = "SESSION_CONNECTED_USER";
            public static string Modules = "SESSION_FUNCTIONALITIES";
        }

        #endregion
    }
}
using SYT.VendingMachineSystem.Debugging;

namespace SYT.VendingMachineSystem
{
    public class VendingMachineSystemConsts
    {
        public const string LocalizationSourceName = "VendingMachineSystem";

        public const string ConnectionStringName = "Default";

        public const bool MultiTenancyEnabled = true;


        /// <summary>
        /// Default pass phrase for SimpleStringCipher decrypt/encrypt operations
        /// </summary>
        public static readonly string DefaultPassPhrase =
            DebugHelper.IsDebug ? "gsKxGZ012HLL3MI5" : "ac7ef171329a40f2a1980612a9276feb";
    }
}

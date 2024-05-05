namespace VacdmDataFaker.Shared
{
    public static partial class ConfigReader
    {
        public static VacdmConfig ReadVacdmConfig() => ReadVacdm();

        public static EcfmpConfig ReadEcfmpConfig() => ReadEcfmp();
    }
}

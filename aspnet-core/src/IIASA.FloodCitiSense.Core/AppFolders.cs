using Abp.Dependency;

namespace IIASA.FloodCitiSense
{
    public class AppFolders : IAppFolders, ISingletonDependency
    {
        public string TempFileDownloadFolder { get; set; }
        public string SampleProfileImagesFolder { get; set; }
        public string ImagesFolder { get; set; }
        public string FilesFolder { get; set; }
        public string WebLogsFolder { get; set; }
    }
}
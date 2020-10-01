namespace IIASA.FloodCitiSense
{
    public interface IAppFolders
    {
        string TempFileDownloadFolder { get; }

        string SampleProfileImagesFolder { get; }
        string ImagesFolder { get; set; }
        string FilesFolder { get; set; }

        string WebLogsFolder { get; set; }
    }
}
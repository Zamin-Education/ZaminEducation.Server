using Newtonsoft.Json;
using ZaminEducation.Domain.Entities.HomePage;
using ZaminEducation.Service.DTOs.Commons;
using ZaminEducation.Service.Extensions;

namespace ZaminEducation.Service.Helpers;

public class FileHelper
{
    /// <summary>
    /// This function for saving images to the wwwroot file asynchronously
    /// </summary>
    /// <param name="file"><see cref="AttachmentForCreationDto"/></param>
    /// <returns>return saved file name end this file static path</returns>
    public static async Task<(string fileName, string filePath)> SaveAsync(AttachmentForCreationDto file, bool isExist = false)
    {
        // genarate file destination
        string fileName = isExist ? file.FileName : Guid.NewGuid().ToString("N") + "-" + file.FileName;
        string filePath = Path.Combine(EnvironmentHelper.AttachmentPath, fileName);

        // copy image to the destination as stream
        FileStream fileStream = File.OpenWrite(filePath);
        await file.Stream.CopyToAsync(fileStream);

        // clear
        await fileStream.FlushAsync();
        fileStream.Close();

        return (fileName, EnvironmentHelper.FilePath + "/" + fileName);
    }

    /// <summary>
    /// This founction for remove file from wwwroot by given static path which is given by function parametr
    /// </summary>
    /// <param name="staticPath">file static path</param>
    /// <returns>if file is exists and deleted successfully return true else false</returns>
    public static bool Remove(string staticPath)
    {
        string fullPath = Path.Combine(EnvironmentHelper.WebRootPath, staticPath);

        if (!File.Exists(fullPath))
            return false;

        File.Delete(fullPath);
        return true;
    }

    /// <summary>
    /// Set of change home page info to ZaminEducation.Api.wwwroot.HomePagesInfo.json
    /// </summary>
    /// <param name="entity"></param>
    public async static Task<HomePagesInfo> SaveHomePagesInfoAsync(HomePagesInfo entity)
    {
        var oldHomePagesInfo = GetHomePagesInfo();

        if (oldHomePagesInfo is not null)
        {
            entity.Title = string.IsNullOrEmpty(entity.Title) ? oldHomePagesInfo.Title : entity.Title;
            entity.Description1 = string.IsNullOrEmpty(entity.Description1) ? oldHomePagesInfo.Description1 : entity.Description1;
            entity.Description2 = string.IsNullOrEmpty(entity.Description2) ? oldHomePagesInfo.Description2 : entity.Description2;
        }

        string json = JsonConvert.SerializeObject(entity,Formatting.Indented);
        await File.WriteAllTextAsync(EnvironmentHelper.HomePagesInfoConnectinString, json);

        return entity;
    }

    /// <summary>
    /// Remove home pages info from ZaminEducation.Api.wwwroot.HomePagesInfo.json
    /// </summary>
    public static bool RemoveHomePageInfo()
    {
        var exist = GetHomePagesInfo();
        File.WriteAllText(EnvironmentHelper.HomePagesInfoConnectinString, "");

        return exist is not null;
    }

    /// <summary>
    /// Gets home pages info from ZaminEducation.Api.wwwroot.HomePagesInfo.json
    /// </summary>
    /// <returns></returns>
    public static HomePagesInfo GetHomePagesInfo()
        => JsonConvert.DeserializeObject<HomePagesInfo>(
            File.ReadAllText(EnvironmentHelper.HomePagesInfoConnectinString));
}

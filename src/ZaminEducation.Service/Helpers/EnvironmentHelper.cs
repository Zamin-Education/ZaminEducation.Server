﻿namespace ZaminEducation.Service.Helpers;

public class EnvironmentHelper
{
    public static string WebRootPath { get; set; }
    public static string AttachmentPath => Path.Combine(WebRootPath, "images");
    public static string FilePath => "images";
    public static string CertificatePath => "certificates";
    public static string HomePagesInfoConnectinString =>
        @"C:\Users\Muhammadamin\source\repos\ZaminEducation.Server\src\ZaminEducation.Api\wwwroot\ZCApplicantInfo.json";
}

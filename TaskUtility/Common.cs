using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TaskUtility
{
    public static class Common
    {
        public static string SqlConnectionString = ConfigurationManager.ConnectionStrings["SqlConnectionString"].ConnectionString;
        public static string SQLCONNSTR = ConfigurationManager.ConnectionStrings["SqlConnectionString"].ConnectionString;

        public static string GetConfigValue(string KeyString)
        {
            string DynamicSetting;
            DynamicSetting = ConfigurationManager.AppSettings[KeyString];
            return DynamicSetting;
        }
        public static bool DoesFtpDirectoryExist(string ftpServer, string dirPath, string login, string password)
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpServer + dirPath);
                request.Method = WebRequestMethods.Ftp.ListDirectory;
                request.UseBinary = true;
                request.Credentials = new NetworkCredential(login, password);
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                response.Close();
                return true;
            }
            catch (WebException ex)
            {
                return false;
            }
        }
        public static int CreateDirectory(string ftpServer, string login, string password, string[] arr)
        {
            FtpWebRequest reqFTP = null;
            Stream ftpStream = null;
            int Errcode = 0;
            string DirStr = "";
            try
            {
                foreach (var item in arr)
                {
                    DirStr = DirStr + '/' + item.ToString().Trim();
                    try
                    {

                        reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpServer + DirStr.ToString().Trim()));
                        reqFTP.Method = WebRequestMethods.Ftp.MakeDirectory;
                        reqFTP.UseBinary = true;
                        reqFTP.Credentials = new NetworkCredential(login, password);
                        FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                        ftpStream = response.GetResponseStream();
                        List<string> entries = new List<string>();
                        ftpStream.Close();
                        response.Close();


                    }
                    catch (Exception ex)
                    {


                    }

                }
            }
            catch (Exception ex)
            {
                //directory already exist I know that is weak but there is no way to check if a folder exist on ftp...
            }
            return Errcode;
        }
        public static int UploadFileInFTP(string FileName, string localPath, string FolderName)
        {
            int Errcode = 0;
            try
            {

                string ftpUid = Common.GetConfigValue("FTPUid");
                string ftpPwd = Common.GetConfigValue("FTPPWD");
                string ftpServer = Common.GetConfigValue("FTP");
                string ftpDirectory = "";               
                    ftpDirectory = FolderName;

                bool dirExists = DoesFtpDirectoryExist(ftpServer, ftpDirectory, ftpUid, ftpPwd);
                if (!dirExists)
                {
                    string CreateDir = "";

                    string[] arr = ftpDirectory.Split('/');
                    arr = arr.Where(x => !string.IsNullOrEmpty(x)).ToArray();

                    int ErrCode = CreateDirectory(ftpServer, ftpUid, ftpPwd, arr);
                }
                for (int i = 0; i < 3; i++)
                {
                    try
                    {
                        FtpWebRequest requestFTPUploader = (FtpWebRequest)WebRequest.Create(ftpServer + FileName);
                        requestFTPUploader.Credentials = new NetworkCredential(ftpUid, ftpPwd);
                        requestFTPUploader.Method = WebRequestMethods.Ftp.UploadFile;
                        FileInfo fileInfo = new FileInfo(localPath);
                        using (FileStream fileStream = fileInfo.OpenRead())
                        {
                            int bufferLength = 2048;
                            byte[] buffer = new byte[bufferLength];
                            using (Stream uploadStream = requestFTPUploader.GetRequestStream())
                            {
                                int contentLength = fileStream.Read(buffer, 0, bufferLength);
                                while (contentLength != 0)
                                {
                                    uploadStream.Write(buffer, 0, contentLength);
                                    contentLength = fileStream.Read(buffer, 0, bufferLength);
                                }

                            }
                        }
                        // fileInfo.Delete();
                        requestFTPUploader = null;
                        Errcode = 50000;
                        break;
                    }
                    catch (Exception ex)
                    {
                        if (i > 2)
                        {
                            throw ex;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return Errcode;
        }

        private static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
        public static byte[] DownloadFileFromFTP(string Path)
        {
            String FTPServer = Common.GetConfigValue("FTP");
            String FTPUserId = Common.GetConfigValue("FTPUid");
            String FTPPwd = Common.GetConfigValue("FTPPWD");
            try
            {
                FtpWebRequest requestFileDownload = (FtpWebRequest)WebRequest.Create(FTPServer + Path);
                requestFileDownload.UseBinary = true;
                requestFileDownload.KeepAlive = false;
                requestFileDownload.Credentials = new NetworkCredential(FTPUserId, FTPPwd);
                requestFileDownload.Method = WebRequestMethods.Ftp.DownloadFile;
                FtpWebResponse responseFileDownload = (FtpWebResponse)requestFileDownload.GetResponse();
                Stream responseStream = responseFileDownload.GetResponseStream();
                byte[] OPData = ReadFully(responseStream);
                responseStream.Close();
                return OPData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

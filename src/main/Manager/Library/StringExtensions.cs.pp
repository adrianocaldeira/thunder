using System;
using System.IO;
using System.Linq;
using System.Web;

namespace $rootnamespace$.Library
{
    public static class StringExtensions
    {
        public static string MoveImages(this string content, string viewer)
        {
            try
            {
                var dom = CsQuery.CQ.Create(content);

                foreach (var image in dom.Select("img"))
                {
                    var src = image.Attributes["src"].ToLower();

                    if (!src.Contains(viewer.ToLower()) || !src.Contains("temporary")) continue;

                    var queryString = HttpUtility.ParseQueryString(string.Join(string.Empty, src.Split('?').Skip(1)));
                    var file = queryString["file"];
                    var directory = HttpUtility.UrlDecode(queryString["directory"]);
                    var path = Path.Combine(Settings.FileRepository, directory, file);

                    File.Move(path, path.Replace("\\temporary", ""));

                    image.Attributes["src"] = src.Replace("%255ctemporary", "");
                }

                return dom.Render();
            }
            catch (Exception)
            {
                return content;
            }
        }
    }
}
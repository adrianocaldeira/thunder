using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Thunder.Web;
using $rootnamespace$.Library;
using JsonResult = Thunder.Web.Mvc.JsonResult;

namespace $rootnamespace$.Models.Views
{
    /// <summary>
    /// Upload
    /// </summary>
    public class Upload
    {
        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="Upload"/>.
        /// </summary>
        public Upload()
        {
            Description = "Todos os arquivos";
            Extension = "*.*";
            Size = "1MB";
            Directory = "Files\\Temp";
            Callback = "";
            IsHtmlEditor = false;
            QueueSize = 3;
        }

        /// <summary>
        /// Recupera ou define tamanho da fila
        /// </summary>
        public int QueueSize { get; set; }

        /// <summary>
        /// Recupera ou define se o upload é de um editor html
        /// </summary>
        public bool IsHtmlEditor { get; set; }

        /// <summary>
        /// Recupera ou define chamada
        /// </summary>
        public string Callback { get; set; }

        /// <summary>
        /// Recupera ou define arquivo enviado
        /// </summary>
        public HttpPostedFileWrapper File { get; set; }

        /// <summary>
        /// Recupera ou define descrição
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Recupera ou define tamanho
        /// </summary>
        public string Size { get; set; }

        /// <summary>
        /// Recupera ou define extensão
        /// </summary>
        public string Extension { get; set; }

        /// <summary>
        /// Recupera ou define diretório
        /// </summary>
        public string Directory { get; set; }

        /// <summary>
        /// Recupera ou define se o upload é de múltiplo arquivos
        /// </summary>
        public bool Multiple { get; set; }

        /// <summary>
        /// Modelo válido
        /// </summary>
        /// <returns></returns>
        private bool IsValid()
        {
            return File != null && File.ContentLength > 0;
        }

        /// <summary>
        /// Salva arquivo
        /// </summary>
        /// <param name="urlHelper"> </param>
        /// <returns></returns>
        public JsonResult Save(UrlHelper urlHelper)
        {

            try
            {
                if (IsValid())
                {
                    var directoryDecode = HttpUtility.UrlDecode(Directory);
                    var directory = Path.Combine(Settings.FileRepository, directoryDecode);

                    if (!System.IO.Directory.Exists(directory))
                    {
                        System.IO.Directory.CreateDirectory(directory);
                    }

                    var newFileName = Guid.NewGuid() + Path.GetExtension(File.FileName);
                    var pathNewFile = Path.Combine(directory, newFileName);

                    File.SaveAs(pathNewFile);

                    RemoveExpiredFiles(directory);

                    return new JsonResult
                    {
                        Data = new
                        {
                            Name = File.FileName,
                            Path = HttpUtility.UrlEncode(Path.Combine(directoryDecode, newFileName)),
                            Viewer = urlHelper.Action("Viewer", "Files", new
                            {
                                file = newFileName,
                                directory = Directory
                            }),
                            Size = File.ContentLength
                        }
                    };
                }

                return new JsonResult(ResultStatus.Attention)
                {
                    Data = new { Message = "Arquivo não enviado ou inválido" }
                };
            }
            catch (Exception ex)
            {
                return new JsonResult(ResultStatus.Error)
                {
                    Data = new { ex.Message }
                };
            }  
        }

        private static void RemoveExpiredFiles(string directory)
        {
            var files = System.IO.Directory.GetFiles(directory);
            var time = DateTime.Now.AddMinutes(-30);

            foreach (var file in
                from file in files
                let fileInfo = new FileInfo(file)
                where fileInfo.CreationTime <= time
                select file)
            {
                System.IO.File.Delete(file);
            }
        }
    }
}
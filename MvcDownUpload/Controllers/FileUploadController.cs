using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcDownUpload.Controllers
{
    public class FileUploadController : Controller
    {
        // GET: FileUpload
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult FileUpload()
        {
            string erroRegra = "0";
            double tamanho = 0;
            string extensao = String.Empty;
            //tamanho maximo do upload em kb
            double permitido = 200;

            HttpPostedFileBase arquivo;

            int arquivosSalvos = 0;
            for (int i = 0; i < Request.Files.Count; i++)
            {
                arquivo = Request.Files[i];
                //Suas validações
                if (arquivo != null)
                {
                    //identificamos o tamanho do arquivo
                    tamanho = Convert.ToDouble(arquivo.ContentLength) / 1024;
                    string arq = arquivo.FileName;

                    //varifcamos a extensão através dos últimos 4 caracteres
                    extensao = arq.Substring(arq.Length - 4).ToLower();

                    //para saber o nome do arquivo utilizaremos a propriedade GetFileName
                    //passando a string arq
                    string nomeArq = System.IO.Path.GetFileName(tira_acentos(arq));

                    //diretorio onde será gravado o arquivo
                    //criar o diretório arquivos no mesmo local da aplicação
                    string diretorio = this.Server.MapPath("Arq_Upload\\" + tira_acentos(nomeArq));

                    // o tamanho acima do permitido - violação de regra
                    if (tamanho > permitido)
                    {
                        ViewData["Message"] = "Tamanho Máximo permitido é de " + permitido + " kb!";
                        erroRegra = "1";
                    }

                    // extensão diferente de jpg, doc, pdf e gif - violação de regra
                    if ((extensao != ".jpg" && extensao != ".gif" && extensao != ".doc" && extensao != "pdf"))
                    {
                        ViewData["Message"] = "Extensão inválida, só são permitidas .jpg, .doc, .pdf e .gif!";
                        erroRegra = "2";

                    }

                    if (erroRegra == "0")
                    {
                        try
                        {
                            // verifica se já existe o nome do arquivo submetido
                            if (!this.Server.MapPath($"Arq_Upload\\{arq}").Contains(diretorio))
                            {
                                //Salva o arquivo
                                if (arquivo.ContentLength > 0)
                                {
                                    arquivo.SaveAs(diretorio);
                                    arquivosSalvos++;
                                }
                            }
                            else
                            {
                                ViewData["Message"] = "Já existe um arquivo com esse nome!";
                            }
                        }
                        catch
                        {
                            ViewData["Message"] = "Erro no Upload";
                        }
                    }
                }
            }

            ViewData["Message"] = String.Format("{0} arquivo(s) salvo(s) com sucesso.", arquivosSalvos);

            return View("Upload");
        }

        /// <summary>
        /// //Método que remove acentos, espaços e carateres indesejados
        /// </summary>
        /// <param name="texto"></param>
        /// <returns>O texto formatado</returns>
        public static string tira_acentos(string texto)
        {

            string ComAcentos = "!@#$%¨&*()-?:{}][ÄÅÁÂÀÃäáâàãÉÊËÈéêëèÍÎÏÌíîïìÖÓÔÒÕöóôòõÜÚÛüúûùÇç ";

            string SemAcentos = "_________________AAAAAAaaaaaEEEEeeeeIIIIiiiiOOOOOoooooUUUuuuuCc_";

            for (int i = 0; i < ComAcentos.Length; i++)

                texto = texto.Replace(ComAcentos[i].ToString(), SemAcentos[i].ToString()).Trim();



            return texto;
        }

    }
}
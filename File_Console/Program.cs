using iTextSharp.text.pdf;
using PdfSharpCore.Pdf.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using iTextSharp.text.pdf.parser;
using iTextSharp.text;

namespace File_Console
{
    class Program
    {
        static void Main(string[] args)
        {

            //iTextSharp.text.pdf.PdfReader reader = new iTextSharp.text.pdf.PdfReader(@"E:\New\File_Solution\PDFs\Split pdf sample file for Practical round.pdf");
            //int n = reader.NumberOfPages;
            //Rectangle psize = reader.GetPageSize(1);
            //float width = psize.Width;
            //float height = psize.Height;
            //Console.WriteLine("Size of page 1 of {0} => {1} × {2}", n, width, height);
            //Dictionary<string, string> infodict = reader.Info;
            //foreach (KeyValuePair<string, string> kvp in infodict)
            //    Console.WriteLine(kvp.Key + " => " + kvp.Value);
            string fileName = @"E:\New\File_Solution\PDFs\Split pdf sample file for Practical round.pdf";
            //SplitPDFByBookMark(fileName);
            iTextSharp.text.pdf.PdfReader pdfReader = new iTextSharp.text.pdf.PdfReader(fileName);
            Console.WriteLine(pdfReader.NumberOfPages);


           
        }

        public static void SplitPDFByBookMark(string fileName)
        {
            //string fileName = @"E:\New\File_Solution\PDFs\Split pdf sample file for Practical round.pdf";
            string sInFile = fileName;
            iTextSharp.text.pdf.PdfReader pdfReader = new iTextSharp.text.pdf.PdfReader(sInFile);
            try
            {
                IList<Dictionary<string, object>> bookmarks = SimpleBookmark.GetBookmark(pdfReader);

                for (int i = 0; i < bookmarks.Count; ++i)
                {
                    IDictionary<string, object> BM = (IDictionary<string, object>)bookmarks[0];
                    IDictionary<string, object> nextBM = i == bookmarks.Count - 1 ? null : bookmarks[i + 1];

                    string startPage = BM["Page"].ToString().Split(' ')[0].ToString();
                    string startPageNextBM = nextBM == null ? "" + (pdfReader.NumberOfPages + 1) : nextBM["Page"].ToString().Split(' ')[0].ToString();
                    SplitByBookmark(pdfReader, int.Parse(startPage), int.Parse(startPageNextBM), bookmarks[i].Values.ToArray().GetValue(0).ToString() + ".pdf", fileName);
                    Console.WriteLine("Reading....");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void SplitByBookmark(iTextSharp.text.pdf.PdfReader reader, int pageFrom, int PageTo, string outPutName, string inPutFileName)
        {
            iTextSharp.text.Document document = new iTextSharp.text.Document();
            FileStream fs = new System.IO.FileStream(System.IO.Path.GetDirectoryName(inPutFileName) + '\\' + outPutName, System.IO.FileMode.Create);

            try
            {
                
                PdfWriter writer = PdfWriter.GetInstance(document, fs);
                document.Open();
                PdfContentByte cb = writer.DirectContent;
                //holds pdf data
                PdfImportedPage page;
                if (pageFrom == PageTo && pageFrom == 1)
                {
                    document.NewPage();
                    page = writer.GetImportedPage(reader, pageFrom);
                    cb.AddTemplate(page, 0, 0);
                    pageFrom++;
                    fs.Flush();
                    document.Close();
                    fs.Close();

                }
                else
                {
                    while (pageFrom < PageTo)
                    {
                        document.NewPage();
                        page = writer.GetImportedPage(reader, pageFrom);
                        cb.AddTemplate(page, 0, 0);
                        pageFrom++;
                        fs.Flush();
                        document.Close();
                        fs.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (document.IsOpen())
                    document.Close();
                if (fs != null)
                    fs.Close();
            }

        }
    }
}

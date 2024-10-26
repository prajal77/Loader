using iTextSharp.text;
using iTextSharp.text.pdf;
using Loader.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Loader.Pdf
{
    public class PdfEvents : PdfPageEventHelper
    {
        private TransferService transferService = null;
        private int STMId = 0;

        public PdfEvents(int STMId)
        {
            this.STMId = STMId;
            transferService = new TransferService();
        }

        PdfContentByte cb;
        PdfTemplate headerTemplate, footerTemplate;
        BaseFont bf = null;

        DateTime PrintTime = DateTime.Now;

        private string _headers;

        public string Header { get { return _headers; } set { _headers = value; } }


        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            try
            {
                PrintTime = DateTime.Now;
                bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                cb = writer.DirectContent;
                headerTemplate = cb.CreateTemplate(100, 100);
                footerTemplate = cb.CreateTemplate(50, 50);

            }
            catch (DocumentException de)
            {
                throw de;
            }
            catch (System.IO.IOException ioe)
            {
                throw ioe;
            }
        }

        public override void OnEndPage(iTextSharp.text.pdf.PdfWriter writer, iTextSharp.text.Document document)
        {
            base.OnEndPage(writer, document);
            //iTextSharp.text.Font baseFontNormal = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
            //iTextSharp.text.Font baseFontBig = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);

            //Create PdfTable object  
            PdfPTable pdfTab = new PdfPTable(4);
            pdfTab.DefaultCell.Border = PdfPCell.NO_BORDER;
            string Companyname = "";
            string CompanyAddress = "";
            string CompanyPanNo = "";
            string CompanyPhone = "";
            String text = "Page " + writer.PageNumber + " of ";

            var company = transferService.GetCompanyInfo();
            foreach (var i in company)
            {
                Companyname = i.CompanyName;
                CompanyAddress = i.CompanyAddress;
                CompanyPanNo = i.PanNo;
                CompanyPhone = i.PhoneNumber;
            }
            //Add paging to header  
            {
                cb.BeginText();
                cb.SetFontAndSize(bf, 10);
                cb.SetTextMatrix(document.PageSize.GetRight(100), document.PageSize.GetTop(20));
                cb.ShowText(text);
                cb.EndText();
                float len = bf.GetWidthPoint(text, 12);
                //Adds "12" in Page 1 of 12  
                cb.AddTemplate(headerTemplate, document.PageSize.GetRight(100) + len, document.PageSize.GetTop(45));

                cb.BeginText();
                cb.SetFontAndSize(bf, 10);
                cb.SetTextMatrix(document.PageSize.GetLeft(40), document.PageSize.GetTop(20));
                //cb.ShowText("SUBISU/PDO/GP/01");
                var Code = CompanyPanNo;
                cb.ShowText(Code);
                cb.EndText();
                //Adds "12" in Page 1 of 12  
                cb.AddTemplate(headerTemplate, document.PageSize.GetRight(100) + len, document.PageSize.GetTop(45));

                cb.BeginText();
                cb.SetFontAndSize(bf, 15);
                cb.SetTextMatrix(document.PageSize.GetLeft(200), document.PageSize.GetTop(20));
                //var PdfHeader = new Loader.Repository.GenericUnitOfWork().Repository < Loader.Models.ParamValue> ().FindBy(x=>x.PId==4).Select(x=>x.PValue).FirstOrDefault();

                var PdfHeader = Companyname;

                cb.ShowText(PdfHeader);
                cb.EndText();
                float len2 = bf.GetWidthPoint(text, 15);
                cb.AddTemplate(headerTemplate, document.PageSize.GetRight(100) + len, document.PageSize.GetTop(45));
            }
            //Add paging to footer  
            {
                cb.BeginText();
                cb.SetFontAndSize(bf, 10);
                cb.SetTextMatrix(document.PageSize.GetRight(230), document.PageSize.GetBottom(20));
                cb.ShowText("Approved By : .............................................");
                float len = bf.GetWidthPoint(text, 10);
                cb.AddTemplate(footerTemplate, document.PageSize.GetRight(180) + len, document.PageSize.GetBottom(20));
                cb.EndText();
                cb.BeginText();
                cb.SetFontAndSize(bf, 10);
                cb.SetTextMatrix(document.PageSize.GetLeft(40), document.PageSize.GetBottom(20));
                cb.ShowText("Received By : ............................................");
                cb.EndText();
                float len2 = bf.GetWidthPoint(text, 10);
                cb.AddTemplate(footerTemplate, document.PageSize.GetLeft(180) + len2, document.PageSize.GetBottom(20));

                //Paragraph para = new Paragraph("Subisu Cablenet(P.) Ltd.", new Font(Font.FontFamily.HELVETICA, 15));
            }

            var btd = transferService.GetPassList(STMId);

            ///for header
            

            //pdfTab.AddCell(" ");
            var address = CompanyAddress;
            // pdfTab.AddCell(new Phrase("Baluwatar , Kathmandu, Nepal", new Font(Font.FontFamily.TIMES_ROMAN, 9, 1, new BaseColor(0, 0, 0))));
            pdfTab.AddCell(new Phrase(address, new Font(Font.FontFamily.TIMES_ROMAN, 9, 1, new BaseColor(0, 0, 0))));

            pdfTab.SpacingAfter = 20;

            pdfTab.AddCell(new Phrase("G A T E    P A S S ", new Font(Font.FontFamily.TIMES_ROMAN, 11, 1, new BaseColor(0, 0, 0))));
            

            pdfTab.AddCell(AddLabel(new Phrase(), btd[0].TransferBy, "Transfer By : "));

            //pdfTab.SpacingAfter = 20;


            pdfTab.AddCell(AddLabel(new Phrase(), btd[0].ReqNo, "No : "));


            //pdfTab.AddCell(" ");
            pdfTab.AddCell(AddLabel(new Phrase(), btd[0].TransferOn.ToString(), "Transfered Date : "));

            //pdfTab.SpacingAfter = 20;



            pdfTab.AddCell(AddLabel(new Phrase(), btd[0].RequestedBy, "Name : "));
            //pdfTab.AddCell(new Phrase("G A T E    P A S S ", new Font(Font.FontFamily.TIMES_ROMAN, 11, 1, new BaseColor(0, 0, 0))));
            pdfTab.AddCell(AddLabel(new Phrase(), btd[0].RequestDateAD.ToString(), "Requested Date : "));
            pdfTab.AddCell(AddLabel(new Phrase(), btd[0].RequestDateBS.ToString(), "Requested Date BS: "));

            pdfTab.SpacingAfter = 20;

           // pdfTab.AddCell(AddLabel(new Phrase(), btd[0].AreaName, "AreaName : "));
            pdfTab.AddCell("");
            pdfTab.AddCell(AddLabel(new Phrase(), btd[0].BranchName.ToString(), "BranchName : "));

            pdfTab.TotalWidth = document.PageSize.Width - 80f;
            pdfTab.WidthPercentage = 70;
            //pdfTab.HorizontalAlignment = Element.ALIGN_CENTER;      

            //call WriteSelectedRows of PdfTable. This writes rows from PdfWriter in PdfTable  
            //first param is start row. -1 indicates there is no end row and all the rows to be included to write  
            //Third and fourth param is x and y position to start writing  
            pdfTab.WriteSelectedRows(0, -1, 40, document.PageSize.Height - 30, writer.DirectContent);
            //set pdfContent value  

            //Move the pointer and draw line to separate header section from rest of page  
            cb.MoveTo(40, document.PageSize.Height - 100);
            cb.LineTo(document.PageSize.Width - 40, document.PageSize.Height - 100);
            cb.Stroke();

            //Move the pointer and draw line to separate footer section from rest of page  
            cb.MoveTo(40, document.PageSize.GetBottom(50));
            cb.LineTo(document.PageSize.Width - 40, document.PageSize.GetBottom(50));
            cb.Stroke();

        }

        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            base.OnCloseDocument(writer, document);

            headerTemplate.BeginText();
            headerTemplate.SetFontAndSize(bf, 10);
            headerTemplate.SetTextMatrix(0, 25);
            headerTemplate.ShowText((writer.PageNumber).ToString());
            headerTemplate.EndText();

            //footerTemplate.BeginText();
            //footerTemplate.SetFontAndSize(bf, 12);
            //footerTemplate.SetTextMatrix(0, 0);
            //footerTemplate.ShowText((writer.PageNumber - 1).ToString());
            //footerTemplate.EndText();
        }

        private static Phrase AddLabel(Phrase phrase, string cellText, string label)
        {

            var boldFont = FontFactory.GetFont(FontFactory.TIMES_BOLD, 9);
            var normalFont = FontFactory.GetFont(FontFactory.TIMES, 9);
            phrase.Add(new Chunk(label, boldFont));
            phrase.Add(new Chunk(cellText, normalFont));
            return phrase;
            // return new Chunk(cellText, boldFont);
        }

    }
}